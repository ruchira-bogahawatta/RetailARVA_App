using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    public Transform arCamera; // Assign AR Camera in the Inspector
    public GameObject avatar; // Assign AR Camera in the Inspector

    public Animator animator;
    public float moveSpeed = 0.5f;
    public float stopDistance = 25.0f; // Distance at which avatar stops

    void Start()
    {
        FaceAlign();

        animator = avatar.GetComponent<Animator>();
    }

    void Update()
    {
        FaceAlign();

        float distance = Vector3.Distance(avatar.transform.position, arCamera.position);

        if (distance > stopDistance + 1.0f)
        {
            animator.SetFloat("Speed", 2.0f);
            Vector3 direction = (arCamera.position - avatar.transform.position).normalized;
            direction.y = 0; // Keep movement horizontal

            // Move the avatar GameObject
            avatar.transform.position += direction * moveSpeed * Time.deltaTime;

            // Make avatar look at AR Camera (only on Y-axis)
            //avatar.transform.LookAt(new Vector3(arCamera.position.x, avatar.transform.position.y, arCamera.position.z));
        }
        else
        {
            // Stop animation
            animator.SetFloat("Speed", 0f);
        }
    }



    void FaceAlign()
    {
        if (arCamera != null)
        {
            Vector3 cameraForward = arCamera.forward;
            cameraForward.y = 0; // Keep only horizontal rotation
            avatar.transform.forward = -cameraForward; // Rotate 180 degrees
        }
    }
}