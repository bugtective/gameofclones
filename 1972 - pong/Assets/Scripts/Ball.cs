using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

    private Rigidbody2D _rb = default;
    private Vector2 _dir = default;

    private event Action<bool> _onGoal = null;
    private event Action<string> _onBounce = null;

    public Rigidbody2D RigidBody => _rb;

    public void Initialize(Action<bool> onGoalCallback, Action<string> onBounceCallback)
    {
        transform.position = Vector3.zero;
        _onGoal = onGoalCallback;
        _onBounce = onBounceCallback;

        if (!_rb)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
    }

    public void KickOff()
    {
        _rb.velocity = Vector2.right * _speed;
    }

    public void Reset()
    {
        _rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
    }

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.tag.StartsWith("Player"))
        {
            var xDir = 0f;
            string soundName = default;

            if (collision.gameObject.tag == "Player1")
            {
                xDir = 1f;
                soundName = "ball1";
            }
            else
            {
                xDir = -1f;
                soundName = "ball3";
            }
            

            // 1 <- at the top of the racket
            // 0 <- at the middle of the racket
            // -1 <- at the bottom of the racket
            var yDir = (transform.position.y - collision.transform.position.y) / collision.collider.bounds.size.y;

            // Calculate direction, make length = 1 with .normalized
            _dir = new Vector2(xDir, yDir).normalized;

            _rb.velocity = _dir * _speed;

            _onBounce?.Invoke(soundName);
        }
        else if (collision.gameObject.tag == "Goal")
        {
            bool player1 = transform.position.x > 0;
            _onGoal?.Invoke(player1);
            Reset();
        }
        else
        {
            _onBounce?.Invoke("ball2");
        }
    }
}
