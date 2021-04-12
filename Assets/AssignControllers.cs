using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public enum PlayerCount { OnePayer, TwoPlayer };
public class AssignControllers : MonoBehaviour
{
    bool player1Assigned = false;
    bool player2Assigned = false;
    
    public PlayerCount playerCount;
    int player1GamepadIndex = -1;
    ReadOnlyArray<Gamepad> connectedGamepads;
    private readonly string player1Gamepad = "Player1Gamepad";
    private readonly string player2Gamepad = "Player2Gamepad";
    public GameObject player1PressStartText;
    public GameObject player2PressStartText;
    public Button[] characterSelectButtons;
    public EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt(player1Gamepad, -1);
        PlayerPrefs.SetInt(player2Gamepad, -1);
        enabled = false;
    }
  
    public void initialize()
    {
       
        player1PressStartText.SetActive(true);
        player2PressStartText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        connectedGamepads = Gamepad.all;

        if (player1Assigned == false)
        {

            for (int i = 0; i < connectedGamepads.Count; i++)
            {
                if (connectedGamepads[i].startButton.isPressed)
                {
                    PlayerPrefs.SetInt(player1Gamepad, i);
                    player1GamepadIndex = i;
                    player1Assigned = true;
                    player1PressStartText.SetActive(false);
                    

                    if(playerCount == PlayerCount.OnePayer)
                    {
                        if (characterSelectButtons[0] != null)
                        {
                            characterSelectButtons[0].Select();
                        }
                        foreach (Button character in characterSelectButtons)
                        {
                            character.gameObject.SetActive(true);
                        }
                        enabled = false;
                    }
                    else
                    {
                        player2PressStartText.SetActive(true);
                    }
                }
            }
        }

        
        
        if (player2Assigned == false && player1Assigned == true && playerCount == PlayerCount.TwoPlayer)
        {

            for (int i = 0; i < connectedGamepads.Count; i++)
            {
                if (connectedGamepads[i].startButton.isPressed && player1GamepadIndex != i)
                {
                    PlayerPrefs.SetInt(player2Gamepad, i);
                    player2Assigned = true;
                    player2PressStartText.SetActive(false);

                    if (characterSelectButtons[0] != null)
                    {
                        characterSelectButtons[0].Select();
                    }
                    foreach (Button character in characterSelectButtons)
                    {
                        character.gameObject.SetActive(true);
                    }
                    enabled = false;
                }
            }
        }

    }
}
