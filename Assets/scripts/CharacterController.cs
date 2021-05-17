using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using System;
using System.Threading.Tasks;


public class AttackChainQueue 
{
	public bool							lightAttackUsed = false;
	public bool							crouchingLightAttackUsed = false;
	public bool							mediumAttackUsed = false;
	public bool							crouchingMediumAttackUsed = false;
	public bool							heavyAttackUsed = false;
	public bool							crouchingHeavyAttackUsed = false;
	public bool							aerialLightAttackUsed = false;
	public bool							aerialMediumAttackUsed = false;
	public bool							aerialHeavyAttackUsed = false;

	public void resetQueue()
    {
		lightAttackUsed = false;
		crouchingLightAttackUsed = false;
		mediumAttackUsed = false;
		crouchingMediumAttackUsed = false;
		heavyAttackUsed = false;
		crouchingHeavyAttackUsed = false;
		aerialLightAttackUsed = false;
		aerialMediumAttackUsed = false;
		aerialHeavyAttackUsed = false;
	}
}
public class CharacterController : MonoBehaviour
{

	[SerializeField] private LayerMask	platformLayerMask;
	[SerializeField] private LayerMask	playerLayerMask;
	[SerializeField] private LayerMask	pushboxLayerMask;
	public enum CharState				{ Idle, Dashing, Jump, Tumble, AirDash, Block, Crouch, Walk, Backdash, Attack, Hitstun,
        AirAttack
    }
	[HideInInspector]
	public CharState					state = CharState.Idle;
	
	private enum CurrentActiveAttack	{ LightAttack, CrouchingLightAttack, MediumAttack, CrouchingMediumAttack, HeavyAttack, CrouchingHeavyAttack, AerialLightAttack, AerialMediumAttack, AerialHeavyAttack, None }
	CurrentActiveAttack					currentActiveAttack = CurrentActiveAttack.None;
	AttackChainQueue					attackChainQueue = new AttackChainQueue();
	
	public enum DirectionFacing			{ Left, Right}
	public DirectionFacing				directionFacing { get; set; }
	private enum AirDashDirection		{ Right, Left, Neutral }
	private enum AirDashState			{ NA, Ready }
	AirDashDirection					airDashDirection;
	AirDashState						airDashState = AirDashState.Ready;
	public static int					DASHFRAMES = 0;
	public static int					AIRDASHFRAMES = 15;
	public static float					AIRDASHSPEED = .11f;
	public static float					AIRDASHVELOCITY = 10f;
	public static int					PREWALKFRAMES = 2;
	public static float					WALKSPEED = 4.5f;
	public static float					MAXAIRDRIFTSPEED = 10f;
	public static float					AIRDRIFTMULTIPLIER = .2f;
	public static float					BACKDASHSPEED = .17f;
	public static int					BACKDASHFRAMES = 12;
	public static float					MOVESPEED = 12f;
	public static float					JUMPVELOCITY = 20.5f;
	int									backdashFrames = BACKDASHFRAMES;
	int									prewalkframes = PREWALKFRAMES;
	int									airdashFrames = 0;
	int									dashFrames = DASHFRAMES;
	//public float movesSpeed;
	float								moveSpeed = MOVESPEED;
	float								jumpVelocity = JUMPVELOCITY;
	float								airdashSpeed = AIRDASHSPEED;
	int									hitstunFrames = 0;
	int									blockstunFrames = 0;


	// JOYSTICK INPUTS
	CustomInputSystem inputSystem;
	CustomInputSystem joyStickInputsVertical;
	private string[] keys = new string[] { "Jump", "Joystick", "Airdash", "Light", "Medium", "Heavy" };
	private InputBuffer inputQueue;

	// PAUSE SCRIPT
	public PauseController pauseController;

	//private Player_Base playerBase;
	private Rigidbody2D rigidbody2d;

	// todo make private
	private BoxCollider2D boxCollider2d;
	public BoxCollider2D enemyBoxCollider2d;
	public BoxCollider2D hurtbox;
	float dirX, dirY;
	Animator animator;

	public int health = 100;
	// POSSIBLY DELETE BUT THIS IS FOR LOOKING AT THE OTHER CHARACTER

	public Transform target;



	public CuteAlienAttack lightAttack, mediumAttack, heavyAttack, crouchingLightAttack, crouchingMediumAttack, crouchingHeavyAttack,
							aerialLightAttack, aerialMediumAttack, aerialHeavyAttack;
	// ===========================================================

	// variable to hold a reference to our SpriteRenderer component
	private SpriteRenderer mySpriteRenderer;


	// =======================================================================
	// =======================================================================

	// TODO uncomment this
	InputMaster controls;
	//PlayerInput controls;
	Vector2 joystickAxis;
	float left;

	bool airdashDownThisFrame;
	bool horizontalDownThisFrame;
	bool verticalDownThisFrame;
	bool lightDownThisFrame;
	bool mediumDownThisFrame;
	bool heavyDownThisFrame;

	// =======================================================================
	// =======================================================================
	// Use this for initialization
	void Start()
	{

		//TODO: MOVE SOME OF THESE OUTSIDE OF START IF POSSIBLE.. DONE?h
		
		pauseController = GameObject.FindObjectOfType(typeof(PauseController)) as PauseController;
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		animator = GetComponent<Animator>(); // todo uncomment

		
		inputQueue = new InputBuffer();


	}

	void OnEnable()
	{
		//controls.PlayerControls.Enable();
	}
	void OnDisable()
	{
		//controls.PlayerControls.Disable();
	}

