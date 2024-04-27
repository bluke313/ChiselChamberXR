using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersInvisible : MonoBehaviour
{

void Start() {
    //make collider invisible
    MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }
}
}
