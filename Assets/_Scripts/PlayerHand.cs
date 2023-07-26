using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject frontSide;
    // Synced Variable that keeps track of the player snake length.
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Called when the length has changed. This is more for local client-side updates.
    public static event System.Action<ushort> ChangedLengthEvent;

    #region Private Variables

    private List<CardsTemplate> _HandList;

    #endregion Private Variables

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _HandList = new List<CardsTemplate>();
        if (!IsServer) length.OnValueChanged += LengthChangedEvent;
        if (IsOwner) return;
        //for (int i = 0; i < length.Value - 1; ++i)
        //    InstantiateTail();
    }


    //[ClientRpc]
    //public void AddCardClientRpc(CardsTemplate card)
    //{
    //    if (!IsOwner) return;
    //    Debug.Log("In add card");
    //    _HandList.Add(card);
    //    Debug.Log(_HandList);
    //}



    /// <summary>
    /// Adds a length to the NetworkVariable.
    /// This will only be called on the server.
    /// </summary>
    // [ServerRpc(RequireOwnership = false)]
    [ContextMenu(itemName: "Add Card")]
    public void AddLength()
    {
        length.Value += 1;
        LengthChanged();
    }
    /// <summary>
    /// Called when the NetworkVariable length has changed.
    /// Instantiates tails on the other clients to be synchronized.
    /// </summary>
    private void LengthChanged()
    {
        Debug.Log("Length Changed ");
         Debug.Log(_HandList.Count);   
        //InstantiateTail(_HandList[_HandList.Count]);
        if (!IsOwner) return;
        ChangedLengthEvent?.Invoke(length.Value);
    }
    [ContextMenu(itemName: "Print Lista")]
    private void printLista()
    {
            Debug.Log("Length Changed ");
            Debug.Log(_HandList.Count);
        }
    /// <summary>
    /// Called when the NetworkVariable length has changed.
    /// </summary>
    /// <param name="previousValue">Mandatory callback parameter. Not used.</param>
    /// <param name="newValue">Mandatory callback parameter. Not used.</param>
    private void LengthChangedEvent(ushort previousValue, ushort newValue)
    {
        Debug.Log("LengthChanged Callback");
        LengthChanged();
    }

   
    private void InstantiateTail(CardsTemplate card)
    {
        GameObject tailGameObject = Instantiate(frontSide, transform.position, Quaternion.identity);
        tailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
        //_tails.Add(tailGameObject);
    }


}
