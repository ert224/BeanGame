using UnityEngine;
using Unity.Netcode;
public class PlantObjState : NetworkBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject plantfieldObjInstance; // Reference to the instantiated Canvas
    private ulong objID;
    private Vector3 _targetPosition;
    private NetworkObjectReference GlobalObjRef;


    private void Start()
    {
        Debug.LogError("Canvas Starts in the NEBEC!");
        plantfieldObjInstance.SetActive(true);
    }
    
    public void SetObjID(ulong setID)
    {
        objID = setID;
        Debug.Log("Set object id");
        Debug.Log(objID);
    }
    public void SetTargetPos(Vector3 newTarget)
    {
        _targetPosition = newTarget;
        Debug.Log("Set new vect");
        Debug.Log(_targetPosition);
    }
    public void SetNetworkRef(NetworkObjectReference objref)
    {
        GlobalObjRef = objref;
        Debug.Log(GlobalObjRef);
    }
    public void ActivateCanvasObj()
    {
        if (plantfieldObjInstance == null)
        {
            Debug.Log("Canvas instance not found in the scene!");
            return;
        }

        if (!plantfieldObjInstance.activeSelf)
        {
            Debug.Log("Canvas not active");
            plantfieldObjInstance.SetActive(true);
        }
        else
        {
            Debug.Log("Canvas active");
            plantfieldObjInstance.SetActive(false);
        }
    }

    public void RequestPlantBean()
    {
        Debug.LogError("Inside Bean ");
        Debug.LogError(_targetPosition.ToString());
        Debug.LogError(objID.ToString());
        Debug.Log(GlobalObjRef);
         PlantCardServerRpc(objID, _targetPosition);
        //PlantRefServerRpc(GlobalObjRef, _targetPosition);
    }


    [ServerRpc(RequireOwnership = false)]
    public void PlantCardServerRpc(ulong networkObjectId, Vector3 newTarget)
    {
        Debug.LogError("Inside pLants server");
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

    [ServerRpc(RequireOwnership = false)]
    public void PlantRefServerRpc(NetworkObjectReference playerGameObject, Vector3 newTarget)
    {
        if (!playerGameObject.TryGet(out NetworkObject networkObject))
        {
            Debug.Log("error");
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
