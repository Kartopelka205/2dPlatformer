using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D) , typeof(TouchingDerections) , typeof(Damageable))]

public class PlayerConroller : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 4f;
    public float jumpImpulse = 12f;

    Vector2 moveInput;
    TouchingDerections touchingDerections;
    Damageable damageable;


    public float CurrentMoveSpeed{ get
    {
        if(canMove){
            if (IsMoving && !touchingDerections.IsOnWall){
        if(touchingDerections.IsGrounded){
            if(IsRunning)
            {
                return runSpeed;
            } else 
            {
                return walkSpeed;
            } 
        }
        else{
            return airWalkSpeed;
        }
        }
        else{
                return 0;
            } 
        }else {
            return 0;
        }
        
    } }

    public bool canMove{ get{
        return animator.GetBool(AnimationStrings.canMove);
    }}
    Rigidbody2D rb;
    Animator animator;

    public bool IsAlive{
        get {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

     private void Awake() {
      rb = GetComponent<Rigidbody2D>();  
      animator = GetComponent<Animator>();
      touchingDerections = GetComponent<TouchingDerections>();
      damageable = GetComponent<Damageable>();
    }
    [SerializeField]private bool _isMoving = false;
    public bool IsMoving { get{
        return _isMoving;
    } private set{
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving , value);
    } }

    [SerializeField]private bool _isRunning = false;

    public bool IsRunning {
        get{
            return _isRunning;
        }set{
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning , value);
        }
    }
public bool _isFacingRight= true;
    

    public bool IsFacingRight { get{ return _isFacingRight;} private set{
        if(_isFacingRight != value){
            transform.localScale *= new Vector2 (-1,1);
        }
        _isFacingRight = value;

    } }

    private void FixedUpdate() {
        if(!damageable.LockVelocity)
        rb.velocity = new Vector2 (moveInput.x * CurrentMoveSpeed , rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity , rb.velocity.y);
    }

     public void OnMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();
    
        if(IsAlive){
             IsMoving = moveInput != Vector2.zero;

        SetFaceingDirection(moveInput);
        }else {
            IsMoving = false;
        }
    }

    private void SetFaceingDirection(Vector2 moveInput){
        if(moveInput.x > 0 && !IsFacingRight ){
            IsFacingRight = true;
        }else if (moveInput.x < 0 && IsFacingRight){
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context){
        if (context.started){
            IsRunning = true;
        }else if (context.canceled){
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context){
        if(context.started && touchingDerections.IsGrounded && canMove){
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x , jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context){
        if(context.started){
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnHit(int damage , Vector2 knockback){
        rb.velocity = new Vector2(knockback.x , rb.velocity.y + knockback.y);
    }
}
