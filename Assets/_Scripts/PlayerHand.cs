using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHand : NetworkBehaviour
{

    // Called when the length has changed. This is more for local client-side updates.
    public static event System.Action<ushort> ChangedLengthEvent;
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
        AddLengthServerRpc();
    }

    private void LengthChangedEvent(ushort previousValue, ushort newValue)
    {
        LengthChanged();
    }


    /// <summary>
    /// Adds a length to the NetworkVariable.
    /// This will only be called on the server.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void AddLengthServerRpc()
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

    [SerializeField]
    private AllPrefabs allPrefabs;
    [ContextMenu(itemName: "Spawn card")]
    private void InstantiateCard()
    {
        if (!IsServer) return;
        ulong clientId = OwnerClientId;
        Debug.Log("Client Id");
        Debug.Log(clientId);
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

            
            NetworkObject cardGameObj = Instantiate(networkCardPrefab, transform.position, transform.rotation);
            cardGameObj.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
            cardGameObj.GetComponent<NetworkObject>().Spawn(true);
            if (cardGameObj.TryGetComponent(out SpawnCard moveCard))
            {
                Debug.Log("Current Transform");
                Debug.Log(transform.position);
                moveCard.networkedOwner = transform;
                moveCard.followTransform = _lastCard;
                _lastCard = cardGameObj.transform;
                moveCard.BeginMoveToParent();


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


