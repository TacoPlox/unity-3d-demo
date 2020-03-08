using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedJump : MonoBehaviour
{

    public float fallGravityMultiplier = 2.5f;
    public float lowJumpGravityMultiplier = 2f;

    CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.velocity.y < 0f) {
            Vector3 additionalVerticalMovement = Vector3.zero;
            additionalVerticalMovement.y -= Vector3.up.y * Physics.gravity.y * (fallGravityMultiplier - 1) * Time.deltaTime;
            characterController.velocity.Set(characterController.velocity.x, characterController.velocity.y + additionalVerticalMovement.y, characterController.velocity.z);
        } else if (characterController.velocity.y > 0f && !Input.GetKey(KeyCode.Space)) {
            Vector3 additionalVerticalMovement = Vector3.zero;
            additionalVerticalMovement.y -= Vector3.up.y * Physics.gravity.y * (lowJumpGravityMultiplier - 1) * Time.deltaTime;
            // characterController.velocity.Scale(additionalVerticalMovement);
            characterController.velocity.Set(characterController.velocity.x, characterController.velocity.y + additionalVerticalMovement.y, characterController.velocity.z);
        }
    }
}
