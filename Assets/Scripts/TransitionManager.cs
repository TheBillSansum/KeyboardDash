using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image starShape;
    public float transitionTime;
    public bool runTransition;
    public bool goingUp = false;
    public float maxScale = 500;
    public float speed = 50;
    public float scale = 1f;
    private bool hasCompletedCycle = false;

    void Start()
    {
        starShape.gameObject.SetActive(false);
        starShape.transform.localScale = Vector3.zero;
        goingUp = true;
    }

    void Update()
    {
        if (!hasCompletedCycle)
        {
            if (goingUp)
            {
                scale += speed * Time.deltaTime;
                if (scale >= maxScale)
                {
                    goingUp = false;
                }
            }
            else
            {
                scale -= speed * Time.deltaTime;
                if (scale <= 0)
                {
                    goingUp = true;
                    hasCompletedCycle = true;
                    starShape.gameObject.SetActive(false);
                }
            }

            starShape.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void StartTransition()
    {
        runTransition = true;
        starShape.gameObject.SetActive(true);
        hasCompletedCycle = false;
        
    }
}
