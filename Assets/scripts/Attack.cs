using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType { Mid, Low, Overhead }
public enum HitType { Normal, Launcher, Aerial, Spike}
public class AttackFrameData : ScriptableObject
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
	
	public CharacterController			characterController;
	public GameObject					attackHitboxes;
	public LayerMask					mask;
	public int							damage = 1;
	private int							hitstunFrames = 10;
	public int							hitAdvantage = 0;
	private int							blockstunFrames = 5;
	public int							blockAdvantage = 0;
	public int							enableHitboxFrame = 0;
	protected int						disableHitboxFrame = 1;
	public int							activeFrames = 0;
	public int							totalFrames = 0;
	protected int						currentActiveFrame = 1;
	public float						blockPushback = 1;
	public Vector2						hitTrajectory;
	//public Attack lightAttack;
	protected Hitbox					hitbox { get; set; } //TODO: delet this
	protected Hitbox[]					hitboxes;
	public bool							attacking = false;
	public bool							chainingAttackAllowed = false;
	public bool							followUpAttackChained = false;
	public bool							jumpCancelAllowed = false;
	
	//public enum BlockType { Mid, Low, Overhead}
	public BlockType					blockType = BlockType.Mid;
	public HitType						hitType;
	public enum DirectionFacing { Left, Right }
	DirectionFacing						directionFacing { get; set; }
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
		hitboxes = attackHitboxes.GetComponents<Hitbox>();
		initializeHitboxes(hitboxes);
		hitbox = hitboxes[0];
		calculateFrameData();
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

	private void calculateFrameData()
    {

		blockstunFrames = totalFrames - enableHitboxFrame + blockAdvantage;
		hitstunFrames = totalFrames - enableHitboxFrame + hitAdvantage;
		disableHitboxFrame = enableHitboxFrame + activeFrames + 1;

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
		chainingAttackAllowed = true;
        UnityEngine.Debug.Log("collisioned with being called");
		Vector2 tempHitTrajectory;
		float tempBlockPushback;
		if (characterController.directionFacing == CharacterController.DirectionFacing.Left)
		{
			tempHitTrajectory.x = hitTrajectory.x * -1;
			tempHitTrajectory.y = hitTrajectory.y;
			tempBlockPushback = blockPushback * -1;
			
		}
        else
        {
			tempHitTrajectory.x = hitTrajectory.x;
			tempHitTrajectory.y = hitTrajectory.y;
			tempBlockPushback = blockPushback;
		}
        //Hurtbox hurtbox = collider.GetComponent<Hurtbox>();
        Hurtbox hurtbox = collider.transform.parent.gameObject.GetComponent<Hurtbox>();
		Debug.Log("Hurtbox.name: " + hurtbox.name);
        jumpCancelAllowed = (bool)(hurtbox?.getHitBy(damage, hitstunFrames, blockstunFrames, tempBlockPushback, tempHitTrajectory, blockType, hitType));

		hitbox.setCollidedState();
    }



}