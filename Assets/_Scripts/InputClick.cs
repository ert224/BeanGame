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

    private RaycastHit2D rayHit;

    public void OnClick(InputAction.CallbackContext context)
    {
        Debug.Log("In click eevent ");
        if (!IsOwner) return;
        //if(!)
        if (!context.started) return;
        rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay((Vector3)Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;

        Debug.Log("ray hit collider");
        Debug.Log(rayHit.collider.gameObject.name);
        //plantCardServerRpc(rayHit.collider.gameObject);
        var networkBehaviour = rayHit.collider.gameObject.GetComponent<NetworkBehaviour>();

        Debug.Log("Owner of clicked object: " + networkBehaviour.OwnerClientId);

        PlantCardServer();

    }

    private Vector3 downOffset = new Vector3(0, -50, 0);

   // [ServerRpc]
    public void PlantCardServer()
    {
        Debug.Log("Inside plant field ");
        // Make sure rayHit.collider is not null
        if (rayHit.collider == null)
        {
            Debug.Log("rayHit.collider is null");
            return;
        }
        // Make sure the object has a SpawnCard script
        var spawnCard = rayHit.collider.gameObject.GetComponent<SpawnCard>();
        if (spawnCard != null)
        {

            // Compute new target position
            Vector3 newTarget = rayHit.collider.gameObject.transform.position + downOffset;

            // Set the new target position
            spawnCard.SetTargetLocation(newTarget);
        }
        else
        {
            Debug.Log("Null Spawn card");
        }
    }


}
