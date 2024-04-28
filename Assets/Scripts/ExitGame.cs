using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/**
Exit the app
*/
public class ExitGame : MonoBehaviour
{
    private void Interact()
    {
        //exit
        Debug.Log("yo wtf");
        Application.Quit();
        //exit if running in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}