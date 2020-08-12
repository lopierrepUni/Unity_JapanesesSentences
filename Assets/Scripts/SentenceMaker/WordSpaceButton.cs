﻿using Assets.Scrips.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordSpaceButton : MonoBehaviour
{
    private string _correctAnswer;
    //private string correctAnswer;    
    private bool _isFill = false;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D box = gameObject.GetComponentInChildren<BoxCollider2D>();
        box.size = GetComponentInChildren<RectTransform>().sizeDelta;
    }


    public bool Check()
    {
        if (transform.childCount == 2)
        {
            var answOpt = transform.GetChild(1).gameObject;
            answOpt.GetComponent<AnswerOptionButton>().Hide();

            string currentAnswer = answOpt.GetComponent<AnswerOptionButton>().word;
            bool correct = currentAnswer.Equals(_correctAnswer);
            if (correct)
            {
                GetComponentInChildren<Text>().color = Color.green;
                GetComponentInChildren<Text>().text = _correctAnswer;
                return true;
            }
            else
            {
                GetComponentInChildren<Text>().text = $@"<color=#ff0000ff>{currentAnswer}</color>" + "\n" + $"<color=#00ff00ff>{_correctAnswer}</color>";
                return false;
            }
        }
        else
        {
            GetComponentInChildren<Text>().text = $@"<color=#ff0000ff>---</color>" + "\n" + $"<color=#00ff00ff>{_correctAnswer}</color>";
            return false;
        }



    }
    public void ReturnAnswOpt()
    {
        try
        {
            var answOpt = transform.GetChild(1).gameObject;
            answOpt.GetComponent<AnswerOptionButton>().StartCoroutine(answOpt.GetComponent<AnswerOptionButton>().ReturnOrigianlPos());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public void MoveAnswOpt(GameObject newWordSpace)
    {
        try
        {
            var answOpt = transform.GetChild(1).gameObject;
            answOpt.GetComponent<AnswerOptionButton>().StartCoroutine(answOpt.GetComponent<AnswerOptionButton>().SwapPos(newWordSpace));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    public void SetCorrectAnswer(string correctAnswer)
    {
        _correctAnswer = correctAnswer;
    }
    public void SetIsFill(bool isFill)
    {
        _isFill = isFill;
    }
    public bool GetIsFill()
    {
        return _isFill;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //NOT IN USE. Generate answer options for the specific wordSpace
    void DisplayAnswersPanel()
    {
        //try
        //{
        //    #region Display panel
        //    Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        //    GameObject oldPanel = GameObject.FindGameObjectWithTag("OptionAnswerPanel");
        //    if (oldPanel != null)
        //    {
        //        Destroy(oldPanel);
        //    }
        //    GameObject panel = Instantiate(AnswersPanel, new Vector3(665f, 125f, -1), Quaternion.identity);
        //    panel.name = "OptionAnswerPanel";
        //    panel.transform.parent = canvas.transform;
        //    List<Button> answerOptionButtons = new List<Button>();
        //    #endregion

        //    #region Adjust Sizes
        //    float x = 0, y = 0.7f, height, separation;
        //    switch (words.Count())
        //    {
        //        case 3:
        //            height = 0.3f;
        //            separation = 0.15f;
        //            break;
        //        case 4:
        //            height = 0.25f;
        //            separation = 0.1f;
        //            break;

        //        default:
        //            height = 0.2f;
        //            separation = 0.083f;
        //            break;
        //    }
        //    #endregion

        //    #region Generate Answer Option Buttons
        //    y = y - separation - height / 2;
        //    int i = 0;
        //    foreach (var word in words)
        //    {
        //        Button answerOptionButton = Instantiate(AnswerOptionButton);
        //        answerOptionButton.name = "AnswerOptionButton" + i;
        //        i++;
        //        answerOptionButton.transform.parent = panel.transform;
        //        answerOptionButton.transform.localPosition = new Vector3(0, y, -1);
        //        answerOptionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(321.8071f, 100f);
        //        answerOptionButton.GetComponent<AnswerOptionButton>().word = word;
        //        answerOptionButton.GetComponent<AnswerOptionButton>().wordSpace = button;
        //        answerOptionButton.GetComponent<AnswerOptionButton>().panel = panel;

        //        y = y - separation - height;
        //        answerOptionButtons.Add(answerOptionButton);
        //    }
        //    #endregion
        //}
        //catch (System.Exception e)
        //{
        //    Debug.LogError(e.ToString());
        //}
    }
}