	// This function is called just one time by Unity the moment the component loads
	private void Awake()
	{

		inputSystem = ScriptableObject.CreateInstance<CustomInputSystem>();
		joyStickInputsVertical = ScriptableObject.CreateInstance<CustomInputSystem>();

		// get a reference to the SpriteRenderer component on this gameObject
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		rigidbody2d = transform.GetComponent<Rigidbody2D>();

		boxCollider2d = transform.GetComponent<BoxCollider2D>();

		enemyBoxCollider2d = target.GetComponent<BoxCollider2D>();


		// TODO: UNCOMMENT THIS
		//controls = new InputMaster();

		//     controls.PlayerControls.Joystick.performed += context => joystickAxis = context.ReadValue<Vector2>();
		//     controls.PlayerControls.Joystick.canceled += context => joystickAxis = Vector2.zero;



		//     controls.PlayerControls.Airdash.performed += context => airdashDownThisFrame = buttonDown(context);
		//     controls.PlayerControls.Airdash.canceled += context => airdashDownThisFrame = false;

		//     controls.PlayerControls.Light.performed += context => lightDownThisFrame = buttonDown(context);
		//     controls.PlayerControls.Light.canceled += context => lightDownThisFrame = false;

		/*
		controls.PlayerControls.Up.performed += context => upButtonDown = buttonDown(context); 
		controls.PlayerControls.Up.canceled += context => upButtonDown = false;

		controls.PlayerControls.Down.performed += context => downButtonDown = buttonDown(context); 
		controls.PlayerControls.Down.canceled += context => downButtonDown = false;

		controls.PlayerControls.Left.performed += context => leftButtonDown = buttonDown(context);
		controls.PlayerControls.Left.canceled += context => leftButtonDown = false;

		controls.PlayerControls.Right.performed += context => rightButtonDown = buttonDown(context);
		controls.PlayerControls.Right.canceled += context => rightButtonDown = false;
		*/

	}

	public void SetTarget()
    {
		bool targetSet = false;
		GameObject[] characters;
	
	
			characters = GameObject.FindGameObjectsWithTag("Player");
			if (characters.Length <= 1)
			{
				
				UnityEngine.Debug.Log("characters size: " + characters.Length + " " + characters[0].name);
			}

			foreach (GameObject player in characters)
			{
				
				if (player.GetInstanceID() != gameObject.GetInstanceID())
				{
					UnityEngine.Debug.Log("target was set! " + player.name + " " + gameObject.name + " " + player.transform.position.x + " " + gameObject.transform.position.x);
					target = player.GetComponent<Transform>();
					targetSet = true;
					break;
				}
			}

		enemyBoxCollider2d = target.GetComponent<BoxCollider2D>();
		
		UnityEngine.Debug.Log("SetTargetended");

	}
	public void OnMove(InputAction.CallbackContext context)
	{
		
		joystickAxis = context.ReadValue<Vector2>();
	}
	public void OnAirdash(InputAction.CallbackContext context)
	{
		airdashDownThisFrame = buttonDown(context);
	}

	//     controls.PlayerControls.Airdash.performed += context => airdashDownThisFrame = buttonDown(context);
	//     controls.PlayerControls.Airdash.canceled += context => airdashDownThisFrame = false;

	public void OnLightAttack(InputAction.CallbackContext context)
	{
		
		lightDownThisFrame = buttonDown(context);
	}
	public void OnMediumAttack(InputAction.CallbackContext context)
	{
		mediumDownThisFrame = buttonDown(context);
	}

	public void OnHeavyAttack(InputAction.CallbackContext context) 
	{

		heavyDownThisFrame = buttonDown(context);
	}

	bool buttonDown(InputAction.CallbackContext context)
	{

		bool buttonBool;
		var control = context.control; // Grab control.
									   //var value = context.GetValue<float>(); // Read value from control.

		// Can do control-specific checks.
		var button = control as ButtonControl;
		if (button != null && button.wasPressedThisFrame)
		{
			buttonBool = true;
		}
		else
		{
			buttonBool = false;
		}

		return buttonBool;
	}
	void LateUpdate()
	{
	
		
		if (target.position.x < transform.position.x && state != CharState.Dashing && IsGrounded())
		{

			Quaternion rotation = new Quaternion(0, 0, 0, 0);
			transform.rotation = rotation;
			directionFacing = DirectionFacing.Left;
			
		}
		else if (target.position.x > transform.position.x && state != CharState.Dashing && IsGrounded())
		{
			Quaternion rotation = new Quaternion(0, 180, 0, 0);
			transform.rotation = rotation;
			directionFacing = DirectionFacing.Right;

		}


	}



