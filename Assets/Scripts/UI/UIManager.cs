using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Screens")]
    [Tooltip("The main menu screen")]
    public GameObject mainMenu;
    [Tooltip("The lobby screen")]
    public GameObject lobbyMenu;
    void Awake()
    {
        Instance = this;
        mainMenu.SetActive(true);
        lobbyMenu.SetActive(false);
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
    void Update()
    {
        
    }
}
