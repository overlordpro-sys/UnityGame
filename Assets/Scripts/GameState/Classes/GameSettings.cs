using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class GameSettings {
        public int PlayerNum;
        public PlayerConfig[] PlayerConfigs;

        public GameSettings() {
            PlayerNum = 2;
            PlayerConfigs = new PlayerConfig[4];
            PlayerConfigs[0] = new PlayerConfig("Player 1", PlayerCharacter.A);
            PlayerConfigs[1] = new PlayerConfig("Player 2", PlayerCharacter.B);
            PlayerConfigs[2] = new PlayerConfig("Player 3", PlayerCharacter.C);
            PlayerConfigs[3] = new PlayerConfig("Player 4", PlayerCharacter.D);
        }

        public void SetNumPlayers(int numPlayers) {
            PlayerNum = numPlayers;
        }
    }
}