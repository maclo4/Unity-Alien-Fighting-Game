using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { Mid, Low, Overhead }
public class AttackFrameData
{
	public int damage = 1;
	public int hitstunFrames = 10;
	public int blockstunFrames = 5;
	public int enableHitboxFrame = 0;
	public int disableHitboxFrame = 1;
	public int totalFrames = 0;
}
/// <summary>
/// This is the parent class for all light attacks, maybe even all normal attacks.
/// It has everything implemented except for the update function. Inherit this class
/// and create the update function to use this.
/// 
/// 
/// </summary>
public class Attack : MonoBehaviour, IHitboxResponder
{

	public CharacterController characterController;
	public GameObject lightAttackHitboxes;
	public LayerMask mask;
	public int damage = 1;
	public int hitstunFrames = 10;
	public int blockstunFrames = 5;
	public int enableHitboxFrame = 0;
	public int disableHitboxFrame = 1;
	public int totalFrames = 0;
	protected int currentActiveFrame = 1;
	//public Attack lightAttack;
	protected Hitbox hitbox { get; set; } //TODO: delet this
	protected Hitbox[] hitboxes;
	public bool attacking = false;
	
	//public enum BlockType { Mid, Low, Overhead}
	public BlockType blockType = BlockType.Mid;
	public enum DirectionFacing { Left, Right }
	DirectionFacing directionFacing { get; set; }
	//public StickManController character;
	//public float hitboxPointXOffset;
	//public float hitboxPointYOffset;
	//public int numberOfHitboxes;
	//public Vector2 hitboxSize;


	//int i = 0;
	public void Awake()
	{
		enabled = false;
		//hitboxes = gameObject.GetComponents<Hitbox>();
		hitboxes = lightAttackHitboxes.GetComponents<Hitbox>();
		initializeHitboxes(hitboxes);
		hitbox = hitboxes[0];

		//for (int j = 0; j < numberOfHitboxes; j++)
		//{
		//	hitboxes.Add(gameObject.AddComponent<Hitbox>());
		//}

        //Vector2 hitboxPoint = transform.position;
        //hitboxPoint.x = hitboxPoint.x + hitboxPointXOffset;
       
        //hitbox.initializeHitbox(mask, hitboxPoint, hitboxSize);
        //hitbox.setResponder(this);
		//hitbox.hitboxSize = new Vector2(1, .5f);

	}

	public void initializeHitboxes(Hitbox[] _hitboxes)
    {
		foreach(Hitbox hitbox in _hitboxes)
        {
			UnityEngine.Debug.Log("initializing hitbox...");
			hitbox.setResponder(this);
        }
    }
	
	public void attack()
	{

		enabled = true;
		// and do the rest of your attack
		//attacking = true;
		//hitbox.startCheckingCollision();
		//hitbox.hitboxUpdate();

	}

	public void endAttack()
	{
		enabled = false;
		hitbox.stopCheckingCollision();
	}

    void IHitboxResponder.collisionedWith(Collider2D collider)
    {
        UnityEngine.Debug.Log("collisioned with being called");
	
        //Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
        Hurtbox hurtbox = collider.transform.parent.gameObject.GetComponent<Hurtbox>();
		Debug.Log("Hurtbox.name: " + hurtbox.name);
        hurtbox?.getHitBy(damage, hitstunFrames, blockstunFrames, blockType);
		hitbox.stopCheckingCollision();
    }

	//protected void standardAttackUpdate(AttackFrameData frameData)
 //   {
	//	if (currentActiveFrame == frameData.disableHitboxFrame)
	//	{
	//		hitboxes[0].stopCheckingCollision();
	//		UnityEngine.Debug.Log("Stop checking collision");
	//	}
	//	else if (currentActiveFrame == frameData.enableHitboxFrame)
	//	{
	//		hitboxes[0].startCheckingCollision();
	//	}
	//	else if (currentActiveFrame >= frameData.totalFrames)
	//	{
	//		characterController.setNormalState();
	//		currentActiveFrame = 0;
	//		enabled = false;
	//		return;
	//	}

	//	hitboxes[0].hitboxUpdate();

	//	currentActiveFrame++;

	//}

}