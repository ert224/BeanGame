using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Camera _mainCamera;
    // private Vector3 _mouseInput;
    private Vector3 _mouseInput = Vector3.zero;

    [SerializeField] private float speed = 3f;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
        // SetSpawnTransform();

    }

    private void Initialize()
    {
        _mainCamera = Camera.main;
    }

    private void SetSpawnTransform()
    {
        if (!IsOwner)
        {
            int playerIndex = GetPlayerIndex();

            if (playerIndex == 0)
            {
                _targetPosition = new Vector3(0f, -65f, 0f);
            }
            else if (playerIndex == 1)
            {
                _targetPosition = new Vector3(-140f, 0f, 0f);
                _targetRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else if (playerIndex == 2)
            {
                _targetPosition = new Vector3(140f, 0f, 0f);
                _targetRotation = Quaternion.Euler(0f, 0f, 90f);
            }
            else if (playerIndex == 3)
            {
                _targetPosition = new Vector3(0f, 65f, 0f);
                _targetRotation = Quaternion.Euler(0f, 0f, 180f);
            }

            transform.position = _targetPosition;
            transform.rotation = _targetRotation;
        }
    }

    private int GetPlayerIndex()
    {
        NetworkBehaviour[] players = GameObject.FindObjectsOfType<PlayerController>();

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == this)
            {
                return i;
            }
        }

        return -1; // Return -1 if player not found
    }
    private void Update()
    {
        if (!IsOwner || !Application.isFocused) return; //check if owner is on window 
        _mouseInput.x = Input.mousePosition.x;
        _mouseInput.y = Input.mousePosition.y;
        _mouseInput.z = _mainCamera.nearClipPlane;
        Vector3 mouseWorldCoordinates = _mainCamera.ScreenToWorldPoint(_mouseInput);
        mouseWorldCoordinates.z = 0f;

        //what I need
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldCoordinates, Time.deltaTime * speed);

        //rotate
        if (mouseWorldCoordinates != transform.position)
        {
            Vector3 targetDirection = mouseWorldCoordinates - transform.position;
            targetDirection.z = 0f;
            transform.up = targetDirection;
        }
    }
}
