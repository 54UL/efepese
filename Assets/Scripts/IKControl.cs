using UnityEngine;
using System;
using System.Collections;


public class IKControl : MonoBehaviour
{

    public Animator animator;

    public bool ikActive = false;
    public Transform lookObj = null;
    public Transform rightHandPivot = null;
    public Transform leftHandPivot = null;
    public float handsWight = 1;

    void Start()
    {
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {

            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {

                // Set the look target position, if one has been assigned
                if (lookObj != null)
                {
                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(lookObj.position);

                   // animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handsWight);
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handsWight);
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handsWight);

                    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPivot.position);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPivot.position);
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
               animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
               // animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}