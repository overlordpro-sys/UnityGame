using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable {
    public event EventHandler OnHealthChanged;

    [SerializeField] public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }

    public void TakeDamage(float damageAmount) {
        CurrentHealth-=damageAmount;
        if (CurrentHealth <= 0) {
            CurrentHealth = 0;
            Die();
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ResetHealth() {
        CurrentHealth = MaxHealth;
    }

    public void Die() {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
    }
}
