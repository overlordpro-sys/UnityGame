using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.GameState.Classes;
using UnityEngine;

namespace Assets.Scripts.UI.ScoreHeader {
    public class PlayerScoreHeader : MonoBehaviour {
        public static PlayerScoreHeader Instance;

        [SerializeField] private Transform playerScoreContainer;
        [SerializeField] private Transform playerScoreInstancePrefab;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        public void ClearContainer() {
            foreach (Transform child in playerScoreContainer) {
                Destroy(child.gameObject);
            }
        }

        public void UpdateScores(PlayerData[] playersData) {
            ClearContainer();
            for (int i = 0; i < GameManager.Instance.GameSettings.PlayerNum; i++) {
                PlayerData playerData = playersData[i];
                Transform playerScoreTransform = Instantiate(playerScoreInstancePrefab, playerScoreContainer);
                playerScoreTransform.gameObject.SetActive(true);
                PlayerScoreInstance playerScoreInstance = playerScoreTransform.GetComponent<PlayerScoreInstance>();
                playerScoreInstance.SetPlayerScore("P" + playerData.PlayerId.ToString(), playerData.PlayerRoundsWon);
            }
        }
    }
}
