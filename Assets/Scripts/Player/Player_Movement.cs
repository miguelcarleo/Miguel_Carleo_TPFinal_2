using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [HideInInspector] public float currentMoveSpeed;
    public float walkingMoveSpeed = 3;
    public float backwardsWalkingSpeed = 2;
    public float runningSpeed = 4.5f;
    public float backwardsRunningSpeed = 3.5f;
    public float airSpeed = 1.5f;
    [HideInInspector] public Vector3 dir;
    [HideInInspector] public float hzInput, vInput;
    CharacterController controller;

    [SerializeField] float groundYOffset;
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] float gravity = -9.81f;
    [SerializeField] float jumpForce = 10;
    [HideInInspector] public bool jumped;
    Vector3 velocity;

    public MovementBaseState previousState;
    public MovementBaseState currentState;

    public IdleState idle = new IdleState();
    public WalkingState walking = new WalkingState();
    public RunningState running = new RunningState();
    public JumpState jump = new JumpState();

    [HideInInspector] public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        SwitchState(idle);
    }

    private void Update()
    {
        if (GameManager.isActive)
        {
            GetDirectionAndMove();
            Gravity();
            Falling();

            animator.SetFloat("hzInput", hzInput);
            animator.SetFloat("vInput", vInput);

            currentState.UpdateState(this);
        }
    }

    public void SwitchState(MovementBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    void GetDirectionAndMove()
    {
        hzInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        Vector3 airDir = Vector3.zero;
        if (!IsGrounded()) airDir = transform.forward * vInput + transform.right * hzInput;
        else dir = transform.forward * vInput + transform.right * hzInput;
        controller.Move((dir.normalized * currentMoveSpeed + airDir.normalized * airSpeed) * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundYOffset, groundLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Gravity()
    {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }

    void Falling() => animator.SetBool("Falling", !IsGrounded());

    public void JumpForce() => velocity.y += jumpForce;

    public void Jumped() => jumped = true;
}
