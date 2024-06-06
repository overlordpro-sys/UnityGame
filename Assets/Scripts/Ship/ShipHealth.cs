using System;
using System.Collections;
using Assets.Scripts.Game;
using Assets.Scripts.GameState.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Ship {
    public class ShipHealth : MonoBehaviour, IDamageable {
        public event EventHandler OnHealthChanged;

        [SerializeField] private Ship ship;
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
            // Disable physics
            ship.Rigidbody.isKinematic = true;
            ship.Rigidbody.velocity = Vector2.zero;
            ship.Collider.enabled = false;
            ship.Animator.SetTrigger("Die");
            StartCoroutine(DestroyAfterAnimation(ship.Animator));
        }

        IEnumerator DestroyAfterAnimation(Animator animator) {
            // Assuming "Die" is the name of the death animation state
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float waitTime = stateInfo.length;

            // Wait for the animation to finish
            yield return new WaitForSeconds(waitTime);

            // Destroy the player GameObject
            GameManager.Instance.OnPlayerDied(ship.PlayerData);
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