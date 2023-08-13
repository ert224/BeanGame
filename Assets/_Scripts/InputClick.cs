using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputClick : NetworkBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private GameObject plantfieldObjPrefab; // Reference to your Canvas prefab
    private GameObject plantfieldObjInstance; // Reference to the instantiated Canvas
    private ulong numCoins01 = 0;
    private ulong numCoins02 = 0;
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

    private RaycastHit2D rayHit;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!IsOwner || !Application.isFocused) return;
        if (!context.started) return;
        rayHit = Physics2D.GetRayIntersection(_mainCamera.ScreenPointToRay((Vector3)Mouse.current.position.ReadValue()));
        if (!rayHit.collider) return;
        Debug.LogError("Owner ID: " + OwnerClientId);
        var holdID = OwnerClientId;
        Debug.Log("ray hit collider");
        Debug.Log(rayHit.collider.gameObject.name);
        var networkBehaviour = rayHit.collider.gameObject.GetComponent<NetworkBehaviour>();
        //var networkObjectRef = new NetworkObjectReference(rayHit.collider.gameObject);
        if (networkBehaviour.IsOwner)
        {
            Debug.Log("Card and Client Match");
            var downOffset = rayHit.collider.transform.position - new Vector3(-32, -120, 0);
            Vector3 currPos = rayHit.collider.gameObject.transform.position;
            //ActivateCanvasObj();
            //PlantCardServerRpc(networkBehaviour.NetworkObject.NetworkObjectId, newTarget);

            changeActiveCanvas(networkBehaviour.NetworkObject.NetworkObjectId, currPos, holdID);
        }
    }


    public void changeActiveCanvas(ulong networkObjectId, Vector3 newTarget, ulong cardOwnerID)
    {
        if (plantfieldObjInstance.TryGetComponent(out PlantObjState plantScript))
        {

            plantScript.ActivateCanvasObj();
            plantScript.SetObjID(networkObjectId);
            plantScript.SetTargetPos(newTarget);
            plantScript.SetCardOwnerID(cardOwnerID);

        }
    }
    public Vector3 SetCardLocation(ulong player)
    {
        Vector3 hold = new Vector3(0f, 0f, 0f);
        Debug.Log("Network Rsponse");
        if (player == 0)
        {
            hold = new Vector3(-60f, -60f, 0f);
        }
        else if (player == 1)
        {
            hold = new Vector3(-178f, 60f , 0f);
        }
        else if (player == 2)
        {
            hold = new Vector3(-60f, 60f, 0f);
        }
        else if (player == 3)
        {
            hold = new Vector3(178f, 60f, 0f);
        }
        return hold;
        //return new Vector3(0f, -115f, 0f); // default rotation
    }
}
