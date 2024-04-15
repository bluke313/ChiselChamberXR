using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Return : MonoBehaviour
{
    public GameObject xrControllerR;
    public GameObject xrControllerL;

    public Transform objectToReturn1;
    public Transform returnLocation1;

    public Transform objectToReturn2;
    public Transform returnLocation2;

    public Transform objectToReturn3;
    public Transform returnLocation3;

    public Transform objectToReturn4;
    public Transform returnLocation4;

    public void Interact()
    {

        Debug.Log("Tools returned!");

        xrControllerR.SetActive(false);
        xrControllerL.SetActive(false);

        // return the objects to the return locations
        objectToReturn1.position = returnLocation1.position;
        objectToReturn1.rotation = returnLocation1.rotation;

        objectToReturn2.position = returnLocation2.position;
        objectToReturn2.rotation = returnLocation2.rotation;

        objectToReturn3.position = returnLocation3.position;
        objectToReturn3.rotation = returnLocation3.rotation;

        objectToReturn4.position = returnLocation4.position;
        objectToReturn4.rotation = returnLocation4.rotation;


        xrControllerR.SetActive(true);
        xrControllerL.SetActive(true);
    }
}
