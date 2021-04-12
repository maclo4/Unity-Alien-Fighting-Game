using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    
    [SerializeField] public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void TogglePause(InputAction.CallbackContext context)
    {
      
        if (buttonDown(context)) //Input.GetButtonDown("Pause")
        {
            isPaused = !isPaused;
            Debug.Log("puase");
        }
        if (isPaused)
        {
            activateMenu();
        }
        else
        {
            deactivateMenu();
        }
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

        //UnityEngine.Debug.Log(buttonBool);
        return buttonBool;
    }
    // Update is called once per frame
    //void Update()
    //{
        
    //    if (Input.GetButtonDown("Pause")) //Input.GetButtonDown("Pause")
    //    {
    //        isPaused = !isPaused;
    //        Debug.Log("puase");
    //    }
    //    if (isPaused)
    //    {
    //        activateMenu();
    //    }
    //    else
    //    {
    //        deactivateMenu();
    //    }
    //}

    void activateMenu()
    {
        Time.timeScale = 0;
        PauseMenuUI.SetActive(true);
        
    }
    void deactivateMenu()
    {
        Time.timeScale = 1;
        PauseMenuUI.SetActive(false);
       
    }
}
