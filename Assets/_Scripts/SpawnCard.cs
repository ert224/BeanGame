using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class SpawnCard : NetworkBehaviour
{
    public Transform networkedOwner;
    // The Transform object that this tail is following
    public Transform followTransform;
    [SerializeField, Tooltip("Duration of the lerp")] private float lerpDuration = 0.1f; // Adjust this value
    public Vector3 _targetPosition;

    public Vector3  velocity = Vector3.zero;

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();
    public void SetTargetLocation(Vector3 target)
    {
        _targetPosition = target;
    }

    private float increTime = 0;
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, increTime / lerpDuration);
        increTime += Time.deltaTime;
        if (!IsOwner) return;
        UpdatePlayerPositionServerRpc(transform.position.x, transform.position.y, 0);
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerPositionServerRpc(float xPos, float yPos, float zPos)
    {
        Position.Value = new Vector3(xPos, yPos, zPos); 

    }
}
