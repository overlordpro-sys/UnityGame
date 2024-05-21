using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class PlayerConfig {
        public string PlayerName;
        public Color PlayerColor;

        public PlayerConfig(string playerName, Color color) {
            PlayerName = playerName;
            PlayerColor = color;
        }
    }
}

