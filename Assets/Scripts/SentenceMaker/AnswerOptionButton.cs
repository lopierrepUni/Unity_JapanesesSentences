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

    private bool isSelected = false;
    private Rigidbody2D rb;
    private Vector3 direction;
    private float moveSpeed = 3f;

    Camera mainCamera;
    float zAxis = 0;
    Vector3 clickOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Text>().text = word;
        BoxCollider2D box = gameObject.GetComponentInChildren<BoxCollider2D>();
        box.size = GetComponentInChildren<RectTransform>().sizeDelta;
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        isSelected = true;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isSelected = false;
    }
    public void OnDrag(PointerEventData eventData)
    {

    }

    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {
            if (isSelected)
            {
                GameObject panel = GameObject.FindGameObjectWithTag("MainPanel");
                panel.GetComponent<HorizontalLayoutGroup>().enabled = false;
                touch = Input.GetTouch(0);
                Vector2 ray = Camera.main.ScreenToWorldPoint(touch.position);
                transform.parent = panel.transform;
                transform.position = ray;
            }


        }


    }

}
