using UnityEngine;
using System;
using System.Collections;


public class IKControl : MonoBehaviour
{
    public AnimationClip basePoseAnimationClip;
    public Animator animator;
    public bool ikActive = false;
    
    public Transform target = null;

    [Header("Pivots")]
    public Transform rightHandPivot = null;
    public Transform leftHandPivot = null;
    public Transform rightHintPivot = null;
    public Transform leftHintPivot = null;

    [Header("Weights")]
    [Range(0, 1)]
    public float handsWeight = 1;
    [Range(0, 1)]
    public float headWeight = 1;
    [Range(0, 1)]
    public float bodyWeight = 1;

    void Start()
    {
        // Check if animator and animationClip have been assigned
        if (animator == null || basePoseAnimationClip == null)
        {
            Debug.LogError("Animator or AnimationClip not assigned");
            return;
        }

        // Set pivot positions from animation clip
        float sampleTime = 0.1f;
        basePoseAnimationClip.SampleAnimation(gameObject, sampleTime);

        if (rightHandPivot != null)
            rightHandPivot.position = animator.GetBoneTransform(HumanBodyBones.RightHand).position;

        if (leftHandPivot != null)
            leftHandPivot.position = animator.GetBoneTransform(HumanBodyBones.LeftHand).position;

        if (rightHintPivot != null)
            rightHintPivot.position = animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position;

        if (leftHintPivot != null)
            leftHintPivot.position = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position;
    }

    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                // Set the look lookObj position, if one has been assigned
                if (target)
                {
                    // Set the look target position and weight
                    animator.SetLookAtPosition(target.position);
                    animator.SetLookAtWeight(headWeight);

                    //// Set the right hand position and weight
                    //if (rightHandPivot != null)
                    //{
                    //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handsWeight);
                    //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handsWeight);
                    //    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPivot.position);
                    //    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandPivot.rotation);
                    //}

                    //// Set the left hand position and weight
                    //if (leftHandPivot != null)
                    //{
                    //    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handsWeight);
                    //    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handsWeight);
                    //    animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPivot.position);
                    //    animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPivot.rotation);
                    //}

                    //// Set the right elbow position and weight
                    //if (rightHintPivot != null)
                    //{
                    //    animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, bodyWeight);
                    //    animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHintPivot.position);
                    //}

                    //// Set the left elbow position and weight
                    //if (leftHintPivot != null)
                    //{
                    //    animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, bodyWeight);
                    //    animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftHintPivot.position);
                    //}
                }
            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }
}