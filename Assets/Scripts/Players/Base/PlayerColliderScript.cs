using Unity.Netcode;
using UnityEngine;

public class PlayerColliderScript : NetworkBehaviour
{
    [SerializeField] private Player _player;

    // Ground
    [SerializeField] private float _groundBoxXOffset; // Offset from player collider box width
    [SerializeField] private float _groundBoxY;
    [SerializeField] private LayerMask _groundLayer;
    public override void OnNetworkSpawn()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal bool IsGrounded()
    {
        return Physics2D.BoxCast(_player.Body.position, new Vector2(_player.Collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY), 0, Vector2.down, _player.Collider.bounds.extents.y, _groundLayer);
    }
}