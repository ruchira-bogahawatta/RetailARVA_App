using UnityEngine;
using System.Collections; // Required for Coroutines

public class AvatarMovement : MonoBehaviour
{
    public Transform arCamera; // Assign AR Camera in the Inspector
    public GameObject avatar; // Assign Avatar in the Inspector

    private Animator animator;
    public float moveSpeed = 0.5f;
    public float stopDistance = 20.0f; // Stopping threshold
    private bool isMoving = false;
    private bool isWaiting = false; // Flag to check if we're waiting before movement

    void Start()
    {
        if (avatar == null)
        {
            Debug.LogError("Avatar is not assigned in the inspector.");
            return;
        }

        animator = avatar.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component missing from avatar.");
        }
        else
        {
            animator.applyRootMotion = false; // Disable root motion if manually moving
        }
    }

    void Update()
    {
        if (avatar == null || arCamera == null) return;

        float distance = Vector3.Distance(avatar.transform.position, arCamera.position);

        if (distance > stopDistance)
        {
            if (!isMoving && !isWaiting) 
            {
                StartCoroutine(StartWalkingAfterDelay(0.5f)); // Wait 0.5 seconds before moving
            }
        }
        else
        {
            StopMoving();
        }
    }

    IEnumerator StartWalkingAfterDelay(float delay)
    {
        isWaiting = true;
        animator.SetFloat("Speed", 2.0f); // Start animation immediately
        yield return new WaitForSeconds(delay); // Wait before actual movement
        isWaiting = false;
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveTowardsCamera();
        }
    }

    void MoveTowardsCamera()
    {
        FaceAlign(); // Ensure correct facing direction

        Vector3 direction = (arCamera.position - avatar.transform.position).normalized;
        direction.y = 0; // Ensure horizontal movement

        avatar.transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void StopMoving()
    {
        if (isMoving || isWaiting)
        {
            animator.SetFloat("Speed", 0f);
            isMoving = false;
            isWaiting = false;
            StopAllCoroutines(); // Cancel any running delay
        }
    }

    void FaceAlign()
    {
        if (arCamera == null) return;
        Vector3 cameraForward = arCamera.position - avatar.transform.position;
        cameraForward.y = 0; // Keep only horizontal rotation
        avatar.transform.forward = -cameraForward; // Rotate 180 degrees
    }
}
