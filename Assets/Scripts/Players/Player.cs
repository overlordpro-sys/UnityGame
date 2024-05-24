using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameState.Classes;
using Assets.Scripts.Players;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Game Object
    
    // Scripts
    internal PlayerHealth PlayerHealth;
    internal PlayerMovement PlayerMovement;

    internal PlayerConfig PlayerConfig;


    public void Awake() {

        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerMovement = GetComponent<PlayerMovement>();
    }
}
