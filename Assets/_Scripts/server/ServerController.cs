using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class ServerController : NetworkBehaviour
{
    [SerializeField] private GameObject backSidePrefab;
    private SerializedDeck beanDeck;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer) return;

        if (IsServer)
        {
            StartCoroutine(DelayedInstantiateDeck());
            beanDeck = gameObject.AddComponent<SerializedDeck>();
            beanDeck.CreateNewDeck();
            beanDeck.ShuffleDeck();
            beanDeck.PrintDeck();
            StartCoroutine(WaitForTwoClients());

        }

    }

    public void InstantiateDeck()
    {
        Debug.Log("Intnant back Side dek");
        Vector3 location = new Vector3(0f, 0f, 0f);
        Quaternion turnDeck = Quaternion.Euler(0f, 0f, 90f);
        GameObject deckObject = Instantiate(backSidePrefab, location, turnDeck);
        NetworkObject netObj = deckObject.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
        }
        else
        {
            Debug.LogError("Failed to spawn deck as NetworkObject component is missing from the prefab");
        }
    }
    private IEnumerator DelayedInstantiateDeck()
    {
        yield return new WaitForEndOfFrame();
        InstantiateDeck();
    }

    private void AssignCardToPlayer()
    {
        Debug.Log("Inside Addign tp player");
        if (!IsServer) return;
        CreateHandForClients();
        PrintClientsLists();
        AssignHands();


    }

    private void AssignHands()
    {
        Debug.Log("Inside Addign tp player");
        if (!IsServer) return;
        ushort count = 0;
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            List<SerializedCard> clientHand = _ClientsLists[count];
            foreach (SerializedCard cardPop in clientHand)
            {
                Debug.Log("pooped");
                Debug.Log(cardPop.GetCardType());
                var playerHand = client.PlayerObject.GetComponent<PlayerHand>();
                playerHand.AddCardClientRpc(cardPop);
            }
            count++;
        }
         Debug.Log("loops");
         PrintClientsLists();
    }
    private List<List<SerializedCard>> _ClientsLists;

    private void CreateHandForClients()
    {
        _ClientsLists = new List<List<SerializedCard>>();
        Debug.Log("Inside Adding to player");
        if (!IsServer) return;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            List<SerializedCard> _ClientHand = new List<SerializedCard>();
            for (int i = 0; i < 5; i++)
            {
                SerializedCard cardPop = beanDeck.PopCard();
                Debug.Log("Popped");
                Debug.Log(cardPop.GetCardType());
                _ClientHand.Add(cardPop);
            }
            _ClientsLists.Add(_ClientHand);
        }
    }



    private void PrintClientsLists()
    {
        Debug.Log("Printing Player Hands:");
        for (int i = 0; i < _ClientsLists.Count; i++)
        {
            List<SerializedCard> playerHand = _ClientsLists[i];
            string handString = $"Player {i + 1} Hand: ";

            for (int j = 0; j < playerHand.Count; j++)
            {
                SerializedCard card = playerHand[j];
                handString += $"{card.GetCardType()}, ";
            }
            // Remove the trailing comma and space from the last card.
            if (playerHand.Count > 0)
            {
                handString = handString.Remove(handString.Length - 2);
            }

            Debug.Log(handString);
        }
    }






    private IEnumerator WaitForTwoClients()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClientsList.Count >= 2);
        // InstantiateDeck();
        AssignCardToPlayer();
    }
    /// <summary>
    /// 
    /// </summary>
  
}
