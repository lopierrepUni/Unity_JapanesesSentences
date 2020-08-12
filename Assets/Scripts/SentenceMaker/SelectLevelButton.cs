using Assets.Scrips.Utilities;
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevelButton : MonoBehaviour
{
    public Button lvlButton;
    public Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        try
        {


            Button btn = lvlButton.GetComponent<Button>();
            btn.onClick.AddListener(OnClick);

            int lvl = int.Parse(GetComponentInChildren<Text>().text);
            DataBaseManager.CreateAccessibleDB("MainDataBase.s3db");
            SqliteConnection connection = DataBaseManager.CreateConection("MainDataBase.s3db");
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            string sqlQuery = $"SELECT Progress FROM Progress_Lvl WHERE Level= {lvl}";
            command.CommandText = sqlQuery;
            IDataReader reader = command.ExecuteReader();
            float progress = 0;
            while (reader.Read())
            {
                progress = reader.GetInt32(0);
                break;
            }
            reader.Close();
            connection.Close();//tal vez esto genere problemas
            progressBar.value = progress;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        int lvl = int.Parse(GetComponentInChildren<Text>().text);
        QuestionImage.SetLvl(lvl);
        SceneManager.LoadScene(1);
    }
}