	// Update is called once per frame
	void Update()
	{

		
		//lightAttackClass.attack();

		inputSystem.getXAxisButtonDown(joystickAxis, ref horizontalDownThisFrame, ref verticalDownThisFrame);
		airdashDownThisFrame = inputSystem.getAirdashDownThisFrame(airdashDownThisFrame);
		lightDownThisFrame = inputSystem.getLightDownThisFrame(lightDownThisFrame);
		mediumDownThisFrame = inputSystem.getMediumDownThisFrame(mediumDownThisFrame);

		//UnityEngine.Debug.Log("HorizontalDown: " + horizontalDownThisFrame);
		/*
		if (pauseController.isPaused)
        {
			return;
        }
		*/
		// get inputs each frame and add them to the queue
		setInputQueue();


		switch (state)
		{
			
			case CharState.Idle:


				if (state == CharState.Idle)
				{
					dash();
				}
				if (state == CharState.Idle)
				{
					backDash();
				}
				// jumping is allowed, handles checking for jumps as well
				if (state == CharState.Idle)
				{
					jump();
				}
				// running is allowed, handles checking for dashes as well
				if (state == CharState.Idle)
				{
					isWalk();
				}

				if(state == CharState.Idle)
                {
					crouch();
                }

				if (state == CharState.Idle)
				{
					checkForGroundedAttack();
				}

				
				if (state == CharState.Idle && animator.GetBool("isIdle") == false)
                {

					animator.SetBool("isIdle", true);
                }

				else if(state != CharState.Idle)
                {
					animator.SetBool("isIdle", false);
                }

				break;

			case CharState.Dashing:

				jump();
				if(state == CharState.Dashing) {
					checkForGroundedAttack();
				}
				
				if (state == CharState.Dashing && isDashing())
				{
					dash();
				}
				else{
					animator.SetBool("isDashing", false);
                }


				break;


			case CharState.Jump:

				aerialDrift();
				if (IsGrounded() && rigidbody2d.velocity.y <= 0)
				{
					//inputQueue.printBuffer();
					state = CharState.Idle;
					airDashState = AirDashState.Ready;
					animator.SetBool("isJumping", false);
					animator.SetBool("isGrounded", true);

				}
				else if (checkForAirdash())
				{
					beginAirdash();
				}
				else if (state == CharState.Jump)
				{
					checkForAerialAttack();
				}


				if(state != CharState.Jump)
                {
					animator.SetBool("isJumping", false);
                }
				/*else
				{
					state = CharState.Jump;
				}
				*/


				break;

			case CharState.AirDash:

				//executeAirdash();
				//float airDashSpeedDropMultiplier = 5f;
				//rollSpeed -= rollSpeed * airDashSpeedDropMultiplier * Time.deltaTime;

				//float rollSpeedMinimum = 50f;
				//GetComponent<Rigidbody2D>().useGravity = false;

				airdashAerialDrift();
				rigidbody2d.gravityScale = 1;
				airdashFrames++;
				if(airdashFrames >= 7)
                {
					checkForAerialAttack();
				}
				if (airdashFrames == AIRDASHFRAMES || state != CharState.AirDash)
				{
					animator.SetBool("isAirdash", false);
					animator.SetBool("isAirdashForward", false);
					animator.SetBool("isJumping", true);
					state = CharState.Jump;
					airdashFrames = 0;

					rigidbody2d.gravityScale = 5;

				}
				break;
		
			case CharState.Crouch:

				if (state == CharState.Crouch)
				{
					checkForGroundedAttack();
				}
				if (state == CharState.Crouch)
				{
					jump();
				}
				if(state == CharState.Crouch)
                {
					crouch();
                }
                if(state != CharState.Crouch)
                {
					animator.SetBool("isCrouching", false);
                }

				break;
			case CharState.Walk:

				//dash();
				jump();
				if(state == CharState.Walk)
                {
					checkForGroundedAttack();
                }
				if (state == CharState.Walk)
				{
					crouch();
				}
				if (state == CharState.Walk && isWalk())
				{

					walk();
				}
				if(state != CharState.Walk)
                {
					animator.SetBool("isWalking", false);
                }
				break;
			case CharState.Backdash:
				
				if (isBackDash())
				{
					executeBackdash();
				}
				break;
			case CharState.Attack:

				groundedAttackUpdate();

				break;
			case CharState.AirAttack:

				if (currentActiveAttack == CurrentActiveAttack.None) { }


				else if (currentActiveAttack == CurrentActiveAttack.AerialLightAttack && aerialLightAttack.chainingAttackAllowed == true)
				{
					if (checkForAerialAttack())
					{
						aerialLightAttack.followUpAttackChained = true;
					}

				}
				else if (currentActiveAttack == CurrentActiveAttack.AerialMediumAttack && aerialMediumAttack.chainingAttackAllowed == true)
				{
					if (checkForAerialAttack())
					{
						aerialMediumAttack.followUpAttackChained = true;
					}

				}
				else if (currentActiveAttack == CurrentActiveAttack.AerialHeavyAttack && aerialHeavyAttack.chainingAttackAllowed == true)
				{
					if (checkForAerialAttack())
					{
						aerialHeavyAttack.followUpAttackChained = true;
					}

				}
				break;
			case CharState.Block:

				blockstunFrames--;
		
				if (blockstunFrames <= 0)
				{
					UnityEngine.Debug.Log("blockstun ended");
					//setNormalState();
					animator.SetBool("isCrouchBlocking", false);
					animator.SetBool("isBlocking", false);
					determineNextFrameState();
				}
				break;

			case CharState.Hitstun:

				hitstunFrames--;

				if(hitstunFrames <= 0)
                {
					setNormalState();

                }
				break;
		}


	}

	public void handleAttackFollowups(CuteAlienAttack cuteAlienAttack, bool isJumpCancelable)
    {
		if (checkForGroundedAttack())
		{
			cuteAlienAttack.followUpAttackChained = true;
		}
		else if (cuteAlienAttack.jumpCancelAllowed == true)
		{
			bool executedJump = jump();
			if(executedJump == true)
            {
				cuteAlienAttack.followUpAttackChained = true;
				attackChainQueue.resetQueue();
				currentActiveAttack = CurrentActiveAttack.None;
			}
		}
	}

	// TODO MAYBE USE FIXED UPDATE AGAIN AT SOME POINT? APPARENTLY ITS BETTER FOR PHYSICS
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

	public long getElapsedTimeBetweenInputs(long first, long last)
	{

		return first - last;
	}

	//todo make this syncronous
	private async Task setInputQueue()
	{
		foreach (string key in keys)
		{
			// basically, each key takes turns checking if it needs to be added to the queue, when it's the axes turns 
			// you gotta use a special function but for the rest of the keys you can use getbuttondown
			// 
			// Input.GetButtonDown(key)
	
	
			if ((key == "Airdash" && airdashDownThisFrame) || (key == "Light" && lightDownThisFrame) || (key == "Medium" && mediumDownThisFrame) || (key == "Joystick" && (horizontalDownThisFrame || verticalDownThisFrame)))
			{


				if (key == "Joystick" && joystickAxis.x > 0.1 && joystickAxis.y > 0.1f)
				{
					inputQueue.Enqueue("UpRight");
					UnityEngine.Debug.Log("upright enqueed");
				}
				else if (key == "Joystick" && joystickAxis.x < -0.1 && joystickAxis.y > 0.1f)
				{
					UnityEngine.Debug.Log("upleft enqueed");
					inputQueue.Enqueue("UpLeft");
				}
				else if (key == "Joystick" && joystickAxis.x > 0.1 && joystickAxis.y < -0.1f)
				{
					inputQueue.Enqueue("DownRight");
					UnityEngine.Debug.Log("downright enqueed");
				}
				else if (key == "Joystick" && joystickAxis.x < -0.1 && joystickAxis.y < -0.1f)
				{
					inputQueue.Enqueue("DownLeft");
					UnityEngine.Debug.Log("downleft enqueed");
				}
				else if (key == "Joystick" && joystickAxis.x > 0.1 )
				{
					inputQueue.Enqueue("Right");
					UnityEngine.Debug.Log("Right enqueed");
				}
				else if (key == "Joystick" && joystickAxis.x < -0.1 )
				{
					inputQueue.Enqueue("Left");
					UnityEngine.Debug.Log("Left enqueed");
				}
				else if (key == "Joystick" && joystickAxis.y > 0.1)
				{
					UnityEngine.Debug.Log("jump enqueed from vertical axis");
					inputQueue.Enqueue("Up");
				}
				else if (key == "Joystick" && joystickAxis.y < -0.1)
				{
					inputQueue.Enqueue("Down");
					UnityEngine.Debug.Log("Down enqueed");
				}

				else if (key != "Joystick")
				{
					UnityEngine.Debug.Log(key);
					inputQueue.Enqueue(key);
				}

			}


		}
	}

