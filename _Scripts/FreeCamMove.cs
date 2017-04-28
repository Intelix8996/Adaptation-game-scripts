using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamMove : MonoBehaviour {

    private float moveX = 0;
    private float moveY = 0;
    private float moveZ = 0;

    private float mouseX = 0;
    private float mouseY = 0;

    public float Sensitivity = 1f;
    public float MoveSpeed = 1f;
    public float MoveSpeedAdd = 1f;

    private Vector3 Movement;

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Up/Down");
        moveZ = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(KeyCode.LeftShift))
            MoveSpeed += MoveSpeedAdd;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            MoveSpeed -= MoveSpeedAdd;

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            MoveSpeed /= 2.5f;
        if (Input.GetKeyUp(KeyCode.LeftAlt))
            MoveSpeed *= 2.5f;

        Movement = new Vector3(moveX, moveY, moveZ) * MoveSpeed;

        Move(Movement);
        Look(mouseX, mouseY);
    }

    void Move (Vector3 Movement)
    {
        transform.Translate(Movement);
    }

    void Look (float X, float Y)
    {
        transform.eulerAngles += new Vector3(-Y, X, 0) * Sensitivity;
    }

    private void OnDisable()
    {
        transform.eulerAngles = new Vector3(55, 0, 0);
        Cursor.visible = true;
    }
}
