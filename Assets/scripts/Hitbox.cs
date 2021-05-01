using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitboxResponder
{
     void collisionedWith(Collider2D collider);
}

public enum ColliderState
{
    Closed,
    Open,
    Colliding,
    Collided
}


public class Hitbox : MonoBehaviour
{
    public LayerMask mask;
    public Vector2 hitboxSize = Vector2.one;
    public Vector2 hitboxOffset;
    private Vector2 hitboxPoint;
    public float radius = 0.5f;
    public Color inactiveColor;
    public Color collisionOpenColor;
    public Color collidingColor;
    public Color collidedColor;
    private Color currentColor;
    

    private IHitboxResponder _responder = null;
    private ColliderState _state;
    public CharacterController character;
  //  public Vector2 position, boxSize;
    //public float rotation;
   // public LayerMask mask;


    public void Awake()
    {
      // hitboxPoint = transform.position;
        //angle = 0;
      
    }

    public void initializeHitbox(LayerMask _mask, Vector2 _hitboxOffset = default(Vector2), Vector2 _hitboxSize = default(Vector2))
    {
        hitboxOffset = _hitboxOffset;
        hitboxSize = _hitboxSize;
        mask = _mask;
    }
    
    public void hitboxUpdate()
    {
        
        if (_state == ColliderState.Closed || _state == ColliderState.Collided) { return; }

        if (character.directionFacing == CharacterController.DirectionFacing.Left)
        {
            hitboxPoint.x = transform.position.x + hitboxOffset.x;
        }
        else
        {
            hitboxPoint.x = transform.position.x + (hitboxOffset.x * -1);
        }
        hitboxPoint.y = transform.position.y + hitboxOffset.y;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(hitboxPoint, hitboxSize, 0f, mask);
        //UnityEngine.Debug.Log("hitboxPoint: " + hitboxPoint.x + ":" + hitboxPoint.y);

        foreach (Collider2D collider in colliders)
        {

            if (collider != null)
            {
                _state = ColliderState.Colliding;
                _responder?.collisionedWith(collider);
                Debug.Log(collider.gameObject); // todo this isnt tested

                // We should do something with the colliders
            }
            else
            {
                _state = ColliderState.Open;
            }
        }

    }
    /*
        and your methods
    */
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //UnityEngine.Debug.Log("transform position: " + transform.position.x + ":" + transform.position.y);
        checkGizmoColor();

        //Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);

        Gizmos.DrawCube(hitboxOffset, new Vector3(hitboxSize.x, hitboxSize.y, 0)); // Because size is halfExtents
        //hitboxOffset.x = transform.position.x + hitboxOffset.x;
        //hitboxOffset.y = transform.position.y + hitboxOffset.y;
        //drawRectangle(hitboxSize.x, hitboxSize.y, hitboxPoint, currentColor);
        //Gizmos.DrawLine(transform.position, new Vector3(0, 0, 0));
        //Gizmos.DrawWireSphere(transform.position, 10f);
        //UnityEngine.Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);

        //Gizmos.matrix = transform.localToWorldMatrix;
        //Gizmos.DrawCube(transform.position, new Vector3(hitboxSize.x * 2, hitboxSize.y * 2, 0)); // Because size is halfExtents


    }

    private void drawRectangle(float width, float height, Vector3 position, Color color)
    {
        Vector3 tempPosition = position;
        tempPosition.x += width;
       
        UnityEngine.Debug.DrawLine(position, new Vector3(position.x + width, position.y), Gizmos.color);
        UnityEngine.Debug.DrawLine(position, new Vector3(position.x, position.y + height), Gizmos.color);
        UnityEngine.Debug.DrawLine(new Vector3(position.x, position.y + height), new Vector3(position.x + width, position.y + height), Gizmos.color);
        UnityEngine.Debug.DrawLine(new Vector3(position.x + width, position.y + height), new Vector3(position.x + width, position.y), Gizmos.color);
        // UnityEngine.Debug.DrawLine(position, tempPosition, Color.red);
    }
    
    private void checkGizmoColor()
    {
        switch (_state)
        {
            case ColliderState.Closed:

                Gizmos.color = inactiveColor;
                break;
            case ColliderState.Open:

                Gizmos.color = collisionOpenColor;
                break;
            case ColliderState.Colliding:
                
                Gizmos.color = collidingColor;
                break;
            case ColliderState.Collided:
                Gizmos.color = collidedColor;
                break;
        }
    }
    public void startCheckingCollision()
    {
        _state = ColliderState.Open;
    }
    
    public void setCollidedState()
    {
        _state = ColliderState.Collided;
    }
    public void stopCheckingCollision()
    {
        _state = ColliderState.Closed;
        
    }
    public void setResponder(IHitboxResponder responder)
    {
        _responder = responder;
    }
}
