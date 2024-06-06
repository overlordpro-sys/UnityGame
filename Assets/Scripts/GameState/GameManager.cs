using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.Ship;
using Assets.Scripts.GameState.Classes;
using Assets.Scripts.UI.ScoreHeader;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game {
    public class GameManager : MonoBehaviour {
        public event EventHandler GameStart;
        public event EventHandler GameEnd;
        public event EventHandler RoundStart;
        public event EventHandler RoundEnd;
        public delegate void PlayerDeathHandler(PlayerData player);
        public event PlayerDeathHandler PlayerDied;


        public static GameManager Instance { get; private set; }
        public GameSettings GameSettings { get; set; }
        public PlayerData[] PlayersData { get; set; }
        public List<GameObject> AlivePlayers { get; set; } = new List<GameObject>();
        public GameObject ShipPrefab;

        private Vector2[] spawnPoints = new Vector2[4];


        private void Awake() {
            if (Instance == null) {
                Instance = this;
                GameSettings = new GameSettings();
                PlayersData = new PlayerData[4];
                PlayersData[0] = new PlayerData(1, Color.red);
                PlayersData[1] = new PlayerData(2, Color.yellow);
                PlayersData[2] = new PlayerData(3, Color.green);
                PlayersData[3] = new PlayerData(4, Color.blue);
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            InitSpawnpoints();
            GameStart += OnGameStart;
            GameEnd += OnGameEnd;
            RoundStart += OnRoundStart;
            RoundEnd += OnRoundEnd;
            PlayerDied += UpdateAlivePlayers;
        }



        private void InitSpawnpoints() {
            float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
            float screenHeight = Camera.main.orthographicSize * 2;

            float xEdge = screenWidth / 2 - 100;
            float yEdge = screenHeight / 2 - 100;

            // Top left, top right, bottom left, bottom right
            spawnPoints[0] = new Vector2(-xEdge, yEdge);
            spawnPoints[1] = new Vector2(xEdge, -yEdge);
            spawnPoints[2] = new Vector2(-xEdge, -yEdge);
            spawnPoints[3] = new Vector2(xEdge, yEdge);
        }

        public void StartGame() {
            Debug.Log("Starting game with " + GameSettings.PlayerNum + " players");
            foreach (PlayerData player in PlayersData) {
                Debug.Log("Player " + player.PlayerId + " has color " + player.PlayerColor);
            }
            StartCoroutine(StartGameCoroutine());
        }

        private IEnumerator StartGameCoroutine() {
            // Load your game scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level/Scenes/Game");

            // Wait until the scene fully loads
            while (!asyncLoad.isDone) {
                yield return null;
            }

            BoundaryManager.Instance.UpdateBoundaries();
            GameStart.Invoke(this, EventArgs.Empty);
        }

        private void AssignPlayerInputs() {
            for (int i = 0; i < GameManager.Instance.GameSettings.PlayerNum; i++) {
                PlayerData playerData = PlayersData[i];
                // Instantiate player at the appropriate spawn point
                GameObject player = Instantiate(ShipPrefab, spawnPoints[i], Quaternion.identity);

                Ship.Ship playerScript = player.GetComponent<Ship.Ship>();
                playerScript.PlayerData = playerData;

                // Get the PlayerInput component and set the device
                PlayerInput playerInput = player.GetComponent<PlayerInput>();
                playerInput.neverAutoSwitchControlSchemes = true;
                playerInput.SwitchCurrentControlScheme(playerData.PlayerInputDevices.ToArray());

                // Optional: Set other player-specific settings
                AlivePlayers.Add(player);
            }
        }

        private void OnGameStart(object sender, EventArgs e) {
            RoundStart.Invoke(this, EventArgs.Empty);
        }

        private void OnGameEnd(object sender, EventArgs e) {
            UI_GameOver.Instance.SetUIActive();
        }

        private void OnRoundStart(object sender, EventArgs e) {
            AssignPlayerInputs();
            PlayerScoreHeader.Instance.UpdateScores(PlayersData);
        }

        private void OnRoundEnd(object sender, EventArgs e) {
            foreach (GameObject player in AlivePlayers) {
                Destroy(player);
            }
            AlivePlayers.Clear();

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets) {
                Destroy(bullet);
            }

            // Check if any players have 3 rounds won
            bool gameEnd = false;
            foreach (PlayerData player in PlayersData) {
                if (player.PlayerRoundsWon == 3) {
                    Debug.Log("Player " + player.PlayerId + " wins the game!");
                    // End the game
                    gameEnd = true;
                    UI_GameOver.Instance.UpdateTitle("Game Over: Player " + player.PlayerId + " Wins!");
                    GameEnd.Invoke(this, EventArgs.Empty);
                }
            }

            if (!gameEnd) {
                RoundStart.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateAlivePlayers(PlayerData deadPlayer) {
            Debug.Log("Player " + deadPlayer.PlayerId + " died");
            foreach (GameObject player in AlivePlayers) {
                if (player.GetComponent<Ship.Ship>().PlayerData.PlayerId == deadPlayer.PlayerId) {
                    AlivePlayers.Remove(player);
                    break;
                }
            }

            if (AlivePlayers.Count == 1) {
                PlayerData winner = AlivePlayers[0].GetComponent<Ship.Ship>().PlayerData;
                Debug.Log("Player " + winner.PlayerId + " wins the round!");
                PlayersData[winner.PlayerId - 1].PlayerRoundsWon++;
                RoundEnd.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnPlayerDied(PlayerData player) {
            PlayerDied?.Invoke(player);
        }

        private void ResetPlayerScores() {
            foreach (PlayerData player in PlayersData) {
                player.PlayerRoundsWon = 0;
            }
        }

        public void RestartGame() {
            ResetPlayerScores();
            RoundStart.Invoke(this, EventArgs.Empty);
        }

        public void MainMenu() {
            ResetPlayerScores();
            SceneManager.LoadScene("MainMenu");
        }
    }
}