using Assets.Scrips.Clases;
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


    private Sprite sprite;
    private SqliteCommand command;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            RunDbCode("MainDataBase.s3db");
            string sqlQuery = "SELECT * FROM Image_Sentence";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            List<ImageSentence> imageSentenceList = new List<ImageSentence>();
            while (reader.Read())
            {
                imageSentenceList.Add(new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
            }

            sprite = Resources.Load<Sprite>("SentencesImages/" + imageSentenceList.ElementAt(1).Image_Id);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;

            string sentence = imageSentenceList.ElementAt(1).Sentence;
            List<string> words = sentence.Split('*').ToList();

            #region Generate the spaces for the words

            //int sign = Application.platform == RuntimePlatform.Android ? -1 : 1;
            float y = words.Count <= 5 ? -4 : -3f;


            //First line
            if (words.Count >= 3)
            {
                Instantiate(wordSpace, new Vector3(0, y, -1), Quaternion.identity);
                Instantiate(wordSpace, new Vector3(-3.7f, y, -1), Quaternion.identity);
                Instantiate(wordSpace, new Vector3(3.7f, y, -1), Quaternion.identity);
                if (words.Count >= 4)
                {
                    Instantiate(wordSpace, new Vector3(-7.4f, y, -1), Quaternion.identity);
                    if (words.Count >= 5)
                    {
                        Instantiate(wordSpace, new Vector3(7.4f, y, -1), Quaternion.identity);

                        y = y - 1.5f;
                        // Second line
                        if (words.Count >= 6)
                        {
                            Instantiate(wordSpace, new Vector3(0, y, -1), Quaternion.identity);
                            if (words.Count >= 7)
                            {
                                Instantiate(wordSpace, new Vector3(-3.7f, y, -1), Quaternion.identity);
                                if (words.Count >= 8)
                                {
                                    Instantiate(wordSpace, new Vector3(3.7f, y, -1), Quaternion.identity);
                                }
                                if (words.Count >= 9)
                                {
                                    Instantiate(wordSpace, new Vector3(-7.4f, y, -1), Quaternion.identity);
                                }
                                if (words.Count >= 10)
                                {
                                    Instantiate(wordSpace, new Vector3(7.4f, y, -1), Quaternion.identity);
                                }
                            }
                        }
                    }
                }


            }
            #endregion





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
    void RunDbCode(string fileName)
    {
        try
        {

            //Where to copy the db to
            string dbDestination = Path.Combine(Application.persistentDataPath, "data");
            dbDestination = Path.Combine(dbDestination, fileName);
            /*var deleteDir = dbDestination.Replace("\\", "/");
            try
            {
                File.Delete(deleteDir);
            }
            catch (Exception e)
            {

            }*/

            //Where the db file is at
            string dbStreamingAsset = Path.Combine(Application.streamingAssetsPath, fileName);

            byte[] result;

            //Read the File from streamingAssets. Use WWW for Android
            if (dbStreamingAsset.Contains("://") || dbStreamingAsset.Contains(":///"))
            {
                WWW www = new WWW(dbStreamingAsset);

                result = www.bytes;
            }
            else
            {
                result = File.ReadAllBytes(dbStreamingAsset);
            }
            Debug.Log("Loaded db file");



            //Create Directory if it does not exist
            if (!Directory.Exists(Path.GetDirectoryName(dbDestination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(dbDestination));
            }

            //Copy the data to the persistentDataPath where the database API can freely access the file
            try

            {
                File.WriteAllBytes(dbDestination, result);
                Debug.Log("Copied db file");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to rewrite DataBase: " + e.Message);
            }

            try
            {
                //Tell the db final location for debugging
                Debug.Log("DB Path: " + dbDestination.Replace("/", "\\"));
                //Add "URI=file:" to the front of the url beore using it with the Sqlite API
                dbDestination = "URI=file:" + dbDestination;

                //Now you can do the database operation below
                //open db connection
                var connection = new SqliteConnection(dbDestination);
                connection.Open();

                command = connection.CreateCommand();
                Debug.Log("Success!");
            }
            catch (Exception e)
            {
                Debug.LogError("Failed: " + e.Message);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed: " + e.Message);
            throw;
        }
    }
}