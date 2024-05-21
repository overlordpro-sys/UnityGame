using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class PlayerConfig {
        public string PlayerName;
        public PlayerCharacter PlayerCharacter;

        public PlayerConfig(string playerName, PlayerCharacter playerCharacter) {
            PlayerName = playerName;
            PlayerCharacter = playerCharacter;
        }
    }
}

