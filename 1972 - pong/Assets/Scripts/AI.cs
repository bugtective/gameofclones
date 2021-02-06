using UnityEngine;

public class AI : MonoBehaviour
{
    private float _speed = 10f;
    private Vector3 _initialPos = default;
    private Rigidbody2D _rb = default;
    private Rigidbody2D _ballRigidBody = default;

    public void Initialize(string tag, Vector3 pos, Rigidbody2D ballRigidBody)
    {
        gameObject.tag = tag;
        _initialPos =  new Vector3(pos.x, pos.y, 0f);

        if (!_rb)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        _ballRigidBody = ballRigidBody;
    }

    public void Reset()
    {
        _rb.velocity = Vector2.zero;
        transform.position = _initialPos;
    }

    void FixedUpdate()
    {
        if (_ballRigidBody.velocity.x <= 0)
        {
            return;
        }

        if (_ballRigidBody.velocity.y != 0)
        {
            if (_ballRigidBody.position.y > _rb.position.y)
            {
               _rb.position += Vector2.up * _speed * Time.deltaTime;
            }
            if (_ballRigidBody.position.y < _rb.position.y)
            {
                _rb.position += Vector2.down * _speed * Time.deltaTime;
            }
        }
    }
}
