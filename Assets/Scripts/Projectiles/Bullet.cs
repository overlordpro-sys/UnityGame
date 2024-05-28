using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public RuntimeAnimatorController DefaultAnimatorController;
    public RuntimeAnimatorController HitAnimatorController;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Animator animator;

    [SerializeField] private float size;
    [SerializeField] private float speed;
    [SerializeField] private float accel;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float knockBack;

    private List<IBulletModifier> modifiers = new List<IBulletModifier>();


    private Vector2 lastPosition;
    private float totalDistance = 0;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        rigidbody.velocity = transform.right * speed;
        animator.runtimeAnimatorController = DefaultAnimatorController;
        lastPosition = rigidbody.position;
        ApplyModifiers();
    }

    private void ApplyModifiers() {
        foreach (var modifier in modifiers) {
            modifier.Apply(this);
        }
    }

    private void AddModifiers(List<IBulletModifier> modifiers) {
        this.modifiers.AddRange(modifiers);
    }

    private void AddModifier(IBulletModifier modifier) {
        this.modifiers.Add(modifier);
    }

    private void HandleAcceleration() {
        rigidbody.velocity += (Vector2)transform.right * accel * Time.deltaTime;
    }

    private void HandleRange() {
        if (totalDistance > range) {
            Destroy(gameObject);
        }
        totalDistance += Vector2.Distance(lastPosition, rigidbody.position);
        lastPosition = rigidbody.position;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (other.gameObject.TryGetComponent(out IDamageable damageable)) {
                damageable.TakeDamage(damage);
            }
            if (other.gameObject.TryGetComponent(out IMoveable moveable)) {
                moveable.TakeKnockback(knockBack, this.transform);
            }
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector2.zero;
            collider.enabled = false;
            animator.runtimeAnimatorController = HitAnimatorController;
            Destroy(gameObject, (float)0.333);
        }
        else if (other.gameObject.CompareTag("Border")) {
            Destroy(gameObject);
        }

    }

    private void FixedUpdate() {
        HandleAcceleration();
        HandleRange();
    }

    private void OnDestroy() {
        // Clean up & Animation
    }
}
