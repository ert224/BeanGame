using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{
    // Synced Variable that keeps track of the player snake length.
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Called when the length has changed. This is more for local client-side updates.
    public static event System.Action<ushort> ChangedLengthEvent;

    #region Private Variables

    private List<SerializedCard> _HandList;
    private List<GameObject> _CardsList;
    #endregion Private Variables
    private Transform _lastCard;
    private Collider2D _collider2D;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _HandList = new List<SerializedCard>();
        _CardsList = new List<GameObject>();
        _lastCard = transform;
        _collider2D = GetComponent<Collider2D>();
        if (!IsServer) length.OnValueChanged += LengthChangedEvent;
        if (IsOwner) return;
        //for (int i = 0; i < length.Value - 1; ++i)
        //   InstantiateCard(0);
    }


    [ClientRpc]
    public void AddCardClientRpc(SerializedCard card)
    {
        if (!IsOwner) return;
        Debug.Log("In add card");
        _HandList.Add(card);
        giveType.Value = card.GetCardType(); // Update the NetworkVariable value
        Debug.Log("card tyoepe ${card.GetCardType()}");
        Debug.Log(card.GetCardType());
        RequestAddCardServerRpc();
        Debug.Log(_HandList);
    }
    // Request the server to add a card.
    [ServerRpc]
    private void RequestAddCardServerRpc()
    {
        AddLength(); // The server will add the card by invoking the AddLength method.
    }


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
        InstantiateCard();
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



    public NetworkVariable<string> giveType = new NetworkVariable<string>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private float positionRage = 5f;
   [ContextMenu(itemName: "Spawn card")]
    private void InstantiateCard()
    {
        Debug.Log($"Card {giveType.Value} Index");
        ///Debug.Log(_HandList[index].GetCardTag());
        GameObject cardPrefab = getCardType(giveType.Value).gameObject;
        NetworkObject cardObj = cardPrefab.GetComponent<NetworkObject>();

        if (cardObj != null)
        {
            GameObject cardGameObj = Instantiate(cardPrefab, transform.position, Quaternion.identity);
            cardGameObj.GetComponent<SpriteRenderer>().sortingOrder = -length.Value; //previously spawned card 
            if (cardGameObj.TryGetComponent(out SpawnCard spawncard))
                {
                    spawncard.networkedOwner = transform;
                    spawncard.followTransform = _lastCard; //previouly spawend
                    _lastCard = cardGameObj.transform;
                    Physics2D.IgnoreCollision(cardGameObj.GetComponent<Collider2D>(), _collider2D);
                }
             _CardsList.Add(cardGameObj);
        }
        else
        {
            Debug.LogError("No GameObject found with tag ");
        }
        //cardObj.Spawn();

    }

    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject coffee;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject wax;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject blue;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject chili;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject stink;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject green;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject soy;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject black;
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject red;

    private NetworkObject getCardType(string popType)
    {
        if (popType== "coffee"){
            return coffee.GetComponent<NetworkObject>();
        }
        else if (popType == "wax")
        {
            return wax.GetComponent<NetworkObject>();

        }
        else if (popType == "blue")
        {
            return blue.GetComponent<NetworkObject>();

        }
        else if (popType == "chili")
        {
            return chili.GetComponent<NetworkObject>();

        }
        else if (popType == "stink")
        {
            return stink.GetComponent<NetworkObject>();

        }
        else if (popType == "green")
        {
            return green.GetComponent<NetworkObject>();

        }
        else if (popType == "soy")
        {
            return soy.GetComponent<NetworkObject>();

        }
        else if (popType == "black")
        {
            return black.GetComponent<NetworkObject>();

        }
        else if (popType == "red")
        {
            return red.GetComponent<NetworkObject>();
        }
        return null;
    }


}
