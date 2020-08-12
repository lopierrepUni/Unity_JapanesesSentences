using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button startButton;
    public GameObject Lvls;
    public static int lvl;
    // Start is called before the first frame update
    void Start()
    {
        lvl = 1;
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        LeanTween.scale(Lvls, new Vector3(1, 1, 1), 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
