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
        PlantCard(objID, _targetPosition); // here 
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
    private Vector3 field1 = new Vector3(-237, 33, 0);
    private Vector3 field2 = new Vector3(-237, -33, 0);
    public Vector3 SetCardLocation01(ulong player,Vector3 currPos)
    {
        Vector3 hold = new Vector3(0f, 0f, 0f);
        Debug.Log("Network Rsponse");
        if (player == 0)
        {
            hold = currPos - (currPos - new Vector3(-237, 33, 0));
        }
        else if (player == 1)
        {
            hold = currPos - (currPos - new Vector3(-237, 33, 0));
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
    }

    public Vector3 SetCardLocation02(ulong player, Vector3 currPos)
    {
        Vector3 hold = new Vector3(0f, 0f, 0f);
        Debug.Log("Network Rsponse");
        if (player == 0)
        {
            hold = currPos - (currPos - new Vector3(-237, -33, 0));
        }
        else if (player == 1)
        {
            hold = currPos - (currPos - new Vector3(-237, -33, 0));
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
    }

}
