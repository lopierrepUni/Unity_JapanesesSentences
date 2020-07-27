using Assets.Scrips.Classes;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scrips.Utilities
{

    public class Functions : MonoBehaviour
    {
        public static SqliteConnection RunDbCode(string fileName)
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


                    Debug.Log("Success!");
                    return connection;
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed: " + e.Message);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed: " + e.Message);
                return null;
            }
        }

        private static List<GameObject> WordSpaces = new List<GameObject>();
        public static List<GameObject> GeneretaWordsSpaces(List<string> words, GameObject wordSpace, List<GameObject> wordSpaces)
        {
            foreach (var wordspace in WordSpaces)
            {
                Destroy(wordspace);
            }
            WordSpaces = wordSpaces;


            float y = words.Count <= 500 ? -400 : -300;
            //First line
            if (words.Count >= 3)
            {
                wordSpaces.Add(Instantiate(wordSpace, new Vector3(0, y, -1), Quaternion.identity));
                wordSpaces.Add(Instantiate(wordSpace, new Vector3(-370f, y, -1), Quaternion.identity));
                wordSpaces.Add(Instantiate(wordSpace, new Vector3(370f, y, -1), Quaternion.identity));
                if (words.Count >= 4)
                {
                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(-740f, y, -1), Quaternion.identity));
                    if (words.Count >= 5)
                    {
                        wordSpaces.Add(Instantiate(wordSpace, new Vector3(740f, y, -1), Quaternion.identity));

                        y = y - 1.5f;
                        // Second line
                        if (words.Count >= 6)
                        {
                            wordSpaces.Add(Instantiate(wordSpace, new Vector3(0, y, -1), Quaternion.identity));
                            if (words.Count >= 7)
                            {
                                wordSpaces.Add(Instantiate(wordSpace, new Vector3(-370f, y, -1), Quaternion.identity));
                                if (words.Count >= 8)
                                {
                                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(370f, y, -1), Quaternion.identity));
                                }
                                if (words.Count >= 9)
                                {
                                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(-740f, y, -1), Quaternion.identity));
                                }
                                if (words.Count >= 10)
                                {
                                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(740f, y, -1), Quaternion.identity));
                                }
                            }
                        }
                    }
                }
            }
            return wordSpaces;
        }

        public static List<Word> GenereteWordsGroup(Word word, SqliteConnection connection)
        {
            List<Word> wordsGroup = new List<Word>();

            return wordsGroup;
        }
    }



}
