using UnityEngine;

public interface IMoveable {
    void TakeKnockback(float knockbackForce, Transform other);
}
