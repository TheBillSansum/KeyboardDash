using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DelayedStay : MonoBehaviour
{

    public float requiredTime;
    public float currentTime;
    public Material gradient;
    public Image fillImage;
    public TextMeshProUGUI fillPercentage;
    public bool runOnce = false;

    public FinishCriteria finishCriteria;

    private void Start()
    {
        runOnce = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            currentTime += Time.deltaTime;
        }
    }


    public void Update()
    {
        UpdateGradient();
    }

    private void UpdateGradient()
    {
        float ratio = Mathf.Clamp01(currentTime / requiredTime);
        fillImage.fillAmount = Mathf.Clamp01(currentTime / requiredTime);
        fillPercentage.text = (ratio * 100).ToString("0") + "%";

        Color color = new Color(1f, 0f, 0f, 1f); 
        color = Color.Lerp(color, new Color(0f, 1f, 0f, 0.5f), ratio); 

        gradient.color = color;

        if(ratio >= 1)
        {
            finishCriteria.LevelPassed();
        }
    }
}
