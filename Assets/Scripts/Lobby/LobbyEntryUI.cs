using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyEntryUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _playersText;
    [SerializeField] private TextMeshProUGUI _gameModeText;


    private Lobby _lobby;


    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => {
            LobbyManager.Instance.JoinLobby(_lobby);
        });
    }

    public void UpdateLobby(Lobby lobby) {
        this._lobby = lobby;

        _lobbyNameText.text = lobby.Name;
        _playersText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        //_gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value ?? String.Empty;
    }

}
