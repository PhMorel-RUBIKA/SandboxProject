using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f;  // The distance to dash
    public float dashDuration = 0.2f;  // The duration of the dash
    public float dashCooldown = 2f;  // The cooldown between dashes

    private bool isDashing = false;  // Flag to check if the player is currently dashing
    private Vector3 dashStartPosition;  // The starting position of the dash
    private float dashTimer = 0f;  // Timer for tracking the dash duration
    private float dashCooldownTimer = 0f;  // Timer for tracking the dash cooldown

    private Rigidbody rb;  // Reference to the player's Rigidbody2D component
    public static PlayerDash instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isDashing)
        {
            UpdateDash();
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    public void OnDashPerformed(InputAction.CallbackContext value)
    {
        if (isDashing && dashCooldownTimer > 0f) return;
        StartDash();
        SatchelManager.instance.OnSatchelDropPerformed(value);
    }

    private void StartDash()
    {
        isDashing = true;
        dashStartPosition = transform.position;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        Vector2 dashDirection = PlayerMovement.instance._moveVector.normalized;
        Debug.Log(dashDirection * dashDistance * Time.deltaTime);
        rb.AddForce(dashDirection * dashDistance, ForceMode.Impulse);
    }

    private void UpdateDash()
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }
}
