using Assets.Scrips.Classes;
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

public class BackgroundImage : MonoBehaviour
{
    public GameObject wordSpace;

    private List<GameObject> wordSpaces = new List<GameObject>();
    private Sprite sprite;
    private SqliteCommand command;
    private SqliteConnection connection;
    private ImageSentence imageSentence;
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
            List<ImageSentence> imageSentenceList = new List<ImageSentence>();
            while (reader.Read())
            {
                imageSentence = new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
                break;
            }
            sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentence.Image_Id);
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

            string sentence = imageSentence.Sentence;
            List<string> words = sentence.Split('*').ToList();
            wordSpaces = Functions.GeneretaWordsSpaces(words, wordSpace, wordSpaces);
            connection.Close();
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