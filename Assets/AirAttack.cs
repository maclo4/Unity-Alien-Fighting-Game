using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttack : Attack
{
    public bool isGrounded { get; set; } = false;
    
    public void Update()
    {
        isGrounded = characterController.IsGrounded();
        UnityEngine.Debug.Log("isGrounded: " + isGrounded);

        if (isGrounded == true || currentActiveFrame > totalFrames || followUpAttackChained == true)
        {
            if (followUpAttackChained == false)
            {
                UnityEngine.Debug.Log("setnormalstate from attack update");
                // characterController.cancelAerialAttack();
                characterController.setNormalState();

            }
            if(isGrounded == true || followUpAttackChained == true)
            {
                hitboxes[0].stopCheckingCollision();
                //followUpAttackChained = false;
            }

           // hitboxes[0].stopCheckingCollision();
            isGrounded = false;
            currentActiveFrame = 0;
            enabled = false;
            chainingAttackAllowed = false;
            followUpAttackChained = false;
            jumpCancelAllowed = false;

        return;
        }
        //Debug.Log("current frame: " + currentActiveFrame);
        else if (currentActiveFrame == disableHitboxFrame)
        {
            hitboxes[0].stopCheckingCollision();
            UnityEngine.Debug.Log("Stop checking collision");
        }
        else if (currentActiveFrame == enableHitboxFrame)
        {
            hitboxes[0].startCheckingCollision();
        }
        //stop processing the attack here
        //else if (currentActiveFrame > totalFrames || followUpAttackChained == true)
        //{
        //    if (followUpAttackChained == false)
        //    {
        //        UnityEngine.Debug.Log("setnormalstate from attack update");

        //        characterController.setNormalState();

        //        followUpAttackChained = false;
        //    }

        //    currentActiveFrame = 0;
        //    enabled = false;
        //    chainingAttackAllowed = false;
        //    followUpAttackChained = false;
        //    jumpCancelAllowed = false;
        //    return;
        //}

        hitboxes[0].hitboxUpdate();

        currentActiveFrame++;

    }
    
}
