using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly string selectedCharacter = "SelectedCharacter";


    bool characterSelected = false;
    public GameObject startButton;
    public GameObject player1Button;
    public GameObject player2Button;


    ReadOnlyArray<Gamepad> connectedGamepads;
    public AssignControllers assignControllers;
    private void Awake()
    {

        connectedGamepads = Gamepad.all;
    }
    public void addGamepad()
    {
       
        connectedGamepads = Gamepad.all;
    }
    public void OnUIButtonPress()
    {
     
        Debug.Log("Button pressed");
    }
    
    public void SelectCharacter()
    {
        PlayerPrefs.SetInt(selectedCharacter, 0);
       
        startButton.SetActive(true);
    }

    public void Assign1PlayerControllers()
    {
        player1Button.SetActive(false);
        player2Button.SetActive(false);

        //todo: idk if theres a better way of doing this, but unity crashed whenever I had
        //a while loop that was essentially infinite even tho it was actually just waiting for inputs.
        // so im just using this other object's Update() as the loop
        assignControllers.playerCount = PlayerCount.OnePayer;
        assignControllers.initialize();
        assignControllers.enabled = true;

    }
    public void Assign2PlayerControllers()
    {
        player1Button.SetActive(false);
        player2Button.SetActive(false);

        //todo: idk if theres a better way of doing this, but unity crashed whenever I had
        //a while loop that was essentially infinite even tho it was actually just waiting for inputs.
        // so im just using this other object's Update() as the loop
        assignControllers.playerCount = PlayerCount.TwoPlayer;
        assignControllers.initialize();
        assignControllers.enabled = true;

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
