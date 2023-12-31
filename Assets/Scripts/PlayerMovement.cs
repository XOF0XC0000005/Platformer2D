using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    private const string Horizontal = "Horizontal";
    private const string Jump = "Jump";

    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _movementSmoothing = .05f;
    [SerializeField] private float _jumpForce = 450f;
    [SerializeField] private LayerMask _jumplableGround;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _velocity = Vector2.zero;
    private BoxCollider2D _playerBoxCollider;

    private int _speedParametr = Animator.StringToHash("speed");
    private int _isJumpingParametr = Animator.StringToHash("isJumping");
    private int _isFallingParametr = Animator.StringToHash("isFalling");
    private float _horizontalMove = 0f;
    private bool _facingRight = true;
    private bool _isJumping = false;
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _playerBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        _horizontalMove = Input.GetAxisRaw(Horizontal) * _speed;

        if (Input.GetButtonDown(Jump) && IsGrounded())
        {
            _isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        _isJumping = false;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void Move()
    {
        float multiplier = 10f;

        Vector2 targetVelocity = new Vector2(_horizontalMove * Time.fixedDeltaTime * multiplier, _rigidbody2D.velocity.y);
        _animator.SetFloat(_speedParametr, Mathf.Abs(_horizontalMove));

        if (!IsCollideWhenFalling() && IsGrounded())
        {
            _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        }
            
        if (_horizontalMove > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_horizontalMove < 0 && _facingRight)
        {
            Flip();
        }

        if (_isJumping)
        {
            _animator.SetTrigger(_isJumpingParametr);
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
        }

        if (!IsGrounded())
        {
            _animator.SetTrigger(_isFallingParametr);
        }
    }

    private bool IsGrounded()
    {
        float extraHeight = .03f;
        RaycastHit2D raycastHit = Physics2D.Raycast(_playerBoxCollider.bounds.center, Vector2.down, _playerBoxCollider.bounds.extents.y + extraHeight, _jumplableGround);

        return raycastHit.collider != null;
    }

    private bool IsCollideWhenFalling()
    {
        float extraWidth = .02f;
        RaycastHit2D raycastHitRight = Physics2D.Raycast(_playerBoxCollider.bounds.center, Vector2.right, _playerBoxCollider.bounds.extents.y + extraWidth, _jumplableGround);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(_playerBoxCollider.bounds.center, Vector2.left, _playerBoxCollider.bounds.extents.y + extraWidth, _jumplableGround);

        return raycastHitLeft.collider != null || raycastHitRight.collider != null;
    }
}
