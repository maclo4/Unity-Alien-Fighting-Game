

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
//using InputBuffer;
//using InputTime;


/*

public class InputTime
{
	public long elapsedMilliseconds;
	public string input;
	public InputTime(string INPUT, long ELAPSEDMILLISECONDS)
    {
		elapsedMilliseconds = ELAPSEDMILLISECONDS;
		input = INPUT;
    }

}
public class InputBuffer
{
	// input buffer variable to store 10 most recent inputs
	List<InputTime> inputBuffer;
	Stopwatch stopwatch;

	public int size { get; private set; }

	// constructor (kinda pointless)
	public InputBuffer()
	{
		inputBuffer = new List<InputTime>();
		stopwatch = new Stopwatch();
		stopwatch.Start();
		size = 10;
	}
	

	// add an input to the end of the list
	public void Enqueue(string input)
	{
		InputTime inputTime = new InputTime(input, stopwatch.ElapsedMilliseconds);

		inputBuffer.Add(inputTime);
		
		while (inputBuffer.Count > size)
		{

			inputBuffer.RemoveAt(0);
		}
	}
	// returns a specified amount of the most recent inputs from the list
	public List<InputTime> getTopInputs(int numInputs = 3)
    {
		List<InputTime> topInputs = new List<InputTime>(numInputs);
		int i = inputBuffer.Count - 1;
		
		while (i >= inputBuffer.Count - numInputs && i >=0)
        {
			topInputs.Add(inputBuffer[i]);
			i--;
        }
		
		return topInputs;
    }

	// mainly for testing
	public void printBuffer()
    {
		List<InputTime> printTop = getTopInputs(20);
		int i = 0;
		foreach (InputTime inputs in printTop)
		{
			UnityEngine.Debug.Log(i + ": " + inputs.input + ", " + inputs.elapsedMilliseconds);
			i++;
		}
	}

	public long getCurrentTime()
    {
		return stopwatch.ElapsedMilliseconds;
    }

}

*/

/*
public class CustomInputSystem : ScriptableObject
{

	bool lastXAxisState = false;
	bool lastYAxisState = false;
	bool currentYAxisState = false;
	bool currentXAxisState = false;

	
	bool lastAirdashState = false;
	
	bool lastLightState = false;
	/// <summary>
	/// Gets the axis input like an on key down event, returning <c>true</c> only 
	/// on the first press, after this return <c>false</c> until the next press. 
	/// Only works for axis between 0 (zero) to 1 (one).
	/// </summary>
	// <param name="axisName">Axis name configured on input manager.</param>
	// TODO GET RID OF THIS PARAM AXISNAME
	public void getXAxisButtonDown(Vector2 joystickAxis, ref bool horizontalDownThisFrame, ref bool verticalDownThisFrame)
	{
		bool tempLastX = lastXAxisState;
		currentXAxisState =  (joystickAxis.x > 0.5 || joystickAxis.x < -0.5);
		currentYAxisState = (joystickAxis.y > 0.5 || joystickAxis.y < -0.5);
		
		if (currentXAxisState && lastYAxisState && !currentYAxisState)
        {
			horizontalDownThisFrame = true;
			//verticalDownThisFrame = false;
        }
		// prevent keep returning true when axis still pressed.
		else if (currentXAxisState && lastXAxisState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			horizontalDownThisFrame = false;
		}
        else
        {
			lastXAxisState = currentXAxisState;
			horizontalDownThisFrame = currentXAxisState;
        }


		// now for the y axis
		if (currentYAxisState && tempLastX && !currentXAxisState)
        {
			//horizontalDownThisFrame = false;
			verticalDownThisFrame = true;
		}
		
		else if (currentYAxisState && lastYAxisState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			verticalDownThisFrame = false;
		}
        else
        {
			lastYAxisState = currentYAxisState;
			verticalDownThisFrame = currentYAxisState;
        }



	}
	public bool getAirdashDownThisFrame(bool airdashButtonDown)
    {
		


		// prevent keep returning true when axis still pressed.
		if (airdashButtonDown && lastAirdashState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			return false;
		}


		lastAirdashState = airdashButtonDown;

		//UnityEngine.Debug.Log("getaxisbuttondown last value was false so return current value= " + currentInputValue + Input.GetAxis(axisName));
		return airdashButtonDown;
	}
	public bool getLightDownThisFrame(bool lightButtonDown)
	{



		// prevent keep returning true when axis still pressed.
		if (lightButtonDown && lastLightState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			return false;
		}


		lastLightState = lightButtonDown;

		//UnityEngine.Debug.Log("getaxisbuttondown last value was false so return current value= " + currentInputValue + Input.GetAxis(axisName));
		return lightButtonDown;
	}


}
*/

