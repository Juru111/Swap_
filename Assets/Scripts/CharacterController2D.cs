using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private float grabTime = .5f;
	[SerializeField] private Player player;

	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private LayerMask m_WhatIsPlayer;
	[SerializeField] private LayerMask m_WhatIsItem;

	[SerializeField] private Transform m_GroundCheckA;                          // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_GroundCheckB;                          // Same as above
	[SerializeField] private Transform m_CeilingCheckA;                         // A position marking where to check is there another player above.
	[SerializeField] private Transform m_CeilingCheckB;                         // Same as above
	[SerializeField] private Transform m_GrabCheckA;							// A position marking where to check is there another player above.
	[SerializeField] private Transform m_GrabCheckB;                            // Same as above

	[SerializeField] private PrefabDataBase prefabDataBase;


	const float k_GroundedDepth = .1f;					// Depth of the overlap line (area) to determine if grounded
	const float k_CeilingDepth = .1f;					// Depth of the overlap line (area) to determine is there another player above.
	public bool m_Grounded { private set; get; }        // Whether or not the player is grounded.
	public bool m_IsGoingUp { private set; get; }
	public bool m_IsGrabing { private set; get; }
	private bool m_JumpBloced;                          // Whether or not the player jump is blocked by other player.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;					// For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;



	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnAttackEvent;
	public BoolEvent IsInAirEvent;
	public BoolEvent IsGoingUpInAirEvent;
	private bool m_wasAttacking = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnAttackEvent == null)
			OnAttackEvent = new BoolEvent();

		if (IsInAirEvent == null)
			IsInAirEvent = new BoolEvent();

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
				if (wasGrounded != m_Grounded)
                {
					IsInAirEvent.Invoke(m_Grounded);
				}
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

		m_IsGoingUp = (m_Rigidbody2D.velocity.y >= 0);

	}


	public void Move(float move, bool attack, bool jump, bool grab)
	{
		// If Attacking
		if (attack && !player.isHoldingItem)
		{
			if (!m_wasAttacking)
			{
				m_wasAttacking = true;
				OnAttackEvent.Invoke(true);
				// Freeze player in position
				m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			}

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
				m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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

		// If the player should jump...
		if (m_Grounded && jump && !m_JumpBloced)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

		// If the player should grab...
		if (grab && m_Grounded && !attack)
		{	
			if(player.isHoldingItem)
            {
				//...trow held item
				StartCoroutine(TrowItem(player.itemHeld, player.itemHeldColor));
			}
			else
            {
				//...try grab an item
				StartCoroutine(TryGrabItem());
			}
        }
	}

	public void WindMovement(Direction dir, float windSpeed)
    {
        switch (dir)
        {
            case Direction.Up:
				Vector3 targetVelocity = new Vector2(m_Rigidbody2D.velocity.x, windSpeed * 10f);
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
				break;
            case Direction.Left:
                break;
            case Direction.Right:
                break;
            default:
				Debug.LogError("Wrong wind dirrection!");
                break;
        }
        // Move the character by finding the target velocity
        //Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
		// And then smoothing it out and applying it to the character
		//m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		player.itemHeldIndicator.transform.localScale = theScale;
	}

	private IEnumerator TryGrabItem()
    {
		m_IsGrabing = true;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		
		Collider2D grabCollider = Physics2D.OverlapArea(m_GrabCheckA.position, m_GrabCheckB.position, m_WhatIsItem);
		if(grabCollider != null && grabCollider.TryGetComponent(out Item grabbedItem) && !grabbedItem.isTaken)
        {
			grabbedItem.SetMyTakenStatus(true);
			player.SetHoldingItem(true);
			yield return new WaitForSeconds(grabTime/4);
			grabbedItem.GoToPlayer(player.transform.position, grabTime);
			yield return new WaitForSeconds(grabTime*3/4);
			player.SetMyItem(grabbedItem.MyItemType, grabbedItem.MyItemColor, grabbedItem.mySprite);
			if(grabbedItem.MyItemType == ItemTypes.Marker)
            {
				player.SetHoldingItem(false);
			}

		}
		else
        {
			yield return new WaitForSeconds(grabTime);
		}
		
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		m_IsGrabing = false;
		yield return null;
    }

	private IEnumerator TrowItem(ItemTypes itemType, ItemColors itemColor)
	{
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;

		GameObject instantiatedObject = Instantiate(GiveItemPrefab(itemType, itemColor), transform.position, Quaternion.identity);
		if(instantiatedObject.TryGetComponent(out Item trowedItem))
        {
			trowedItem.TrowMe(m_FacingRight, grabTime);
			player.SetMyItem(ItemTypes.NONE, ItemColors.NONE, null);
		}
		else
        {
			Debug.LogError("Instantiated object isn's an item!");
        }
        yield return new WaitForSeconds(grabTime);

		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		yield return null;
	}

	private GameObject GiveItemPrefab(ItemTypes itemType, ItemColors itemColor)
    {
		switch (itemType)
		{
			case ItemTypes.Armor:
				return prefabDataBase.armor;
			case ItemTypes.Key:
				switch (itemColor)
				{
					case ItemColors.Green:
						return prefabDataBase.key.green;
					case ItemColors.Blue:
						return prefabDataBase.key.blue;
					case ItemColors.Violet:
						return prefabDataBase.key.violet;
					default:
						Debug.LogError("It shouldn't happen");
						return null;
				}
			case ItemTypes.Crystal:
				switch (itemColor)
				{
					case ItemColors.Green:
						return prefabDataBase.crystal.green;
					case ItemColors.Blue:
						return prefabDataBase.crystal.blue;
					case ItemColors.Violet:
						return prefabDataBase.crystal.violet;
					default:
						Debug.LogError("It shouldn't happen");
						return null;
				}
			default:
				Debug.LogError("It shouldn't happen");
				return null;
		}
	}
}
