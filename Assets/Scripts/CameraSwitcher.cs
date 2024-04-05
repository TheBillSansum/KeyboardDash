using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCameraBase camera1;
    public CinemachineVirtualCameraBase camera2;
    public float transitionDuration = 1.0f; // Adjust the smoothness of the transition

    private bool transitionInProgress = false;

    void Update()
    {
        // Check if the transition is not in progress and any key is pressed
        if (!transitionInProgress && Input.anyKeyDown)
        {
            // Start the camera transition
            StartCoroutine(SwitchCamera());
        }
    }

    IEnumerator SwitchCamera()
    {
        // Set transition in progress
        transitionInProgress = true;

        // Get the Cinemachine Brain
        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>();

        // Play the blend
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        cinemachineBrain.m_DefaultBlend.m_Time = transitionDuration;

        // Deactivate the first camera
        camera1.Priority = 0;

        // Activate the second camera
        camera2.Priority = 10;

        // Wait for the duration of the transition
        yield return new WaitForSeconds(transitionDuration);

        // Reset transition status
        transitionInProgress = false;
    }
}