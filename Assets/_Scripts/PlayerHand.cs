using System.Collections.Generic;
using System.Linq;
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
    private List<GameObject> _CardsList;
    #endregion Private Variables
    private Transform _lastCard;
    private Collider2D _collider2D;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _CardsList = new List<GameObject>();
        _lastCard = transform;
        _collider2D = GetComponent<Collider2D>();
        if (!IsServer) length.OnValueChanged += LengthChangedEvent;
        if (IsOwner) return;
        //for (int i = 0; i < length.Value - 1; ++i)
        //    InstantiateCard();
        //Subscribe to the LengthChangedEvent only if it's the owner client
        //if (IsOwner)
        //{
        //    length.OnValueChanged += LengthChangedEvent;
        //}
        //If not the owner, spawn the cards based on the length received from the server
        //if (!IsOwner)
        //{
        //    for (int i = 0; i < length.Value; ++i)
        //    {
        //        InstantiateCard();
        //    }
        //}

    }

    private SerializedCard _card;
    [ClientRpc]
    public void AddCardClientRpc(SerializedCard card)
    {
        if (!IsServer) return;
        Debug.Log("In add card");
        Debug.Log($"Card type:");
        _card = card;
        Debug.Log(_card.GetCardType());
        RequestAddCardServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestAddCardServerRpc()
    {
        AddLengthServer(); // The server will add the card by invoking the AddLength method.
    }

    /// <summary>
    /// Adds a length to the NetworkVariable.
    /// This will only be called on the server.
    /// </summary>
    public void AddLengthServer()
    {
        if (!IsServer) return;
        length.Value += 1;
        LengthChanged();
    }

    private void LengthChanged()
    {
        InstantiateCard();
        if (!IsOwner) return;
        
        ChangedLengthEvent?.Invoke(length.Value);
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



    [SerializeField]
    private AllPrefabs allPrefabs;
    [ContextMenu(itemName: "Spawn card")]
    private void InstantiateCard()
    {
        if (allPrefabs == null)
        {
            Debug.LogError("allPrefabs is null. Please make sure it is assigned in the Unity Editor.");
            return;
        }
        if (allPrefabs.ReturnPrefab(_card.GetCardType()))
        {
            GameObject cardPrefab = allPrefabs.ReturnPrefab(_card.GetCardType());
            NetworkObject networkCardPrefab = cardPrefab.GetComponent<NetworkObject>();

            if (networkCardPrefab == null)
            {
                Debug.LogError("NetworkObject component is missing from the card prefab.");
                return;
            }
            
            NetworkObject cardGameObj = Instantiate(networkCardPrefab, transform.position, Quaternion.identity);
            cardGameObj.GetComponent<SpriteRenderer>().sortingOrder = length.Value;
            cardGameObj.GetComponent<NetworkObject>().Spawn(true);
            if(cardGameObj.TryGetComponent(out SpawnCard moveCard)){
                Debug.Log("Current Transform");
                Debug.Log(transform.position);
                moveCard.networkedOwner = transform;
                moveCard.followTransform = _lastCard;
                _lastCard = cardGameObj.transform;
                Physics2D.IgnoreCollision(cardGameObj.GetComponent<Collider2D>(), _collider2D);

            }

            _CardsList.Add(cardGameObj.gameObject);

        }
        else
        {
            Debug.Log("Prefab is null");
            Debug.Log(allPrefabs.ReturnPrefab(_card.GetCardType()));
        }
    }

}

