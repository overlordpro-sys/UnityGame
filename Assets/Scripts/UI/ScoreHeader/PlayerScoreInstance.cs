using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.ScoreHeader {
    public class PlayerScoreInstance : MonoBehaviour {
        [SerializeField] private TMPro.TextMeshProUGUI playerName;
        [SerializeField] private TMPro.TextMeshProUGUI playerScore;

        public void SetPlayerScore(string name, int score) {
            playerName.text = name;
            playerScore.text = score.ToString();
        }
    }
}
