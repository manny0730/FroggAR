using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Frog : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UI_GameManager UI_GameManagerScript;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource ThudSound;
    public Transform playerCamera;

    [Header("Jump Settings")]
    //The maximum distance the camera can be from the frog to control it.
    public float maxControlDistance = 15f;
    //The minimum force applied to a jump.
    public float minJumpForce = 5f;
    //The maximum force applied to a jump.
    public float maxJumpForce = 20f;
    //The time in seconds to reach maximum jump force.
    public float maxHoldTime = 2f;
    //The angle of the jump in degrees. 45 degrees gives the maximum range.
    [Range(0f, 90f)]
    public float jumpAngle = 45f;

    [Header("Rotation Settings")]
    //How quickly the frog turns to face the camera's direction.
    public float rotationSpeed = 10f;

    [Header("Trajectory Settings")]
    //The number of points to use for the trajectory line.
    public int trajectoryPoints = 30;
    //The time step between each point on the trajectory line.
    public float trajectoryTimeStep = 0.1f;

    private Rigidbody rb;
    private LineRenderer lineRenderer;

    private int frogLives = 7;
    private bool isGrounded = false;
    private bool isCharging = false;
    private bool jumpRequested = false;
    private bool isPlayerInRange = false;
    private float chargeStartTime;
    private Vector3 jumpForceVector;

    private Vector3 FrogSpawnPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (playerCamera != null)
        {
            isPlayerInRange = Vector3.Distance(playerCamera.position, transform.position) <= maxControlDistance;
        }
        else
        {
            isPlayerInRange = false;
        }
        HandleTouchInput();

        if (isCharging)
        {
            UpdateTrajectory();
        }

        if (isPlayerInRange && UI_GameManagerScript.finalStepActive)
        {
            UI_GameManagerScript.disableFinalStep();
        }
        else if (!isPlayerInRange && !UI_GameManagerScript.finalStepActive)
        {
            UI_GameManagerScript.enableFinalStep();
        }
    }

    void FixedUpdate()
    {
        if (isPlayerInRange)
        {
            OrientFrog();
        }

        if (jumpRequested)
        {
            PerformJump();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (!isPlayerInRange)
                    {
                        return;
                    }

                    if (isGrounded)
                    {
                        isCharging = true;
                        chargeStartTime = Time.time;
                        lineRenderer.enabled = true;
                    }
                    break;

                case TouchPhase.Ended:
                    if (isCharging)
                    {                        
                        isCharging = false;
                        jumpRequested = true;
                        lineRenderer.enabled = false;
                    }
                    break;
            }
        }
    }

    private void OrientFrog()
    {
        if (playerCamera == null) return;
        Vector3 direction = playerCamera.forward;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    private void UpdateTrajectory()
    {
        float holdDuration = Time.time - chargeStartTime;
        float holdRatio = Mathf.Clamp01(holdDuration / maxHoldTime);

        float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, holdRatio);

        Quaternion jumpRotation = Quaternion.AngleAxis(-jumpAngle, transform.right);
        Vector3 jumpDirection = jumpRotation * transform.forward;

        jumpForceVector = jumpDirection * currentJumpForce;

        Vector3 initialVelocity = jumpForceVector / rb.mass;
        Vector3 gravity = Physics.gravity;

        lineRenderer.positionCount = trajectoryPoints;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * trajectoryTimeStep;
            Vector3 pointPosition = transform.position + initialVelocity * t + 0.5f * gravity * t * t;
            lineRenderer.SetPosition(i, pointPosition);
        }
    }

    private void PerformJump()
    {
        rb.AddForce(jumpForceVector, ForceMode.Impulse);
        jumpSound.Play();
        isGrounded = false;
        jumpRequested = false;
    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            if (!jumpRequested && !isCharging)
            {
                isGrounded = true;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

            if (collision.gameObject.CompareTag("Checkpoint"))
            {
                FrogSpawnPoint = transform.position;
            }
        }

        if (collision.gameObject.CompareTag("KillZone"))
        {
            ThudSound.Play();
            if (frogLives > 1)
            {
                respawn();
            }
            else
            {                
                Time.timeScale = 0;
                UI_GameManagerScript.enableLoseScreen();
            }
        }

    }
    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
    void respawn()
    {
        transform.position = FrogSpawnPoint;
        UI_GameManagerScript.decreaseHealth();
        frogLives -= 1;
    }
}
