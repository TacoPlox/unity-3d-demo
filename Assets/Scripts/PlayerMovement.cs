using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Animator animator;
    CharacterController characterController;

    private float vMovement = 0f;
    private float hMovement = 0f;
    private Vector3 movement = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;

    private bool running = false;
    private bool jumping = false;

    private bool wasJumping = false;

    enum AnimationState {
        IDLE = 0,
        WALKING = 1,
        RUNNING = 2,
        JUMPING = 3,
        FALLING = 4,
        LANDING = 5,
    }



    public float walkSpeed = 3f;
    public float runSpeed = 4.5f;
    public float turnSpeed = 5f;
    public float jumpSpeed = 4f;



    public float fallGravityMultiplier = 2.5f;
    public float lowJumpGravityMultiplier = 2f;


   

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        
        
        //Character can move when on the ground
        if (characterController.isGrounded) {
            jumping = false;
            //Update character movement
            vMovement = Input.GetAxis("Vertical");
            hMovement = Input.GetAxis("Horizontal");
            movement = new Vector3(hMovement, 0f, vMovement);

            //Prevent faster diagonal movement
            movement = Vector3.ClampMagnitude(movement, 1f);

            //Check if running
            // running = Input.GetKey(KeyCode.LeftShift);
            running = Input.GetButton("Run");

            movement *= running ? runSpeed : walkSpeed;

            //Update rotation
            Vector3 newForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
            rotation = Quaternion.LookRotation(newForward);
            transform.rotation = rotation;

            //Jump
            if (Input.GetButtonDown("Jump")) {
                movement.y += jumpSpeed;
            }
        }


        //Apply gravity Mario Bros. style
        if (characterController.velocity.y < 0f) {
            movement.y += Physics.gravity.y * fallGravityMultiplier * Time.deltaTime;
        } else if (characterController.velocity.y > 0f && !Input.GetButton("Jump")) {
            movement.y += Physics.gravity.y * lowJumpGravityMultiplier * Time.deltaTime;
        } else {
            movement.y += Physics.gravity.y * Time.deltaTime;
        }


        characterController.Move(movement * Time.deltaTime);
        


        //Update animation state
        int state = (int)AnimationState.IDLE;

        if (!characterController.isGrounded) {

            if (characterController.velocity.y > 0f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping")  && !animator.GetCurrentAnimatorStateInfo(0).IsName("Falling")) {
                state = (int)AnimationState.JUMPING;
            } else {
                state = (int)AnimationState.FALLING;
            }

        } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Falling") && characterController.isGrounded) {
            state = (int)AnimationState.LANDING;

        } else if (running) {

            state = (int)AnimationState.RUNNING;

        } else if (vMovement != 0f || hMovement != 0f) {

            state = (int)AnimationState.WALKING;

        }

        animator.SetInteger("state", state);
    }

    private void OnAnimatorMove() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);    

        if (stateInfo.fullPathHash == Animator.StringToHash("Jumping.Falling.Landing")) {
            animator.ApplyBuiltinRootMotion();
        }
    }

}
