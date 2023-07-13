using System.Globalization;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Camera _mainCamera;

    private StartNetwork startNetwork;
    private CardsData beanDeck;
    [SerializeField] private GameObject backSidePrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        if (IsServer)
        {
            beanDeck = gameObject.AddComponent<CardsData>();
            beanDeck.CreateNewCards();
            beanDeck.printCardDeck();
        }
        startNetwork = FindObjectOfType<StartNetwork>();
        SetSpawnTransform();

        if (startNetwork.getCount() >= 1)
        {
            InstantiateDeck();
        }
    }

    public void InstantiateDeck()
    {
        Vector3 location = new Vector3(0f, 0f, 0f);
        Quaternion turnDeck = Quaternion.Euler(0f, 0f, 90f);
        GameObject spawnDeck = Instantiate(backSidePrefab, location, turnDeck);
    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsOwner) return; 
        if (!IsLocalPlayer) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == backSidePrefab)
                {
                    AssignCardToPlayer();
                }
            }
        }
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

    private void SetSpawnTransform()
    {
        Debug.Log("Network Rsponse");
        Debug.Log(startNetwork.getCount());
        if (!IsServer) return; // only execute when server

        if (startNetwork.getCount() == 0)
        {
            transform.position = new Vector3(0f, -115f, 0f);
        }
        else if (startNetwork.getCount() == 1)
        {
            transform.position = new Vector3(-233f, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (startNetwork.getCount() == 2)
        {
            transform.position = new Vector3(233f, 0f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (startNetwork.getCount() == 3)
        {
            transform.position = new Vector3(0f, 115f, 0f);
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

    }
}
