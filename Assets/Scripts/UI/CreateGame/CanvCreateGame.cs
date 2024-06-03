using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.GameState.Classes;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI {
    public class PanelCreateGame : MonoBehaviour {
        [SerializeField] private Transform playerContainer;
        [SerializeField] private Transform playerInstancePrefab;

        public void Start() {
            UpdateNumPlayers(GameManager.Instance.GameSettings.PlayerNum);
        }

        public void UpdatePlayersFromSlider(Slider slider) {
            UpdateNumPlayers((int)slider.value);
        }

        public void UpdateNumPlayers(int numPlayers) {
            GameSettings settings = GameManager.Instance.GameSettings;
            // Remove excess players
            ClearPlayers();

            // Add new players until new numPlayers
            for (int i = 0; i < numPlayers; i++) {
                Transform playerSingleTransform = Instantiate(playerInstancePrefab, playerContainer);
                playerSingleTransform.gameObject.SetActive(true);
                PlayerInstance lobbyPlayerSingleUI = playerSingleTransform.GetComponent<PlayerInstance>();

                lobbyPlayerSingleUI.InitPlayer(GameManager.Instance.PlayersData[i]);
            }
            settings.PlayerNum = numPlayers;

        }

        private void ClearPlayers() {
            foreach (Transform child in playerContainer) {
                Destroy(child.gameObject);
            }
        }

    }
}

