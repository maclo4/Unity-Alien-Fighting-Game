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
    public Color hurtboxColor;

    public void Awake()
    {
        hurtbox.enabled = true;
        hurtbox.isTrigger = true;
        Gizmos.color = hurtboxColor;

    }
    public bool getHitBy(int damage, int hitstun, int blockstun, float pushback, Vector2 hitTrajectory, BlockType blockType)
    {

        // Do something with the damage and the state
        Debug.Log("GetHitBy: " + damage);

        if (characterController.wasAttackBlocked(blockType, blockstun, pushback) == false)
        {
            // apply hitstun if attack was NOT blocked

            characterController.setHitstunState(damage, hitstun, hitTrajectory); //TODO change pushback to something trajectory
            healthBar.TryGetComponent<TMPro.TextMeshProUGUI>(out TMPro.TextMeshProUGUI healthText);
            healthText.text = (characterController.health.ToString() + "/100");
            return true;
        }
        else
        {
            return false;
        }
    }

    //public void Update()
    //{
    //    Debug.Log("Hurtbox update");
    //}

    private void OnDrawGizmos()
    {
        // You can simply reuse the code from the hitbox,
        // but taking the size, rotation and scale from the collider

        Gizmos.color = hurtboxColor;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawCube(hurtbox.offset, new Vector3(hurtbox.size.x, hurtbox.size.y, 0)); // Because size is halfExtents
    }

}
