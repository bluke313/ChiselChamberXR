using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
//using Unity.Networking.Transport;
using Unity.Netcode.Transports.UTP;

public class NetworkManagerHandler : NetworkBehaviour
{
    private UnityTransport transport;
    public TMP_Dropdown modeDropdown;
    public Button singleplayer;
    public GameObject cubePrefab;
    private GameObject[] spawnedCubes;

    private GameObject worldCube;

    //cube spawn positions
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-8f, 1f, -6f),  
        new Vector3(4f, 1f, -6f),   
        new Vector3(-8f, 1f, 6f),   
        new Vector3(4f, 1f, 6f)     
    };

    void Start() {
        spawnedCubes = new GameObject[4];
        // NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        // "192.168.1.100",  //host address
        // (ushort)7777, //port number
        // "0.0.0.0" //server listen address, 0.0.0.0 is listen to all
        // );//`127.0.0.1` is localhost

        // NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        // "127.0.0.1",  //host address
        // (ushort)7777, //port number
        // "0.0.0.0" //server listen address, 0.0.0.0 is listen to all
        // );
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
        "192.168.1.100",  //host address
        (ushort)7777, //port number
        "0.0.0.0" //server listen address, 0.0.0.0 is listen to all
        );
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        // Iterate through the found buttons
        foreach (Button button in buttons)
        {
            // Check if the button's GameObject matches the desired name
            if (button.gameObject.name == "SinglePlayer")
            {
                singleplayer =  button;
            }
        }
        modeDropdown.onValueChanged.AddListener(HandleDropdownChange);
        singleplayer.onClick.AddListener(HandleSinglePlayer);
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    //deactivate single player cube
    //start host or start client
    private void HandleDropdownChange(int index) {
        worldCube = GameObject.Find("Cube");
        if (worldCube.activeSelf){
            worldCube.SetActive(false);
        }
        if (index == 1) {
            NetworkManager.Singleton.StartHost();
        } else if (index == 2) {
            NetworkManager.Singleton.StartClient();
        }
    }

    private void HandleSinglePlayer(){
        // GameObject cubeObject = GameObject.Find("Cube");
        // if (cubeObject != null)
        // {
            worldCube.SetActive(true);
        // }
        foreach(GameObject cube in spawnedCubes){
            if(cube != null){
                cube.GetComponent<NetworkObject>().Despawn();
            }
            
        }
        NetworkManager.Singleton.Shutdown();
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

    // private void OnClientDisconnected(ulong clientId) {
    //     if (NetworkManager.Singleton.IsServer) {
    //         int clientIndex = FindClientIndex(NetworkManager.Singleton.ConnectedClientsList, clientId);
    //         if (clientIndex != -1)
    //         {
    //             if (spawnedCubes[clientIndex] != null)
    //             {
    //                 Destroy(spawnedCubes[clientIndex]);
    //                 spawnedCubes[clientIndex] = null;
    //             }
    //         }
    //     }
    // }

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
        if(spawnedCubes[clientIndex] != null){
            spawnedCubes[clientIndex].GetComponent<NetworkObject>().Despawn();
        }
        GameObject instance = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        instance.GetComponent<NetworkObject>().Spawn();
        spawnedCubes[clientIndex] = instance;


}

}