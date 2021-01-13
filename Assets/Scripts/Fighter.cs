using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter: MonoBehaviour
{
    public float speed = 3f;
    public float rotSpeed = 3f;
    public float jumpPower = 3f;
    public float groundedThreshold = 0.15f;
    protected Transform cameraTransform;
    protected Animator animator;
    protected Rigidbody rigidbody;
    protected CapsuleCollider capsule;
    protected Vector3 moveDir;
    protected Vector3 initialPos;
    protected bool grounded;
    public float minPossibleY = -50f;//prag sub care nu mai exista platforme
    public float combatRange = 4f;
    protected bool isInCombatRange = false;
    public Transform activeOpponent = null;
    public Vector3 toOpp;
    protected Transform headTransform;
    protected AnimatorStateInfo stateInfo;
    protected void GetCommonComponents()
    {
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
        initialPos = transform.position;
        headTransform = animator.GetBoneTransform(HumanBodyBones.Head);
    }

    protected void CheckGroundedStatus()
    {
        if (transform.position.y < minPossibleY)
            transform.position = initialPos; // a cazut de pe platforma, il teleportam la pozitia initiala

        Ray ray = new Ray(); //aruncam raza in jos putin de deasupra talpilor
        Vector3 centerRayOrigin = transform.position + Vector3.up * groundedThreshold;
        ray.direction = Vector3.down;

        grounded = false; //presupunem ca e in aer
        //aruncam 9 raze: una din centrul capsulei si 8 de pe marginile sale, in cruce si diagonala
        for (float offsetX = -1f; offsetX <= 1f; offsetX += 1)
        {
            for (float offsetZ = -1f; offsetZ <= 1f; offsetZ += 1)
            {
                Vector3 rayOffset = new Vector3(offsetX, 0f, offsetZ).normalized;
                ray.origin = centerRayOrigin + rayOffset * capsule.radius;
                if (Physics.Raycast(ray, 2f * groundedThreshold))
                    grounded = true; //e pe sol, raza a lovit pamant sub picioare
            }
        }
        animator.SetBool("Midair", !grounded);
    }
    protected void ApplyRootRotation(Vector3 lookDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //roteste smooth de la rotatia curenta la rotatia tinta
        float realRotSpeed = isInCombatRange ? rotSpeed * 3f : rotSpeed;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                                              Time.deltaTime * realRotSpeed);
    }
    protected void LookAtOpponent()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (activeOpponent != null && stateInfo.IsName("Grounded"))
        {
            Vector3 fwd = transform.forward;
            Vector3 headLookAtTarget = activeOpponent.position + Vector3.up * 1.2f;
            Vector3 headLookDir = headLookAtTarget - headTransform.position;//suceste gatul
            float theDot = Mathf.Max(0, Vector3.Dot(fwd, headLookDir));// uita-te la opponent doar daca e in fata ta...
            Quaternion newHeadRotation = Quaternion.LookRotation(headLookDir);//..ca sa nu suceasca
            headTransform.rotation = Quaternion.Slerp(headTransform.rotation, newHeadRotation, theDot);

        }
    }
}
