

using IngameDebugConsole;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    private Lobby _hostLobby;

    private float heartbeatTimerMax = 15;
    private float heartbeatTimer;

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //DebugLogConsole.AddCommand("lobby.create", "Creates a test lobby", CreateLobby);
        //DebugLogConsole.AddCommand("lobby.list", "List lobbies", ListLobbies);
        //DebugLogConsole.AddCommand("lobby.join_first", "Join first lobby", JoinLobbyFirst);
        //DebugLogConsole.AddCommand<string>("lobby.join_code", "Join lobby by code", JoinLobbyByCode);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "TestLobby";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            _hostLobby = lobby;
            Debug.Log("Lobby created: " + lobby.Name + " " + lobby.Id + " " + lobby.LobbyCode);
            PrintPlayers(_hostLobby);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError("Failed to create lobby: " + e.Message);
        }
    }

    private async void ListLobbies()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyFirst()
    {
        try
        {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void JoinLobbyByCode(string code)
    {
        try
        {
            await Lobbies.Instance.JoinLobbyByCodeAsync(code);
            Debug.Log("Joined lobby with code " + code);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void HandleLobbyHeartbeat()
    {
        if (_hostLobby != null)
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0f)
            {
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in Lobby " + lobby.Name);
        foreach (Unity.Services.Lobbies.Models.Player player in lobby.Players)
        {
            Debug.Log(player.Id);
        }
    }
}
