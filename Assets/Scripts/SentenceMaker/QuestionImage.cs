using Assets.Scrips.Classes;
using Assets.Scrips.Functions;
using Assets.Scrips.Utilities;
using Assets.Scripts.Utilities;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestionImage : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler //Es necesario heredar los 3 aunque solo use un drag para que funcione
{


    public Button WordSpace;
    public GameObject SentenceLine;
    public GameObject SentencesPanel;

    public Button AnswerOption;
    public GameObject AnswersColum;
    public GameObject AnswersColumsPanel;

    public GameObject panel;
    public GameObject Scorevalue;
    public GameObject TimeValue;
    public GameObject ImageCountValue;
    public GameObject ResultsPopup;
    public GameObject ScoreBorad;

    public GameObject SwapedImage;

    private Sprite sprite;
    private SqliteCommand command;
    private SqliteConnection connection;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private bool Checked;
    private bool touchInImage = false;
    private List<GameObject> WordSpaces;
    private int score = 0;
    private Time time;
    private int RemainingQuestions;
    private ImageSentence imageSentence;
    private static int _lvl;
    private int NumOfCorrectSenteces = 0;
    private int numOfSentecesInLvl = 0;
    private int remainingImages;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            ResultsPopup.transform.localScale = new Vector2(0, 0);
            #region Connect to database and get initial data for first imageSentence
            DataBaseManager.CreateAccessibleDB("MainDataBase.s3db");
            connection = DataBaseManager.CreateConection("MainDataBase.s3db");
            connection.Open();
            command = connection.CreateCommand();
            #region Get the number of sentences
            string sqlQuery = "SELECT count(1) FROM Image_Sentence";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            int numSenrtence = 1;
            while (reader.Read())
            {
                numSenrtence = reader.GetInt32(0);
                break;
            }
            reader.Close();
            #endregion

            #region Get a random imageSentence

            sqlQuery = $@"SELECT * FROM Image_Sentence WHERE Level = {_lvl}";
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                ImageSentenceList.AddSentenceImage(new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
            }
            numOfSentecesInLvl = ImageSentenceList.Count();
            remainingImages = numOfSentecesInLvl - 1;
            ImageCountValue.GetComponentInChildren<Text>().text = remainingImages.ToString();
            ImageSentenceList.Shuffle();
            reader.Close();
            #endregion

            #endregion

            #region Display image and wordSpaces
            GenerateQuestion();

            Scorevalue.GetComponentInChildren<Text>().text = score.ToString();
            #endregion
            //connection.Close();
            //connection.Dispose();

        }
        catch (Exception e)
        {
            Debug.LogError("Failed: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Swipe();
        if (Input.touchCount <= 0)
        {
            return;
        }

        var touch = Input.touches[0];

        if (touch.tapCount == 2)
        {

            if (!Checked)
            {
                if (WordSpaces?.Count > 0)
                {
                    Check();
                }
                if (ImageSentenceList.Count() == 0)
                {
                    try
                    {
                        FinalScore();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                    }
                }

                Checked = true;
            }
        }
    }
    public void Swipe()
    {
        try
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Began)
                {
                    //save began touch 2d point
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                }
                if (t.phase == TouchPhase.Ended)
                {
                    //save ended touch 2d point
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    //create vector from the two points
                    // currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    if (Math.Abs(secondPressPos.x - firstPressPos.x) > 300 && firstPressPos.x > secondPressPos.x && touchInImage)
                    {
                        if (ImageSentenceList.Count() > 0)
                        {

                            SwapedImage.GetComponent<Image>().sprite = sprite;
                            var color = SwapedImage.GetComponent<Image>().color;
                            SwapedImage.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 255);

                            SwapedImage.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
                            SwapedImage.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;

                            LeanTween.moveLocalX(SwapedImage, -1920, 0.3f);
                            LeanTween.rotateZ(SwapedImage, 30, 0.3f);

                            GenerateQuestion();
                            panel.GetComponent<HorizontalLayoutGroup>().enabled = true;
                            Checked = false;
                            remainingImages--;
                            ImageCountValue.GetComponentInChildren<Text>().text = remainingImages.ToString();

                        }
                        else
                        {
                            FinalScore();
                        }
                        touchInImage = false;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public void Check()
    {
        bool allOk = true;
        foreach (var wordSpace in WordSpaces)
        {
            if (wordSpace.GetComponent<WordSpaceButton>().Check())
            {
                score = score + 10;
                Scorevalue.GetComponentInChildren<Text>().text = score.ToString();
            }
            else
            {
                allOk = false;
            }
        }
        if (allOk)
        {
            NumOfCorrectSenteces++;
        }
    }

    void GenerateQuestion()
    {
        imageSentence = ImageSentenceList.GetSentenceImage();
        sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentence.Image_Id);
        gameObject.GetComponent<Image>().sprite = sprite;
        //LeanTween.alpha(gameObject.GetComponent<Image>().color,255,0.3f);
        WordSpaces = ImageSenteceFunc.GeneretaButtons(command, imageSentence.Sentence, WordSpace, SentenceLine, SentencesPanel, AnswerOption, AnswersColum, AnswersColumsPanel);
    }
    public void OnDrag(PointerEventData eventData) { }

    public void OnBeginDrag(PointerEventData eventData)
    {
        touchInImage = true;
    }
    public static void SetLvl(int lvl)
    {
        _lvl = lvl;
    }
    public void OnEndDrag(PointerEventData eventData)
    {

    }
    public void FinalScore()
    {
        try
        {
            TimeValue.GetComponent<Timer>().SetRun(false);
            string timeString = TimeValue.GetComponentInChildren<Text>().text;
            string sqlQuery = $"SELECT Progress, Best_Score, Best_Score_Time FROM Progress_Lvl WHERE Level= {_lvl}";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            float lastProgress = 0;
            int bestScore = 0;
            string bestTime="";
            string titleS = "Results";
            string scoreS = $"Score: {score}";
            string timeS = $"Time: {timeString}";
            string lastScoreS;
            string lastTimeS;
            string progressUp = "+0%";

            while (reader.Read())
            {
                lastProgress = reader.GetFloat(0);
                bestScore = reader.GetInt32(1);
                bestTime = reader.GetString(2);

                break;
            }
            lastScoreS = $"Best Score: {bestScore}";
            lastTimeS = $"Best Time: {bestTime}";

            float actualProgress = (float)NumOfCorrectSenteces / (float)numOfSentecesInLvl;
            string apS = actualProgress.ToString().Replace(",", ".");
            reader.Close();
            if (actualProgress > lastProgress)
            {
                progressUp = $"+{actualProgress - lastProgress}%";
                sqlQuery = $"UPDATE Progress_Lvl SET Progress = {apS} WHERE Level= {_lvl}";
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
            }
            if (score > bestScore)
            {
                titleS = "NEW RECORD!!";
                lastScoreS = $"Last Best Score: {bestScore}";
                lastTimeS = $"Last Best Time: {bestTime}";
                sqlQuery = $"UPDATE Progress_Lvl SET Best_Score = {score}, Best_Score_Time='{timeString}'WHERE Level= {_lvl}";
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
            }
            ScoreBorad.SetActive(false);
            ResultsPopup.transform.GetChild(0).GetComponentInChildren<Text>().text = titleS;
            ResultsPopup.transform.GetChild(1).GetComponentInChildren<Text>().text = scoreS;
            ResultsPopup.transform.GetChild(2).GetComponentInChildren<Text>().text = timeS;
            ResultsPopup.transform.GetChild(3).GetComponentInChildren<Text>().text = lastScoreS;
            ResultsPopup.transform.GetChild(4).GetComponentInChildren<Text>().text = lastTimeS;
            GameObject progressPanel = ResultsPopup.transform.GetChild(5).gameObject;
            progressPanel.transform.GetChild(2).GetComponent<Text>().text = progressUp;

            LeanTween.scale(ResultsPopup, new Vector3(1, 1, 0), 0.3f);
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
}