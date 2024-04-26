using Unity.Netcode;
using UnityEngine;

public class PlayerColliderScript : NetworkBehaviour
{
    [SerializeField] private PlayerBase _playerBase;

    // Ground
    [SerializeField] private float _groundBoxXOffset; // Offset from PlayerBase collider box width
    [SerializeField] private float _groundBoxY;
    [SerializeField] private LayerMask _groundLayer;
    public override void OnNetworkSpawn()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    internal bool IsGrounded()
    {
        return Physics2D.BoxCast(_playerBase.Body.position, new Vector2(_playerBase.Collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY), 0, Vector2.down, _playerBase.Collider.bounds.extents.y, _groundLayer);
    }

    internal void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_playerBase.Body.position + Vector2.down * _playerBase.Collider.bounds.extents.y, new Vector2(_playerBase.Collider.bounds.extents.x * 2 + _groundBoxXOffset, _groundBoxY));
    }
}