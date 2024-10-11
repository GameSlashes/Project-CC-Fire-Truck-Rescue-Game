using UnityEngine;

public class IKControl : MonoBehaviour
{
    public Animator animator;

    [Header("IK Settings for Hands")]
    public bool enableIKHands = true;                // Enable or disable IK for hands
    public Transform rightHandTarget;                // Target for right hand IK
    public Transform leftHandTarget;                 // Target for left hand IK
    [Range(0, 1)] public float handIKWeight = 1.0f;  // Weight for hand IK (1 = full influence)

    [Header("IK Settings for Feet")]
    public bool enableIKFeet = true;                 // Enable or disable IK for feet
    public Transform rightFootTarget;                // Target for right foot IK
    public Transform leftFootTarget;                 // Target for left foot IK
    [Range(0, 1)] public float footIKWeight = 1.0f;  // Weight for foot IK (1 = full influence)

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // Handle IK for hands
        if (enableIKHands)
        {
            // Right Hand IK
            if (rightHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handIKWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
            }

            // Left Hand IK
            if (leftHandTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handIKWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
            }
        }
        else
        {
            // Reset IK weights if hands IK is disabled
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
        }

        // Handle IK for feet
        if (enableIKFeet)
        {
            // Right Foot IK
            if (rightFootTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, footIKWeight);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
            }

            // Left Foot IK
            if (leftFootTarget != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footIKWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footIKWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
            }
        }
        else
        {
            // Reset IK weights if feet IK is disabled
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0);
        }
    }
}
