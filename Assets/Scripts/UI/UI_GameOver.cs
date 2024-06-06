using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

public class UI_GameOver : MonoBehaviour {
    public static UI_GameOver Instance;
    [SerializeField] private TMPro.TextMeshProUGUI titleHeader;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
        SetInactive();
    }

    public void UpdateTitle(string text) {
        titleHeader.text = text;
    }

    public void SetUIActive() {
        gameObject.SetActive(true);
    }

    public void SetInactive() {
        gameObject.SetActive(false);
    }

    public void RestartGame() {
        GameManager.Instance.RestartGame();
        SetInactive();
    }

    public void MainMenu() {
        GameManager.Instance.MainMenu();
        SetInactive();
    }

}
