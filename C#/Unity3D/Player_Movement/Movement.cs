using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour {

	/// <summary>
    /// I try not to use GetComponent in "not prefab objects" in order not to 
    /// complicate the algorithm in terms of a 'Big O', so I use links
    /// </summary>

    //joystick and buttons(links request)
    public Joystick joystick;
    public JoyButton Jump;
    public JoyButton Sprint;

    //speed, stamina and jump force(6, 5, 12)
	float Speed;
    float stamina;
    float JumpForce;

    //required components
    [SerializeField]
    Rigidbody body;
    [SerializeField]
    Camera cam;

    //movement and rotation vectors
    private Vector3 move;
    private Vector3 camdirection;

    void Start()
    {
        //variables initialization
        stamina = 5;
        Speed = 6;
        JumpForce = 12;
        camdirection = Vector3.zero;
    }

    void Update()
    {
        //checks joystick position on horizontal and change Speed
        ChangeJoySpeed();
    }

    void FixedUpdate()
    {
        PlayerJump();
        PlayerRun();

        MovementAndRotation();
    }

    //////////////////////////////////////////////////////////////////

    private void ChangeJoySpeed()
    {
        if (Mathf.Abs(joystick.Horizontal) < 0.2)
        {
            Speed = 3;
        }
        else Speed = 6;
    }

    private void PlayerJump()
    {
        //jump
        if (Jump.Presed && IsGrounded())
        {
            if (body.velocity.y < 2)
            {
                body.AddForce(Vector3.up * JumpForce * 20 * Time.deltaTime, ForceMode.Impulse);
            }
        }
    }

    private void PlayerRun()
    {
        //run
        //TODO change on RUN_BUTTON
        if (Sprint.Presed)
        {
            if (stamina > 0)
            {
                Speed = 8;
                stamina -= 1 * Time.fixedDeltaTime;
            }
        }
        else
            if (Sprint.Presed)
            Speed = 6;

        if (stamina < 5 && !(Sprint.Presed))
        {
            stamina += 1 * Time.fixedDeltaTime;
        }
    }

    //Sets Movement And Rotation
    private void MovementAndRotation()
    {
        //movement
        Vector3 hor = cam.transform.right * joystick.Horizontal;
        Vector3 ver = transform.forward * joystick.Vertical;
        move = (ver + hor).normalized * Time.fixedDeltaTime * Speed;
        body.MovePosition(body.position + move);
        //rotation
        body.rotation = Quaternion.Lerp(body.rotation, Quaternion.Euler(camdirection), .2f);
    }

    //if player on the ground
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.02f);
    }

    //sets player direction
    public void SetDirection(Vector3 camdirection)
    {
        this.camdirection = camdirection;
    }
    //////////////////////////////////////////////////////////////////
}
