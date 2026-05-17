using UnityEngine;

public class YoungPrince_2DPlayerController : MonoBehaviour
{
    [Header("이동 및 점프 설정")]
    [SerializeField] public float _moveSpeed = 10f;
    [SerializeField] public float _jumpForce = 8f;

    [Header("바닥 감지 설정")]
    [SerializeField] public Transform _groundCheck;
    [SerializeField] public float _groundCheckRadius = 0.5f;
    [SerializeField] public LayerMask _groundLayer;

    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;
    private bool _isHappy = false;
    private bool _lookRight = true;
    private float _horizontalInput;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // 2D 캐릭터가 물리 충돌 시 회전해서 넘어지는 것 방지
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_isHappy) return;

        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (_horizontalInput > 0 && !_lookRight)
        {
            Flip();
        }
        else if (_horizontalInput < 0 && _lookRight)
        {
            Flip();
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
            _isGrounded = false;
        }
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        CheckGrounded();

        Move();
    }

    void CheckGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
    }

    void Move()
    {
        _rb.linearVelocity = new Vector2(_horizontalInput * _moveSpeed, _rb.linearVelocity.y);
    }

    void Flip()
    {
        _lookRight = !_lookRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Jump()
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
        Debug.Log("점프 중");
    }

    void UpdateAnimation()
    {
        bool isMoving = Mathf.Abs(_rb.linearVelocity.x) > 0.1f;

        _anim.SetBool("IsMoving", isMoving);
        _anim.SetBool("IsGrounded", _isGrounded);
    }

    public void TriggerEnding()
    {
        _isHappy = true;
        _rb.linearVelocity = Vector2.zero;
        _anim.SetBool("IsHappy", true);
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}