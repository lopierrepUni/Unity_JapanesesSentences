using Assets.Scrips.Classes;
using Assets.Scrips.Utilities;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scrips.Functions
{

    public class ImageSenteceFunc : MonoBehaviour
    {
        private static List<Button> wordSpaces = new List<Button>();


        public static List<Button> GeneretaWordsSpaceButtons(List<string> words, Button wordSpace, SqliteCommand command, int NumOfAnswerOptions, Canvas canvas, Button checkButton)
        {
            try
            {
                #region Clean old wordspaces
                foreach (var wordspace in wordSpaces)
                {
                    DestroyImmediate(wordspace.gameObject);
                }
                wordSpaces.Clear();
                #endregion

                float y = words.Count <= 500 ? -400 : -300;
                //First line
                if (words.Count >= 3)
                {
                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(-370f, y, -1), Quaternion.identity));
                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(0, y, -1), Quaternion.identity));
                    wordSpaces.Add(Instantiate(wordSpace, new Vector3(370f, y, -1), Quaternion.identity));
                    if (words.Count >= 4)
                    {
                        wordSpaces.Insert(0, Instantiate(wordSpace, new Vector3(-740f, y, -1), Quaternion.identity));
                        if (words.Count >= 5)
                        {
                            wordSpaces.Add(Instantiate(wordSpace, new Vector3(740f, y, -1), Quaternion.identity));
                            y = y - 1.5f;                           
                            #region Second Line 
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
                            #endregion
                        }
                    }
                }
                #region Set parent, name and group of words
                for (int i = 0; i < wordSpaces.Count; i++)
                {
                    wordSpaces.ElementAt(i).name = "WordSpace" + i;
                    wordSpaces.ElementAt(i).transform.parent = canvas.transform;
                    wordSpaces.ElementAt(i).GetComponent<WordSpaceButton>().words = GenereteWordsGroup(words.ElementAt(i), command, NumOfAnswerOptions);
                    wordSpaces.ElementAt(i).GetComponent<WordSpaceButton>().correctAnswer = words.ElementAt(i);

                }
                #endregion
                checkButton.GetComponent<CheckButton>().wordsSpaces = wordSpaces;
                return wordSpaces;
            }
            catch (Exception e)
            {

                Debug.LogError("Failed: " + e.Message);
                return null;
            }
        }

        public static List<string> GenereteWordsGroup(string stringWord, SqliteCommand command, int NumOfAnswerOptions)
        {
            try
            {
                #region Get word data                
                string sqlQuery = $@"SELECT * FROM Words WHERE Word='{stringWord}'";
                command.CommandText = sqlQuery;
                IDataReader reader = command.ExecuteReader();
                Word word = null;
                while (reader.Read())
                {

                    word = new Word(reader.GetInt32(0), reader.GetString(1), (WordCategory)reader.GetInt32(2), reader.GetInt32(3));
                }
                reader.Close();
                #endregion 

                #region Add a NumOfAnswerOptions 
                sqlQuery = $@"SELECT Word FROM Words WHERE Id IN (SELECT Id FROM Words where Category={(int)word.Category} and Level<={word.Level} and Word<>'{stringWord}' ORDER BY RANDOM() LIMIT {NumOfAnswerOptions - 1})";
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();
                List<string> wordsGroup = new List<string>();
                while (reader.Read())
                {
                    wordsGroup.Add(reader.GetString(0));
                }
                wordsGroup.Add(stringWord);
                #endregion

                #region Shuffle 
                var rng = new System.Random();
                wordsGroup=wordsGroup.OrderBy(a => rng.Next()).ToList();
                reader.Close();
                #endregion

                return wordsGroup;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed: " + e.Message);
                return null;
            }
        }
    }



}
