using Assets.Scrips.Classes;
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
    public GameObject wordSpace;
    public GameObject BackgroudImage;
    public Button button;

    private List<GameObject> wordSpaces = new List<GameObject>();
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
            connection = Functions.RunDbCode("MainDataBase.s3db");
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
            connection.Close();
            button.onClick.AddListener(NextQuestion);//adds a listener for when you click the button           
        }
        catch (Exception e)
        {
            Debug.LogError("Failed: " + e.Message);
        }

    }

    void NextQuestion()
    {
        try
        {
            if (questionNumer < imageSentenceList.Count())
            {
                sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentenceList.ElementAt(questionNumer).Image_Id);
                BackgroudImage.GetComponent<SpriteRenderer>().sprite = sprite;
                string sentence = imageSentenceList.ElementAt(questionNumer).Sentence;
                List<string> words = sentence.Split('*').ToList();
                wordSpaces = Functions.GeneretaWordsSpaces(words, wordSpace, wordSpaces);
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