	public void determineNextFrameState()
    {
		state = CharState.Idle;
		if (state == CharState.Idle)
		{
			dash();
		}
		if (state == CharState.Idle)
		{
			backDash();
		}
		// jumping is allowed, handles checking for jumps as well
		if (state == CharState.Idle)
		{
			jump();
		}
		// running is allowed, handles checking for dashes as well
		if (state == CharState.Idle)
		{
			isWalk();
		}

		if (state == CharState.Idle)
		{
			crouch();
		}

		if (state == CharState.Idle)
		{
			checkForGroundedAttack();
		}


		if (state == CharState.Idle && animator.GetBool("isIdle") == false)
		{
			animator.SetBool("isIdle", true);
		}

		else if (state != CharState.Idle)
		{
			animator.SetBool("isIdle", false);
		}

	}
	public void resetAnimationState()
    {
		
		//state = CharState.Idle;
		currentActiveAttack = CurrentActiveAttack.None;
		//animator.SetTrigger("triggerReset");
		attackChainQueue.resetQueue();
		determineNextFrameState();
	}
    public void setNormalState()
    {
		// todo uncomment isidle line?
		state = CharState.Idle;
		currentActiveAttack = CurrentActiveAttack.None;
		attackChainQueue.resetQueue();
		animator.SetBool("isJumping", false);
		animator.SetBool("isGrounded", false);
		//animator.SetBool("isIdle", true);
		animator.SetBool("isRunning", false);
		animator.SetBool("isWalking", false);
		
		animator.SetBool("isBlocking", false);
		animator.SetBool("isCrouchBlocking", false);
		animator.SetBool("isCrouching", false);
		animator.SetBool("wasHitOnGround", false);
	}


	// TODO: CHANGE TO VOID
	// todo: allow jumps from analog stick/vertical axis
	private bool jump()
	{
		//store the 2 most recent inputs to check if they are left-left or right-right
		List<InputTime> jumpInputs = new List<InputTime>();
		jumpInputs = inputQueue.getTopInputs(1);
		long elapsedTime = -1;
		if (jumpInputs.Count == 1)
		{
			elapsedTime = getElapsedTimeBetweenInputs(inputQueue.getCurrentTime(), jumpInputs[0].elapsedMilliseconds);
		}

		if ((jumpInputs.Count != 0 && (jumpInputs[0].input == "Jump" || jumpInputs[0].input == "Up" || jumpInputs[0].input == "UpRight" || jumpInputs[0].input == "UpLeft") && elapsedTime < 100)
			&& IsGrounded()) //|| Input.GetButtonDown("Jump")
		{

			// set jump states

			animator.SetBool("isJumping", true);
			animator.SetBool("isGrounded", false);
			animator.SetBool("isIdle", false);
			animator.SetBool("isRunning", false);
			animator.SetBool("isWalking", false);
			animator.SetBool("isCrouching", false);
			// give jump velocity
			//float jumpVelocity = 20f;
			if ((state == CharState.Walk || state == CharState.Attack || state == CharState.Idle) && jumpInputs[0].input == "UpLeft")
			{

				rigidbody2d.velocity = (Vector2.left * jumpVelocity * .3f) + (Vector2.up * jumpVelocity);
			}
			else if ((state == CharState.Walk || state == CharState.Attack || state == CharState.Idle) && jumpInputs[0].input == "UpRight")
			{

				rigidbody2d.velocity = (Vector2.right * jumpVelocity * .3f) + (Vector2.up * jumpVelocity);
			}
			else
			{

				rigidbody2d.velocity = rigidbody2d.velocity * .8f + (Vector2.up * jumpVelocity);
			}



			state = CharState.Jump;
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
			rigidbody2d.velocity = rigidbody2d.velocity + (Vector2.right * joystickAxis.x * AIRDRIFTMULTIPLIER);
		}


	}

	private void airdashAerialDrift()
	{
		if ((rigidbody2d.velocity.x < MAXAIRDRIFTSPEED && rigidbody2d.velocity.x > -1 * MAXAIRDRIFTSPEED) || 
			(rigidbody2d.velocity.x > MAXAIRDRIFTSPEED && joystickAxis.x < 0 ) || ( rigidbody2d.velocity.x < -1 * MAXAIRDRIFTSPEED && joystickAxis.x > 0) )
		{

			rigidbody2d.velocity = rigidbody2d.velocity + (Vector2.right * joystickAxis.x * AIRDRIFTMULTIPLIER);
		}


	}
	// TODO: CHANGE TO VOID
	// TODO: CHANGE ALL MOVEMENT FORMULAS TO USE TIME SINCE LAST FRAME SO THAT MOVEMENT WILL BE CONSISTENT	
	private bool dash()
	{

		//store the 2 most recent inputs to check if they are left-left or right-right
		List<InputTime> dashInputs = new List<InputTime>();
		dashInputs = inputQueue.getTopInputs(2);
		long elapsedTime = -1;
		if (dashInputs.Count == 2)
		{
			elapsedTime = getElapsedTimeBetweenInputs(dashInputs[0].elapsedMilliseconds, dashInputs[1].elapsedMilliseconds);
		}


		if ((state != CharState.Dashing && state != CharState.Idle && state != CharState.Walk) || dashInputs.Count != 2 || elapsedTime == -1)
		{
			return false;
		}
		// if you are running in the direction of the other character
		// check that the analog stick is far enough towards 1 and that it is in the direction of the enemy
		else if (elapsedTime < 250 &&
				((dashInputs[0].input == "Right" && dashInputs[1].input == "Right" && joystickAxis.x > .85 && target.position.x > transform.position.x) ||
				(dashInputs[0].input == "Left" && dashInputs[1].input == "Left" && joystickAxis.x < -.85 && target.position.x < transform.position.x)))
		{

			// set the correct animations and state
			state = CharState.Dashing;
			animator.SetBool("isRunning", true);
			animator.SetBool("isJumping", false);
			animator.SetBool("isWalking", false);

			// apply velocity to the rigidbody in the direction of the enemy
			if (target.position.x > transform.position.x)
			{
				rigidbody2d.velocity = Vector2.right * moveSpeed;
			}
			else
			{
				rigidbody2d.velocity = Vector2.left * moveSpeed;
			}

			// return true to say that the requirements for running were met
			// TODO: THIS COULD PROBABLY BE LEFT OUT AND JUST USE THE CHARSTATE INSTEAD
			return true;
		}

		else
		{
			return false;
		}



	}
	// TODO CHANGE TO VOID
	private bool backDash()
	{
		//store the 2 most recent inputs to check if they are left-left or right-right
		List<InputTime> dashInputs = new List<InputTime>();
		dashInputs = inputQueue.getTopInputs(2);
		long elapsedTime = -1;
		long timeSinceInput = -1;
		if (dashInputs.Count == 2)
		{
			elapsedTime = getElapsedTimeBetweenInputs(dashInputs[0].elapsedMilliseconds, dashInputs[1].elapsedMilliseconds);
			timeSinceInput = getElapsedTimeBetweenInputs(inputQueue.getCurrentTime(), dashInputs[0].elapsedMilliseconds);
		}



		// make sure that the char isnt in a bad state and that the elapsed time and inputs have values
		if ((state != CharState.Dashing && state != CharState.Idle && state != CharState.Walk && state != CharState.Backdash) || dashInputs.Count != 2 || elapsedTime == -1)
		{
			return false;
		}

		else if (elapsedTime < 250 && timeSinceInput < 3 && ((dashInputs[0].input == "Right" && dashInputs[1].input == "Right" && joystickAxis.x > .85 && target.position.x < transform.position.x) ||
				(dashInputs[0].input == "Left" && dashInputs[1].input == "Left" && joystickAxis.x < -.85 && target.position.x > transform.position.x)))
		{
			// set the correct animations and state
			state = CharState.Backdash;
			animator.SetBool("isRunning", false);
			animator.SetBool("isBackDashing", true);
			animator.SetBool("isJumping", false);
			animator.SetBool("isWalking", false);


			return true;
		}
		else
		{
			return false;
		}

	}
	void executeBackdash()
	{

		rigidbody2d.velocity = Vector2.zero; ;
		// apply velocity to the rigidbody in the direction of the enemy
		if (target.position.x > transform.position.x)
		{
			transform.position = new Vector2(transform.position.x + (BACKDASHSPEED * -1), transform.position.y);
		}
		else
		{
			transform.position = new Vector2(transform.position.x + BACKDASHSPEED, transform.position.y);
		}


	}
	// TODO CHANGE TO VOID
	private bool isBackDash()
	{
		if (backdashFrames >= 0)
		{
			backdashFrames--;
			return true;
		}
		else
		{
			state = CharState.Idle;
			animator.SetBool("isBackDashing", false);
			backdashFrames = BACKDASHFRAMES;
			return false;
		}
	}


