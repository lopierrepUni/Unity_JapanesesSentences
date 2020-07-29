using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOptionButton : MonoBehaviour
{
    public Button button;
    public Button wordSpace;


    public List<string> words;
    public string word;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
         //gameObject.GetComponentInChildren<Text>().text = "asdasdasdasd"; 
         gameObject.GetComponentInChildren<Text>().text = words.ElementAt(0); //paso la palabra como una lista de un solo string porque si paso solo la palabra como una variable tipo string no funciona .-.
         button.onClick.AddListener(DisplayAnswersPanel);
    }

    void DisplayAnswersPanel()
    {
        wordSpace.GetComponentInChildren<Text>().text = words.ElementAt(0);
        Destroy(panel);
    }

    // Update is called once per frame
    void Update()
    {
      

    }

}
