using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
/// <summary>
/// Utilised in the starting animation for the game, Camera 1 faces the monitor and Camera 2 is the main cam for the whole game, facing the keyboard
/// </summary>
public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCameraBase camera1;
    public CinemachineVirtualCameraBase camera2;
    public float transitionDuration = 1.0f; // Adjust the smoothness of the transition
    public GameObject userInterface;

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
    public void Start()
    {
        userInterface.SetActive(false);
    }

    IEnumerator SwitchCamera()
    {
        transitionInProgress = true; // Set transition in progress

        CinemachineBrain cinemachineBrain = FindObjectOfType<CinemachineBrain>(); // Get the Cinemachine Brain

        // Play the blend
        cinemachineBrain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        cinemachineBrain.m_DefaultBlend.m_Time = transitionDuration;
        
        camera1.Priority = 0; // Deactivate the first camera

        camera2.Priority = 10; // Activate the second camera

        yield return new WaitForSeconds(transitionDuration); // Wait for the duration of the transition

        // Reset transition status
        transitionInProgress = false;
        userInterface.SetActive(true);
    }
}