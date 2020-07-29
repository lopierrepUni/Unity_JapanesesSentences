using Assets.Scrips.Classes;
using Assets.Scrips.Functions;
using Assets.Scrips.Utilities;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImage : MonoBehaviour
{
    public Button wordSpace;
    public Button checkButton;

    public Canvas canvas;

  
    private Sprite sprite;
    private SqliteCommand command;
    private SqliteConnection connection;
    private ImageSentence imageSentence;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            #region Connect to database and obtain initial data
            DataBaseManager.CreateAccessibleDB("MainDataBase.s3db");
            connection = DataBaseManager.CreateConection("MainDataBase.s3db");
            connection.Open();
            command = connection.CreateCommand();
            string sqlQuery = "SELECT * FROM Image_Sentence";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            List<ImageSentence> imageSentenceList = new List<ImageSentence>();
            while (reader.Read())
            {
                imageSentence = new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                break;
            }
            reader.Close();
            #endregion
            sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentence.Image_Id);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

            string sentence = imageSentence.Sentence;
            List<string> words = sentence.Split('*').ToList();
            ImageSenteceFunc.GeneretaWordsSpaceButtons(words, wordSpace, command, 4, canvas, checkButton);
            
            connection.Close();
            connection.Dispose();
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