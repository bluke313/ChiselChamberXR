using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Return : MonoBehaviour
{
    public GameObject xrControllerR;
    public GameObject xrControllerL;

    public GameObject objectToReturn1;
    public Transform returnLocation1;

    public GameObject objectToReturn2;
    public Transform returnLocation2;

    public GameObject objectToReturn3;
    public Transform returnLocation3;

    public GameObject objectToReturn4;
    public Transform returnLocation4;

    public GameObject objectToReturn5;
    public Transform returnLocation5;

    public GameObject objectToReturn6;
    public Transform returnLocation6;

    public void Interact()
    {

        Debug.Log("Tools returned!");

        xrControllerR.SetActive(false);
        xrControllerL.SetActive(false);

        // return the objects to the return locations
        objectToReturn1.transform.position = returnLocation1.position;
        objectToReturn1.transform.rotation = returnLocation1.rotation;
        objectToReturn1.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToReturn2.transform.position = returnLocation2.position;
        objectToReturn2.transform.rotation = returnLocation2.rotation;
        objectToReturn2.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToReturn3.transform.position = returnLocation3.position;
        objectToReturn3.transform.rotation = returnLocation3.rotation;
        objectToReturn3.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToReturn4.transform.position = returnLocation4.position;
        objectToReturn4.transform.rotation = returnLocation4.rotation;
        objectToReturn4.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToReturn5.transform.position = returnLocation5.position;
        objectToReturn5.transform.rotation = returnLocation5.rotation;
        objectToReturn5.GetComponent<Rigidbody>().velocity = Vector3.zero;

        objectToReturn6.transform.position = returnLocation6.position;
        objectToReturn6.transform.rotation = returnLocation6.rotation;
        objectToReturn6.GetComponent<Rigidbody>().velocity = Vector3.zero;


        xrControllerR.SetActive(true);
        xrControllerL.SetActive(true);

        objectToReturn1.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReturn2.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReturn3.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReturn4.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReturn5.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToReturn6.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
