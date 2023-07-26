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
            InstantiateDeck();
            beanDeck = gameObject.AddComponent<SerializedDeck>();
            beanDeck.CreateNewDeck();
            beanDeck.ShuffleDeck();
            beanDeck.PrintDeck();
            StartCoroutine(WaitForTwoClients());

        }

    }

    public void InstantiateDeck()
    {
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


    private void AssignCardToPlayer()
    {
        if (!IsServer) return;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId == NetworkManager.Singleton.LocalClientId) // skip the server
                continue;

            SerializedCard cardPop = beanDeck.PopCard();
            Debug.Log("pooped");
            Debug.Log(cardPop.GetCardType());

            var playerHand = client.PlayerObject.GetComponent<PlayerHand>();
            //playerHand.AddCardClientRpc(cardPop);
        }
    }

    private IEnumerator WaitForTwoClients()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.ConnectedClientsList.Count >= 2);
        AssignCardToPlayer();
    }
}
