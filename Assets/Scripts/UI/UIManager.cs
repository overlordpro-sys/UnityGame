using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [Tooltip("The main menu screen")]
    public GameObject mainMenu;
    [Tooltip("The lobby screen")]
    public GameObject lobbyMenu;
    [Tooltip("The Text Input Window")]
    public GameObject inputWindow;

    private void Awake() {
        Instance = this;
        AwakeAll();
        lobbyMenu.SetActive(false);
        inputWindow.SetActive(false);
    }

    public void AwakeAll() {
        mainMenu.SetActive(true);
        lobbyMenu.SetActive(true);
        inputWindow.SetActive(true);
    }

    public void SetLobbyMenu() {
        mainMenu.SetActive(false);
        lobbyMenu.SetActive(true);
    }

    public void SetMainMenu() {
        mainMenu.SetActive(true);
        lobbyMenu.SetActive(false);
    }

    // Update is called once per frame
    private void Update() {

    }
}
