using Assets.Scrips.Clases;
using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

public class BackgroundImage : MonoBehaviour
{

    private Sprite sprite;
    SqliteCommand command;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            RunDbCode("MainDataBase.s3db");
            string sqlQuery = "SELECT * FROM Image_Sentence";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            List<ImageSentence> list = new List<ImageSentence>();
            while (reader.Read())
            {
                list.Add(new ImageSentence(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));

            }

            sprite = Resources.Load<Sprite>("Images/" + list.ElementAt(0).Image_Id);
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
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