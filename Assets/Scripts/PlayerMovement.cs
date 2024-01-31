using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
   public Rigidbody2D body;
   [SerializeField] private float moveSpeed;
   private bool isGrounded;
   // Start is called before the first frame update
   void Start() {
   }

   // Update is called once per frame
   void Update() {
      body.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, body.velocity.y);
      if (Input.GetKey(KeyCode.Space) && isGrounded) {
         body.velocity = new Vector2(body.velocity.x, 5);
      }
   }
   void OnCollisionEnter2D(Collision2D other) {
      if (other.gameObject.tag == "Ground") {
         isGrounded = true;
      }
   }

   void OnCollisionExit2D(Collision2D other) {
      if (other.gameObject.tag == "Ground") {
         isGrounded = false;
      }
   }
}
