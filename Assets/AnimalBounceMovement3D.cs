using UnityEngine;

public class AnimalBounceMovement3D : MonoBehaviour
{
    public float bounceForce = 10f;             // The strength of the bounce.
    public float groundCheckDistance = 0.2f;   // Distance to check for the ground beneath the character.
    public LayerMask groundLayer;              // Layer mask for ground detection.
    public float rotationSpeed = 5f;           // Speed at which the character rotates to face the direction.
    public float targetThreshold = 1.5f;       // Distance to consider the target "reached".
    public float targetPauseTime = 2f;         // Time to wait before selecting a new target after reaching the current one.
    public float bounceInterval = 0.5f;        // Time between each bounce force application (how frequently the character bounces).

    public float groundMinX = -10f;            // Min X boundary of the ground area.
    public float groundMaxX = 10f;             // Max X boundary of the ground area.
    public float groundMinZ = -10f;            // Min Z boundary of the ground area.
    public float groundMaxZ = 10f;             // Max Z boundary of the ground area.

    public float minTargetRadius = 5f;         // Minimum radius within which the target can appear.
    public float maxTargetRadius = 10f;        // Maximum radius within which the target can appear.

    private Rigidbody rb;                      // The Rigidbody component of the animal.
    private Vector3 targetPosition;            // The position the character is moving toward.
    private bool isTurning;                    // Whether the character is currently turning.
    private bool isBouncing;                   // Whether the character is currently bouncing.
    private bool hasReachedTarget;             // Whether the target has been reached.
    private float pauseTimer;                  // Timer for the pause after reaching the target.
    private float bounceTimer;                 // Timer to control how frequently the character applies the bounce force.

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SelectNewTarget(); // Choose the first target position.
    }

    void FixedUpdate()
    {
        bool isGrounded = IsGrounded();

        if (isGrounded && !isBouncing && !isTurning && !hasReachedTarget)
        {
            StartTurn(); // Start turning toward the new target position.
        }

        if (isTurning)
        {
            SmoothTurn();
        }

        if (isBouncing)
        {
            // Decrease the bounce timer to apply force at the set intervals.
            bounceTimer -= Time.deltaTime;

            // Apply the bounce force at the regular intervals, only if grounded.
            if (bounceTimer <= 0f && isGrounded)
            {
                ApplyBounceForce();
                bounceTimer = bounceInterval; // Reset the bounce timer to apply force again.
            }

            // Check if the character reached the target.
            if (Vector3.Distance(transform.position, targetPosition) <= targetThreshold)
            {
                hasReachedTarget = true;
                isBouncing = false;
                Debug.Log("Target reached! Now pausing before selecting a new target.");

                // Start the pause timer to wait before selecting a new target.
                pauseTimer = targetPauseTime;
            }
        }

        // If the target is reached and the pause timer is active, decrease the pause time.
        if (hasReachedTarget)
        {
            pauseTimer -= Time.deltaTime;
            if (pauseTimer <= 0)
            {
                // Once the pause time is over, select a new target and start the next cycle of bouncing.
                SelectNewTarget();
            }
        }

        // Debug visualization of the target position
        Debug.DrawLine(transform.position, targetPosition, Color.blue, 0.1f);
        Debug.DrawRay(transform.position, Vector3.down * groundCheckDistance, Color.green);
    }

    bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        bool grounded = Physics.Raycast(rayOrigin, Vector3.down, out hit, groundCheckDistance, groundLayer);

        Debug.Log("Grounded: " + grounded);

        return grounded;
    }

    void SelectNewTarget()
    {
        // Get a random radius between the min and max values
        float randomRadius = Random.Range(minTargetRadius, maxTargetRadius);

        // Get a random angle (in radians) to generate the target within the circle defined by the radius
        float randomAngle = Random.Range(0f, 2f * Mathf.PI);

        // Calculate the target's X and Z position using polar coordinates (radius and angle)
        float randomX = transform.position.x + randomRadius * Mathf.Cos(randomAngle);
        float randomZ = transform.position.z + randomRadius * Mathf.Sin(randomAngle);

        // Ensure the random target is within the defined ground boundaries
        randomX = Mathf.Clamp(randomX, groundMinX, groundMaxX);
        randomZ = Mathf.Clamp(randomZ, groundMinZ, groundMaxZ);

        Vector3 randomTargetPosition = new Vector3(randomX, transform.position.y, randomZ);

        // Perform a raycast from the randomly selected position to get the ground level (Y position).
        RaycastHit hit;
        if (Physics.Raycast(randomTargetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            targetPosition = hit.point;  // Set the target position to the ground hit point.
        }
        else
        {
            // If no ground is found (unlikely in most cases), just set a fallback target position at the same height.
            targetPosition = randomTargetPosition;
        }

        Debug.Log("New Target Position: " + targetPosition);

        // Reset the flags so we can move towards the new target.
        hasReachedTarget = false;
        StartTurn(); // Start turning toward the new target after selecting it.
    }

    void StartTurn()
    {
        isTurning = true; // Start turning toward the new target.
        Debug.Log("Turning toward new target...");
    }

    void SmoothTurn()
    {
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;
        directionToTarget.y = 0; // Ignore the Y-axis for rotation.

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // Smoothly rotate toward the target direction.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        // Check if the rotation is close enough to consider the turn complete.
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isTurning = false; // Finish turning.
            Debug.Log("Finished turning. Starting bounce...");
            StartBounce();
        }
    }

    void StartBounce()
    {
        // Once turning is done, start bouncing toward the target.
        isBouncing = true;
        bounceTimer = bounceInterval; // Initialize the bounce timer.

        Debug.Log("Bouncing toward target...");
    }

    void ApplyBounceForce()
    {
        // Apply a force in the direction of the target only if grounded.
        Vector3 directionToTarget = (targetPosition - transform.position).normalized;

        // Apply a continuous force toward the target.
        rb.velocity = Vector3.zero; // Reset the velocity before applying the force.
        rb.AddForce((directionToTarget + Vector3.up).normalized * bounceForce, ForceMode.Impulse);
    }
}
