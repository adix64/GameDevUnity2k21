using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public float speed = 3f;
    public float rotSpeed = 3f;
    Transform cameraTransform;
    Rigidbody rigidbody;
    Vector3 moveDir;
    Vector3 initialPos;
    public float minPossibleY = -50f;//prag sub care nu mai exista platforme
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {//de N ori pe secunda, N fluctuant
        GetMoveDirection();

        ApplyRootMotion();

        ApplyRootRotation();

        HandleJump();
    }
    private void ApplyRootMotion()
    {
        //transform.position += dir * speed * Time.deltaTime; //doar daca nu avem rigidbody atasat
        float velY = rigidbody.velocity.y; //pastram viteza pe axa verticala, calculata de motorul fizic
        rigidbody.velocity = moveDir * speed; // suprascriem viteza corpului cu directia de miscare
        rigidbody.velocity = new Vector3(rigidbody.velocity.x,
                                         velY, //pentru gravitatie, cadere libera
                                         rigidbody.velocity.z);
    }
    private void ApplyRootRotation()
    {
        if (moveDir.magnitude < 10e-3f) //rotim doar daca directia de miscare e nenula
            return;
        Quaternion targetRotation = Quaternion.LookRotation(moveDir);
        //roteste smooth de la rotatia curenta la rotatia tinta
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);
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
        if (transform.position.y < minPossibleY)
            transform.position = initialPos; // a cazut de pe platforma, il teleportam la pozitia initiala
    }
}
