using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;

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

		while (i >= inputBuffer.Count - numInputs && i >= 0)
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