public class chipp_controller : MonoBehaviour
{

	[SerializeField] private LayerMask platformLayerMask;
	[SerializeField] private LayerMask playerLayerMask;
	private enum CharState { Normal, Dashing, Jump, Tumble, AirDash, Block, Crouch, Walk, Backdash, LightAttack }
	CharState state = CharState.Normal;

	private enum AirDashDirection { Right, Left, Neutral}
	private enum AirDashState { NA, Ready }
	AirDashDirection airDashDirection;
	AirDashState airDashState = AirDashState.Ready;
	public static int DASHFRAMES = 0;
	public static int AIRDASHFRAMES = 15;
	public static float AIRDASHSPEED = .11f;
	public static float AIRDASHVELOCITY = 10f;
	public static int PREWALKFRAMES = 2;
	public static float WALKSPEED = 3f;
	public static float MAXAIRDRIFTSPEED = 10f;
	public static float AIRDRIFTMULTIPLIER = .2f;
	public static float BACKDASHSPEED = .17f;
	public static int BACKDASHFRAMES = 12;
	public static float MOVESPEED = 12f;
	public static float JUMPVELOCITY = 20.5f;
	int backdashFrames = BACKDASHFRAMES;
	int prewalkframes = PREWALKFRAMES;
	int airdashFrames = AIRDASHFRAMES;
	int dashFrames = DASHFRAMES;
	//public float movesSpeed;
	float moveSpeed = MOVESPEED;
	float jumpVelocity = JUMPVELOCITY;
	float airdashSpeed = AIRDASHSPEED;


	// JOYSTICK INPUTS
	CustomInputSystem inputSystem;
	CustomInputSystem joyStickInputsVertical;
	private string[] keys = new string[] { "Jump", "Joystick", "Airdash", "Light", "Medium", "Heavy" };
	private InputBuffer inputQueue;

	// PAUSE SCRIPT
	public PauseController pauseController;

	//private Player_Base playerBase;
	private Rigidbody2D rigidbody2d;
	
	private BoxCollider2D boxCollider2d;
	private BoxCollider2D enemyBoxCollider2d;
	float dirX, dirY;
	Animator anim;

	// POSSIBLY DELETE BUT THIS IS FOR LOOKING AT THE OTHER CHARACTER

	private Transform target;
	
	
	// ===========================================================

	// variable to hold a reference to our SpriteRenderer component
	private SpriteRenderer mySpriteRenderer;

	
	// =======================================================================
	// =======================================================================

	InputMaster controls;
	Vector2 joystickAxis;
	float left;

	bool airdashDownThisFrame;
	bool horizontalDownThisFrame;
	bool verticalDownThisFrame;
	bool lightDownThisFrame;
	bool mediumDownThisFrame;
	//bool downButtonDown;
	//bool upButtonDown;
	//bool leftButtonDown;
	//bool rightButtonDown;
	// =======================================================================
	// =======================================================================
	// Use this for initialization
	void Start()
	{
		
		//TODO: MOVE SOME OF THESE OUTSIDE OF START IF POSSIBLE.. DONE?h
		pauseController = GameObject.FindObjectOfType(typeof(PauseController)) as PauseController;
		Application.targetFrameRate = 60;
		anim = GetComponent<Animator>();
		
	
		inputQueue = new InputBuffer();
		

	}

	void OnEnable()
	{
		controls.PlayerControls.Enable();
	}
	void OnDisable()
	{
		controls.PlayerControls.Disable();
	}

