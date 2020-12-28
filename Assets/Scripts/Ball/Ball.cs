using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] ScoreSO _score;
    Rigidbody2D _rigidbody2D;
    public float _moveSpeed = 10;
    Shooter _shooter;
    float _delay = 2;
    Material _material;
    float _nextTime;
    int _contact;
    float _lastY = -99;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _material = GetComponent<Renderer>().material;
        _material.SetColor("_Color", Color.white);
        _nextTime = Time.time + _delay;
        _contact = 0;
    }

    void Update()
    {
        _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * _moveSpeed;
        if (Time.time < _nextTime)  return;
        if (transform.position.y == _lastY)   
        {
            _rigidbody2D.velocity += Vector2.down * 0.05f;
        }
        _lastY = transform.position.y;
        _nextTime = Time.time + _delay;
    }

    public void SetShooter(Shooter shooter) => _shooter = shooter;
    public void BackToPool() =>  _shooter?.BackToPool(this);

    internal void Shoot(Vector3 direction)
    {
        _rigidbody2D.AddForce(direction);
    }

    internal void BallScore()
    {
        _contact++;
        var color = Color.Lerp(Color.white, Color.yellow, (float) _contact / 50);
        _material.SetColor("_Color", color);
        if (_contact > 50)
        {
            _score.Score = _score.Score + _contact / 50;
        }
    }
}
