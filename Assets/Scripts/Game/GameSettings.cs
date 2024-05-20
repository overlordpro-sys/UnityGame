using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game {
    [System.Serializable]
    public class GameSettings {
        public int PlayerNum;
        public string[] PlayerNames;
        public PlayerCharacter[] PlayerCharacters;

        public GameSettings(int playerNum, string[] playerNames, PlayerCharacter[] playerCharacters) {
            PlayerNum = playerNum;
            PlayerNames = playerNames;
            PlayerCharacters = playerCharacters;
        }

        public GameSettings() {
            PlayerNum = 2;
            PlayerNames = new string[] { "Player", "Player" };
            PlayerCharacters = new PlayerCharacter[] { PlayerCharacter.A, PlayerCharacter.B };
        }
    }
}