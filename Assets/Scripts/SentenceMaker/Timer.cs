using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject time;
    private float timer = 0;
    private string niceTime;
    private bool _run = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_run)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            gameObject.GetComponent<Text>().text = niceTime;
        }
    }
    public string GetTimeValue()
    {
        return niceTime;
    }
    public void SetRun(bool run)
    {
        _run = run;
    }



}
