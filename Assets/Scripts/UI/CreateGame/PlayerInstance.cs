using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameState.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstance : MonoBehaviour {
    [SerializeField] private Animator playerVariantAnimator;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Button prevCharacterButton;
    [SerializeField] private Button nextCharacterButton;
    [SerializeField] private Button renameButton;

    private PlayerConfig playerConfig;


    // Use switch instead because playerVariantIndex keeps getting reset on removal and re adding of players
    private void OnNextCharacterButton() {
        playerConfig.PlayerCharacter = PlayerCharacter.GetNextCharacter(this.playerConfig.PlayerCharacter); // Changes player character and invokes lobby update
        UpdatePlayerGUI(this.playerConfig);

    }

    private void OnPrevCharacterButton() {
        playerConfig.PlayerCharacter = PlayerCharacter.GetPreviousCharacter(this.playerConfig.PlayerCharacter); // Changes player character and invokes lobby update
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
        prevCharacterButton.onClick.AddListener(OnPrevCharacterButton);
        nextCharacterButton.onClick.AddListener(OnNextCharacterButton);
        renameButton.onClick.AddListener(OnRenameButton);
        UpdatePlayerGUI(player);
    }

    public void UpdatePlayerGUI(PlayerConfig player) {
        playerNameText.text = playerConfig.PlayerName;
        playerVariantAnimator.runtimeAnimatorController =
    Resources.Load(player.PlayerCharacter.ResourcePath) as RuntimeAnimatorController;
        playerVariantAnimator.Play("Base Layer.Run");

    }
}
