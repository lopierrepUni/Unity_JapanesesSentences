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
        private static List<GameObject> SentenceLines = new List<GameObject>();
        private static List<GameObject> AnswerColums = new List<GameObject>();


        public static void GeneretaButtons(SqliteCommand command, List<string> words, Button wordSpace, GameObject SentenceLine, GameObject SentencesPanel,
                                                                                              Button AnswerOption, GameObject AnswersColum, GameObject AnswersColumsPanel)
        {
            try
            {
                GenerateLines(words, SentencesPanel, SentenceLine, wordSpace, SentenceLines, false);
                List<string> answerOptions = GenereteWordsGroup(words, command);
                GenerateLines(answerOptions, AnswersColumsPanel, AnswersColum, AnswerOption, AnswerColums, true);
            }
            catch (Exception e)
            {

                Debug.LogError("Failed: " + e.Message);

            }
        }
        public static void GenerateLines(List<string> words, GameObject bigPanel, GameObject smallPanel, Button button, List<GameObject> oldSmallsPanels, bool answerOptions)
        {
            #region Clean old wordspaces
            foreach (var oldSmallsPanel in oldSmallsPanels)
            {
                DestroyImmediate(oldSmallsPanel.gameObject);
            }
            oldSmallsPanels.Clear();
            #endregion

            #region Generate panels
            int buttonsPerSmallPanel = answerOptions ? 10 : 5;
            int numLines = (int)Math.Ceiling(((double)words.Count) / buttonsPerSmallPanel);

            int wordNumber = 0;
            for (int i = 0; i < numLines; i++)
            {
                GameObject line = Instantiate(smallPanel);
                line.transform.parent = bigPanel.transform;
                SentenceLines.Add(line);
                #region Generate buttons
                int num = 1;
                for (int j = wordNumber; j < wordNumber + buttonsPerSmallPanel; j++)
                {
                    Button b = Instantiate(button);
                    b.transform.parent = line.transform;
                    if (answerOptions)
                    {
                        b.GetComponent<AnswerOptionButton>().word = words.ElementAt(j);
                        b.name = "AnswerOption" + num;
                    }
                    else
                    {
                        b.GetComponent<WordSpaceButton>().correctAnswer = words.ElementAt(j);
                        b.name = "WordSpace" + num;
                    }

                    num++;
                }
                wordNumber = wordNumber + buttonsPerSmallPanel;
                #endregion
            }
            #endregion
        }
        public static List<string> GenereteWordsGroup(List<string> words, SqliteCommand command)
        {
            try
            {
                #region Generate the list to Sqlite
                string list = "";
                foreach (var word in words)
                {
                    list = list + $@", '{word}'";
                }
                list = list.Substring(2, list.Length - 2);
                #endregion

                #region Get words data                
                string sqlQuery = $@"SELECT * FROM Words WHERE Word IN ({list})";
                command.CommandText = sqlQuery;
                IDataReader reader = command.ExecuteReader();
                List<Word> Words = new List<Word>();
                while (reader.Read())
                {
                    Words.Add(new Word(reader.GetInt32(0), reader.GetString(1), (WordCategory)reader.GetInt32(2), reader.GetInt32(3)));
                }
                reader.Close();
                #endregion

                #region Generate list of words that are not particles
                list = "";
                int maxLvl = 0;
                foreach (var word in Words)
                {
                    if (word.Category != WordCategory.Particle)
                    {
                        list = list + $@", '{word.stringword}'";
                    }
                    maxLvl = word.Level > maxLvl ? word.Level : maxLvl;
                }
                list = list.Substring(2, list.Length - 2);
                #endregion


                #region Select a random group of words differents than the originals words that are not partciles


                sqlQuery = $@"SELECT Word FROM Words WHERE Id IN (SELECT Id FROM (select * from Words Except select * from Words where Word IN ({list})) where  Level<={maxLvl}  ORDER BY RANDOM() LIMIT 5);";
                command.CommandText = sqlQuery;
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    words.Add(reader.GetString(0));
                }
                #endregion

                #region Shuffle 
                var rng = new System.Random();
                words = words.OrderBy(a => rng.Next()).ToList();
                reader.Close();
                #endregion

                return words;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed: " + e.Message);
                return null;
            }
        }
    }



}