	// This function is called just one time by Unity the moment the component loads
	private void Awake()
	{
		inputSystem = ScriptableObject.CreateInstance<CustomInputSystem>();
		joyStickInputsVertical = ScriptableObject.CreateInstance<CustomInputSystem>();

		// get a reference to the SpriteRenderer component on this gameObject
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		//playerBase = gameObject.GetComponent<Player_Base>();
		rigidbody2d = transform.GetComponent<Rigidbody2D>();
		
		boxCollider2d = transform.GetComponent<BoxCollider2D>();

		target = GameObject.FindGameObjectWithTag("Other_Player").GetComponent<Transform>();
		enemyBoxCollider2d = target.GetComponent<BoxCollider2D>();

		controls = new InputMaster();
		controls.PlayerControls.Joystick.performed += context => joystickAxis = context.ReadValue<Vector2>();
		controls.PlayerControls.Joystick.canceled += context =>	joystickAxis = Vector2.zero;

		

		controls.PlayerControls.Airdash.performed += context => airdashDownThisFrame = buttonDown(context); 
		controls.PlayerControls.Airdash.canceled += context => airdashDownThisFrame = false;

		controls.PlayerControls.Light.performed += context => lightDownThisFrame = buttonDown(context);
		controls.PlayerControls.Light.canceled += context => lightDownThisFrame = false;

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

	bool buttonDown(InputAction.CallbackContext context) {

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

		//UnityEngine.Debug.Log(buttonBool);
		return buttonBool;
	}
	void LateUpdate()
	{
		
		
		if(target.position.x < transform.position.x && state != CharState.Dashing && IsGrounded())
        {
			mySpriteRenderer.flipX = false;
		}
        else if(target.position.x > transform.position.x && state != CharState.Dashing && IsGrounded())
        {
			mySpriteRenderer.flipX = true;
		}
	

	}


	
	// Update is called once per frame
	void Update()
	{



		//UnityEngine.Debug.Log("testing inputsysyem: " + controls.PlayerControls.Airdash.performed);
		inputSystem.getXAxisButtonDown(joystickAxis, ref horizontalDownThisFrame, ref verticalDownThisFrame);
		airdashDownThisFrame = inputSystem.getAirdashDownThisFrame(airdashDownThisFrame);
		lightDownThisFrame = inputSystem.getLightDownThisFrame(lightDownThisFrame);

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
			case CharState.Normal:


				if (state == CharState.Normal) 
				{
					dash();
				}
				if (state == CharState.Normal) 
				{
					backDash();
				}
				// jumping is allowed, handles checking for jumps as well
				if (state == CharState.Normal)
				{
					jump();
				}
				// running is allowed, handles checking for dashes as well
				if (state == CharState.Normal) 
				{
					isWalk();
				}
				if(state == CharState.Normal)
                {
					//anim.SetBool("isLightAttack", true);
                }

				//dirX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

				//transform.position = new Vector2(transform.position.x + dirX, transform.position.y);
				
				break;

			case CharState.Dashing:

				//if (jump()) { UnityEngine.Debug.Log("just from a dash"); }
				jump();
				if(state == CharState.Dashing)
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
					//inputQueue.printBuffer();
					state = CharState.Normal;
					airDashState = AirDashState.Ready;
					anim.SetBool("isJumping", false);
					anim.SetBool("isGrounded", true);

				}
				else if (airdashDownThisFrame && airDashState == AirDashState.Ready)
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
				airdashFrames--;
			
				if (airdashFrames == 0)
				{
					anim.SetBool("isAirdash", false);
					anim.SetBool("isAirdashForward", false);
					anim.SetBool("isJumping", true);
					state = CharState.Jump;
					airdashFrames = AIRDASHFRAMES;
				
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
				if (true) //gamepad.leftStick.ReadValue() > -.1
				{
					state = CharState.Normal;
				}
				//UnityEngine.Debug.Log("crouch");

				break;
			case CharState.Walk:

				//dash();
				if (jump()) { }
				else if (isWalk())
				{

					walk();
				}
				break;
			case CharState.Backdash:
                if (isBackDash())
                {
					executeBackdash();
                }
				break;
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

	private void setInputQueue()
    {
		foreach (string key in keys)
		{
			// basically, each key takes turns checking if it needs to be added to the queue, when it's the axes turns 
			// you gotta use a special function but for the rest of the keys you can use getbuttondown
			// 
			// Input.GetButtonDown(key)
			
			if ((key == "Airdash" && airdashDownThisFrame) || (key == "Light" && lightDownThisFrame) || (key == "Joystick" && (horizontalDownThisFrame || verticalDownThisFrame)))
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
				else if (key == "Joystick" && joystickAxis.x > 0.1)
				{
					inputQueue.Enqueue("Right");
					UnityEngine.Debug.Log("Right enqueed");
				}
				else if (key == "Joystick" && joystickAxis.x < -0.1)
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

		if ((jumpInputs.Count != 0 && (jumpInputs[0].input == "Jump" || jumpInputs[0].input == "Up" || jumpInputs[0].input == "UpRight"  || jumpInputs[0].input == "UpLeft"  ) && elapsedTime < 100 ) 
			&& IsGrounded()) //|| Input.GetButtonDown("Jump")
		{
			
			// set jump states
			
			anim.SetBool("isJumping", true);
			anim.SetBool("isGrounded", false);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
			anim.SetBool("isWalking", false);

			// give jump velocity
			//float jumpVelocity = 20f;
			if (state == CharState.Walk && jumpInputs[0].input == "UpLeft" )
			{
				UnityEngine.Debug.Log("diag jump");
				rigidbody2d.velocity = (Vector2.left * jumpVelocity * .3f)  + (Vector2.up * jumpVelocity);
			}
			else if(state == CharState.Walk && jumpInputs[0].input == "UpRight")
            {
				rigidbody2d.velocity = (Vector2.right * jumpVelocity * .3f) + (Vector2.up * jumpVelocity);
			}
            else
            {
				UnityEngine.Debug.Log("noormal jump");
				rigidbody2d.velocity = rigidbody2d.velocity * .8f + (Vector2.up * jumpVelocity);
			}



			state = CharState.Jump;
			return true;
		}
        else{
			
			return false; }
	}

	private void aerialDrift()
    {
		if (rigidbody2d.velocity.x < MAXAIRDRIFTSPEED &&  rigidbody2d.velocity.x > -1 * MAXAIRDRIFTSPEED)
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
		if (dashInputs.Count == 2) {
			elapsedTime = getElapsedTimeBetweenInputs(dashInputs[0].elapsedMilliseconds, dashInputs[1].elapsedMilliseconds);
		}


		if ((state != CharState.Dashing && state != CharState.Normal && state != CharState.Walk) || dashInputs.Count != 2 || elapsedTime == -1 ) 
		{ 
			return false; 
		}
		// if you are running in the direction of the other character
		// check that the analog stick is far enough towards 1 and that it is in the direction of the enemy
		else if (elapsedTime < 250 && 
				((dashInputs[0].input == "Right" && dashInputs[1].input == "Right" && joystickAxis.x > .85 && target.position.x > transform.position.x) ||
				(dashInputs[0].input == "Left" && dashInputs[1].input == "Left"  && joystickAxis.x < -.85 && target.position.x < transform.position.x)))
		{
			
			// set the correct animations and state
			state = CharState.Dashing;
			anim.SetBool("isRunning", true);
			anim.SetBool("isJumping", false);
			anim.SetBool("isWalking", false);

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
			timeSinceInput = getElapsedTimeBetweenInputs(inputQueue.getCurrentTime(), dashInputs[0].elapsedMilliseconds );
		}



		// make sure that the char isnt in a bad state and that the elapsed time and inputs have values
		if ((state != CharState.Dashing && state != CharState.Normal && state != CharState.Walk	&& state != CharState.Backdash) || dashInputs.Count != 2 || elapsedTime == -1) 
		{ 
			return false; 
		}
		
		else if (elapsedTime < 250 && timeSinceInput < 3 && ((dashInputs[0].input == "Right" && dashInputs[1].input == "Right" && joystickAxis.x > .85 && target.position.x < transform.position.x) ||
				(dashInputs[0].input == "Left" && dashInputs[1].input == "Left" && joystickAxis.x < -.85 && target.position.x > transform.position.x)))
		{
			// set the correct animations and state
			state = CharState.Backdash;
			anim.SetBool("isRunning", false);
			anim.SetBool("isBackDashing", true);
			anim.SetBool("isJumping", false);
			anim.SetBool("isWalking", false);

			
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
		if(backdashFrames >= 0)
        {
			backdashFrames--;
			return true;
        }
		else
        {
			state = CharState.Normal;
			anim.SetBool("isBackDashing", false);
			backdashFrames = BACKDASHFRAMES;
			return false;
        }
    }

	// TODO DELETE
	/*
	private bool oldDash()
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
	/*
		//dash();
		// dont dash unless state is dashing or normal
		if(state != CharState.Dashing && state != CharState.Normal && state != CharState.Walk){ return false; }
		else if (Input.GetAxisRaw("Horizontal") > .85)
		{

		
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
				//mySpriteRenderer.flipX = true;
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
				//mySpriteRenderer.flipX = false;
			}
			return true;
		}
		

		
			return false; 
	}
	*/
	
	
	
	// TODO CHANGE TO VOID
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
		if (joystickAxis.x > .1 || joystickAxis.x < -.1)
		{
			state = CharState.Walk;
			anim.SetBool("isWalking", true);

			transform.position = new Vector2(transform.position.x + (WALKSPEED * joystickAxis.x * Time.deltaTime), transform.position.y);

		}

	}
	// TODO DELETE
	private void oldWalk()
    {
		if (joystickAxis.x > .1)
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

			transform.position = new Vector2(transform.position.x + (WALKSPEED * joystickAxis.x), transform.position.y);
			//rigidbody2d.velocity = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;

			//rigidbody2d.velocity = Vector2.right * (Input.GetAxisRaw("Horizontal"));
			// if the variable isn't empty (we have a reference to our SpriteRenderer
			if (mySpriteRenderer != null)
			{
				// flip the sprite
				mySpriteRenderer.flipX = true;
			}
		}
		else if (joystickAxis.x < -.1)
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

			transform.position = new Vector2(transform.position.x + (WALKSPEED * joystickAxis.x), transform.position.y);
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
	
	// TODO CHANGE TO VOID
	private bool isWalk()
	{
		
		// if the analog stick is being held between a certain range for a certain number of frames, PREWALKFRAMES, then start walking
		if (prewalkframes == 0 && IsGrounded() &&
			(joystickAxis.x > .1 || joystickAxis.x < -.1))// && 
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
		else if (IsGrounded() && (joystickAxis.x > .1 || joystickAxis.x < -.1) &&
			 (joystickAxis.x < .75 || joystickAxis.x > -.75))
        {
			prewalkframes--;
			return false;
		}
		else {
			prewalkframes = PREWALKFRAMES;
			state = CharState.Normal;
	
			anim.SetBool("isWalking", false);
			return false;
		}
    }

// TODO ADD ANIMATION
private void crouch()
{
	// check for crouch
	if (joystickAxis.x < -.2)
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
	else if(airDashDirection == AirDashDirection.Left)
	{
		rigidbody2d.velocity = Vector2.left * AIRDASHVELOCITY;
				//AIRDASHSPEED;
		transform.position = new Vector2(transform.position.x - AIRDASHSPEED, transform.position.y);

	}

}

private void setAirdash()
{

	
	// character facing left and airdashing right
	if (joystickAxis.x > .1 && mySpriteRenderer.flipX == false)
	{
		// TODO: CHANGE THE NAMING OF THE AIRDASH BOOLEANS
		airDashState = AirDashState.NA;
		state = CharState.AirDash;
		airDashDirection = AirDashDirection.Right;

		anim.SetBool("isJumping", false);
		anim.SetBool("isAirdash", true);
		rigidbody2d.velocity = Vector2.zero;


	}
	// character facing right and airdashing right
	else if (joystickAxis.x > .1 && mySpriteRenderer.flipX == true)
	{
		// TODO: CHANGE THE NAMING OF THE AIRDASH BOOLEANS
		airDashDirection = AirDashDirection.Right;
		airDashState = AirDashState.NA;
		state = CharState.AirDash;

		rigidbody2d.velocity = Vector2.zero;
		anim.SetBool("isAirdashForward", true);
		anim.SetBool("isJumping", false);
	


	}
	// character facing right and airdashing left
	else if (joystickAxis.x < -.1 && mySpriteRenderer.flipX == true)
	{
		airDashState = AirDashState.NA;
		state = CharState.AirDash;
		airDashDirection = AirDashDirection.Left;

		rigidbody2d.velocity = Vector2.zero;
		anim.SetBool("isAirdash", true);
		anim.SetBool("isJumping", false);

	}
	// character facing left and airdashing left
	else if (joystickAxis.x < -.1 && mySpriteRenderer.flipX == false)
	{
		airDashDirection = AirDashDirection.Left;
		airDashState = AirDashState.NA;
		state = CharState.AirDash;

		rigidbody2d.velocity = Vector2.zero;
		anim.SetBool("isAirdashForward", true);
		anim.SetBool("isJumping", false);
		
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
		
		}
	else
	{
		rayColor = Color.red;
			IsOnOtherPlayer();
		}
	UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
	UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
	UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);
		
	return raycastHit.collider != null;
}
	private bool IsOnOtherPlayer()
	{
		//return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
		
		float extraHeightText = .2f;
		RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size , 0f, Vector2.down, extraHeightText, playerLayerMask);

		Color rayColor;
		if (raycastHit.collider != null && rigidbody2d.velocity.y <= 0)
		{
			rayColor = Color.green;

			// enemy is to the left
			if (target.position.x > transform.position.x)
			{
				//float shift = target.position.x - transform.position.x;
				float shift = (enemyBoxCollider2d.bounds.size.x + .1f); //- (target.position.x - transform.position.x)) ;
				transform.position = new Vector2(target.position.x - shift, transform.position.y);
			}
            else
            {
				//float shift = target.position.x - transform.position.x;
				float shift = (enemyBoxCollider2d.bounds.size.x + .1f); //- (target.position.x - transform.position.x)) ;
				transform.position = new Vector2(target.position.x + shift, transform.position.y);
			}
			
		

		}
		else
		{
			rayColor = Color.red;
		}
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
		UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);


		return raycastHit.collider != null;
	}



}

