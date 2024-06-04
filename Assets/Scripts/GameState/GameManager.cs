using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.GameState.Classes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        public GameSettings GameSettings { get; set; }
        public PlayerData[] PlayersData { get; set; }
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
            float screenWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
            float screenHeight = Camera.main.orthographicSize * 2;

            float xEdge = screenWidth / 2 - 100;
            float yEdge = screenHeight / 2 - 100;

            // Top left, top right, bottom left, bottom right
            spawnPoints[0] = new Vector2(-xEdge, yEdge);
            spawnPoints[1] = new Vector2(xEdge, yEdge);
            spawnPoints[2] = new Vector2(-xEdge, -yEdge);
            spawnPoints[3] = new Vector2(xEdge, -yEdge);
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

            AssignPlayerInputs();
        }

        private void AssignPlayerInputs() {
            for (int i = 0; i < GameManager.Instance.GameSettings.PlayerNum; i++) {
                PlayerData playerData = PlayersData[i];
                // Instantiate player at the appropriate spawn point
                GameObject player = Instantiate(ShipPrefab, spawnPoints[i], Quaternion.identity);

                // Get the PlayerInput component and set the device
                PlayerInput playerInput = player.GetComponent<PlayerInput>();
                playerInput.neverAutoSwitchControlSchemes = true;
                playerInput.SwitchCurrentControlScheme(playerData.PlayerInputDevices.ToArray());

                // Optional: Set other player-specific settings
                //player.GetComponent<PlayerController>().SetupPlayer(PlayersData[i]);
            }
        }


    }
}