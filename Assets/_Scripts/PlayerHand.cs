using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHand : NetworkBehaviour
{
    [SerializeField, Tooltip("The tail prefab that will be spawned when the player eats the food.")] private GameObject frontSide;
    // Synced Variable that keeps track of the player snake length.
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    // Called when the length has changed. This is more for local client-side updates.
    public static event System.Action<ushort> ChangedLengthEvent;

    #region Private Variables

    private List<GameObject> _HandList;
    private Transform _lastCard;
    private Collider2D _collider2D;

    #endregion Private Variables

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _HandList = new List<GameObject>();
        _lastCard = transform;
        _collider2D = GetComponent<Collider2D>();
        if (!IsServer) length.OnValueChanged += LengthChangedEvent;
        // If there was another player already in the match, the beginning tails of them won't be updated. These lines check the length of the snake and spawn the tails of the other clients accordingly.
        if (IsOwner) return;
        for (int i = 0; i < length.Value - 1; ++i)
            InstantiateTail();
    }
    /// <summary>
    /// Called when the NetworkVariable length has changed.
    /// </summary>
    /// <param name="previousValue">Mandatory callback parameter. Not used.</param>
    /// <param name="newValue">Mandatory callback parameter. Not used.</param>
    private void LengthChangedEvent(ushort previousValue, ushort newValue)
    {
        Debug.Log("LengthChanged Callback");
        LengthChanged();
    }
    /// <summary>
    /// Called when the NetworkVariable length has changed.
    /// Instantiates tails on the other clients to be synchronized.
    /// </summary>
    private void LengthChanged()
    {
        Debug.Log("In lengthCahge");
        InstantiateTail();
        if (!IsOwner) return;
        ChangedLengthEvent?.Invoke(length.Value);
    }

    /// <summary>
    /// When the player is despawned, destroy the remaining tails.
    /// </summary>
    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
        DestroyTails();
    }

    /// <summary>
    /// Destroys the tails from the scene.
    /// </summary>
    private void DestroyTails()
    {
        while (_HandList.Count != 0)
        {
            GameObject tail = _HandList[0];
            _HandList.RemoveAt(0);
            Destroy(tail);
        }
    }

    /// <summary>
    /// Adds a length to the NetworkVariable.
    /// This will only be called on the server.
    /// </summary>
    // [ServerRpc(RequireOwnership = false)]
    [ContextMenu(itemName:"Add Card")]
    public void AddLength()
    {
        length.Value += 1;
        LengthChanged();
    }

    /// <summary>
    /// Creates a new tail object.
    /// </summary>
    private void InstantiateTail()
    {
        GameObject tailGameObject = Instantiate(frontSide, transform.position, Quaternion.identity);
        tailGameObject.GetComponent<SpriteRenderer>().sortingOrder = -length.Value;
        _HandList.Add(tailGameObject);
    }
}
