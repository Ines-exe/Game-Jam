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
        CheckGr
