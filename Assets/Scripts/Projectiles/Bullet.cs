using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public RuntimeAnimatorController DefaultAnimatorController;
    public RuntimeAnimatorController HitAnimatorController;
    private Rigidbody2D rb;
    new private Collider2D collider;
    private Animator animator;

    [SerializeField] private float size = 1;
    [SerializeField] private float speed = 10;
    [SerializeField] private float accel = 0;
    [SerializeField] private float range = 40;
    [SerializeField] private float damage = 20;
    [SerializeField] private float knockBack = 5;

    private List<IBulletModifier> modifiers = new List<IBulletModifier>();


    private Vector2 lastPosition;
    private float totalDistance = 0;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        rb.velocity = transform.right * speed;
        animator.runtimeAnimatorController = DefaultAnimatorController;
        lastPosition = rb.position;
    }

    public void IgnoreOwnerCollision(Collider2D owner) {
        Physics2D.IgnoreCollision(collider, owner);
    }

    // Bullet modifiers
    public void AddModifiers(List<IBulletModifier> modifiers) {
        this.modifiers.AddRange(modifiers);
        ApplyModifiers();
    }
    public void ApplyModifiers() {
        foreach (var modifier in modifiers) {
            modifier.Apply(this);
        }
    }

    private void ApplyAcceleration() {
        rb.velocity += (Vector2)transform.right * accel * Time.deltaTime;
    }

    private void CheckRange() {
        if (totalDistance > range) {
            Destroy(gameObject);
        }
        totalDistance += Vector2.Distance(lastPosition, rb.position);
        lastPosition = rb.position;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            ProcessPlayerCollision(other);
        }
        else if (other.gameObject.CompareTag("Border")) {
            Destroy(gameObject);
        }
    }

    private void ProcessPlayerCollision(Collision2D other) {
        if (other.gameObject.TryGetComponent(out IDamageable damageable)) {
            damageable.TakeDamage(damage);
        }
        if (other.gameObject.TryGetComponent(out IMoveable moveable)) {
            moveable.TakeKnockback(knockBack, this.transform);
        }
        PlayHitAnimation();
        Destroy(gameObject, (float)0.333);
    }

    private void PlayHitAnimation() {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        collider.enabled = false;
        animator.runtimeAnimatorController = HitAnimatorController;
    }

    private void FixedUpdate() {
        ApplyAcceleration();
        CheckRange();
    }
}
