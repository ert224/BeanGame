using UnityEngine;
using System;
using System.Collections.Generic;

public class CardsData : MonoBehaviour
{
    // [SerializeField] private GameObject prefab;

    private List<CardsTemplate> cardsList = new List<CardsTemplate>();

    public void CreateNewCards()
    {
        //int index = 0;

        // Add objects of type "coffee"
        for (int i = 0; i < 24; i++)
        {
            cardsList.Add(new CardsTemplate("coffee", 24, GameObject.FindGameObjectWithTag("24CoffeeTag"), 1));
        }

        // Add objects of type "wax"
        for (int i = 0; i < 22; i++)
        {
            cardsList.Add(new CardsTemplate("wax", 22, GameObject.FindGameObjectWithTag("22WaxTag"), 1));
        }

        // Add objects of type "blue"
        for (int i = 0; i < 20; i++)
        {
            cardsList.Add(new CardsTemplate("blue", 20, GameObject.FindGameObjectWithTag("20BlueTag"), 1));
        }

        // Add objects of type "chili"
        for (int i = 0; i < 18; i++)
        {
            cardsList.Add(new CardsTemplate("chili", 18, GameObject.FindGameObjectWithTag("18ChiliTag"), 1));
        }

        // Add objects of type "stink"
        for (int i = 0; i < 16; i++)
        {
            cardsList.Add(new CardsTemplate("stink", 16, GameObject.FindGameObjectWithTag("16StinkTag"), 1));
        }

        // Add objects of type "green"
        for (int i = 0; i < 14; i++)
        {
            cardsList.Add(new CardsTemplate("green", 14, GameObject.FindGameObjectWithTag("14GreenTag"), 1));
        }

        // Add objects of type "soy"
        for (int i = 0; i < 12; i++)
        {
            cardsList.Add(new CardsTemplate("soy", 12, GameObject.FindGameObjectWithTag("12SoyTag"), 1));
        }

        // Add objects of type "black"
        for (int i = 0; i < 10; i++)
        {
            cardsList.Add(new CardsTemplate("black", 10, GameObject.FindGameObjectWithTag("10BlackEyedTag"), 1));
        }

        // Add objects of type "red"
        for (int i = 0; i < 8; i++)
        {
            cardsList.Add(new CardsTemplate("red", 8, GameObject.FindGameObjectWithTag("08RedTag"), 1));
        }
    }


    public CardsTemplate Pop()
    {
        // Check if there are any cards left in the deck
        if (cardsList.Count == 0)
        {
            Debug.LogError("No cards left in the deck.");
            return new CardsTemplate(null, -1, null,0);
        }

        CardsTemplate topCard = cardsList[0];
        cardsList.RemoveAt(0);
        return topCard;
    }

    public void printCardDeck()
    {
        // Print the card types and values
        foreach (CardsTemplate card in cardsList)
        {
            Debug.Log("Card Type: " + card.GetCardType() + ", Value: " + card.GetValue());
        }
    }

    public void ShuffleBeans()
    {
        System.Random random = new System.Random();

        for (int i = 0; i < cardsList.Count - 1; i++)
        {
            int randomIndex = random.Next(i, cardsList.Count);
            CardsTemplate temp = cardsList[i];
            cardsList[i] = cardsList[randomIndex];
            cardsList[randomIndex] = temp;
        }
    }

    public List<CardsTemplate> GetCardDeck()
    {
        return this.cardsList;
    }
}
