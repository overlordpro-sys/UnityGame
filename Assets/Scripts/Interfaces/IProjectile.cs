using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    float Speed { get; set; }
    float Damage { get; set; }
    float KnockBack { get; set; }
    void Move();
    void Hit();
}
