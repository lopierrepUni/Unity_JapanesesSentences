using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

public class AnswerOptionButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Button button;
    public Button wordSpace;
    public string word;

    private GameObject panel;
    private GameObject dropWordSpace;
    private GameObject currentWordSpace;
    private GameObject answerColum;
    private BoxCollider2D box;
    private Vector2 originalPos;
    private Vector2 originalSize;
    private Sprite originalSprite;
    private Vector2 posInWS;


    private bool isOnWordSpace = false;
    private bool isOnColumnAnswer = true;
    private bool isSelected = false;
    private bool LayoutsAreDisable = false;





    // Start is called before the first frame update
    void Start()
    {
        panel = GameObject.FindGameObjectWithTag("MainPanel");

        GetComponentInChildren<Text>().text = word;
        box = GetComponent<BoxCollider2D>();
        box.size = GetComponentInChildren<RectTransform>().sizeDelta / 2;
        answerColum = transform.parent.gameObject;
        isOnColumnAnswer = true;
    }


    // Update is called once per frame
    void Update()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {
            if (isSelected)
            {
                #region Disable layouts
                if (!LayoutsAreDisable)
                {
                    LayoutsAreDisable = true;
                    #region  Disable Panel Layout to be able to move the answer button
                    GameObject panel = GameObject.FindGameObjectWithTag("MainPanel");
                    panel.GetComponent<HorizontalLayoutGroup>().enabled = false;
                    #endregion
                    #region Disable the column Layout so the answers buttons inside don´t move
                    GameObject[] answColums = GameObject.FindGameObjectsWithTag("AnswersColumn");
                    foreach (var item in answColums)
                    {
                        if (item.GetComponent<VerticalLayoutGroup>() != null)
                        {
                            item.GetComponent<VerticalLayoutGroup>().enabled = false;
                        }
                    }

                    #endregion
                }
                #endregion

                #region Move the answer button with touch
                touch = Input.GetTouch(0);
                Vector2 ray = Camera.main.ScreenToWorldPoint(touch.position);
                //transform.parent = panel.transform;
                transform.position = ray;
                #endregion
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("WordSpace"))
        {
            dropWordSpace = col.gameObject;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(panel.transform);
        if (isOnColumnAnswer)
        {
            originalPos = transform.position;
            originalSize = gameObject.GetComponent<RectTransform>().sizeDelta;
            originalSprite = gameObject.GetComponent<Image>().sprite;
        }
        if (isOnWordSpace)
        {
            gameObject.GetComponent<Image>().sprite = originalSprite;
            gameObject.GetComponent<RectTransform>().sizeDelta = originalSize;
            posInWS = transform.localPosition;
        }
        isSelected = true;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            #region EndDrag on wordSpace
            if (dropWordSpace != null && box.IsTouching(dropWordSpace.GetComponent<BoxCollider2D>()))
            {
                #region Check if is the ws is filled with and asnwopt and swap or replace it
                WordSpaceButton spaceWord = dropWordSpace.GetComponent<WordSpaceButton>();
                if (spaceWord.GetIsFill())
                {
                    #region Replace answopt
                    if (isOnColumnAnswer)
                    {
                        spaceWord.ReturnAnswOpt();
                    }
                    #endregion
                    #region Swap asnwopt
                    if (isOnWordSpace)
                    {
                        spaceWord.MoveAnswOpt(currentWordSpace);
                    }
                    #endregion
                }
                #endregion

                #region Put answopt in ws
                transform.SetParent(dropWordSpace.transform, false);
                transform.position = dropWordSpace.transform.position;
                GetComponent<RectTransform>().sizeDelta = dropWordSpace.GetComponent<RectTransform>().sizeDelta;
                dropWordSpace.GetComponent<WordSpaceButton>().SetIsFill(true);
                currentWordSpace = dropWordSpace;
                #endregion

                isOnWordSpace = true;
                isOnColumnAnswer = false;
                gameObject.GetComponent<Image>().sprite = spaceWord.GetComponent<Image>().sprite;

            }
            else
            {


                #endregion

                #region EndDrag on columnAnswer
                var answColumns = GameObject.FindGameObjectsWithTag("AnswersColumn");
                bool isReturning = false;
                foreach (var item in answColumns)
                {
                    if (box.IsTouching(item.GetComponent<BoxCollider2D>()))
                    {
                        isReturning = true;
                    }
                }
                if (isReturning)
                {
                    StartCoroutine(ReturnOrigianlPos());
                }

                #endregion
                #region EndDrag on no wordSpace nor columnAnswer
                else
                {
                    if (isOnWordSpace)
                    {
                        // transform.position = transform.parent.position;
                        StartCoroutine(ReturnToCurrentWordSpace());


                    }
                    else if (isOnColumnAnswer)
                    {
                        StartCoroutine(ReturnOrigianlPos());
                    }
                }
                #endregion
            }
            isSelected = false;

        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }

    }
    //func para que al sacar del wordSpace y poner en la columna la answer option retorne a su pos origianl en la columna de respeustas
    public IEnumerator ReturnOrigianlPos()
    {
        GameObject panel = GameObject.FindGameObjectWithTag("MainPanel");
        transform.SetParent(panel.transform);
        gameObject.GetComponent<Image>().sprite = originalSprite;
        GetComponent<RectTransform>().sizeDelta = originalSize;
        LeanTween.move(gameObject, originalPos, 0.2f);
        currentWordSpace = null;
        isOnColumnAnswer = true;
        isOnWordSpace = false;
        yield return new WaitForSeconds(0.2f);
        transform.SetParent(answerColum.transform, false);
        transform.position = originalPos;
        //transform.position = originalPos;    
    }
    public IEnumerator ReturnToCurrentWordSpace()
    {
        WordSpaceButton spaceWord = dropWordSpace.GetComponent<WordSpaceButton>();
        transform.SetParent(panel.transform, false);
        LeanTween.move(gameObject, currentWordSpace.transform.position, 0.2f);
        yield return new WaitForSeconds(0.2f);
        GetComponent<RectTransform>().sizeDelta = dropWordSpace.GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<Image>().sprite = spaceWord.GetComponent<Image>().sprite;
        transform.SetParent(currentWordSpace.transform, false);
        transform.position = posInWS;

    }
    public IEnumerator SwapPos(GameObject newWordSpace)
    {
        transform.SetParent(panel.transform);
        gameObject.GetComponent<Image>().sprite = originalSprite;
        gameObject.GetComponent<RectTransform>().sizeDelta = originalSize;
        LeanTween.move(gameObject, newWordSpace.transform.position, 0.1f);

        yield return new WaitForSeconds(0.2f);

        GetComponent<RectTransform>().sizeDelta = dropWordSpace.GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<Image>().sprite = newWordSpace.GetComponent<Image>().sprite;
        transform.SetParent(newWordSpace.transform, false);
        transform.position = newWordSpace.transform.position;
        currentWordSpace = newWordSpace;
    }
    public void Hide()
    {
        GetComponentInChildren<Text>().text = "";
        GetComponentInChildren<Image>().enabled = false;
    }


    public void OnDrag(PointerEventData eventData) { }
}
