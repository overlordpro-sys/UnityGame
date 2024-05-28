using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public RuntimeAnimatorController DefaultAnimatorController;
    public RuntimeAnimatorController HitAnimatorController;
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Animator animator;
    private GameObject owner;

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
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        rigidbody.velocity = transform.right * speed;
        animator.runtimeAnimatorController = DefaultAnimatorController;
        lastPosition = rigidbody.position;
        ApplyModifiers();
    }

    public void ApplyModifiers() {
        foreach (var modifier in modifiers) {
            modifier.Apply(this);
        }
    }

    public void IgnoreOwnerCollision(Collider2D owner) {
        Physics2D.IgnoreCollision(collider, owner);
    }

    public void AddModifiers(List<IBulletModifier> modifiers) {
        this.modifiers.AddRange(modifiers);
    }

    public void AddModifier(IBulletModifier modifier) {
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
