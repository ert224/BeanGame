using System;
using System.Globalization;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Camera _mainCamera;

    private StartNetwork startNetwork;
  
    [SerializeField] private GameObject backSidePrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        startNetwork = FindObjectOfType<StartNetwork>();
        SetSpawnTransform();
    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
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
