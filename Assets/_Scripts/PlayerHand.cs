using System;
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
        ulong playerID = OwnerClientId;
        Debug.Log("Client Id");
        Debug.Log(playerID);
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

            Vector3 location = new Vector3(0f, 0f, 0f);
            Quaternion playerRotation = GetPlayerRotation(playerID);
            NetworkObject cardGameObj = Instantiate(networkCardPrefab, location, playerRotation);

            cardGameObj.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
            cardGameObj.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId,true);

            if (cardGameObj.TryGetComponent(out SpawnCard moveCard))
            {

                Debug.Log("Current Transform");
                Debug.Log(transform.position);
                moveCard.networkedOwner = transform;
                Vector3 cardLocation = SetCardLocation(playerID);
                moveCard.SetTargetLocation(cardLocation);
                moveCard.followTransform = _lastCard;
                _lastCard = cardGameObj.transform;
                // moveCard.BeginMoveToParent();
            }

            _CardsList.Add(cardGameObj.gameObject);

        }
        else
        {
            Debug.Log("Prefab is null");
            Debug.Log(allPrefabs.ReturnPrefab(_card.GetCardType()));
        }
    }

    public void PrintCardsInHand()
    {
        Debug.LogError("Cards in hand:");
        if (_CardsList == null || _CardsList.Count == 0)
        {
            Debug.Log("No cards in hand.");
            return;
        }

        foreach (GameObject card in _CardsList)
        {
            if (card != null)
            {
                Debug.Log(card.name);
            }
            else
            {
                Debug.Log("A card in the list is null.");
            }
        }
    }

    [ClientRpc]
    public void PrintCardsInHandClientRpc()
    {
        //if (!IsServer) return;
        PrintCardsInHand();
    }


    //public void moveRemCards(ulong index)
    //{
    //    Debug.LogError("New Index print hand:");
    //    if (_CardsList == null || _CardsList.Count == 0)
    //    {
    //        Debug.Log("No cards in hand.");
    //        return;
    //    }

    //    for (int i = Convert.ToInt32(index); i < _CardsList.Count; i++)
    //    {
    //        GameObject card = _CardsList[i];
    //        if (card != null)
    //        {
    //            Debug.Log(card.name);
    //        }
    //        else
    //        {
    //            Debug.Log("A card in the list is null.");
    //        }
    //    }
    //}





    /// <summary>
    /// ////////////
    /// </summary>
    private ulong CountRot = 0;
    public Vector3 SetCardLocation(ulong player)
    {
        float Rot = CountRot * 29;
        Vector3 hold = new Vector3(0f, 0f, 0f) ;
        Debug.Log("Network Rsponse");
        if (player == 0)
        {
            hold = new Vector3(-60f+ Rot, -60f, 0f);
        }
        else if (player == 1)
        {
            hold = new Vector3(-178f, 60f - Rot, 0f);
        }
        else if (player == 2)
        {
            hold = new Vector3(-60f - Rot, 60f, 0f);
        }
        else if (player == 3)
        {
            hold = new Vector3(178f, 60f - Rot, 0f);
        }
        // Handle case where player does not match expected values (0, 1, 2, 3)
        // You can return a default rotation, log an error, throw an exception, etc.
        Debug.Log("Invalid player value for getcard location: " + player);
        Debug.Log(CountRot);
        CountRot++;

        return hold;

        //return new Vector3(0f, -115f, 0f); // default rotation
    }

    private Quaternion GetPlayerRotation(ulong player)
    {
        Debug.Log("Network Rsponse");
        if (player == 0)
        {
            return Quaternion.Euler(0f, 0f, 0f);
        }
        else if (player == 1)
        {
            return Quaternion.Euler(0f, 0f, -90f);
        }
        else if (player == 2)
        {
            return Quaternion.Euler(0f, 0f, 90f);
        }
        else if (player == 3)
        {
            return Quaternion.Euler(0f, 0f, 180f);
        }

        // Handle case where player does not match expected values (0, 1, 2, 3)
        // You can return a default rotation, log an error, throw an exception, etc.
        Debug.Log("Invalid player value for GetPlayerRotation: " + player);
        return Quaternion.identity; // default rotation
    }

}


