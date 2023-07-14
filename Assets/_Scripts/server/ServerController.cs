using System;
using Unity.Netcode;
using UnityEngine;

public class ServerController : NetworkBehaviour
{
    [SerializeField] private GameObject backSidePrefab;
    private CardsData beanDeck;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer) return;

        if (IsServer)
        {
            InstantiateDeck();
            beanDeck = gameObject.AddComponent<CardsData>();
            beanDeck.CreateNewCards();
            beanDeck.ShuffleBeans();
            beanDeck.printCardDeck();
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
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            if (client.ClientId == NetworkManager.Singleton.LocalClientId) // skip the server
                continue;

            // Assuming CardsData has a method Pop that removes a card from the deck
            CardsTemplate cardPop = beanDeck.Pop();
            Debug.Log("pooped");
            Debug.Log(cardPop.GetCardType());

            // Distribute the card
            //Vector3 location = new Vector3(-60f, -60f, 0f);
            //Quaternion rotCard = Quaternion.Euler(0f, 0f, 0f);
            //GameObject spawnCard = Instantiate(cardPop.prefab, location, rotCard);
            //spawnCard.GetComponent<NetworkObject>().Spawn();

            // Add the card to the player's hand
            var playerHand = client.PlayerObject.GetComponent<PlayerHand>();
            playerHand.AddCard(cardPop);
        }
    }
    private void DistributeDeck(CardsTemplate cardTemp)
    {
        if(!IsOwner) return;

        Vector3 location = new Vector3(-60f, -60f, 0f);
        Quaternion rotCard = Quaternion.Euler(0f, 0f, 0f);
        GameObject spawnCard = Instantiate(cardTemp.prefab, location, rotCard);
        spawnCard.GetComponent<NetworkObject>().Spawn();


    }


}