	public bool checkForAerialAttack()
	{
		if (checkForLightAttack() == true && currentActiveAttack != CurrentActiveAttack.AerialLightAttack && attackChainQueue.aerialLightAttackUsed == false)
		{

			aerialLightAttack.attack();
			attackChainQueue.lightAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.LightAttack;
			animator.SetTrigger("isAerialLightAttack");
			//resetAnimationStates();
			return true;
		}
		if (checkForMediumAttack() == true && currentActiveAttack != CurrentActiveAttack.AerialLightAttack && attackChainQueue.aerialMediumAttackUsed == false)
		{

			aerialLightAttack.attack();
			attackChainQueue.lightAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.LightAttack;
			animator.SetTrigger("isAerialLightAttack");
			//resetAnimationStates();
			return true;
		}
		else
        {
			return false;
        }
	}

	public void groundedAttackUpdate()
    {
		if (currentActiveAttack == CurrentActiveAttack.None) { }


		else if (currentActiveAttack == CurrentActiveAttack.LightAttack && lightAttack.chainingAttackAllowed == true)
		{
			if (checkForGroundedAttack())
			{
				lightAttack.followUpAttackChained = true;
			}

		}
		else if (currentActiveAttack == CurrentActiveAttack.CrouchingLightAttack && crouchingLightAttack.chainingAttackAllowed == true)
		{
			if (checkForGroundedAttack())
			{
				crouchingLightAttack.followUpAttackChained = true;
			}
		}
		else if (currentActiveAttack == CurrentActiveAttack.MediumAttack && mediumAttack.chainingAttackAllowed == true)
		{
			if (checkForGroundedAttack())
			{
				mediumAttack.followUpAttackChained = true;
			}
		}
		else if (currentActiveAttack == CurrentActiveAttack.CrouchingMediumAttack && crouchingMediumAttack.chainingAttackAllowed == true)
		{
			if (checkForGroundedAttack())
			{
				crouchingMediumAttack.followUpAttackChained = true;
			}
		}
		else if (currentActiveAttack == CurrentActiveAttack.HeavyAttack && heavyAttack.chainingAttackAllowed == true)
		{
			if (checkForGroundedAttack())
			{
				heavyAttack.followUpAttackChained = true;
			}

		}
		else if (currentActiveAttack == CurrentActiveAttack.CrouchingHeavyAttack && crouchingHeavyAttack.chainingAttackAllowed == true)
		{
			//if (checkForGroundedAttack())
			//{
			//	crouchingHeavyAttack.followUpAttackChained = true;
			//}
			handleAttackFollowups(crouchingHeavyAttack, true);
		}
		//else if ((currentActiveAttack == CurrentActiveAttack.LightAttack && lightAttack.jumpCancelAllowed == true) ||
		//	(currentActiveAttack == CurrentActiveAttack.CrouchingLightAttack && crouchingLightAttack.jumpCancelAllowed == true) ||
		//	(currentActiveAttack == CurrentActiveAttack.MediumAttack && mediumAttack.jumpCancelAllowed == true) ||
		//	(currentActiveAttack == CurrentActiveAttack.CrouchingMediumAttack && crouchingMediumAttack.jumpCancelAllowed == true) ||
		//	(currentActiveAttack == CurrentActiveAttack.HeavyAttack && heavyAttack.jumpCancelAllowed == true) ||
		//	(currentActiveAttack == CurrentActiveAttack.CrouchingHeavyAttack && crouchingHeavyAttack.jumpCancelAllowed == true))
		//{
		//	UnityEngine.Debug.Log("Jump() called");
		//	jump();
		//}

	}
	public bool checkForGroundedAttack()
    {
		if (checkForLightAttack() == true && currentActiveAttack != CurrentActiveAttack.LightAttack && attackChainQueue.lightAttackUsed == false) {

			lightAttack.attack();
			attackChainQueue.lightAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.LightAttack;
			animator.SetTrigger("isLightAttack");
			//resetAnimationStates();
			return true;
		}
		else if (checkForCrouchingLightAttack() == true && currentActiveAttack != CurrentActiveAttack.CrouchingLightAttack && attackChainQueue.crouchingLightAttackUsed == false) {

			crouchingLightAttack.attack();
			attackChainQueue.crouchingLightAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.CrouchingLightAttack;
			animator.SetTrigger("isCrouchingLightAttack");
			//resetAnimationStates();
			return true;
		}
		else if (checkForMediumAttack() == true && currentActiveAttack != CurrentActiveAttack.MediumAttack && attackChainQueue.mediumAttackUsed == false)
		{

			mediumAttack.attack();
			attackChainQueue.mediumAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.MediumAttack;
			animator.SetTrigger("isMediumAttack");
			//resetAnimationStates();
			return true;
		}
		else if (checkForCrouchingMediumAttack() == true && currentActiveAttack != CurrentActiveAttack.CrouchingMediumAttack && attackChainQueue.crouchingMediumAttackUsed == false)
		{

			crouchingMediumAttack.attack();
			attackChainQueue.crouchingMediumAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.CrouchingMediumAttack;
			animator.SetTrigger("isCrouchingMediumAttack");
			//resetAnimationStates();
			return true;
		}
		else if (checkForHeavyAttack() == true && currentActiveAttack != CurrentActiveAttack.HeavyAttack && attackChainQueue.heavyAttackUsed == false)
		{

			heavyAttack.attack();
			attackChainQueue.heavyAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.HeavyAttack;
			animator.SetTrigger("isHeavyAttack");
			//resetAnimationStates();
			return true;
		}
		else if (checkForCrouchingHeavyAttack() == true && currentActiveAttack != CurrentActiveAttack.CrouchingHeavyAttack && attackChainQueue.crouchingHeavyAttackUsed == false)
		{

			crouchingHeavyAttack.attack();
			attackChainQueue.crouchingHeavyAttackUsed = true;
			state = CharState.Attack;
			currentActiveAttack = CurrentActiveAttack.CrouchingHeavyAttack;
			animator.SetTrigger("isCrouchingHeavyAttack");
			//resetAnimationStates();
			//animator.SetBool("")
			return true;
		}
		return false;
    }

