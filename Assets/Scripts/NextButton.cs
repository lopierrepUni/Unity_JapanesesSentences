﻿using Assets.Scrips.Classes;
using Assets.Scrips.Functions;
using Assets.Scrips.Utilities;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NextButton : MonoBehaviour
{
    public Button wordSpace;
    public GameObject BackgroudImage;
    public Button button;
    public Canvas canvas;

    private List<Button> wordSpaces = new List<Button>();
    private Sprite sprite;
    private SqliteCommand command;
    private SqliteConnection connection;
    private List<ImageSentence> imageSentenceList;
    private int questionNumer = 1;

    // Start is called before the first frame update
    void Start()
    {

        try
        {
            connection = DataBaseManager.CreateConection("MainDataBase.s3db");
            connection.Open();
            command = connection.CreateCommand();
            string sqlQuery = "SELECT * FROM Image_Sentence";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            imageSentenceList = new List<ImageSentence>();
            while (reader.Read())
            {
                imageSentenceList.Add(new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
            }
            reader.Close();
            button.onClick.AddListener(NextImage);//adds a listener for when you click the button           
          
           
        }
        catch (Exception e)
        {
            Debug.LogError("Failed: " + e.Message);
        }

    }

    void NextImage()
    {
        try
        {
            if (questionNumer < imageSentenceList.Count())
            {
                sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentenceList.ElementAt(questionNumer).Image_Id);
                BackgroudImage.GetComponent<SpriteRenderer>().sprite = sprite;
                string sentence = imageSentenceList.ElementAt(questionNumer).Sentence;
                List<string> words = sentence.Split('*').ToList();
                ImageSenteceFunc.GeneretaWordsSpaceButtons(words, wordSpace,command, 4, canvas);
                questionNumer++;
            }
            else
            {
                //Generar un aviso de que ya se respondieron todas las preguntas del nivel
                Debug.Log("Log: No more images founds");
            }


        }
        catch (Exception e)
        {
            Debug.LogError("Failed: " + e.Message);
        }
    }



    // Update is called once per frame
    void Update()
    {


    }
}