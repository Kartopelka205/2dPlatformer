using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDerections), typeof(Damageable))]
public class Knigt : MonoBehaviour
{
  public float walkSpeed = 3f;
  public DetectionZone attackZone;
  public DetectionZone cliffDetectionZone;
  public float walkStopRate = 0.05f;
  Rigidbody2D rb;
  TouchingDerections touchingDerections;
  Animator animator;
  Damageable damageable;

    public enum WalkableDirection { Right, Left};

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection{
        get {return _walkDirection;}
        set{
            if(_walkDirection != value){
                gameObject.transform.localScale  = new Vector2(gameObject.transform.localScale.x * -1 , gameObject.transform.localScale.y);
             if (value == WalkableDirection.Right){
                walkDirectionVector = Vector2.right;
            } else if(value == WalkableDirection.Left){
                walkDirectionVector = Vector2.left;
            }
            } 
            _walkDirection = value;}
    }
    public bool _hasTargget = false;
    public bool HasTargget { get{return _hasTargget ;} 
    private set{
        _hasTargget = value;
        animator.SetBool(AnimationStrings.hasTargget , value);
    } }
    public  bool CanMove {get {
        return animator.GetBool(AnimationStrings.canMove);
    }}

    public float AttackCooldown { get{
        return animator.GetFloat(AnimationStrings.attackCooldown);
    } private set{
        animator.SetFloat(AnimationStrings.attackCooldown , Mathf.Max(value , 0));
    } }

    private void Awake() {
    rb = GetComponent<Rigidbody2D>();
    touchingDerections = GetComponent<TouchingDerections>();
    animator = GetComponent<Animator>();
    damageable = GetComponent<Damageable>();
  }
  private void Update() {
    HasTargget = attackZone.detectedColliders.Count > 0 ;
    if(AttackCooldown > 0 ){
        AttackCooldown -= Time.deltaTime;
    }
  }
  private void FixedUpdate() {
    if(touchingDerections.IsGrounded && touchingDerections._isOnWall){
        FlipDirection();
    }
    if(!damageable.LockVelocity)
    {
        if (CanMove){
    rb.velocity = new Vector2 (walkSpeed * walkDirectionVector.x , rb.velocity.y);
    } else {
        rb.velocity = new Vector2 (Mathf.Lerp(rb.velocity.x , 0 , walkStopRate) , rb.velocity.y);
    }
    }
  }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right){
            WalkDirection = WalkableDirection.Left;
        }else if(WalkDirection == WalkableDirection.Left){
            WalkDirection = WalkableDirection.Right;
        }else{
            Debug.LogError("Current walkable direction is not set to legal values right or left");
        }
    }

    public void OnHit(int damage , Vector2 knockback){
        rb.velocity = new Vector2(knockback.x , rb.velocity.y + knockback.y);
    }
    public void OnCliffDetected(){
        if(touchingDerections.IsGrounded){
            FlipDirection();
        }
    }
}
