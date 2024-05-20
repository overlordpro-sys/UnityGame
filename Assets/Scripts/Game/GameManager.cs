using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

namespace Assets.Scripts.Game {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        public GameSettings GameSettings;

        void Awake() {
            if (Instance == null) {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start() {
            GameSettings = new GameSettings();
        }

        // Update is called once per frame
        void Update() {

        }
    }
}