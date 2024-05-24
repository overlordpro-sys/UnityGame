using System;

public interface IDamageable {
    public event EventHandler OnHealthChanged;

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
    void TakeDamage(float damageAmount);
    void Die();
}
