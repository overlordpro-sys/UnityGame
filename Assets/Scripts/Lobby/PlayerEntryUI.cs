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

    private PlayerCharacter character;

    private Player player;


    // Use switch instead because playerVariantIndex keeps getting reset on removal and re adding of players
    private void OnNextCharacterButton() {
        LobbyManager.Instance.UpdatePlayerCharacter(PlayerCharacter.GetNextCharacter(character)); // Changes player character and invokes lobby update

    }

    private void OnPrevCharacterButton() {
        LobbyManager.Instance.UpdatePlayerCharacter(PlayerCharacter.GetPreviousCharacter(character)); // Changes player character and invokes lobby update
    }

    private void OnKickPlayerButton() {
        LobbyManager.Instance.KickPlayer(player.Id);
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
            if (LobbyManager.Instance.IsLobbyHost()) {
                SetKickPlayerButtonVisible(true);
                kickPlayerButton.onClick.AddListener(OnKickPlayerButton);
            }
        }
        this.player = player;
        playerNameText.text = player.Data[LobbyManager.KEY_PLAYER_NAME].Value;
        character = PlayerCharacter.GetPlayerCharacter(player.Data[LobbyManager.KEY_PLAYER_CHARACTER].Value);
        playerVariantAnimator.runtimeAnimatorController = Resources.Load(character.ResourcePath) as RuntimeAnimatorController;
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
