using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class PlayerEntryUI : MonoBehaviour {
    [SerializeField] private Animator playerVariantAnimator;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button renameButton;
    [SerializeField] private Button kickPlayerButton;

    private int playerVariantIndex = 0;

    private Player player;


    private void OnNextCharacterButton() {
        playerVariantIndex = (playerVariantIndex + 1) % Enum.GetValues(typeof(PlayerCharacter)).Length;
        LobbyManager.Instance.UpdatePlayerCharacter((PlayerCharacter)Enum.GetValues(typeof(PlayerCharacter)).GetValue(playerVariantIndex)); // Changes player character and invokes lobby update
    }

    private void OnPrevCharacterButton() {
        playerVariantIndex = (playerVariantIndex - 1 + Enum.GetValues(typeof(PlayerCharacter)).Length) % Enum.GetValues(typeof(PlayerCharacter)).Length;
        LobbyManager.Instance.UpdatePlayerCharacter((PlayerCharacter)Enum.GetValues(typeof(PlayerCharacter)).GetValue(playerVariantIndex));
    }


    public void UpdatePlayer(Player player) {
        if (player.Id == AuthenticationService.Instance.PlayerId) {
            prevCharacterButton.onClick.AddListener(OnPrevCharacterButton);
            nextCharacterButton.onClick.AddListener(OnNextCharacterButton);
            renameButton.onClick.AddListener(() => {
                UI_InputWindow.Show_Static("Player Name", player.Data[LobbyManager.KEY_PLAYER_NAME].Value, "abcdefghijklmnopqrstuvxywzABCDEFGHIJKLMNOPQRSTUVXYWZ .,-", 20,
                () => {
                    // Cancel
                },
                (string playerName) => {
                    LobbyManager.Instance.UpdatePlayerName(playerName);
                });
            });
        }
        else {
            SetIsPlayerButtonsVisible(false);
        }
        this.player = player;
        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        playerVariantAnimator.runtimeAnimatorController = Resources.Load(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value) as RuntimeAnimatorController;
        playerVariantAnimator.Play("Base Layer.Run");
    }

    private void SetIsPlayerButtonsVisible(bool visible) {
        prevCharacterButton.gameObject.SetActive(visible);
        nextCharacterButton.gameObject.SetActive(visible);
        renameButton.gameObject.SetActive(visible);

    }

    public void SetKickPlayerButtonVisible(bool visible) {
        kickPlayerButton.gameObject.SetActive(visible);
    }
}
