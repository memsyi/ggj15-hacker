﻿using UnityEngine;

namespace UnitySampleAssets._2D
{

    public class PlatformerCharacter2D : MonoBehaviour
    {
        private bool facingRight = true; // For determining which way the player is currently facing.

        [SerializeField] private float maxSpeed = 10f; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	
		[SerializeField] private float jumpReleaseForce = 20f; // Amount of force added when jump is released.
		[SerializeField] private float jumpReleaseLimit = 0.2f; // Minimum speed for jumpReleaseForce to be added.
		[SerializeField] public float ShotSpeed = 20f;

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character

        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.

		public GameObject shotPrefab;



        private void Awake()
        {
            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();
        }


        private void FixedUpdate()
		{
		    // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
			Collider2D coll = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
			if (coll != null) {
				Transform t = coll.transform;
				Bob bob = null;
				while (t != null) {
					bob = t.GetComponent<Bob>();
					if (bob != null)
						break;
					t = t.parent;
				}
				if (bob != null) {
					transform.parent = bob.transform;
				} else {
					transform.parent = null;
				}
			} else {
				transform.parent = null;
			}
			grounded = coll;
		    anim.SetBool("Ground", grounded);

		    // Set the vertical animation
		    anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		}


        public void Move(float move, bool jump, bool jumpHeld)
        {


            //only control the player if grounded or airControl is turned on
            if (grounded || airControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                rigidbody2D.velocity = new Vector2(move*maxSpeed, rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                    // ... flip the player.
                    Flip();
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                    // ... flip the player.
                    Flip();
            }
            // If the player should jump...
            if (grounded && jump && anim.GetBool ("Ground")) {
				// Add a vertical force to the player.
				grounded = false;
				anim.SetBool ("Ground", false);
				rigidbody2D.AddForce (new Vector2 (0f, jumpForce));
			} else if (!jumpHeld && !grounded && rigidbody2D.velocity.y > 0) {
				Vector2 v = rigidbody2D.velocity;
				rigidbody2D.velocity = new Vector2(v.x, 0);
				rigidbody2D.AddForce (new Vector2 (0f, jumpReleaseForce));
			}
        }

		public void Shoot(bool shoot)
		{
			if (shoot) {
				GameObject shotClone;
				shotClone = (Instantiate(shotPrefab,
				                         transform.position,
				                         transform.rotation) as GameObject);
				Rigidbody2D body = shotClone.GetComponent<Rigidbody2D>();
				body.velocity = new Vector2(ShotSpeed, 0f);
			}
		}
	
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}