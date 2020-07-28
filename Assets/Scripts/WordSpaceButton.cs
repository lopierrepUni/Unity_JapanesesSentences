using Assets.Scrips.Classes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WordSpaceButton : MonoBehaviour
{
    public Button button;
    public GameObject AnswersPanel;
    public Button AnswerOptionButton;


    public List<string> words;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponentInChildren<Text>().text = words.ElementAt(0);
        button.onClick.AddListener(DisplayAnswersPanel);
    }

    void DisplayAnswersPanel()
    {
        Canvas canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        GameObject panel = Instantiate(AnswersPanel, new Vector3(654.4f, 180.8f, -1), Quaternion.identity);
        panel.transform.parent = canvas.transform;
        List<Button> answerOptionButtons = new List<Button>();
        float x = 0, y = 0.7f, height, separation;
        switch (words.Count())
        {
            case 3:
                height = 0.3f;
                separation = 0.15f;
                break;
            case 4:
                height = 0.25f;
                separation = 0.1f;
                break;

            default:
                height = 0.2f;
                separation = 0.083f;
                break;
        }

        y = y - separation - height / 2;
        foreach (var word in words)
        {
        
            Button answerOptionButton= Instantiate(AnswerOptionButton);
            answerOptionButton.transform.parent = panel.transform;
            answerOptionButton.transform.localPosition=new Vector3(0, y, -1);
            answerOptionButton.transform.hei
            y = y - separation - height;            
            answerOptionButtons.Add(answerOptionButton);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
