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

    private List<CardsTemplate> _HandList;
    private Transform _lastCard;
    private Collider2D _collider2D;

    #endregion Private Variables

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        _HandList = new List<CardsTemplate>();
        _lastCard = transform;
        _collider2D = GetComponent<Collider2D>();
        // if (!IsServer) length.OnValueChanged += LengthChangedEvent;
        // If there was another player already in the match, the beginning tails of them won't be updated. These lines check the length of the snake and spawn the tails of the other clients accordingly.
        if (IsOwner) return;
        //for (int i = 0; i < length.Value - 1; ++i)
        //    InstantiateHand();
    }

    public void AddCard(CardsTemplate card)
    {
        Debug.Log("Inadd card");
        _HandList.Add(card);
        Debug.Log(_HandList);
    }
}
