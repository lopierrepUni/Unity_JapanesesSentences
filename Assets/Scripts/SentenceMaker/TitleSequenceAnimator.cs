using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSequenceAnimator : MonoBehaviour
{
    List<Animator> _animators = new List<Animator>();
    public float waitBetweeb;
    public float waitEnd;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            foreach (Transform child in transform)
            {             
                _animators.AddRange(child.GetComponentsInChildren<Animator>());
            }
            StartCoroutine(DoAnimation());

        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }

    }

    IEnumerator DoAnimation()
    {
        while (true)
        {
            foreach (var animator in _animators)
            {
                animator.SetTrigger("DoAnimation");
                yield return new WaitForSeconds(waitBetweeb);
            }
            yield return new WaitForSeconds(waitEnd);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
