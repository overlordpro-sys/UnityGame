using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyListPanelUI : MonoBehaviour {
    public static LobbyListPanelUI Instance { get; private set; }


    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryPrefab;

    private void Awake() {
        Instance = this;
        this.Show();
    }

    private void Start() {
        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }


    private void LobbyManager_OnKickedFromLobby(object sender, LobbyManager.LobbyEventArgs e) {
        this.Show();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e) {
        this.Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, LobbyManager.LobbyEventArgs e) {
        this.Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e) {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList) {
        foreach (Transform entry in entryContainer) {
            Destroy(entry.gameObject);
        }

        foreach (Lobby lobby in lobbyList) {
            Transform lobbySingleTransform = Instantiate(entryPrefab, entryContainer);
            lobbySingleTransform.gameObject.SetActive(true);
            LobbyEntryUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyEntryUI>();
            lobbyListSingleUI.UpdateLobby(lobby);
            Debug.Log("Instantiate " + lobby.Name);
        }
    }

    public void CreateLobbyButtonClick() {
        LobbyCreatePanelUI.Instance.Show();
        this.Hide();
    }

    // Helpers
    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        LobbyManager.Instance.RefreshLobbyList();
    }
}
