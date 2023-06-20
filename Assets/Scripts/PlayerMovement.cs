using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private const string Horizontal = "Horizontal";

    [SerializeField] private float _speed = 20f;
    [SerializeField] private float _movementSmoothing = .05f;
    [SerializeField] private float _jumpForce = 400f;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Vector2 _velocity = Vector2.zero;

    private int _speedAttribute = Animator.StringToHash("speed");
    private float _horizontalMove = 0f;
    private bool _facingRight = true;
    private bool isJumping = false;
    

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalMove = Input.GetAxisRaw(Horizontal) * _speed;

        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
        isJumping = false;
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
        _animator.SetFloat(_speedAttribute, Mathf.Abs(_horizontalMove));
        _rigidbody2D.velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        if (_horizontalMove > 0 && !_facingRight)
        {
            Flip();
        }
        else if (_horizontalMove < 0 && _facingRight)
        {
            Flip();
        }

        if (isJumping)
        {
            _animator.SetTrigger("isJumping");
            _rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
        }
    }
}
