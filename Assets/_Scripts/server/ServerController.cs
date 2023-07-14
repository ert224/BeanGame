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
            beanDeck.printCardDeck();
        }
    }

    public void InstantiateDeck()
    {
        Vector3 location = new Vector3(0f, 0f, 0f);
        Quaternion turnDeck = Quaternion.Euler(0f, 0f, 90f);
        Instantiate(backSidePrefab, location, turnDeck);
    }

    private void AssignCardToPlayer()
    {
        // Assuming CardsData has a method Pop that removes a card from the deck
        CardsTemplate cardPop = beanDeck.Pop();
        Debug.Log("pooped");
        Debug.Log(cardPop.GetCardType());
        // Assign this card to the player
        // You will need a suitable method or property to do this
    }

}
