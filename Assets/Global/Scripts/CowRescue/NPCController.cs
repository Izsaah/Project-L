using System;
using System.Collections;
using UnityEngine;

namespace ProjectL.Global.Script.CowRescue
{
    public class NPCController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float reachDistance = 0.5f;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string walkingBoolParam = "isWalking";

        private Transform currentTarget = null;
        private Action onReachedCallback = null;
        private bool isMoving = false;

        private void Start()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void WalkTo(Transform target, Action onReached)
        {
            currentTarget = target;
            onReachedCallback = onReached;
            isMoving = true;

            if (animator != null && !string.IsNullOrEmpty(walkingBoolParam))
            {
                animator.SetBool(walkingBoolParam, true);
            }

            StartCoroutine(MoveToTargetRoutine());
        }

        private IEnumerator MoveToTargetRoutine()
        {
            while (isMoving && currentTarget != null)
            {
                // Calculate direction
                Vector3 targetPos = currentTarget.position;
                targetPos.y = transform.position.y; // Keep vertical position

                // Rotate towards target
                Vector3 direction = (targetPos - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                // Move towards target
                transform.position = Vector3.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);

                // Check distance
                if (Vector3.Distance(transform.position, targetPos) <= reachDistance)
                {
                    isMoving = false;
                }

                yield return null;
            }

            // Reached destination
            isMoving = false;
            if (animator != null && !string.IsNullOrEmpty(walkingBoolParam))
            {
                animator.SetBool(walkingBoolParam, false);
            }

            onReachedCallback?.Invoke();
            onReachedCallback = null;
        }

        public void StopWalking()
        {
            isMoving = false;
            StopAllCoroutines();
            if (animator != null && !string.IsNullOrEmpty(walkingBoolParam))
            {
                animator.SetBool(walkingBoolParam, false);
            }
        }
    }
}
