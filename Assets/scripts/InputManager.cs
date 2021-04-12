using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InputManager : MonoBehaviour {

    void OnEnable()
    {
        buttonKeys = new Dictionary<string, KeyCode>();

        // TODO:  Consider reading these from a user preferences file
        buttonKeys["Jump"]  = KeyCode.Space;
        buttonKeys["Left"]  = KeyCode.LeftArrow;
        buttonKeys["Right"] = KeyCode.RightArrow;
    }

	// Use this for initialization
	void Start () 
    {

	}

    Dictionary<string, KeyCode> buttonKeys;
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool GetButtonDown( string buttonName )
    {
        // TODO: Check to see if the game is supposed to be paused
        //  Or maybe if you're in a different input mode (like a window
        //  is open, or if the player is typing in a text box)

        if( buttonKeys.ContainsKey(buttonName) == false )
        {
            Debug.LogError("InputManager::GetButtonDown -- No button named: " + buttonName);
            return false;
        }

        return Input.GetKeyDown( buttonKeys[buttonName] );
    }

    public string[] GetButtonNames()
    {
        return buttonKeys.Keys.ToArray();
    }

    public string GetKeyNameForButton( string buttonName )
    {
        if( buttonKeys.ContainsKey(buttonName) == false )
        {
            Debug.LogError("InputManager::GetKeyNameForButton -- No button named: " + buttonName);
            return "N/A";
        }

        return buttonKeys[buttonName].ToString();
    }

    public void SetButtonForKey( string buttonName, KeyCode keyCode )
    {
        buttonKeys[buttonName] = keyCode;
    }
}
