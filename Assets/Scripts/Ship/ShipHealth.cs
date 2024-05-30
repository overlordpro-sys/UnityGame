using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ship {
    public class ShipHealth : MonoBehaviour, IDamageable {
        public event EventHandler OnHealthChanged;

        [field: SerializeField] public float MaxHealth { get; set; }
        public float CurrentHealth { get; set; }

        [SerializeField] private Image healthBar;

        // Start is called before the first frame update
        void Start() {
            ResetHealth();
            OnHealthChanged += OnOnHealthChanged;
        }

        private void OnOnHealthChanged(object sender, EventArgs e) {
            UpdateHealthBar();
        }

        public void TakeDamage(float damageAmount) {
            CurrentHealth -= damageAmount;
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

        public void UpdateHealthBar() {
            healthBar.fillAmount = GetPercentHealth();
        }

        public float GetPercentHealth() {
            return CurrentHealth / MaxHealth;
        }
    }
}