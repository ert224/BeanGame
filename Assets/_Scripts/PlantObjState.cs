using UnityEngine;
using Unity.Netcode;
using System;

public class PlantObjState : NetworkBehaviour
{
    [SerializeField] private GameObject plantfieldObjInstance; // Reference to the instantiated Canvas
    private NetworkVariable<ulong> canvasCardID = new NetworkVariable<ulong>();

    private ulong objID;
    private ulong cardOwnerID;
    private  int cardsPlanted = 0;
    private int cardsOnHand = 5; 

    private Vector3 _targetPosition;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.LogError("Canvas Starts in the NEBEC!");
        plantfieldObjInstance.SetActive(true);
    }

    private void decHand()
    {
        cardsOnHand--;
    }
    private void incPlanted()
    {
        cardsPlanted++;
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
        //Debug.LogError("Inside Bean ");
        //Debug.LogError("Owner ID: " + OwnerClientId);
        //Debug.Log(_targetPosition.ToString());
        //Debug.Log(objID.ToString());
        //_targetPosition = SetCardLocation01(_targetPosition);
        //Debug.LogError("New target loc");
        //Debug.LogError(_targetPosition.ToString());
        //PlantCard(objID, _targetPosition); // here 
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
            _lastPos = networkObject.transform.position;
            Debug.Log("change move move");
            Debug.Log(_lastPos);
            //if (!IsOwner) return;
            spawnCard.SetTargetLocationServerRpc(newTarget); // Call the new Server RPC here
        }
    }
    Vector3 _lastPos = Vector3.zero;

    public void SetLastPos(Vector3 pos)
    {
        _lastPos = pos;
    }
    ulong countID=0;
    public void setCountID(ulong newId)
    {
        countID=newId;
    }
    public void RequestPlantBean02()
    {
        Debug.LogError("Inside Bean ");
        Debug.LogError("Owner ID: " + OwnerClientId);
        Debug.Log(_targetPosition.ToString());
        Debug.Log(objID.ToString());
        SetLastPos(_targetPosition);
        _targetPosition = SetCardLocation02(_targetPosition);
        Debug.LogError("New target loc");
        Debug.LogError(_targetPosition.ToString());
        PlantCard(objID, _targetPosition); // here 

        decHand();
        incPlanted();

        MoveCards(objID, _lastPos);
        ActivateCanvasObj();

    }

    public void MoveCards(ulong objId,Vector3 last )
    {
        Debug.LogError("Insde Move Card !!!");
        Debug.Log("Cards on Hand: " + cardsOnHand);
        Debug.Log("Cards Planted: " + cardsPlanted);

        for (int i = 0;i<cardsOnHand;i++)
        {
            Debug.Log("ID: "+i);
            Debug.Log(_lastPos.ToString());
            PlantCard(++objID, _lastPos); // here 
            //PlantCard(objID, _lastPos); // here 

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
            hold = currPos - (currPos - new Vector3(32, -120, 0));
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
