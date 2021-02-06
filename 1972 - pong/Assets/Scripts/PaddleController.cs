using UnityEngine;

public class PaddleController : MonoBehaviour
{
    private float _speed = 10f;
    private string _axis = default;
    private Vector3 _initialPos = default;
    private Rigidbody2D _rb = default;

    public void Initialize(string axis, string tag, Vector3 pos)
    {
        _axis = axis;
        gameObject.tag = tag;
        _initialPos =  new Vector3(pos.x, pos.y, 0f);

        if (!_rb)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    public void Reset()
    {
        _rb.velocity = Vector2.zero;
        transform.position = _initialPos;
    }

    void FixedUpdate()
    {
        var verticalMovement = Input.GetAxisRaw(_axis);
        _rb.velocity = new Vector2(0, verticalMovement) * _speed;
    }
}
