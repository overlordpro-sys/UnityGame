using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody2D rigidbody;
    private Collider2D collider;

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
        rigidbody.velocity = transform.right * speed;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.TryGetComponent(out IDamageable damageable)) {
            damageable.TakeDamage(damage);
        }

        if (collision.gameObject.TryGetComponent(out IMoveable moveable)) {
            moveable.TakeKnockback(knockBack, rigidbody.velocity.normalized);
        }
        Destroy(gameObject);
    }


    private void FixedUpdate() {
        HandleAcceleration();
        HandleRange();
    }
}
