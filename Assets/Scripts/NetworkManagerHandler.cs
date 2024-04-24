using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class NetworkManagerHandler : NetworkBehaviour
{
    public TMP_Dropdown modeDropdown;
    public GameObject cubePrefab;

    //cube spawn positions
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-8f, 1f, -6f),  
        new Vector3(4f, 1f, -6f),   
        new Vector3(-8f, 1f, 6f),   
        new Vector3(4f, 1f, 6f)     
    };

    void Start() {
        modeDropdown.onValueChanged.AddListener(HandleDropdownChange);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    //deactivate single player cube
    //start host or start client
    private void HandleDropdownChange(int index) {
        if (GameObject.Find("World/Cube").activeSelf){
            GameObject.Find("World/Cube").SetActive(false);
        }
        if (index == 1) {
            NetworkManager.Singleton.StartHost();
        } else if (index == 2) {
            NetworkManager.Singleton.StartClient();
        }
    }

    private void OnServerStarted()
    {
        Debug.Log("Server connected");
    }

    //when client connects
    //log if it is host or other client
    //spawn cube
    private void OnClientConnected(ulong clientId) {
        if (NetworkManager.Singleton.IsServer && clientId == NetworkManager.Singleton.LocalClientId) {
            Debug.Log("Server as client connected");
        } else if (NetworkManager.Singleton.IsServer) {
            Debug.Log($"Client connected with ID {clientId}, spawning cube.");
        }
        SpawnCube(clientId);
    }

    //find the client in the list passed, return index
    //-1 if not found
    private int FindClientIndex(IReadOnlyList<NetworkClient> clients, ulong clientId)
    {
        for (int i = 0; i < clients.Count; i++)
        {
            if (clients[i].ClientId == clientId)
                return i;
        }
        return -1;
    }

    //spawn a cube at one of 4 positions defined by clientID
    //Instantiate and spawn (quaternion is rotation)
    private void SpawnCube(ulong clientId) {
        if (!NetworkManager.Singleton.IsServer) {
            Debug.LogError("Attempt to spawn cube from non-server instance.");
            return;
        }

        Debug.Log($"Server spawning cube for client {clientId}.");
        int clientIndex = FindClientIndex(NetworkManager.Singleton.ConnectedClientsList, clientId);
        Debug.Log($"List of Clients is {NetworkManager.Singleton.ConnectedClientsList}, Length is {NetworkManager.Singleton.ConnectedClientsList.Count}");
        if (clientIndex == -1) {
            Debug.LogError("Client not found in the connected clients list.");
            return;
        }
        Debug.Log($"Client {clientIndex} spawned");
        Vector3 spawnPosition = spawnPositions[clientIndex % spawnPositions.Length];
        GameObject instance = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        instance.GetComponent<NetworkObject>().Spawn();
}

}