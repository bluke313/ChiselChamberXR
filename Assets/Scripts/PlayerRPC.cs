using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class PlayerRPC : NetworkBehaviour
{
    [SerializeField] Transform cameraTransform;

    void Start()
    {
        //MUST CHANGE PATH TO WHATEVER CAMERA OBJECT THE PLAYER OBJECT SHOULD MOVE WITH
        //cameraTransform = GameObject.Find("TestRig/Camera Offset/Main Camera").transform;
        cameraTransform = GameObject.Find("XR Origin (XR Rig)/Camera Offset/Main Camera").transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Failed to find the camera. Check the name and path in player script.");
        }
    }

    private void Update()
    {
        if (IsLocalPlayer)
        {
            //move player object with the camera's position
            Vector3 newPosition = cameraTransform.position;
            Quaternion newRotation = cameraTransform.rotation;

            MovePlayerServerRpc(newPosition, newRotation);
            //Debug.Log($"Local Position Updated: {newPosition}, rotation: {newRotation}");
        }
    }
    //update transform
    [ServerRpc(RequireOwnership = false)]
    void MovePlayerServerRpc(Vector3 newPosition, Quaternion newRotation)
    {
        //Debug.Log($"Server received new position: {newPosition} and rotation: {newRotation}");
        UpdatePositionClientRpc(newPosition, newRotation);
    }
    //update client transform
    [ClientRpc]
    void UpdatePositionClientRpc(Vector3 newPosition, Quaternion newRotation)
    {
        //Debug.Log($"{gameObject.name} received new position: {newPosition} and rotation: {newRotation}");
        transform.position = newPosition;
        transform.rotation = newRotation;
    }
}