    private void resetAnimationStates()
    {
		animator.SetBool("isRunning", false);
		animator.SetBool("isBackDashing", false);
		animator.SetBool("isJumping", false);
		animator.SetBool("isWalking", false);
		animator.SetBool("isIdle", false);
	}
    private bool checkForLightAttack()
    {
		if (lightDownThisFrame && joystickAxis.y > -.2)
		{
			return true;
		}
		return false;
    }
	private bool checkForCrouchingLightAttack()
	{
		if (lightDownThisFrame && joystickAxis.y <= -.2)
		{

			return true;
		}
		return false;
	}
	private bool checkForMediumAttack()
	{
		if (mediumDownThisFrame && joystickAxis.y > -.2)
		{
			return true;
		}
		return false;
	}

	private bool checkForCrouchingMediumAttack()
	{
		if (mediumDownThisFrame && joystickAxis.y <= -.2)
		{

			return true;
		}
		return false;
	}

	private bool checkForHeavyAttack()
	{
		if (heavyDownThisFrame && joystickAxis.y > -.2)
		{
			return true;
		}
		return false;
	}

	private bool checkForCrouchingHeavyAttack()
	{
		if (heavyDownThisFrame && joystickAxis.y <= -.2)
		{

			return true;
		}
		return false;
	}

	public bool wasAttackBlocked(BlockType blockType, int blockStun, float pushback)
    {
		//holding back, attack hits mid
		if(blockType == BlockType.Mid && (
			(joystickAxis.x > 0.5 && directionFacing == DirectionFacing.Left) ||
			(joystickAxis.x < -0.5 && directionFacing == DirectionFacing.Right))){

			if (joystickAxis.y < -0.5f)
			{
				setBlockstunState(blockStun, pushback, false);
			}
            else
            {
				setBlockstunState(blockStun, pushback, true);
			}
			return true;
        }
		//holding down back back, attack hits low
		else if (blockType == BlockType.Low && (
			(joystickAxis.x > 0.5 && joystickAxis.y < -0.5f && directionFacing == DirectionFacing.Left) || 
			(joystickAxis.x < -0.5 && joystickAxis.y < -0.5f && directionFacing == DirectionFacing.Right))) // target.position.x < transform.position.x
		{

			setBlockstunState(blockStun, pushback, false);
			return true;
		}
		// holding back but not down, attack hits overhead
		else if(blockType == BlockType.Overhead && 
			(joystickAxis.x > 0.5 && joystickAxis.y >= -0.5f && directionFacing == DirectionFacing.Left) ||
			(joystickAxis.x < -0.5 && joystickAxis.y >= -0.5f && directionFacing == DirectionFacing.Right)) {

			setBlockstunState(blockStun, pushback, true);
			return true;
		}
        else
        {
			return false;
        }
		
	
	}

	public void calculateBlockstunLength(int blockstun)
	{
		blockstunFrames = blockstun;

	}
	public void setBlockstunState(int blockStun, float pushback, bool standingBlock)
    {
		state = CharState.Block;

		if (standingBlock == true)
		{
			animator.SetBool("isBlocking", true);
		}
        else
        {
			animator.SetBool("isCrouchBlocking", true);
        }
		animator.SetTrigger("beginBlocking");
		animator.SetBool("isRunning", false);
		animator.SetBool("isBackDashing", false);
		animator.SetBool("isJumping", false);
		animator.SetBool("isWalking", false);
		animator.SetBool("isIdle", false);
		animator.SetBool("isCrouching", false);

		calculateBlockstunLength(blockStun);
		applyPushback(pushback);
	}

	public void setHitstunState(int damage, int hitstun, Vector2 hitTrajectory)
    {
		state = CharState.Hitstun;
		animator.SetBool("wasHitOnGround", true);
		animator.SetBool("isRunning", false);
		animator.SetBool("isBackDashing", false);
		animator.SetBool("isJumping", false);
		animator.SetBool("isWalking", false);
		animator.SetBool("isIdle", false);


		health = health - damage;

		calculateGroundedHitsunLength(hitstun);
		applyHitTrajectory(hitTrajectory);

		if (health <= 0)
		{
			animator.SetBool("isDead", true);
			hurtbox.enabled = false;
			boxCollider2d.enabled = false;
			enabled = false;
		}
	

	}
	

	// todo apply hitstun decay
	public void calculateGroundedHitsunLength(int hitstun)
    {
		hitstunFrames = hitstun;
    }

