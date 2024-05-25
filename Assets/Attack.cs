using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 10;
    public Vector2 knokback = Vector2.zero;
    Collider2D attackCollider;
    private void Awake() {
        attackCollider = GetComponent<Collider2D>();
    }
    private void  OnTriggerEnter2D(Collider2D collision) {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null){
            Vector2 diliveredKnockback = transform.parent.localScale.x > 0 ? knokback : new Vector2(-knokback.x , knokback.y);
            bool goHit = damageable.Hit(attackDamage , diliveredKnockback);
            if(goHit)
            Debug.Log(collision.name + "hit for " + attackDamage);
        }
    }
}
