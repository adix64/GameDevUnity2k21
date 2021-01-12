using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : Fighter
{
    public List<Transform> opponents;
    Transform activeOpponent = null;
    // Start is called before the first frame update
    void Start()
    {
        base.GetCommonComponents();
    }

    // Update is called once per frame
    void Update()
    {//de N ori pe secunda, N fluctuant
        GetMoveDirection();

        UpdateAnimatorParameters();

        HandleAttack();

        HandleJump();
    }
    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1"))
            animator.SetTrigger("Punch");
    }
    private void OnAnimatorMove()
    {
        ApplyRootMotion();
        ApplyRootRotation();
    }
    private void UpdateAnimatorParameters()
    {
        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(moveDir);
        if (!Input.GetKey(KeyCode.LeftShift))
            characterSpaceMoveDir *= 0.5f;
        animator.SetFloat("Forward", characterSpaceMoveDir.z, 0.15f, Time.deltaTime);
        animator.SetFloat("Right", characterSpaceMoveDir.x, 0.15f, Time.deltaTime);
    }
    private void ApplyRootMotion()
    {
        if (!grounded)
        {
            animator.applyRootMotion = false;
            return;
        }
        animator.applyRootMotion = true;
        //transform.position += dir * speed * Time.deltaTime; //doar daca nu avem rigidbody atasat
        float velY = rigidbody.velocity.y; //pastram viteza pe axa verticala, calculata de motorul fizic
        //rigidbody.velocity = moveDir * speed; // suprascriem viteza corpului cu directia de miscare, personaj aluneca de la hardcodare
        rigidbody.velocity = animator.deltaPosition / Time.deltaTime;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                            velY, //pentru gravitatie, cadere libera
                                            rigidbody.velocity.z);
    }
    private void ApplyRootRotation()
    {
        Vector3 lookDirection = GetDirectionToClosestOpponent();
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        //roteste smooth de la rotatia curenta la rotatia tinta
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
    }

    private Vector3 GetDirectionToClosestOpponent()
    {
        Vector3 lookDirection = transform.forward;
        if (moveDir.magnitude > 10e-3f)
            lookDirection = moveDir; //rotim catre directia de miscare daca aceasta e nenula
        float distToClosestOpp = float.MaxValue;
        Transform closestOpp = null;
        for (int i = 0; i < opponents.Count; i++)
        {
            Vector3 toOpp = opponents[i].position - transform.position;
            float distToOpp = toOpp.magnitude;
            if (distToOpp < distToClosestOpp && distToOpp < combatRange)
            {
                closestOpp = opponents[i];
                distToClosestOpp = distToOpp;
            }
        }
        if (closestOpp != null) //s-a gasit cel mai apropiat inamic in combatRange
            lookDirection = (closestOpp.position - transform.position).normalized;

        return lookDirection;
    }

    private void GetMoveDirection()
    {
        float h = Input.GetAxis("Horizontal");// -1 pentru tasta A, 1 pentru tasta D, 0 altfel
        float v = Input.GetAxis("Vertical");// -1 pentru tasta S, 1 pentru tasta W, 0 altfel
        // directia de miscare relativa la spatiul camerei:
        moveDir = cameraTransform.right * h + cameraTransform.forward * v;
        //directia de miscare este in plan orizontal(fara sus-jos); directiile sunt vectori normalizati(adica de lungime 1):
        moveDir = Vector3.ProjectOnPlane(moveDir, Vector3.up).normalized;
    }
    private void HandleJump()
    {
        CheckGroundedStatus();
        if (grounded && Input.GetButtonDown("Jump"))
            rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
    }
}
