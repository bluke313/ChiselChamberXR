using Unity.Netcode;
using UnityEngine;

public class PlayerRPC : NetworkBehaviour
{
    public float speed = 5.0f;

    private void Update()
    {
        if (IsLocalPlayer)
        {
            float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float moveY = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            MovePlayerServerRpc(transform.position + new Vector3(moveX, 0, moveY));
        }
    }
    [ServerRpc]
    void MovePlayerServerRpc(Vector3 newPosition)
    {
        UpdatePositionClientRpc(newPosition);
    }

    [ClientRpc]
    void UpdatePositionClientRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}