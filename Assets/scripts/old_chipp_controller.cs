

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using UnityEngine;



public class old_chipp_controller : MonoBehaviour
{
	[SerializeField] private LayerMask platformLayerMask;
	private enum CharState { Normal, Dashing, Jump, Tumble, AirDash, Block, Crouch, Walk }
	CharState state = CharState.Normal;

	private enum AirDashDirection { Right, Left, Neutral }
	private enum AirDashState { NA, Ready }
	AirDashDirection airDashDirection;
	AirDashState airDashState = AirDashState.Ready;
	static int DASHFRAMES = 0;
	static int AIRDASHFRAMES = 25;
	static float AIRDASHSPEED = .07f;
	static float AIRDASHVELOCITY = 10f;
	static int PREWALKFRAMES = 2;
	static float WALKSPEED = .07f;
	static float MAXAIRDRIFTSPEED = 10f;
	static float AIRDRIFTMULTIPLIER = .4f;
	int prewalkframes = PREWALKFRAMES;
	int airDashFrames = AIRDASHFRAMES;
	int dashFrames = DASHFRAMES;

	//private Player_Base playerBase;
	private Rigidbody2D rigidbody2d;

	private Rigidbody2D otherPlayer;
	private BoxCollider2D boxCollider2d;
	float dirX, dirY, moveSpeed, jumpSpeed, airDashSpeed;
	float distance = 0;
	Animator anim;


	// variable to hold a reference to our SpriteRenderer component
	private SpriteRenderer mySpriteRenderer;



	// Use this for initialization
	void Start()
	{
		Application.targetFrameRate = 60;
		anim = GetComponent<Animator>();
		moveSpeed = 15f;
		jumpSpeed = 20f;
		airDashSpeed = AIRDASHSPEED;
		
	}



	// This function is called just one time by Unity the moment the component loads
	private void Awake()
	{
		// get a reference to the SpriteRenderer component on this gameObject
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		//playerBase = gameObject.GetComponent<Player_Base>();
		rigidbody2d = transform.GetComponent<Rigidbody2D>();

		boxCollider2d = transform.GetComponent<BoxCollider2D>();
		

	}

	/*
	void LateUpdate()
	{
		targetPos = target.position;
		thisPos = transform.position;
		targetPos.x = targetPos.x - thisPos.x;
		targetPos.y = targetPos.y - thisPos.y;
		angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 1));
	}
	*/

