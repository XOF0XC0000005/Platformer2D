using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyWaypointMovement : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private float _speed = 2f;

    private Transform[] _points;
    private int _currentPoint;
    private bool _facingRight = true;
    private Animator _animator;
    private int _speedParametr = Animator.StringToHash("speed");

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _points = new Transform[_path.childCount];

        for (int i = 0; i < _path.childCount;  i++)
        {
            _points[i] = _path.GetChild(i);
        }
    }

    private void FixedUpdate()
    {
        Transform target = _points[_currentPoint];
        Vector3 direction = (target.position - transform.position).normalized;

        if (direction.x < 0 && _facingRight)
        {
            Flip();
        }
        else if (direction.x > 0 && !_facingRight)
        {
            Flip();
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
        _animator.SetFloat(_speedParametr, Mathf.Abs(direction.x));

        if (transform.position == target.position)
        {
            _currentPoint++;

            if (_currentPoint >= _points.Length)
            {
                _currentPoint = 0;
            }
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
