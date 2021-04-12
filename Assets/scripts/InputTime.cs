using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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