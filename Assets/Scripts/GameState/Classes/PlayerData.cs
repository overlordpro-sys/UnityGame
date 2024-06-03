using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class PlayerData {
        // Config
        public int PlayerId { get; set; }
        public Color PlayerColor { get; set; }
        public PlayerInputType PlayerInput { get; set; }
        public List<InputDevice> PlayerInputDevices { get; set; }

        // Game Data
        public int PlayerGold { get; set; }
        public int PlayerRoundsWon { get; set; }


        public PlayerData(int playerID, Color color) {
            PlayerId = playerID;
            PlayerColor = color;
            PlayerInputDevices = null;
        }

        public enum PlayerInputType {
            KeyboardMouse,
            Gamepad,
        }
    }
}