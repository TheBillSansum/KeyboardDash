using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Contains functionality to spawn a black star that starts small and scales up to the whole screen, then ping pongs back to nothing
/// </summary>
public class TransitionManager : MonoBehaviour
{
    public Image starShape;
    public float transitionTime;
    public bool runTransition;
    public bool goingUp = false;
    public float maxScale = 500;
    public float speed = 100;
    public float scale = 1f;
    private bool hasCompletedCycle = false;

    /// <summary>
    /// Make sure all settings are reset to normal
    /// </summary>
    void Start()
    {
        starShape.gameObject.SetActive(false);
        starShape.transform.localScale = Vector3.zero;
        goingUp = true;
    }

    void Update()
    {
        if (!hasCompletedCycle) //Runs the transition
        {
            if (goingUp) //Depending on if getting bigger or smaller
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

    /// <summary>
    /// Starts running the black star transition 
    /// </summary>
    public void StartTransition()
    {
        runTransition = true;
        starShape.gameObject.SetActive(true);
        hasCompletedCycle = false;
    }
}
