using UnityEngine;
using Unity.Netcode;
public class PlantObjState : NetworkBehaviour
{
    [SerializeField] private GameObject plantfieldObjInstance; // Reference to the instantiated Canvas
    private NetworkVariable<ulong> canvasCardID = new NetworkVariable<ulong>();

    private ulong objID;
    private ulong cardOwnerID;

    private Vector3 _targetPosition;
    private NetworkObjectReference GlobalObjRef;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.LogError("Canvas Starts in the NEBEC!");
        plantfieldObjInstance.SetActive(true);
    }
    
    public void SetCardOwnerID(ulong setID)
    {
        cardOwnerID = setID;
        Debug.LogError("Set Asigned Owner id " + cardOwnerID);
    }
    public void SetObjID(ulong setID)
    {
        objID = setID;
        Debug.Log("Set object id " + objID);
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
        Debug.LogError("Owner ID: " + OwnerClientId);
        Debug.Log(_targetPosition.ToString());
        Debug.Log(objID.ToString());
        Debug.Log(GlobalObjRef);
        _targetPosition = SetCardLocation01(_targetPosition);
        Debug.LogError("New target loc");
        Debug.LogError(_targetPosition.ToString());
        PlantCard(objID, _targetPosition); // here 
        ActivateCanvasObj();
    }

    public void RequestPlantBean02()
    {
        Debug.LogError("Inside Bean ");
        Debug.LogError("Owner ID: " + OwnerClientId);
        Debug.Log(_targetPosition.ToString());
        Debug.Log(objID.ToString());
        Debug.Log(GlobalObjRef);
        _targetPosition = SetCardLocation02(_targetPosition);
        Debug.LogError("New target loc");
        Debug.LogError(_targetPosition.ToString());
        PlantCard(objID, _targetPosition); // here 
        ActivateCanvasObj();

    }
    private void PlantCard(ulong networkObjectId, Vector3 newTarget)
    {
        Debug.LogError("Inside pLants server");
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject networkObject))
        {
            var spawnCard = networkObject.gameObject.GetComponent<SpawnCard>();
            if (spawnCard == null)
            {
                Debug.Log("SpawnCard component is missing");
                return;
            }

            Debug.Log("change move move");
            //if (!IsOwner) return;
            spawnCard.SetTargetLocationServerRpc(newTarget); // Call the new Server RPC here
        }
    }

    public Vector3 SetCardLocation01(Vector3 currPos)
    {
        Vector3 hold = new Vector3(0f, 0f, 0f);
        Debug.Log("Network Rsponse");
        if (cardOwnerID == 0)
        {
            hold = currPos - (currPos - new Vector3(-32, -120, 0));
        }
        else if (cardOwnerID == 1)
        {
            hold = currPos - (currPos - new Vector3(-237, 33, 0));
        }
        else if (cardOwnerID == 2)
        {
            hold = currPos - (currPos - new Vector3(-32, 120, 0));
        }
        else if (cardOwnerID == 3)
        {
            hold = currPos - (currPos - new Vector3(237, -33, 0));
        }
        return hold;
    }

    public Vector3 SetCardLocation02( Vector3 currPos)
    {
        Vector3 hold = new Vector3(0f, 0f, 0f);
        Debug.Log("Network Rsponse");
        if (cardOwnerID == 0)
        {
            hold = currPos - (currPos - new Vector3(-237, -32, 0));
        }
        else if (cardOwnerID == 1)
        {
            hold = currPos - (currPos - new Vector3(-237, -32, 0));
        }
        else if (cardOwnerID == 2)
        {
            hold = currPos - (currPos - new Vector3(32, 120, 0));
        }
        else if (cardOwnerID == 3)
        {
            hold = currPos - (currPos - new Vector3(237, 33, 0));
        }
        return hold;
    }

}
