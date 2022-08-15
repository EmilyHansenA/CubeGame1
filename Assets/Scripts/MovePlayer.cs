using UnityEngine;

[RequireComponent(typeof(BoxCollider), (typeof(Rigidbody)))]
public class MovePlayer : MonoBehaviour
{
    Transform target;
    Rigidbody _rigibody;

    [SerializeField] FixedJoystick _joystick;
    [SerializeField] private int _speed = 3;

    private float _leftRange = -3;
    private float _rightRange = 3;

    private void Start()
    {
        _rigibody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.touchCount == 1) setTarget();
        if (target) Move();
        JoystickMove();
    }

    /// <summary>
    /// Движение с помощью джойстика
    /// </summary>
    private void JoystickMove()
    {
        _rigibody.velocity = new Vector3(_joystick.Horizontal * _speed, _rigibody.velocity.y, _joystick.Vertical * _speed);

        if (transform.position.x < _leftRange)
        {
            transform.position = new Vector3(_leftRange, transform.position.y, 0);
        }
        else if (transform.position.x > _rightRange)
        {
            transform.position = new Vector3(_rightRange, transform.position.y, 0);
        }
    }

    /// <summary>
    /// Устанавливает новую точку для передвижения Player
    /// </summary>
    private void setTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider == null) return;
            if (target) Destroy(target.gameObject);

            GameObject newTarget = Instantiate(Resources.Load("Cube"), hit.point, Quaternion.identity) as GameObject;

            target = newTarget.transform;

            target.position = new Vector3(target.position.x, target.position.y + 0.6f, 0f);
        }
    }

    /// <summary>
    /// Двигает игрока на позицию клика
    /// </summary>
    private void Move()
    {
        if ((transform.position.x == target.transform.position.x) || (_joystick.Horizontal != 0 || _joystick.Vertical != 0))
        {
            Destroy(target.gameObject);
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }
}
