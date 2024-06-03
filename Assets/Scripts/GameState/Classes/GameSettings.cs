using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameState.Classes {
    [System.Serializable]
    public class GameSettings {
        public int PlayerNum { get; set; }
        public static readonly List<Color> PlayerColors = new List<Color>()
            { Color.red, Color.yellow, Color.blue, Color.green, Color.cyan, Color.magenta, Color.gray };

        public GameSettings() {
            PlayerNum = 2;
        }

        public static Color GetNextColor(Color color) {
            return PlayerColors.SkipWhile(x => x != color).Skip(1).DefaultIfEmpty(PlayerColors.First()).FirstOrDefault();
        }

        public static Color GetPreviousColor(Color color) {
            return PlayerColors.TakeWhile(x => x != color).DefaultIfEmpty(PlayerColors.Last()).LastOrDefault();
        }
    }
}