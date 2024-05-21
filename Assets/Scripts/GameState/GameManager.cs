using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Assets.Scripts.GameState.Classes;
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

    }
}