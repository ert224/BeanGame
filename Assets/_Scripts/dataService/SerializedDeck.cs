using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedDeck : MonoBehaviour
{
    private List<SerializedCard> cardsList = new List<SerializedCard>();

    public void CreateNewDeck()
    {
        // Add objects of type "coffee"
        for (int i = 0; i < 24; i++)
        {
            cardsList.Add(new SerializedCard("coffee", "24CoffeeTag", 24, 1));
        }

        // Add objects of type "wax"
        for (int i = 0; i < 22; i++)
        {
            cardsList.Add(new SerializedCard("wax", "22WaxTag", 22, 1));
        }

        // Add objects of type "blue"
        for (int i = 0; i < 20; i++)
        {
            cardsList.Add(new SerializedCard("blue", "20BlueTag", 20, 1));
        }

        // Add objects of type "chili"
        for (int i = 0; i < 18; i++)
        {
            cardsList.Add(new SerializedCard("chili", "18ChiliTag", 18, 1));
        }

        // Add objects of type "stink"
        for (int i = 0; i < 16; i++)
        {
            cardsList.Add(new SerializedCard("stink", "16StinkTag", 16, 1));
        }

        // Add objects of type "green"
        for (int i = 0; i < 14; i++)
        {
            cardsList.Add(new SerializedCard("green", "14GreenTag", 14, 1));
        }

        // Add objects of type "soy"
        for (int i = 0; i < 12; i++)
        {
            cardsList.Add(new SerializedCard("soy", "12SoyTag", 12, 1));
        }

        // Add objects of type "black"
        for (int i = 0; i < 10; i++)
        {
            cardsList.Add(new SerializedCard("black", "10BlackEyedTag", 10, 1));
        }

        // Add objects of type "red"
        for (int i = 0; i < 8; i++)
        {
            cardsList.Add(new SerializedCard("red", "08RedTag", 8, 1));
        }
    }

    public SerializedCard PopCard()
    {
        // Check if there are any cards left in the deck
        if (cardsList.Count == 0)
        {
            Debug.LogError("No cards left in the deck.");
            return new SerializedCard(null,null, -1, 0);
        }

        SerializedCard topCard = cardsList[0];
        cardsList.RemoveAt(0);
        return topCard;
    }

    public void PrintDeck()
    {
        // Print the card types and values
        foreach (SerializedCard card in cardsList)
        {
            Debug.Log("Card Type: " + card.GetCardType() + ", Value: " + card.GetValue());
        }
    }

    public void ShuffleDeck()
    {
        System.Random random = new System.Random();

        for (int i = 0; i < cardsList.Count - 1; i++)
        {
            int randomIndex = random.Next(i, cardsList.Count);
            SerializedCard temp = cardsList[i];
            cardsList[i] = cardsList[randomIndex];
            cardsList[randomIndex] = temp;
        }
    }

    public List<SerializedCard> GetCardDeck()
    {
        return this.cardsList;
    }
}
