using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Exit : MonoBehaviour
{ 

    public void LeaveGame()
    {
        Debug.Log("Exit");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_ANDROID
                UnityEngine.XR.XRSettings.enabled = false;
        #else
            Application.Quit();
        #endif

    }
}
