using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameState.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInstance : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private Image playerColor;
    [SerializeField] private TMP_Dropdown playerShipDropdown;
    [SerializeField] private Button prevColorButton;
    [SerializeField] private Button nextColorButton;

    private PlayerData _playerData;


    private void Start() {
        // When device added or removed, update the dropdown
        InputSystem.onDeviceChange += (device, change) => {
            UpdateDropdown();
        };
    }

    // Use switch instead because playerVariantIndex keeps getting reset on removal and re adding of players
    private void OnNextColorButton() {
        _playerData.PlayerColor = GameSettings.GetNextColor(_playerData.PlayerColor);
        UpdatePlayerGUI(this._playerData);


    }

    private void OnPrevColorButton() {
        _playerData.PlayerColor = GameSettings.GetPreviousColor(_playerData.PlayerColor);
        UpdatePlayerGUI(this._playerData);
    }


    public void InitPlayer(PlayerData player) {
        this._playerData = player;
        _playerData.PlayerInputDevices = new List<InputDevice>() { Keyboard.current, Mouse.current };
        _playerData.PlayerInput = PlayerData.PlayerInputType.KeyboardMouse;
        prevColorButton.onClick.AddListener(OnPrevColorButton);
        nextColorButton.onClick.AddListener(OnNextColorButton);
        UpdatePlayerGUI(player);
    }



    public void DropdownValueChanged(TMP_Dropdown dropdown) {
        int index = dropdown.value;
        if (index == 0) {
            _playerData.PlayerInputDevices = new List<InputDevice>() { Keyboard.current, Mouse.current };
            _playerData.PlayerInput = PlayerData.PlayerInputType.KeyboardMouse;
            Debug.Log("KeyboardMouse");
        }
        else {
            _playerData.PlayerInputDevices = new List<InputDevice>() { Gamepad.all[index - 1] };
            _playerData.PlayerInput = PlayerData.PlayerInputType.Gamepad;
            Debug.Log(Gamepad.all[index - 1].device.displayName);
        }
    }


    public void UpdatePlayerGUI(PlayerData player) {
        playerNameText.text = "Player " + _playerData.PlayerId;
        playerColor.color = player.PlayerColor;
        UpdateDropdown();
    }

    private void UpdateDropdown() {
        playerShipDropdown.ClearOptions();
        playerShipDropdown.options.Add(new TMP_Dropdown.OptionData("1. KeyboardMouse"));
        for (int i = 0; i < Gamepad.all.Count; i++) {
            playerShipDropdown.options.Add(new TMP_Dropdown.OptionData((i + 2) + ". " + Gamepad.all[i].device.displayName));
        }
        playerShipDropdown.value = 0;
        playerShipDropdown.RefreshShownValue();
    }
}