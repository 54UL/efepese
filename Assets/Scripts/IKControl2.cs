using UnityEngine;
using System.Collections;

public class IKControl2 : MonoBehaviour
{
    public AnimationClip basePoseAnimationClip;

    public Animator animator;
    public Transform lookTarget;
    public Transform rightAimTarget;
    public Transform leftAimTarget;

    [Header("Weights")]
    [Range(0, 1)]

    public float lookWeight = 1f;
    [Range(0, 1)]
    public float rightAimWeight = 1f;

    [Range(0, 1)]
    public float leftAimWeight = 1f;

    [Header("Ranges")]
    [Range(-10, 10)]
    public float leftAimOffset = 1f;

    [Range(-10, 10)]
    public float rightAimOffset = 1f;


    // This function sets the position and rotation of the head and spine to look at the target.
    void LookAtIK(Transform target, float weight)
    {
        animator.SetLookAtWeight(weight);
        animator.SetLookAtPosition(target.position);
    }

    // This function sets the position and rotation of the hands to aim at the target.
    void AimIK(AvatarIKGoal goal, Transform target, float weight)
    {
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKRotationWeight(goal, weight);
        animator.SetIKPosition(goal, target.position);
        animator.SetIKRotation(goal, target.rotation);
    }
    void Start()
    {
        // Check if animator and animationClip have been assigned
        if (animator == null || basePoseAnimationClip == null)
        {
            Debug.LogError("Animator or AnimationClip not assigned");
            return;
        }

        // Set pivot positions from animation clip
        float sampleTime = 0f;
        basePoseAnimationClip.SampleAnimation(gameObject, sampleTime);

        if (rightAimTarget != null)
        {
            Transform rightHandBone = animator.GetBoneTransform(HumanBodyBones.RightHand);
            rightAimTarget.position = rightHandBone.position;
            rightAimTarget.rotation = rightHandBone.localRotation;
        }

        if (leftAimTarget != null)
        {
            Transform leftHandBone = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            leftAimTarget.position = leftHandBone.position;
            leftAimTarget.rotation = leftHandBone.localRotation;
        }

        //if (rightHintPivot != null)
        //    rightHintPivot.position = animator.GetBoneTransform(HumanBodyBones.RightLowerArm).position;

        //if (leftHintPivot != null)
        //    leftHintPivot.position = animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).position;
    }

    void OnAnimatorIK()
    {
        // If the animator is not set, return.
        if (animator == null)
            return;

        // Look at the target.
        if (lookTarget != null)
        {
            LookAtIK(lookTarget, lookWeight);
        }

        // Aim at the right hand target.
        if (rightAimTarget != null)
        {
            AimIK(AvatarIKGoal.RightHand, rightAimTarget, rightAimWeight);
        }

        // Aim at the left hand target.
        if (leftAimTarget != null)
        {
            AimIK(AvatarIKGoal.LeftHand, leftAimTarget, leftAimWeight);
        }
    }
}
