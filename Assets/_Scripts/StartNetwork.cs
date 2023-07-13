using TMPro;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Helper functions to start the server, host, or client on the NetworkManager.
/// </summary>
public class StartNetwork : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playersCountText;
    [SerializeField] private NetworkVariable<int> playersNum= new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    private void Update()
    {
        playersCountText.text = playersNum.Value.ToString();
        if (!IsServer) return; // only execute when server
        playersNum.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
    public int getCount()
    {
        return playersNum.Value;
    }
}
