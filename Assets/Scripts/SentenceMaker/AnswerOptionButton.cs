using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerOptionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button button;
    public Button wordSpace;


    public string word;
    public GameObject panel;

    private GameObject dropWordSpace;
    private GameObject answerColum;
    private BoxCollider2D box;
    private bool isSelected = false;
    private Vector2 originalPos; 
    private Vector2 origianlSize; 




    // Start is called before the first frame update
    void Start()
    {

        gameObject.GetComponentInChildren<Text>().text = word;
        box = gameObject.GetComponentInChildren<BoxCollider2D>();
        box.size = GetComponentInChildren<RectTransform>().sizeDelta;
        answerColum = transform.parent.gameObject;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("WordSpace"))
        {
            dropWordSpace = col.gameObject;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {


            if (isSelected)
            {

                //Disable Panel Layout to be able to move the answer button
                GameObject panel = GameObject.FindGameObjectWithTag("MainPanel");
                panel.GetComponent<HorizontalLayoutGroup>().enabled = false;

                touch = Input.GetTouch(0);
                Vector2 ray = Camera.main.ScreenToWorldPoint(touch.position);

                //Disable the column Layout so the answers buttons inside don´t move

                if (answerColum.GetComponent<VerticalLayoutGroup>() != null)
                {
                    answerColum.GetComponent<VerticalLayoutGroup>().enabled = false;
                }
                //Move the answer button
                transform.parent = panel.transform;
                transform.position = ray;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPos = transform.position;
        origianlSize = gameObject.GetComponent<RectTransform>().sizeDelta;
        isSelected = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dropWordSpace != null && box.IsTouching(dropWordSpace.GetComponent<BoxCollider2D>()))
        {
            transform.position = dropWordSpace.transform.position;
            gameObject.GetComponent<RectTransform>().sizeDelta = dropWordSpace.GetComponent<RectTransform>().sizeDelta;
        }
        else
        {
            string v = answerColum.name;
            transform.parent = answerColum.transform;
            transform.position = originalPos;
        }
        isSelected = false;
    }
    public void OnDrag(PointerEventData eventData) { }
}
