using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnswerOptionButton : MonoBehaviour
{
    public Button button;
    public Button wordSpace;


    public string word;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
         gameObject.GetComponentInChildren<Text>().text = word; 
         
         button.onClick.AddListener(DisplayAnswersPanel);
    }

    void DisplayAnswersPanel()
    {
        wordSpace.GetComponentInChildren<Text>().text = word;
        Destroy(panel);
    }

    // Update is called once per frame
    void Update()
    {
      

    }

}
