using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Cowgirl : MonoBehaviour
{
    public float moveSpeed = 5f; 
    private Rigidbody2D rb; 
    private Vector3 ogScale; 

    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround; 
    [SerializeField] private Transform m_GroundCheck1;
    [SerializeField] private Transform m_GroundCheck2;
    [SerializeField] private Transform m_GroundCheck3;

    const float k_GroundedRadius = .2f;
    private bool m_Grounded; 
    private bool m_FacingRight = true;  
    private Vector3 m_Velocity = Vector3.zero;

    private float pullSpeed = 5f; 
    private bool isLassoed = false; 

    [Header("Events")]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    private float offset = 2f; 

    // New variable to track jumping
    private bool isJumping = false; 
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        ogScale = transform.localScale;
        animator = GetComponent<Animator>(); // Get the Animator component

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();

        if (isLassoed)
        {
            PullToCloud(); 
        }
    }

    void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders1 = Physics2D.OverlapCircleAll(m_GroundCheck1.position, k_GroundedRadius, m_WhatIsGround);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(m_GroundCheck2.position, k_GroundedRadius, m_WhatIsGround);
        Collider2D[] colliders3 = Physics2D.OverlapCircleAll(m_GroundCheck3.position, k_GroundedRadius, m_WhatIsGround);

        CheckGrounded(colliders1, wasGrounded);
        CheckGrounded(colliders2, wasGrounded);
        CheckGrounded(colliders3, wasGrounded);

        float xDirection = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime; 
        bool crouch = Input.GetKeyDown(KeyCode.S);
        bool jump = Input.GetKeyDown(KeyCode.W); 

        Move(xDirection, crouch, jump); 
    }

    void CheckGrounded(Collider2D[] colliders, bool wasGrounded)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
                break;
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (m_Grounded || m_AirControl)
        {
            // Handle crouching
            if (crouch && !m_wasCrouching)
            {
                m_wasCrouching = true;
                OnCrouchEvent.Invoke(true);
                StartCoroutine(CrouchForSeconds(2f)); // Start coroutine to crouch for 2 seconds
            }

            if (m_wasCrouching)
            {
                move *= m_CrouchSpeed; // Reduce speed when crouching
            }

            // Move character
            Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Flip character if necessary
            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }

        // Jump
        if (m_Grounded && jump)
        {
            m_Grounded = false;
            isJumping = true; // Set jumping state
            rb.AddForce(new Vector2(0f, m_JumpForce));
            animator.SetBool("isJumping", true); // Set the Animator parameter
        }

        // Reset isJumping when landing
        if (m_Grounded && isJumping)
        {
            isJumping = false; // Reset jumping state
            animator.SetBool("isJumping", false); // Reset Animator parameter
        }
    }

    private IEnumerator CrouchForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_wasCrouching = false; // Reset crouch state
        OnCrouchEvent.Invoke(false); // Notify that the player has stood up
    }

    void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void PullToCloud()
    {
        GameObject cloud = GameObject.Find("cloud");
        transform.position = Vector2.MoveTowards(transform.position, cloud.transform.position, pullSpeed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, cloud.transform.position) > 0.4f)
        {
            isLassoed = false; 
            transform.position = new Vector2(cloud.transform.position.x, cloud.transform.position.y + offset);
        }
    }
}
