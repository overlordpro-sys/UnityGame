using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameState.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstance : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerColor;
    [SerializeField] private Button prevColorButton;
    [SerializeField] private Button nextColorButton;
    [SerializeField] private Button renameButton;

    private PlayerConfig playerConfig;


    // Use switch instead because playerVariantIndex keeps getting reset on removal and re adding of players
    private void OnNextColorButton() {
        playerConfig.PlayerColor = GameSettings.GetNextColor(playerConfig.PlayerColor);
        UpdatePlayerGUI(this.playerConfig);

    }

    private void OnPrevColorButton() {
        playerConfig.PlayerColor = GameSettings.GetPreviousColor(playerConfig.PlayerColor);
        UpdatePlayerGUI(this.playerConfig);
    }

    private void OnRenameButton() {
        UI_InputWindow.Show_Static("Player Name", this.playerConfig.PlayerName,
        "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
        () => {
            // Cancel
        }, (string playerName) => {
            this.playerConfig.PlayerName = playerName; 
            UpdatePlayerGUI(this.playerConfig);
        });
    }

    public void InitPlayer(PlayerConfig player) {
        this.playerConfig = player;
        prevColorButton.onClick.AddListener(OnPrevColorButton);
        nextColorButton.onClick.AddListener(OnNextColorButton);
        renameButton.onClick.AddListener(OnRenameButton);
        UpdatePlayerGUI(player);
    }

    public void UpdatePlayerGUI(PlayerConfig player) {
        playerNameText.text = playerConfig.PlayerName;
        playerColor.color = player.PlayerColor;

    }
}
