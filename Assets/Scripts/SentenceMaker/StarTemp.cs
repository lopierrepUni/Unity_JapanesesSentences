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

public class StarTemp : MonoBehaviour
{


    public Button WordSpace;
    public GameObject SentenceLine;
    public GameObject SentencesPanel;

    public Button AnswerOption;
    public GameObject AnswersColum;
    public GameObject AnswersColumsPanel;

    public GameObject panel;

    private Sprite sprite;
    private SqliteCommand command;
    private SqliteConnection connection;
    private ImageSentence imageSentence;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //TODO ESTO VA A IR EN EL BOTON DE START EN EL MENU PRINCIPAL
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
            System.Random r = new System.Random();
            int id = r.Next(1, numSenrtence);
            Debug.Log($@"RANDOM NUM = {id}");
            sqlQuery = $@"SELECT * FROM Image_Sentence WHERE Id={id}";
            command.CommandText = sqlQuery;
            reader = command.ExecuteReader();
            List<ImageSentence> imageSentenceList = new List<ImageSentence>();
            while (reader.Read())
            {
                imageSentence = new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                break;
            }
            reader.Close();
            #endregion

            #endregion

            #region Display image and wordSpaces
            sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentence.Image_Id);
            gameObject.GetComponent<Image>().sprite = sprite;

            string sentence = imageSentence.Sentence;
            List<string> words = sentence.Split('*').ToList();
       
            ImageSenteceFunc.GeneretaButtons(command, words, WordSpace, SentenceLine, SentencesPanel, AnswerOption, AnswersColum, AnswersColumsPanel);
            

            #endregion
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