	public void applyHitTrajectory(Vector2 hitTrajectory)
    {
		if (directionFacing == DirectionFacing.Left)
		{
			rigidbody2d.AddForce(hitTrajectory, ForceMode2D.Impulse);
			//rigidbody2d.velocity =  hitTrajectory;
		}
		else
		{
			hitTrajectory.x *= -1;
			//rigidbody2d.velocity =  hitTrajectory;
            
            rigidbody2d.AddForce(hitTrajectory, ForceMode2D.Impulse);
        }
	}
	public void applyPushback(float pushback)
    {
		if (directionFacing == DirectionFacing.Left)
		{
			rigidbody2d.AddForce(new Vector2(pushback, 0), ForceMode2D.Impulse);
		}
        else
        {
			rigidbody2d.AddForce(new Vector2(pushback * -1, 0), ForceMode2D.Impulse);
		}
    }
	

	// TODO CHANGE TO VOID
	private bool isDashing()
	{

		if (IsGrounded() && (rigidbody2d.velocity.x > 10 || rigidbody2d.velocity.x < -10) &&
			(state == CharState.Dashing || state == CharState.Idle || state == CharState.Walk))
		{
			state = CharState.Dashing;
			animator.SetBool("isRunning", true);

			//todo delete this
			//animator.SetBool("isJumping", false);
			//animator.SetBool("isGrounded", true);
			//animator.SetBool("isIdle", false);
			return true;
		}
		else
		{
			animator.SetBool("isRunning", false);
			state = CharState.Idle;
			dashFrames = DASHFRAMES;
			return false;
		}

	}

	private void walk()
	{
		if (joystickAxis.x > .1 || joystickAxis.x < -.1)
		{
			state = CharState.Walk;
			animator.SetBool("isWalking", true);

			transform.position = new Vector2(transform.position.x + (WALKSPEED * joystickAxis.x * Time.deltaTime), transform.position.y);

		}

	}
	

	// TODO CHANGE TO VOID
	private bool isWalk()
	{

		// if the analog stick is being held between a certain range for a certain number of frames, PREWALKFRAMES, then start walking
		if (IsGrounded() && (joystickAxis.x > .1 || joystickAxis.x < -.1) && (joystickAxis.y > -.1))// prewalkframes <= 0 && 
																									//(Input.GetAxisRaw("Horizontal") < .75 || Input.GetAxisRaw("Horizontal") > -.75))
		{
			
			//prewalkframes = PREWALKFRAMES;
			animator.SetBool("isWalking", true);
			animator.SetBool("isRunning", false);
			animator.SetBool("isJumping", false);
			animator.SetBool("isIdle", false);
			state = CharState.Walk;

			return true;
		}
		// todo delet this
		//else if (IsGrounded() && (joystickAxis.x > .1 || joystickAxis.x < -.1)) //&&
		//	// (joystickAxis.x < .75 || joystickAxis.x > -.75))
		//{
		//	//UnityEngine.Debug.Log("prewalkframes: " + prewalkframes);
		//	//prewalkframes--;
		//	return false;
		//}
		else
		{
			prewalkframes = PREWALKFRAMES;
			state = CharState.Idle;

			animator.SetBool("isWalking", false);
			return false;
		}
	}


	// TODO ADD ANIMATION
	private void crouch()
	{

		// check for crouch
		if (joystickAxis.y < -.2)
		{
			state = CharState.Crouch;
			animator.SetBool("isCrouching", true);
		}
        else
        {
            
			if(state == CharState.Crouch)
            {
				state = CharState.Idle;
			}
            animator.SetBool("isCrouching", false);
			
        }
    }
	

	private void idle()
	{
		if (IsGrounded() && rigidbody2d.velocity.x < 10 && rigidbody2d.velocity.x > -10)
		{
			state = CharState.Idle;
			animator.SetBool("isIdle", true);
			animator.SetBool("isRunning", false);
		}
	}

	private bool checkForAirdash()
    {
		if (airdashDownThisFrame && airDashState == AirDashState.Ready && transform.position.y > 1.25)
        {
			return true;
        }
        else
        {
			return false;
        }

	}
	private void executeAirdash()
	{
		if (airDashDirection == AirDashDirection.Right)
		{
            //rigidbody2d.velocity = (Vector2.right * AIRDASHVELOCITY * .5f) + (Vector2.up * jumpVelocity * .2f);
            transform.position = new Vector2(transform.position.x + AIRDASHSPEED, transform.position.y);
            rigidbody2d.velocity = Vector2.right * AIRDASHVELOCITY;
            //AIRDASHSPEED;

        }
		else if (airDashDirection == AirDashDirection.Left)
		{
			rigidbody2d.velocity = Vector2.left * AIRDASHVELOCITY;
			//AIRDASHSPEED;
			transform.position = new Vector2(transform.position.x - AIRDASHSPEED, transform.position.y);

		}

	}

	private void beginAirdash()
	{


		// character facing left and airdashing right
		if (joystickAxis.x > .1 && directionFacing == DirectionFacing.Left ) // target.position.x < transform.position.x
		{
			// TODO: CHANGE THE NAMING OF THE AIRDASH BOOLEANS
			airDashState = AirDashState.NA;
			state = CharState.AirDash;
			airDashDirection = AirDashDirection.Right;

			animator.SetBool("isJumping", false);
			animator.SetBool("isAirdash", true);
			rigidbody2d.velocity = Vector2.zero;
			//Vector2 force = new Vector2(joystickAxis.x * 14, joystickAxis.y * 11);
			rigidbody2d.AddForce(new Vector2(14, 11), ForceMode2D.Impulse); // todo remove maybe

		}
		// character facing right and airdashing right
		else if (joystickAxis.x > .1 && directionFacing == DirectionFacing.Right)// transform.position.x < target.position.x
		{
			// TODO: CHANGE THE NAMING OF THE AIRDASH BOOLEANS
			airDashDirection = AirDashDirection.Right;
			airDashState = AirDashState.NA;
			state = CharState.AirDash;

			rigidbody2d.velocity = Vector2.zero;
			rigidbody2d.AddForce(new Vector2(14, 11), ForceMode2D.Impulse); // todo remove maybe

			animator.SetBool("isAirdashForward", true);
			animator.SetBool("isJumping", false);



		}
		// character facing right and airdashing left
		else if (joystickAxis.x < -.1 && directionFacing == DirectionFacing.Right) // transform.position.x < target.position.x
		{
			airDashState = AirDashState.NA;
			state = CharState.AirDash;
			airDashDirection = AirDashDirection.Left;

			rigidbody2d.velocity = Vector2.zero;
			rigidbody2d.AddForce(new Vector2(-14, 11), ForceMode2D.Impulse); // todo remove maybe

			animator.SetBool("isAirdash", true);
			animator.SetBool("isJumping", false);

		}
		// character facing left and airdashing left
		else if (joystickAxis.x < -.1 && directionFacing == DirectionFacing.Left) // target.position.x < transform.position.x
		{
			airDashDirection = AirDashDirection.Left;
			airDashState = AirDashState.NA;
			state = CharState.AirDash;

			rigidbody2d.velocity = Vector2.zero;
			rigidbody2d.AddForce(new Vector2(-14, 11), ForceMode2D.Impulse); // todo remove maybe

			animator.SetBool("isAirdashForward", true);
			animator.SetBool("isJumping", false);

		}



	}

