using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInputSystem : ScriptableObject
{

	bool			lastXAxisState = false;
	bool			lastYAxisState = false;
	bool			currentYAxisState = false;
	bool			currentXAxisState = false;


	bool			lastAirdashState = false;

	bool			lastLightState = false;
	bool			lastMediumState = false;
	bool			lastHeavyState = false;
	bool			lastPlatformState = false;
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
		currentXAxisState = (joystickAxis.x > 0.5 || joystickAxis.x < -0.5);
		currentYAxisState = (joystickAxis.y > 0.5 || joystickAxis.y < -0.5);

		if (currentXAxisState && lastYAxisState && !currentYAxisState)
		{
			horizontalDownThisFrame = true;
			lastXAxisState = currentXAxisState;
			//verticalDownThisFrame = false;
		}
		// prevent keep returning true when axis still pressed.
		else if (currentXAxisState && lastXAxisState)
		{
			//UnityEngine.Debug.Log("getxaxisbuttondown return false bc last input was true= " + lastXAxisState);
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
			lastYAxisState = currentYAxisState;
		}

		else if (currentYAxisState && lastYAxisState)
		{
			//UnityEngine.Debug.Log("getyaxisbuttondown return false bc last input was true= " + lastYAxisState);
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
	public bool getPlatformDownThisFrame(bool platformButtonDown)
	{



		// prevent keep returning true when axis still pressed.
		if (platformButtonDown && lastPlatformState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			return false;
		}


		lastPlatformState = platformButtonDown;

		//UnityEngine.Debug.Log("getaxisbuttondown last value was false so return current value= " + currentInputValue + Input.GetAxis(axisName));
		return platformButtonDown;
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

	public bool getMediumDownThisFrame(bool mediumButtonDown)
	{



		// prevent keep returning true when axis still pressed.
		if (mediumButtonDown && lastMediumState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			return false;
		}


		lastMediumState = mediumButtonDown;

		//UnityEngine.Debug.Log("getaxisbuttondown last value was false so return current value= " + currentInputValue + Input.GetAxis(axisName));
		return mediumButtonDown;
	}
	public bool getHeavyDownThisFrame(bool heavyButtonDown)
	{

		// prevent keep returning true when axis still pressed.
		if (heavyButtonDown && lastHeavyState)
		{
			//UnityEngine.Debug.Log("getaxisbuttondown return false bc last input was true= " + lastInputAxisState);
			return false;
		}


		lastHeavyState = heavyButtonDown;

		//UnityEngine.Debug.Log("getaxisbuttondown last value was false so return current value= " + currentInputValue + Input.GetAxis(axisName));
		return heavyButtonDown;
	}

}