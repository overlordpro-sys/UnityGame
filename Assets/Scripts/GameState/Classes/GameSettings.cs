using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class GameSettings {
        public int PlayerNum;
        public PlayerConfig[] PlayerConfigs;
        public static readonly List<Color> PlayerColors = new List<Color>()
            { Color.red, Color.yellow, Color.blue, Color.green, Color.cyan, Color.magenta, Color.gray };

        public GameSettings() {
            PlayerNum = 2;
            PlayerConfigs = new PlayerConfig[4];
            PlayerConfigs[0] = new PlayerConfig("Player 1", Color.red);
            PlayerConfigs[1] = new PlayerConfig("Player 2", Color.yellow);
            PlayerConfigs[2] = new PlayerConfig("Player 3", Color.green);
            PlayerConfigs[3] = new PlayerConfig("Player 4", Color.blue);
        }

        public void SetNumPlayers(int numPlayers) {
            PlayerNum = numPlayers;
        }

        public static Color GetNextColor(Color color) {
            return PlayerColors.SkipWhile(x => x != color).Skip(1).DefaultIfEmpty(PlayerColors.First()).FirstOrDefault();
        }

        public static Color GetPreviousColor(Color color) {
            return PlayerColors.TakeWhile(x => x != color).DefaultIfEmpty(PlayerColors.Last()).LastOrDefault();
        }
    }
}