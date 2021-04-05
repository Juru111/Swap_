using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsPlayer;
	[SerializeField] private Transform m_GroundCheckA;                          // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckB;                          // Same as above
	[SerializeField] private Transform m_CeilingCheckA;                         // A position marking where to check is there another player above.
	[SerializeField] private Transform m_CeilingCheckB;                         // Same as above

	[SerializeField] private Collider2D m_AttackDisableCollider;				// A collider that will be disabled when Attacking

	const float k_GroundedDepth = .1f;  // Depth of the overlap line (area) to determine if grounded
	const float k_CeilingDepth = .1f;  // Depth of the overlap line (area) to determine is there another player above.
	public bool m_Grounded { private set; get; }            // Whether or not the player is grounded.
	private bool m_JumpBloced;           // Whether or not the player jump is blocked by other player.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnAttackEvent;
	private bool m_wasAttacking = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnAttackEvent == null)
			OnAttackEvent = new BoolEvent();

		m_GroundCheckB.position += new Vector3(0f, k_GroundedDepth, 0f); // Adding depth to line of ground check => area check
		m_CeilingCheckB.position -= new Vector3(0f, k_CeilingDepth, 0f); // Adding depth to line of celling check => area check
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a areacast to the groundcheck area hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] groundColliders = Physics2D.OverlapAreaAll(m_GroundCheckA.position, m_GroundCheckB.position, m_WhatIsGround);
		for (int i = 0; i < groundColliders.Length; i++)
		{
			if (groundColliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}



		m_JumpBloced = false;

		// The player is JumpBloced if a areacast to the cellingcheck area hits anything designated as player
		Collider2D[] cellingColliders = Physics2D.OverlapAreaAll(m_CeilingCheckA.position, m_CeilingCheckB.position, m_WhatIsPlayer);
		for (int i = 0; i < cellingColliders.Length; i++)
		{
			if (cellingColliders[i].gameObject != gameObject)
			{
				m_JumpBloced = true;
			}
		}
	}


	public void Move(float move, bool Attack, bool jump)
	{
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// If Attacking
			if (Attack)
			{
				if (!m_wasAttacking)
				{
					m_wasAttacking = true;
					OnAttackEvent.Invoke(true);
				}

				// Freeze player in position
				m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

				//// Disable one of the colliders when Attacking
				//if (m_AttackDisableCollider != null)
				//	m_AttackDisableCollider.enabled = false;
			} else
			{
				//// Enable the collider when not Attacking
				//if (m_AttackDisableCollider != null)
				//	m_AttackDisableCollider.enabled = true;

				if (m_wasAttacking)
				{
					m_wasAttacking = false;
					OnAttackEvent.Invoke(false);
				}

				m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump && !m_JumpBloced)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
