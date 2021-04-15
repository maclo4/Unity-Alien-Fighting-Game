using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine;

public class InitializeScene : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject[] stages;
    GameObject[] players;
    public GameObject p1Healthbar;
    public GameObject p2Healthbar;

    private readonly string selectedCharacter = "SelectedCharacter";
    private readonly string player1Gamepad = "Player1Gamepad";
    private readonly string player2Gamepad = "Player2Gamepad";
    // Start is called before the first frame update
    void Start()
    {
        // InputManager inputManager = GameObject.FindObjectOfType<InputManager>();

        PlayerInputManager playerInputManager = GameObject.FindObjectOfType<PlayerInputManager>();
        
        Debug.Log("Scene is being initialized!");
        //GameObject stage = GameObject.Find("final destination");
        Debug.Log("stage name: " + stages[0].name);
        stages[0].SetActive(true);

        // StickManController playerController;

        //playerInputManager.JoinPlayer();
        //InputControlList<InputDevice> devices = InputUser.GetUnpairedInputDevices();
        //TODO make controller assignment dynamic
        var gamepads = Gamepad.all; //InputSystem.devices;
        
  
        int characterIndex = PlayerPrefs.GetInt(selectedCharacter);
        int player1GamepadIndex = PlayerPrefs.GetInt(player1Gamepad);
        int player2GamepadIndex = PlayerPrefs.GetInt(player2Gamepad);
        Debug.Log("DEvices[" + player1GamepadIndex + "]: " + gamepads[player1GamepadIndex].displayName);
       // Debug.Log("DEvices[2]: " + gamepads[player2GamepadIndex].displayName);
        // PlayerInput.Instantiate(characters[characterIndex], new Vector3(-1, 0, 0), Quaternion.identity);
        //PlayerInput player1 =  
        PlayerInput player1 = PlayerInput.Instantiate(characters[characterIndex], 0, null, -1, gamepads[player1GamepadIndex]);
      
        PlayerInput player2;

        if (player2GamepadIndex == -1)
        {
            player2 = PlayerInput.Instantiate(characters[characterIndex], 1, null, -1, new Gamepad());
        }
        else
        {
            player2 = PlayerInput.Instantiate(characters[characterIndex], 1, null, -1, gamepads[player2GamepadIndex]);
        }

        player1.transform.position = new Vector2(-3, 0);
        player2.transform.position = new Vector2(3, 0);

        //if(player1.gameObject.TryGetComponent<StickManController>(out StickManController playerController1))
        //{
        //    if(player2.gameObject.TryGetComponent<StickManController>(out StickManController playerController2))
        //    {
        //        playerController1.
        //    }
        //}
        // todo get rid of find object and just use player1 and player2.gameobject
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Players.Count: " + players.Length);
        // players[0].transform.position = new Vector2(-10, 0);

        //players[1].
        if (players[0].TryGetComponent<CharacterController>(out CharacterController controller1))
        {
            if (players[1].TryGetComponent<CharacterController>(out CharacterController controller2))
            {
                controller1.target = players[1].GetComponent<Transform>();
                controller2.target = players[0].GetComponent<Transform>();
                controller1.enemyBoxCollider2d = controller1.target.GetComponent<BoxCollider2D>();
                controller2.enemyBoxCollider2d = controller2.target.GetComponent<BoxCollider2D>();
                Debug.Log("Players[0].position: " + players[0].name + ": " + players[0].transform.position);
                controller1.enabled = true;
                controller2.enabled = true;


            }
        }
        if (players[0].TryGetComponent<Hurtbox>(out Hurtbox p1Hurtbox))
        {
            if (players[1].TryGetComponent<Hurtbox>(out Hurtbox p2Hurtbox))
            {
                Debug.Log("hurtboxes found");
                p1Hurtbox.healthBar = p1Healthbar;
                p2Hurtbox.healthBar = p2Healthbar;
            }
        }

        // PlayerInput.Instantiate(characters[characterIndex],  new Vector3(-1, 0, 0), Quaternion.identity);
        // characters[characterIndex].SetActive(true);
    }

    //public void Update()
    //{
    //    Debug.Log("Players[0].position: " + players[0].name + ": " + players[0].transform.position);
    //}
}