	private bool IsGrounded()
	{
		//return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;

		
		float extraHeightText = .1f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);

		Color rayColor;
		if (raycastHit.collider != null)
		{
			rayColor = Color.green;
			//UnityEngine.Debug.Log("isgrounded triggered");
		}
		else
		{
			rayColor = Color.red;
			//StartCoroutine("moveOffOtherPlayer");
			IsOnOtherPlayer(); 
		}
        UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);

        return raycastHit.collider != null;
	}


	private bool IsOnOtherPlayer()
	{
		Color				rayColor = Color.red;
		Vector2				verticalNormal = new Vector2(0, 1);
		float				extraHeightText = .1f;
		ContactFilter2D		contactFilter = new ContactFilter2D();
		List<RaycastHit2D>	raycastHits = new List<RaycastHit2D>();
		Vector3				boxCastSize = boxCollider2d.bounds.size;


		boxCastSize.x += .05f;
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(playerLayerMask);
		int numberHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCastSize, 0f, Vector2.down, contactFilter, raycastHits,  extraHeightText);

		UnityEngine.Debug.Log("raycastHits.Count: " + raycastHits.Count);
		if (raycastHits.Count != 0 && rigidbody2d.velocity.y < 0.1 )
		{
			foreach(RaycastHit2D raycastHit in raycastHits)
            {
				if(raycastHit.normal == verticalNormal && raycastHit.collider.sharedMaterial.name == "pushbox")
                {
					UnityEngine.Debug.Log("is on other player triggered");
					rayColor = Color.green;

                    //StartCoroutine("moveOffOtherCharacter");
                    // enemy is to the left
                    if (target.position.x > transform.position.x)
                    {
                        //float shift = target.position.x - transform.position.x;
                        float shift = (enemyBoxCollider2d.bounds.size.x + .02f); //- (target.position.x - transform.position.x)) ;
                        transform.position = new Vector2(target.position.x - shift, transform.position.y - .2f);
                    }
                    else
                    {
                        //float shift = target.position.x - transform.position.x;
                        float shift = (enemyBoxCollider2d.bounds.size.x + .1f); //- (target.position.x - transform.position.x)) ;
                        transform.position = new Vector2(target.position.x + shift, transform.position.y - .2f);
                    }

                }
				else
				{
					rayColor = Color.red;
				}
			}
			


		}
		else
		{
			rayColor = Color.red;
		}
		
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);


		return raycastHits.Count != 0;
	}

	//IEnumerator airDashCoroutine()
 //   {

 //   }
	//todo can probly delete this bc i think its better to just move them off each other as fast as possible
	IEnumerator moveOffOtherPlayer()
	{
		float				extraHeightText = .1f;
		ContactFilter2D		contactFilter = new ContactFilter2D();
		List<RaycastHit2D>	raycastHits = new List<RaycastHit2D>();
		Vector2				originalVelocity;
		Vector3 boxCastSize = boxCollider2d.bounds.size;


		boxCastSize.x += .05f;
		// set layer mask to filter out non-player pushboxes
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(playerLayerMask);
		
		// check for collisions
		Physics2D.BoxCast(boxCollider2d.bounds.center, boxCastSize, 0f, Vector2.down, contactFilter, raycastHits, extraHeightText);

		// set this so that you only detect horizontal edges
		Vector2 verticalNormal = new Vector2(0, 1);

		Color rayColor = Color.red;

		UnityEngine.Debug.Log("raycastHits.Count: " + raycastHits.Count + ", rigidbody2d.velocity.y: " + rigidbody2d.velocity.y);
		//check if the raycast hit anything and make sure velocity is moving down.
		if (rigidbody2d.velocity.y < 0.1)
		{
			originalVelocity = rigidbody2d.velocity;
			while (raycastHits.Count > 0)
			{
				// reset raycastHits
				//raycastHits.Clear();
				// check for collisions
				Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, contactFilter, raycastHits, extraHeightText);
				foreach (RaycastHit2D raycastHit in raycastHits)
				{
					
					// validate that the collision was with a horizontal edge that  is a pushbox
					// TODO get rid of the pushbox name paramter
					if (raycastHit.normal == verticalNormal && raycastHit.collider.sharedMaterial.name == "pushbox")
					{
						UnityEngine.Debug.Log("is on other player triggered");
						rayColor = Color.green;

						if (target.position.x > transform.position.x)
						{
							transform.position = new Vector2(transform.position.x - .1f, transform.position.y);
						}
						else
						{
							transform.position = new Vector2(transform.position.x + .1f, transform.position.y);
						}
						// only do this once per frame regardless of if there are multiple colliders detected
						break;
					}
					else
					{
						rayColor = Color.red;
					}
				}

				UnityEngine.Debug.Log("raycastHits.Count: " + raycastHits.Count);
				yield return null;
			}
			rigidbody2d.velocity = originalVelocity;
		}
		

		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);

	}

	//IEnumerator moveOffOtherCharacter()
 //   {
		
	
	//	if (target.position.x > transform.position.x)
	//	{
	//		while (IsOnOtherPlayer() == true)
	//		{
	//			transform.position = new Vector2(transform.position.x - .2f, transform.position.y);
	//			yield return null;
	//		}
	//	}
 //       else
 //       {
	//		while (IsOnOtherPlayer() == true)
	//		{
	//			transform.position = new Vector2(transform.position.x + .2f, transform.position.y);
	//			yield return null;
	//		}
	//	}
 //   }

}

