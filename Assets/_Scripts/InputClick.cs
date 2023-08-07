using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputClick : NetworkBehaviour
{
    private Camera _mainCamera;


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();

    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }
    private Vector3 downOffset = new Vector3(-25, -50, 0);
    private RaycastHit2D rayHit;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!IsOwner || !Application.isFocused) return;
        if (!context.started) return;
        rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay((Vector3)Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        Debug.Log("ray hit collider");
        Debug.Log(rayHit.collider.gameObject.name);
        var networkBehaviour = rayHit.collider.gameObject.GetComponent<NetworkBehaviour>();
        if (networkBehaviour.IsOwner)
        {
            Debug.Log("Card and Client Match");
            Vector3 newTarget = rayHit.collider.gameObject.transform.position + downOffset;
            PlantCardServerRpc(networkBehaviour.NetworkObject.NetworkObjectId, newTarget);
        }

    }

    [ServerRpc]
    public void PlantCardServerRpc(ulong networkObjectId, Vector3 newTarget)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            Debug.Log("NetworkObject not found");
            return;
        }

        var spawnCard = networkObject.gameObject.GetComponent<SpawnCard>();
        if (spawnCard == null)
        {
            Debug.Log("SpawnCard component is missing");
            return;
        }

        Debug.Log("change move move");
        spawnCard.SetTargetLocation(newTarget);
        spawnCard.UpdatePlayerPositionServerRpc(newTarget.x, newTarget.y, newTarget.z);
    }


}
