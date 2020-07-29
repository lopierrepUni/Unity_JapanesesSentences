﻿using Assets.Scrips.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordSpaceButton : MonoBehaviour
{
    public Button button;
    public GameObject AnswersPanel;
    public Button AnswerOptionButton;

    public string correctAnswer;
    public List<string> words;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = words.ElementAt(0);
        button.onClick.AddListener(DisplayAnswersPanel);
    }

    void DisplayAnswersPanel()
    {
        try
        {

            #region Display panel
            Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
            GameObject oldPanel = GameObject.FindGameObjectWithTag("OptionAnswerPanel");
            if (oldPanel != null)
            {
                Destroy(oldPanel);
            }
            GameObject panel = Instantiate(AnswersPanel, new Vector3(665f, 125f, -1), Quaternion.identity);
            panel.name = "OptionAnswerPanel";
            panel.transform.parent = canvas.transform;
            List<Button> answerOptionButtons = new List<Button>();
            #endregion

            #region Adjust Sizes
            float x = 0, y = 0.7f, height, separation;
            switch (words.Count())
            {
                case 3:
                    height = 0.3f;
                    separation = 0.15f;
                    break;
                case 4:
                    height = 0.25f;
                    separation = 0.1f;
                    break;

                default:
                    height = 0.2f;
                    separation = 0.083f;
                    break;
            }
            #endregion

            #region Generate Answer Option Buttons
            y = y - separation - height / 2;
            int i = 0;
            foreach (var word in words)
            {
                Button answerOptionButton = Instantiate(AnswerOptionButton);
                answerOptionButton.name = "AnswerOptionButton" + i;
                i++;
                answerOptionButton.transform.parent = panel.transform;
                answerOptionButton.transform.localPosition = new Vector3(0, y, -1);
                answerOptionButton.GetComponent<RectTransform>().sizeDelta = new Vector2(321.8071f, 100f);
                List<string> lw = new List<string>();
                lw.Add(word);
                answerOptionButton.GetComponent<AnswerOptionButton>().words = lw;
                answerOptionButton.GetComponent<AnswerOptionButton>().wordSpace = button;
                answerOptionButton.GetComponent<AnswerOptionButton>().panel = panel;

                y = y - separation - height;
                answerOptionButtons.Add(answerOptionButton);
            }
            #endregion
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
   public  bool check()
    {
        string currentAnswer = GetComponentInChildren<Text>().text;      
        return currentAnswer.Equals(correctAnswer);
    }

    // Update is called once per frame
    void Update()
    {

    }
}