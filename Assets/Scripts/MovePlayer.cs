using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovePlayer : Fighter
{
    public List<Transform> opponents;
    public bool aiming = false;
    public Transform weapon;
    public Transform weaponTip;
    public GameObject projectilePrefab;
    Transform chest;
    Transform upperChest;
    Transform rightHand;

    void Start()
    {
        base.GetCommonComponents();
        chest = animator.GetBoneTransform(HumanBodyBones.Chest);
        upperChest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
        rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {//de N ori pe secunda, N fluctuant
        GetMoveDirection();
        UpdateAnimatorParameters();
        HandleAttack();
        HandleJump();
    }
    private void HandleShooting()
    {
        aiming = Input.GetButton("Fire2");
        dontTurnHead = aiming;
        animator.SetBool("Aiming", aiming);
        weapon.gameObject.SetActive(aiming);//face arma (in)vizibila daca (nu)tinteste
        
        if (!aiming)
            return;
        if (Input.GetButtonDown("Fire1"))
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = weaponTip.position;
            projectile.transform.up = weaponTip.right;
        }

        CopyRightHandTransformOnWeapon();
        Quaternion alignWeaponToCamera = Quaternion.FromToRotation(weaponTip.right,
                                                                   cameraTransform.forward);

        alignWeaponToCamera.ToAngleAxis(out float angle, out Vector3 axis);
        angle *= 0.5f;//jumatate din rotatie
        alignWeaponToCamera = Quaternion.AngleAxis(angle, axis);
        chest.rotation = alignWeaponToCamera * chest.rotation;
        upperChest.rotation = alignWeaponToCamera * upperChest.rotation;
        
        CopyRightHandTransformOnWeapon();
    }
    private void CopyRightHandTransformOnWeapon()
    {
        weapon.position = rightHand.position;
        weapon.rotation = rightHand.rotation;
    }
    private void LateUpdate()
    {
        HandleShooting();
        LookAtOpponent();
    }
    private void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && !aiming)
            animator.SetTrigger("Punch");
    }
    private void OnAnimatorMove()
    {
        ApplyRootMotion();
        Vector3 lookDirection = GetDirectionToactiveOpponentonent();
        if (aiming)
            lookDirection = cameraTransform.forward;
        lookDirection = Vector3.ProjectOnPlane(lookDirection, Vector3.up).normalized;
        ApplyRootRotation(lookDirection);
    }
    private void UpdateAnimatorParameters()
    {
        Vector3 characterSpaceMoveDir = transform.InverseTransformDirection(moveDir);
        if (!Input.GetKey(KeyCode.LeftShift) && !isInCombatRange)
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
    private Vector3 GetDirectionToactiveOpponentonent()
    {
        Vector3 lookDirection = transform.forward;
        if (moveDir.magnitude > 10e-3f)
            lookDirection = moveDir; //rotim catre directia de miscare daca aceasta e nenula
        float distToactiveOpponent = float.MaxValue;
        activeOpponent = null;
        isInCombatRange = false;
        toOpp = transform.forward * 5f; //lasa garda jos daca nu sunt oponenti
        for (int i = 0; i < opponents.Count; i++)
        {
            toOpp = opponents[i].position - transform.position;
            float distToOpp = toOpp.magnitude;
            if (distToOpp < distToactiveOpponent && distToOpp < combatRange)
            {
                activeOpponent = opponents[i];
                distToactiveOpponent = distToOpp;
                isInCombatRange = true;
            }
        }
        animator.SetFloat("DistToOpp", toOpp.magnitude);
        if (activeOpponent != null) //s-a gasit cel mai apropiat inamic in combatRange
            lookDirection = (activeOpponent.position - transform.position).normalized;

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
