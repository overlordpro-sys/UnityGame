using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameState.Classes;
using Assets.Scripts.Players;
using UnityEngine;

public class Player : MonoBehaviour {
    // Game Object
    private GameObject PlayerBody;

    // Scripts
    internal PlayerHealth PlayerHealth;
    internal PlayerMovement PlayerMovement;
    internal PlayerBoost PlayerBoost;
    internal PlayerTurret PlayerTurret;

    internal PlayerConfig PlayerConfig;


    public void Awake() {

        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerBoost = GetComponent<PlayerBoost>();
        PlayerTurret = GetComponent<PlayerTurret>();
    }
}