	// Update is called once per frame
	void Update()
	{
		
		switch (state)
		{
			case CharState.Normal:

				UnityEngine.Debug.Log("normal state");

				if (dash()) { }
				// jumping is allowed, handles checking for jumps as well
				else if (jump()) { }
				// running is allowed, handles checking for dashes as well
				else if (isWalk()) { }

				dirX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

				//transform.position = new Vector2(transform.position.x + dirX, transform.position.y);

				break;

			case CharState.Dashing:

				if (jump()) { UnityEngine.Debug.Log("just from a dash"); }
				else
				{

					isDashing();
					dash();
				}





				//else if()
				break;


			case CharState.Jump:

				aerialDrift();
				if (IsGrounded() && rigidbody2d.velocity.y <= 0)
				{
					UnityEngine.Debug.Log("jump is grounded");
					state = CharState.Normal;
					airDashState = AirDashState.Ready;
					anim.SetBool("isJumping", false);
					anim.SetBool("isGrounded", true);

				}
				else if (Input.GetButtonDown("RButton") && airDashState == AirDashState.Ready)
				{
					setAirdash();
				}

				/*else
				{
					state = CharState.Jump;
				}
				*/

				break;

			case CharState.AirDash:

				airdash();
				//float airDashSpeedDropMultiplier = 5f;
				//rollSpeed -= rollSpeed * airDashSpeedDropMultiplier * Time.deltaTime;

				//float rollSpeedMinimum = 50f;
				//GetComponent<Rigidbody2D>().useGravity = false;

				rigidbody2d.gravityScale = 0;
				airDashFrames--;

				if (airDashFrames == 0)
				{
					anim.SetBool("isAirdash", false);
					anim.SetBool("isJumping", true);
					state = CharState.Jump;
					airDashFrames = AIRDASHFRAMES;
					//slowAirdash();
					rigidbody2d.gravityScale = 5;

					//rb.useGravity = true;
				}
				break;
			case CharState.Block:

				break;
			case CharState.Crouch:
				//if(Input.GetButtonDown("Vertical"))
				dash();
				jump();
				if (Input.GetAxisRaw("Vertical") > -.1)
				{
					state = CharState.Normal;
				}
				//UnityEngine.Debug.Log("crouch");

				break;
			case CharState.Walk:

				//dash();
				if (jump()) ;
				else if (isWalk())
				{

					walk();
				}
				break;
		}


	}
	/*
	private void FixedUpdate()

	{
		switch (state) {
			case CharState.Normal:
				
				// jumping is allowed
				jump();
				// running is allowed
				dash();
		
					//anim.SetBool("isWalking", false);
				
					//anim.SetBool("isRunning", false);
					//anim.SetBool("isJumping", false);
				
				break;

			case CharState.Jump:



                //if (IsGrounded() && rigidbody2d.velocity.y <= 0)
                //{
                //    state = CharState.Normal;
                //    airDashState = AirDashState.Ready;
                //    anim.SetBool("isJumping", false);
                //    anim.SetBool("isGrounded", true);

                //}
                //else if (Input.GetButtonDown("Block") && airDashState == AirDashState.Ready)
                //{
                //    //airDash();
                //    airDashState = AirDashState.NA;
                //    state = CharState.AirDash;
                //    anim.SetBool("isAirdash", true);
                //    anim.SetBool("isJumping", false);

                //    if (Input.GetAxisRaw("Horizontal") > .1)
                //    {
                //        rigidbody2d.velocity = Vector2.zero;

                //        airDashDirection = AirDashDirection.Right;

                //    }
                //    else if (Input.GetAxisRaw("Horizontal") < -.1)
                //    {

                //        rigidbody2d.velocity = Vector2.zero;

                //        airDashDirection = AirDashDirection.Left;

                //    }
                //    else
                //    {
                //        rigidbody2d.velocity = Vector2.zero;

                //        airDashDirection = AirDashDirection.Neutral;
                //    }
                //}
                //else
                //{
                //    state = CharState.Jump;
                //}
                break;

			case CharState.Dashing:

			//	dash();
			//	jump();
			//	if(IsGrounded() && rigidbody2d.velocity.x < 10 && rigidbody2d.velocity.x > -10)
   //             {
			//		state = CharState.Normal;
			//		anim.SetBool("isIdle", true);
   //             }
			//	//else if()
			//	break;
			//case CharState.AirDash:

			//	if (airDashDirection == AirDashDirection.Right)
			//	{
			//		transform.position = new Vector2(transform.position.x + .30f, transform.position.y);
					

			//	}
			//	else
   //             {
			//		transform.position = new Vector2(transform.position.x - .30f, transform.position.y);
				
			//	}

				break;
		}
	}
	*/

	private bool jump()
	{
		if ((Input.GetButtonDown("Vertical") || Input.GetButtonDown("Jump")) && IsGrounded())
		{
			// set jump states
			state = CharState.Jump;
			anim.SetBool("isJumping", true);
			anim.SetBool("isGrounded", false);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
			anim.SetBool("isWalking", false);

			// give jump velocity
			float jumpVelocity = 20f;


			rigidbody2d.velocity = rigidbody2d.velocity * .8f + (Vector2.up * jumpVelocity);




			return true;
		}
		else
		{

			return false;
		}
	}

	private void aerialDrift()
	{
		if (rigidbody2d.velocity.x < MAXAIRDRIFTSPEED && rigidbody2d.velocity.x > -1 * MAXAIRDRIFTSPEED)
		{
			rigidbody2d.velocity = rigidbody2d.velocity + (Vector2.right * Input.GetAxisRaw("Horizontal") * AIRDRIFTMULTIPLIER);
		}

		//      if (Input.GetAxisRaw("Horizontal") > .1)
		//      {
		//	rigidbody2d.velocity = rigidbody2d.velocity + (Vector2.right * Input.GetAxisRaw("Horizontal") * 7f);
		//}
		//else if (Input.GetAxisRaw("Horizontal") < -.1)
		//      {
		//	rigidbody2d.velocity = rigidbody2d.velocity + (Vector2.left * Input.GetAxisRaw("Horizontal") * 7f);
		//}
	}

