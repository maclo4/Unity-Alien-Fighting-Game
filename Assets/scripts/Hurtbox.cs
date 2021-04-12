using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
   // public float  width, height;
    public Vector2 offset, size;
    public BoxCollider2D hurtbox;
    public Animator animator;
    public CharacterController characterController;
    public GameObject healthBar;
    private ColliderState _state = ColliderState.Open;
  

    public void Awake()
    {
        hurtbox.enabled = true;
        hurtbox.isTrigger = true;
        hurtbox.offset = offset;
        hurtbox.size = size;


    }
    public bool getHitBy(int damage, int hitstun, int blockstun, BlockType blockType)
    {

        // Do something with the damage and the state
        Debug.Log("GetHitBy: " + damage);

        if (!characterController.wasAttackBlocked(blockType, blockstun))
        {
            // apply hitstun if attack was NOT blocked
            characterController.setHitstunState(damage, hitstun);
            healthBar.TryGetComponent<TMPro.TextMeshProUGUI>(out TMPro.TextMeshProUGUI healthText);
            healthText.text = (characterController.health.ToString() + "/100");
        }
        return true;
    }

    //public void Update()
    //{
    //    Debug.Log("Hurtbox update");
    //}

    private void OnDrawGizmos()
    {
        // You can simply reuse the code from the hitbox,
        // but taking the size, rotation and scale from the collider

    }

}
