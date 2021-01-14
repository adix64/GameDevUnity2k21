using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public string opponentLayer;
    public string side;
    public int damage = 5;
    //necesita collider cu isTrigger bifat
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer(opponentLayer))
            return;
        Animator opponentAnimator = other.GetComponentInParent<Animator>();
        opponentAnimator.Play(side + "TakeHit");
        opponentAnimator.SetInteger("TakenDamage", damage);
    }
    //necesita rigidbody:
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer != LayerMask.NameToLayer(opponentLayer))
    //        return;
    //    Animator opponentAnimator = collision.gameObject.GetComponentInParent<Animator>();
    //    opponentAnimator.Play(side + "TakeHit");
    //}
}
