using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckButton : MonoBehaviour
{

    public Button button;
    public List<Button> wordsSpaces;


    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(check);
    }
    void check()
    {
        int points=0;
        #region Activate a verification function in WordSpaceButtons
        foreach (var item in wordsSpaces)
        {
            points= item.GetComponent<WordSpaceButton>().check()? points+10: points;
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
}
