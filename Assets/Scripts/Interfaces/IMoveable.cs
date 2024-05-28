using UnityEngine;

public interface IMoveable {
    void TakeKnockback(float knockbackForce, Vector2 knockbackDirection);
}
