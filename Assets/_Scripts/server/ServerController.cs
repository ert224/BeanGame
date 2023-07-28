using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

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
        //netObj.Spawn();
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

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {

            SerializedCard cardPop = beanDeck.PopCard();
            Debug.Log("pooped");
            Debug.Log(cardPop.GetCardType());

            var playerHand = client.PlayerObject.GetComponent<PlayerHand>();
            playerHand.AddCardClientRpc(cardPop);
        }
        //foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        //{
        //    var playerHand = client.PlayerObject.GetComponent<PlayerHand>();
        //    playerHand.InstantiateCard(0);
        //}

    }

    private IEnumerator WaitForTwoClients()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClientsList.Count >= 2);
        // InstantiateDeck();
        AssignCardToPlayer();
    }
}
