using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreatePanelUI : MonoBehaviour {
    public static LobbyCreatePanelUI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI lobbyNameInput;
    [SerializeField] private Slider lobbyPlayersSlider;
    [SerializeField] private TextMeshProUGUI lobbyPlayersSelectedText;


    private string _lobbyName;
    private int _lobbyPlayers;

    public LobbyCreatePanelUI() {

    }

    private void Awake() {
        Instance = this;
        this.Show();
    }

    private void Start() {
        this.Hide();
    }

    public void CreateLobbyClick() {
        LobbyManager.Instance.CreateLobby(lobbyNameInput.text, (int)lobbyPlayersSlider.value, false);
        this.Hide();
    }

    public void CancelClick() {
        this.Hide();
        LobbyListPanelUI.Instance.Show();
    }

    public void ResetFields() {
        lobbyNameInput.text = "";
        lobbyPlayersSlider.value = 2;
    }

    public void SetSelectedPlayers(Slider slider) {
        lobbyPlayersSelectedText.text = slider.value.ToString();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
        ResetFields();
    }
}
