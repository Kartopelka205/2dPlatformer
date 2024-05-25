using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class TouchingDerections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    CapsuleCollider2D touchingCol;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];
    Animator animator;
     
     [SerializeField]private bool _isGrounded;
     [SerializeField]public bool _isOnCeiling;
    [SerializeField] public bool _isOnWall;

    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    public bool IsGrounded { get{
        return _isGrounded;
    } private set{
        _isGrounded = value;
        animator.SetBool(AnimationStrings.isGrounded , value);
    } }
    public bool IsOnWall { get{
        return _isOnWall;
    } private set{
        _isOnWall = value;
        animator.SetBool(AnimationStrings.isOnWall , value);
    } }

    public bool IsOnCeiling{ get{
        return _isOnCeiling;
    } private set{
        _isOnCeiling = value;
        animator.SetBool(AnimationStrings.isOnCeiling , value);
    } }

    private Vector2 wallCheckDerections => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    private void Awake() {
         touchingCol = GetComponent<CapsuleCollider2D>();
         animator = GetComponent<Animator>();
    }
    private void FixedUpdate() {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter , groundHits , groundDistance ) > 0;
        IsOnWall =  touchingCol.Cast(wallCheckDerections , castFilter , wallHits , wallDistance) > 0 ;
        IsOnCeiling = touchingCol.Cast(Vector2.up , castFilter , ceilingHits , ceilingDistance) > 0 ;
    }

}
