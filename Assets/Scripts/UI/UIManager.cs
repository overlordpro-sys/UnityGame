using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [Tooltip("The main menu screen")]
    public GameObject mainMenu;
    [Tooltip("The lobby screen")]
    public GameObject createGameMenu;
    [Tooltip("The Text Input Window")]
    public GameObject inputWindow;

    private void Awake() {
        Instance = this;
        AwakeAll();
        createGameMenu.SetActive(false);
        inputWindow.SetActive(false);
    }

    public void AwakeAll() {
        mainMenu.SetActive(true);
        createGameMenu.SetActive(true);
        inputWindow.SetActive(true);
    }

    public void SetCreateGameMenu() {
        mainMenu.SetActive(false);
        createGameMenu.SetActive(true);
    }

    public void SetMainMenu() {
        mainMenu.SetActive(true);
        createGameMenu.SetActive(false);
    }

    public void StartGame() {
        GameManager.Instance.StartGame();
    }

    public void ExitGame() {
        Application.Quit();
    }

    // Update is called once per frame
    private void Update() {

    }
}
