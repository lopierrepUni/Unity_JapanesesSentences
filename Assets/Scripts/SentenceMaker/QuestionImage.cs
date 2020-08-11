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
    private int lvl;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            lvl = StartButton.lvl;

            //
            
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

            sqlQuery = $@"SELECT * FROM Image_Sentence";
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                ImageSentenceList.AddSentenceImage(new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
            }
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
                    Debug.Log("Ultima pregunta");
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
                            SwapedImage.GetComponent<Image>().color = new Color(color.r,color.g,color.b,255);

                            SwapedImage.GetComponent<RectTransform>().sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
                            SwapedImage.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;

                            LeanTween.moveLocalX(SwapedImage, -1920, 0.3f);
                            LeanTween.rotateZ(SwapedImage, 30, 0.3f);



                            GenerateQuestion();
                            panel.GetComponent<HorizontalLayoutGroup>().enabled = true;
                            Checked = false;
                        }
                        else
                        {
                            Debug.Log("Ultima pregunta");
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
        foreach (var wordSpace in WordSpaces)
        {
            if (wordSpace.GetComponent<WordSpaceButton>().Check())
            {
                score = score + 10;
                Scorevalue.GetComponentInChildren<Text>().text = score.ToString();

            }
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



    public void OnEndDrag(PointerEventData eventData)
    {

    }
}