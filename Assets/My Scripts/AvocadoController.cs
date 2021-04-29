using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class AvocadoController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D; // The rigidbody
	private  bool m_FacingRight = true; // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero; // The velocity

	int lives = 3; // Amount of lives the player has

	[Header("Events")]
	[Space]

	bool alive = true; // If the player is alive or not

	public CinemachineVirtualCamera vcam; // The camera
	public Transform dead; // The dead body when the player dies
	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	public ParticleSystem Dust; // The particles that the player leaves 
	public AvocadoMovement movement; // The movement script

	public Dead DeadController; // The dead controller script

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
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

	bool flying = false; // detect if the player is in the air 

	public void Move(float move, bool crouch, bool jump, bool climb)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
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
			// The player should not be grounded
			m_Grounded = false;
			//Doing the actual jump
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}


		// If the player is climbing
		if (climb)
        {
			// I we press up arrow key
			if (Input.GetKey(KeyCode.UpArrow)) {
				Vector3 targetVelocity = new Vector2(move * 10f, 7f);
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity/2, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			} else
            {
				if (Input.GetKey(KeyCode.DownArrow))
				{
					Vector3 targetVelocity = new Vector2(move * 10f, -7f);
					m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity / 2, targetVelocity, ref m_Velocity, m_MovementSmoothing);
				}
				else
				{
					m_Rigidbody2D.gravityScale = 0f;
					Vector3 targetVelocity = new Vector2(move * 10f, 0);
					m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity / 2, targetVelocity, ref m_Velocity, m_MovementSmoothing);
				}
			}
        } else
        {
			m_Rigidbody2D.gravityScale = 0.5f;
		}

		// If the player touches the ground dust should appear
		if (flying && m_Grounded)
        {
			CreateDust();
        }
	
		if (!m_Grounded)
        {
			flying = true;
        } else
        {
			flying = false;
        }
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (this.gameObject.active)
		{
			// If the touch the thorns
			if (other.gameObject.tag == "Thorn")
            {
				// If the amount of lives it has is greater than 0
				if (lives > 0)
                {
					// It body bcomes red
					StartCoroutine(FlashRed());
					// If it has three lives, it loses one
					if (lives == 3)
					{
						GameObject.Find("Third Heart").active = false;
					}
					else
					{
						// If it has two lives left, it loses another one
						if (lives == 2) GameObject.Find("Second Heart").active = false;
						else
						{
							// If it has only one life it is dead, then a dead body should appear on its place
							if (lives == 1)
							{
								GameObject.Find("First Heart").active = false;
								DestroyComponent();
								dead.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(transform.up * 5f, ForceMode2D.Impulse);
								dead.gameObject.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(transform.up * 5f, ForceMode2D.Impulse);

								dead.gameObject.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(transform.right * movement.GetHorizontalDirection() * 2f, ForceMode2D.Impulse);
								dead.gameObject.transform.GetChild(1).GetComponent<Rigidbody2D>().AddForce(transform.right * movement.GetHorizontalDirection() * 2f, ForceMode2D.Impulse);

								DeadController.StartFlashRed();
							}
						}
					}
				}
				// Reducing the number of lives 
				lives--;
			}
		}
	}


	// Making a sprite red
	IEnumerator FlashRed()
	{
		SpriteRenderer sprite = this.gameObject.GetComponent<SpriteRenderer>();
		Color red = new Color32(255, 208, 208, 255);
		sprite.color = red;

		//Making a sprite more red in a loop
		for (int i = 209; i <=255; i++)
        {
			sprite.color = new Color32(255, (byte)i, (byte)i, 255);
			yield return new WaitForSeconds(0.0001f);
		}
		sprite.color = Color.white;
	}

	void DestroyComponent()
	{
		// Removes the rigidbody from the game object
		// Sets up the oush of the dead body and its arms
		if (transform.localScale.x < 0)
        {
			dead.localScale = new Vector3(-1f, 1f, 1f);
			GameObject.Find("Dead Left Arm").transform.rotation.eulerAngles.Set(0f, 0f, -30f);

			JointMotor2D nextMotor1 = new JointMotor2D();
			nextMotor1.maxMotorTorque = 0.005f;
			nextMotor1.motorSpeed = -400f;

			JointAngleLimits2D nextLimits1 = new JointAngleLimits2D();
			nextLimits1.max = 60f;
			nextLimits1.min = 30f;

			GameObject.Find("Dead Left Arm").GetComponent<HingeJoint2D>().motor = nextMotor1;
			GameObject.Find("Dead Left Arm").GetComponent<HingeJoint2D>().limits = nextLimits1;

			GameObject.Find("Dead Right Arm").transform.rotation.eulerAngles.Set(0f, 0f, 30f);

			JointMotor2D nextMotor2 = new JointMotor2D();
			nextMotor2.maxMotorTorque = 0.005f;
			nextMotor2.motorSpeed = 400f;

			JointAngleLimits2D nextLimits2 = new JointAngleLimits2D();
			nextLimits2.max = -30f;
			nextLimits2.min = -60f;

			GameObject.Find("Dead Right Arm").GetComponent<HingeJoint2D>().motor = nextMotor2;
			GameObject.Find("Dead Right Arm").GetComponent<HingeJoint2D>().limits = nextLimits2;

		}

		dead.position = transform.position;
		vcam.m_Follow = dead;

		for (int i = 0; i < dead.transform.childCount; i++)
        {
			GameObject currentChild = dead.transform.GetChild(i).gameObject;
			currentChild.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
		}

		alive = false; 
		this.gameObject.active = false;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		CreateDust();
	}

	void CreateDust()
    {
		if (m_Grounded) Dust.Play();
    }

	public bool isFacingRight()
    {
		return m_FacingRight;
    }
}

