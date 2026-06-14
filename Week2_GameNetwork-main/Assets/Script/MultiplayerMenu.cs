using Unity.Netcode;
using UnityEngine;

public class MultiplayerMenu : MonoBehaviour
{
    private void Start()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void OnClientConnected(ulong clientId)
    {
        // hiding UI for LOCAL client
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnServerStarted()
    {
        gameObject.SetActive(false);
    }

    private void OnClientDisconnected(ulong clientId)
    {
        // will show menu again if you get disconnected
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton == null) return;

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
    }
}