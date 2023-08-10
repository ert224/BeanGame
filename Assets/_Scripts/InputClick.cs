using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputClick : NetworkBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameObject plantfieldObjPrefab; // Reference to your Canvas prefab
    private GameObject plantfieldObjInstance; // Reference to the instantiated Canvas

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
        plantfieldObjInstance = Instantiate(plantfieldObjPrefab);
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
        var networkObjectRef = new NetworkObjectReference(rayHit.collider.gameObject);
        if (networkBehaviour.IsOwner)
        {
            Debug.Log("Card and Client Match");
            Vector3 newTarget = rayHit.collider.gameObject.transform.position + downOffset;
            //ActivateCanvasObj();
            //PlantCardServerRpc(networkBehaviour.NetworkObject.NetworkObjectId, newTarget);
            changeActiveCanvas(networkBehaviour.NetworkObject.NetworkObjectId, newTarget, networkObjectRef);
        }
    }


    public void changeActiveCanvas(ulong networkObjectId, Vector3 newTarget, NetworkObjectReference objref)
    {
        if (plantfieldObjInstance.TryGetComponent(out PlantObjState plantScript))
        {
            
            plantScript.ActivateCanvasObj();
            var networkBehaviour = rayHit.collider.gameObject.GetComponent<NetworkBehaviour>();

            plantScript.SetObjID(networkObjectId);
            plantScript.SetTargetPos(newTarget);
            plantScript.SetNetworkRef(objref);
            //PlantCardServerRpc(networkObjectId, newTarget);
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
