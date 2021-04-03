using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_AttackSpeed = .36f;			// Amount of maxSpeed applied to Attacking movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character                  
	[SerializeField] private Transform m_GroundCheckA;                          // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckB;							// Same as above
	[SerializeField] private Collider2D m_AttackDisableCollider;				// A collider that will be disabled when Attacking

	const float k_GroundedDepth = .1f;  // Depth of the overlap line (area) to determine if grounded
	public bool m_Grounded { private set; get; }            // Whether or not the player is grounded.
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
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a areacast to the groundcheck area hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheckA.position, m_GroundCheckB.position, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool Attack, bool jump)
	{
		// If Attacking, check to see if the character can stand up
		//if (!Attack)
		//{
		//	 If the character has a ceiling preventing them from standing up, keep them Attacking
		//	if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
		//	{
		//		Attack = true;
		//	}
		//}

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

				// Reduce the speed by the AttackSpeed multiplier
				move *= m_AttackSpeed;

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
		if (m_Grounded && jump)
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
