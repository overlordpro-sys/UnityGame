using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanelUI : MonoBehaviour {
    public static LobbyPanelUI Instance { get; private set; }


    [SerializeField] private Transform _playerEntryContainer;
    [SerializeField] private Transform _playerEntryPrefab;

    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _playerCountText;

    [SerializeField] private Button _startButton;
    private void Awake() {
        Instance = this;
        this.Show();
    }

    private void Start() {
        LobbyManager.Instance.OnJoinedLobby += UpdateLobby_Event;
        LobbyManager.Instance.OnJoinedLobbyUpdate += UpdateLobby_Event;
        LobbyManager.Instance.OnLobbyGameModeChanged += UpdateLobby_Event;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnLeftLobby;

        Hide();
    }

    private void LobbyManager_OnLeftLobby(object sender, System.EventArgs e) {
        ClearLobby();
        Hide();
    }

    private void UpdateLobby_Event(object sender, LobbyManager.LobbyEventArgs e) {
        UpdateLobby();
    }

    private void UpdateLobby() {
        _startButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());
        UpdateLobby(LobbyManager.Instance.GetJoinedLobby());
    }

    private void UpdateLobby(Lobby lobby) {
        ClearLobby();

        foreach (Player player in lobby.Players) {
            Transform playerSingleTransform = Instantiate(_playerEntryPrefab, _playerEntryContainer);
            playerSingleTransform.gameObject.SetActive(true);
            PlayerEntryUI lobbyPlayerSingleUI = playerSingleTransform.GetComponent<PlayerEntryUI>();

            lobbyPlayerSingleUI.SetKickPlayerButtonVisible(
                LobbyManager.Instance.IsLobbyHost() &&
                player.Id != AuthenticationService.Instance.PlayerId // Don't allow kick self
            );

            lobbyPlayerSingleUI.UpdatePlayer(player);
        }

        //changeGameModeButton.gameObject.SetActive(LobbyManager.Instance.IsLobbyHost());

        _lobbyNameText.text = lobby.Name;
        _playerCountText.text = lobby.Players.Count + "/" + lobby.MaxPlayers;
        //gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;

        Show();
    }
    private void ClearLobby() {
        foreach (Transform child in _playerEntryContainer) {
            Destroy(child.gameObject);
        }
    }

    // Helpers
    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }
}
