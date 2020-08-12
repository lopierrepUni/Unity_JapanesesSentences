using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseLvlSelection : MonoBehaviour
{
    public Button lvlButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = lvlButton.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnClick()
    {
        LeanTween.scale(transform.parent.gameObject.transform.parent.gameObject, new Vector3(0, 0, 1), 0.2f);        
    }
}
