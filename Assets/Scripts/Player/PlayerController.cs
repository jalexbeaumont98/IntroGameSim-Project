using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] public float jumpForce = 10f;

    [SerializeField] private float airSpeed = 5f;
    [SerializeField] float accelRate = 15f;   // how fast you accelerate
    [SerializeField] float decelRate = 10f;   // how fast you slow down

    [Header("Stuff!")]
    [SerializeField] private int hp;
    [SerializeField] private int maxHp = 3;


    [Header("Ground Detection Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.05f, 0.05f);
    [SerializeField] private float coyoteTime = 0.1f;


    private bool isDropping;

    public bool groundedNow;
    private bool wasGrounded = false;
    private float coyoteTimer = 0f;

    private float horizontalMovement;

    [Header("Gravity")]
    [SerializeField] private float baseGravity = 2;
    [SerializeField] private float fallSpeedMultiplier = 2f;
    [SerializeField] private float maxFallSpeed = 18f;

    [Header("Firing Settings")]
    public float fireRate = 2f;  // shots per second — higher = faster
    private float nextFireTime = 0f;

    [Header("Other References")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private CinemachineVirtualCameraBase ogCam;
    [SerializeField] private CinemachineVirtualCameraBase deathCam;

    [SerializeField] private PlayerShootController shootCon;

    //private refs
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    public static event System.Action<int> OnPlayerHPChange;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

    }

    public void SetHP(int hpIn)
    {
        hp = hpIn;
        OnPlayerHPChange?.Invoke(hp);
    }

    void Update()
    {

        HandleMovement();
        //DroppingPlatform();

    }

    void LateUpdate()
    {
        UpdateAnimator();
        FlipTowardsMouse();
    }

    void HandleMovement()
    {

        if (GroundCheck())
        {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        }

        else
        {
            float targetSpeed = horizontalMovement * airSpeed;
            float rate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelRate : decelRate;

            float newX = Mathf.MoveTowards(rb.velocity.x, targetSpeed, rate * Time.deltaTime);

            rb.velocity = new Vector2(newX, rb.velocity.y);
        }

        Gravity();
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        print("attempted a jump!");

        if (context.performed)
        {
            if (GroundCheck())
            {
                print("on the ground! but not for long..");

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                groundedNow = false;
            }

            else if (context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextFireTime)
        {
            shootCon.FireProjectile();
            print("shooted!");

            // Set next allowed time to fire
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    /* todo platform logic

    public void DropPlatform(InputAction.CallbackContext context)
    {

        if (context.started)
            isDropping = true;

        if (context.canceled)
            isDropping = false;

    }
    

    private void DroppingPlatform()
    {

        if (!isDropping) return;


        print("in the block!");
        Collider2D hit = Physics2D.OverlapBox(
            groundChecker.position,
            groundCheckSize,
            0f,
            groundLayer
        );



        if (hit != null)
        {
            Platform platform = hit.transform.GetComponent<Platform>();

            if (platform != null)
            {
                platform.SetDropdown();
            }
        }
    }

    */

    void UpdateAnimator()
    {
        anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isJumping", !GroundCheck());
        anim.SetFloat("verticalVelocity", rb.velocity.y);
    }


    void FlipTowardsMouse()
    {
        // Convert mouse position to world space
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // If the mouse is to the right of the player, face right (not flipped)
        if (mouseWorldPos.x > transform.position.x)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            shootCon.SetFlipped(false);
        }

        else
        {
            GetComponent<SpriteRenderer>().flipX = true;
            shootCon.SetFlipped(true);
        }
        
    }




    public bool GroundCheck()
    {

        groundedNow = Physics2D.OverlapBox(
            groundChecker.position,
            groundCheckSize,
            0f,
            groundLayer
        );

        if (groundedNow)
        {
            // Reset timer whenever you touch the ground
            coyoteTimer = coyoteTime;

            if (!wasGrounded)
            {
                //dosomething
            }
        }
        else
        {
            // Count down if not grounded
            coyoteTimer -= Time.deltaTime;
        }

        //print(groundedNow);

        // Still "grounded" if on ground OR timer is active
        return groundedNow || coyoteTimer > 0f;
    }

    private void Gravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }

        else
        {
            rb.gravityScale = baseGravity;
        }

    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        print("damage: " + damage + " hp: " + hp);

        OnPlayerHPChange?.Invoke(hp);
        if (hp <= 0)
            Die();
    }

    private void Die()
    {
        transform.position = respawnPoint.position;
        hp = maxHp;
        OnPlayerHPChange?.Invoke(hp);
        //CameraController.GetMainCineCam().Priority = 0;
        deathCam.Priority = 25;

        print("player ded :(");
        //MenuMan.Instance.SetDeathMenu();
        SceneManager.LoadScene("GameOver");
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundChecker.position, groundCheckSize);
    }
}
