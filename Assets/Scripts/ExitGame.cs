using UnityEngine;
/**
Exit the app
*/
public class ExitGameOnClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        //exit
        Application.Quit();
        //exit if running in editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}