	private bool dash()
	{
		//UnityEngine.Debug.Log("in dash function " + Input.GetAxisRaw("Horizontal"));


		/*
		if ((dirX > .5 || dirX < -.5) && !anim.GetCurrentAnimatorStateInfo(0).IsName("isRunning"))
		{

			state = CharState.Dashing;
			anim.SetBool("isRunning", true);
			//return true;
		}
		*/
		//dash();
		// dont dash unless state is dashing or normal
		if (state != CharState.Dashing && state != CharState.Normal && state != CharState.Walk) { return false; }
		else if (Input.GetAxisRaw("Horizontal") > .85)
		{

			//	TODO:::::: IMPLEMENT PIVOTING WITH A SIMILAR METHOD TO THIS
			//if (mySpriteRenderer.flipX == false)
			//{
			//	mySpriteRenderer.flipX = true;
			//	anim.SetBool("isWalking", false);
			//	//anim.SetBool("isRunning", true);
			//	state = CharState.Normal;
			//	goto SKIPWALK;
			//	//if (dash())
			//	//{
			//	//	goto SKIPWALK; // END OF THIS FUNCTION
			//	//}
			//}
			state = CharState.Dashing;
			anim.SetBool("isRunning", true);
			anim.SetBool("isJumping", false);
			anim.SetBool("isWalking", false);
			//anim.SetBool("isCrouching", false);
			//anim.SetBool("isIdle", false);


			rigidbody2d.velocity = Vector2.right * moveSpeed;
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
			{
				// flip the sprite
				mySpriteRenderer.flipX = true;
			}
			return true;
		}
		else if (Input.GetAxisRaw("Horizontal") < -.85)
		{
			state = CharState.Dashing;
			anim.SetBool("isRunning", true);
			anim.SetBool("isJumping", false);
			anim.SetBool("isWalking", false);
			//anim.SetBool("isCrouching", false);
			//anim.SetBool("isIdle", false);

			rigidbody2d.velocity = Vector2.left * moveSpeed;
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
			{
				// flip the sprite
				mySpriteRenderer.flipX = false;
			}
			return true;
		}



		return false;
	}
	private bool isDashing()
	{

		if (IsGrounded() && (rigidbody2d.velocity.x > 10 || rigidbody2d.velocity.x < -10) &&
			(state == CharState.Dashing || state == CharState.Normal || state == CharState.Walk))
		{
			state = CharState.Dashing;
			//anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", true);
			anim.SetBool("isJumping", false);
			anim.SetBool("isGrounded", true);
			anim.SetBool("isIdle", false);
			return true;
		}
		//else if (dashFrames != 0)
		//{
		//	dashFrames--;
		//}
		else
		{
			anim.SetBool("isRunning", false);
			state = CharState.Normal;
			dashFrames = DASHFRAMES;
			return false;
		}

	}

	private void walk()
	{
		if (Input.GetAxisRaw("Horizontal") > .1)
		{
			if (mySpriteRenderer.flipX == false)
			{
				mySpriteRenderer.flipX = true;
				anim.SetBool("isWalking", false);
				//anim.SetBool("isRunning", true);
				state = CharState.Normal;
				goto SKIPWALK;
				//if (dash())
				//{
				//	goto SKIPWALK; // END OF THIS FUNCTION
				//}
			}
			state = CharState.Walk;
			anim.SetBool("isWalking", true);

			transform.position = new Vector2(transform.position.x + (WALKSPEED * Input.GetAxisRaw("Horizontal")), transform.position.y);
			//rigidbody2d.velocity = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

			//rigidbody2d.velocity = Vector2.right * (Input.GetAxisRaw("Horizontal"));
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
			{
				// flip the sprite
				mySpriteRenderer.flipX = true;
			}
		}
		else if (Input.GetAxisRaw("Horizontal") < -.1)
		{
			if (mySpriteRenderer.flipX == true)
			{
				mySpriteRenderer.flipX = false;
				anim.SetBool("isWalking", false);
				//anim.SetBool("isRunning", true);
				state = CharState.Normal;
				goto SKIPWALK;

			}
			state = CharState.Walk;
			anim.SetBool("isWalking", true);

			transform.position = new Vector2(transform.position.x + (WALKSPEED * Input.GetAxisRaw("Horizontal")), transform.position.y);
			//rigidbody2d.velocity = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
			//rigidbody2d.velocity = Vector2.left * -(Input.GetAxisRaw("Horizontal"));
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
			{
				// flip the sprite
				mySpriteRenderer.flipX = false;
			}

		}
	SKIPWALK:;
	}

	private bool isWalk()
	{

		// if the analog stick is being held between a certain range for a certain number of frames, PREWALKFRAMES, then start walking
		if (prewalkframes == 0 && IsGrounded() &&
			(Input.GetAxisRaw("Horizontal") > .1 || Input.GetAxisRaw("Horizontal") < -.1))// && 
																						  //(Input.GetAxisRaw("Horizontal") < .75 || Input.GetAxisRaw("Horizontal") > -.75))
		{
			//prewalkframes = PREWALKFRAMES;
			anim.SetBool("isWalking", true);
			anim.SetBool("isRunning", false);
			anim.SetBool("isJumping", false);
			anim.SetBool("isIdle", false);
			state = CharState.Walk;

			return true;
		}
		else if (IsGrounded() && (Input.GetAxisRaw("Horizontal") > .1 || Input.GetAxisRaw("Horizontal") < -.1) &&
			 (Input.GetAxisRaw("Horizontal") < .75 || Input.GetAxisRaw("Horizontal") > -.75))
		{
			prewalkframes--;
			return false;
		}
		else
		{
			prewalkframes = PREWALKFRAMES;
			state = CharState.Normal;

			anim.SetBool("isWalking", false);
			return false;
		}
	}

