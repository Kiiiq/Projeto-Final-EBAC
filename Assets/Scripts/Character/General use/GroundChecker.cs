using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private float distanceToGround = 2.5f;

    private bool isGrounded=true;

    

    public bool IsGrounded()
    {
        return isGrounded;
    }

    private void Update()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, distanceToGround))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded)
        {
            Debug.DrawRay(transform.position, Vector3.down * distanceToGround, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * distanceToGround, Color.red);
        }
    }
}
