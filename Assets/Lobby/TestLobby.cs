using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour {
    private Lobby _hostLobby;
    // TODO ADD HEART BEAT

    // Start is called before the first frame update
    async void Start() {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in" + AuthenticationService.Instance.PlayerId);
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        DebugLogConsole.AddCommand("lobby.create", "Creates a test lobby", CreateLobby);
        DebugLogConsole.AddCommand("lobby.list", "List lobbies", ListLobbies);
    }

    async void CreateLobby() {
        try {
            string lobbyName = "TestLobby";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);
            _hostLobby = lobby;
            Debug.Log("Lobby created: " + lobby.Name + " " + lobby.Id);
        }
        catch (LobbyServiceException e) {
            Debug.LogError("Failed to create lobby: " + e.Message);
        }
    }


    async void ListLobbies() {
        try {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results) {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException e) {
            Debug.Log(e);
        }
    }


}