	private void crouch()
	{
		// check for crouch
		if (Input.GetAxisRaw("Vertical") < -.2)
		{
			state = CharState.Crouch;
		}
		else
		{
			//change animation to standing

		}
	}

	private void idle()
	{
		if (IsGrounded() && rigidbody2d.velocity.x < 10 && rigidbody2d.velocity.x > -10)
		{
			state = CharState.Normal;
			anim.SetBool("isIdle", true);
			anim.SetBool("isRunning", false);
		}
	}

	private void airdash()
	{
		if (airDashDirection == AirDashDirection.Right)
		{
			transform.position = new Vector2(transform.position.x + AIRDASHSPEED, transform.position.y);
			rigidbody2d.velocity = Vector2.right * AIRDASHVELOCITY;
			//AIRDASHSPEED;

		}
		else
		{
			rigidbody2d.velocity = Vector2.left * AIRDASHVELOCITY;
			//AIRDASHSPEED;
			transform.position = new Vector2(transform.position.x - AIRDASHSPEED, transform.position.y);

		}

	}
	private void slowAirdash()
	{
		//transform.position = new Vector2(transform.position.x + .30f, transform.position.y);
		rigidbody2d.velocity = rigidbody2d.velocity * .5f;




	}
	private void setAirdash()
	{
		//airDash();
		airDashState = AirDashState.NA;
		state = CharState.AirDash;
		anim.SetBool("isAirdash", true);
		anim.SetBool("isJumping", false);

		if (Input.GetAxisRaw("Horizontal") > .1)
		{
			rigidbody2d.velocity = Vector2.zero;

			airDashDirection = AirDashDirection.Right;

		}
		else if (Input.GetAxisRaw("Horizontal") < -.1)
		{

			rigidbody2d.velocity = Vector2.zero;

			airDashDirection = AirDashDirection.Left;

		}
		else
		{
			rigidbody2d.velocity = Vector2.zero;

			airDashDirection = AirDashDirection.Neutral;
		}
	}
	/*
	private bool airDash()
	{

			state = CharState.AirDash;
			anim.SetBool("isRunning", true);
			transform.position = new Vector2(transform.position.x + .5, transform.position.y);

		if (Input.GetAxisRaw("Horizontal") > .01)
			{
			//float jumpVelocity = 10f;
			//rigidbody2d.velocity = Vector2.right * moveSpeed;

			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
				{
					// flip the sprite
					mySpriteRenderer.flipX = true;
				}
			}
			else if (Input.GetAxisRaw("Horizontal") < -.01)
			{
			//rigidbody2d.velocity = Vector2.left * moveSpeed;
			//transform.position = new Vector2(transform.position.x - .5, transform.position.y);
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
				{
					// flip the sprite
					mySpriteRenderer.flipX = false;
				}

			}
			return true;


	}
	*/
	private bool IsGrounded()
	{
		//return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;

		float extraHeightText = .1f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);

		Color rayColor;
		if (raycastHit.collider != null)
		{
			rayColor = Color.green;
		}
		else
		{
			rayColor = Color.red;
		}
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);

		//UnityEngine.Debug.Log(raycastHit.collider);
		//Debug.log()
		/*
		if(raycastHit.collider != null)
        {
			//anim.SetBool("isGrounded", true);
			//state = CharState.Normal;
			anim.SetBool("isJumping", false);
			anim.SetBool("isGrounded", true);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
		}
        else {
			//state = CharState.Jump;
			anim.SetBool("isJumping", true);
			anim.SetBool("isGrounded", false);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
		}*/
		return raycastHit.collider != null;
	}
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatControl : MonoBehaviour
{

	float dirX, moveSpeed;

	Animator anim;

	// Use this for initialization
	void Start()
	{
		anim = GetComponent<Animator>();
		moveSpeed = 5f;
	}

	// Update is called once per frame
	void Update()
	{
		dirX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

		transform.position = new Vector2(transform.position.x + dirX, transform.position.y);

		if (dirX != 0 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
		{
			anim.SetBool("isWalking", true);
		}
		else
		{
			anim.SetBool("isWalking", false);
		}

		if (Input.GetButtonDownDown("Fire1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Kick"))
		{
			anim.SetBool("isWalking", false);
			anim.SetTrigger("hit");
		}

	}
}
*/