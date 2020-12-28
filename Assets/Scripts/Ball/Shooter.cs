using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooter : MonoBehaviour
{
    [SerializeField] Ball _ballPrefab;
    [SerializeField] BallGameManager _manager;
    [SerializeField] BallSO _data;
    Camera _mainCamera;
    TextMeshPro _text;
    LaunchPreview _launchPreview;
    Vector3 _startPosition;
    Vector3 _endPosition;
    int _ballsBacked;
    Queue<Ball> _ballPool;
    int ballAmount;
    bool _touchUI = false;

    void Awake()
    {
        _text = GetComponentInChildren<TextMeshPro>();
        _launchPreview = GetComponent<LaunchPreview>();
        ballAmount = _data.BallAmount;
        _ballsBacked = _data.BallAmount;
        _ballPool = new Queue<Ball>();
        _mainCamera = Camera.main;
    }
    void Update()
    {
        _text.text = "x" + _ballsBacked.ToString();
        if (ballAmount != _data.BallAmount)
        {
            ballAmount = _data.BallAmount;
            _ballsBacked++;
        }
        if (_ballsBacked == ballAmount)
        {
            //MouseController();
            TouchController();
        }
    }

    void MouseController()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition) + Vector3.back * -5;
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(worldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnDraging(worldPosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnEndDrag(worldPosition);
        }
    }

    void TouchController()
    {
        if (Input.touchCount > 0)
        {
            var worldPosition = _mainCamera.ScreenToWorldPoint(Input.GetTouch(0).position) + Vector3.back * -5;
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    _touchUI = true;
                }      
                OnBeginDrag(worldPosition);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (_touchUI)   return;
                OnDraging(worldPosition);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {                
                _launchPreview.Reset();   
                if (_touchUI)
                {
                    _touchUI = false;
                    return;
                }     
                OnEndDrag(worldPosition);
            }
        }
    }

    void OnBeginDrag(Vector3 worldPosition)
    {
        _startPosition = worldPosition;
        _launchPreview.SetStartPoint(transform.position);
    }

    void OnDraging(Vector3 worldPosition)
    {
        _endPosition = worldPosition;
        _launchPreview.SetEndPoint(transform.position - (_endPosition - _startPosition));
    }

    void OnEndDrag(Vector3 worldPosition)
    {
        if (_endPosition.y > _startPosition.y)  return;
        var direction = _endPosition - _startPosition;
        direction.Normalize();
        StartCoroutine(ShootBalls(direction, ballAmount));
    }

    IEnumerator ShootBalls(Vector3 direction, int amount)
    {
        while(amount > 0)
        {
            yield return new WaitForSeconds(0.07f);
            CreateBalls(direction);
            amount--;
        }
    }

    void CreateBalls(Vector3 direction)
    {
        var ball = GetBall();
        ball.transform.position = transform.position;
        ball.Shoot(-direction);
    }

    Ball GetBall()
    {
        if (_ballPool.Count == 0)
            AddNewBall();
        var ball = _ballPool.Dequeue();
        ball.transform.position = transform.position;
        ball.transform.rotation = transform.rotation;
        ball.gameObject.SetActive(true);
        _ballsBacked--;
        if (_ballsBacked < 0)   _ballsBacked = 0;
        return ball;
    }

    void AddNewBall()
    {
        var ball = Instantiate(_ballPrefab, transform.position, Quaternion.identity);
        ball.SetShooter(this);
        ball.gameObject.SetActive(false);
        _ballPool.Enqueue(ball);
    }
    
    public void BackToPool(Ball ball)
    {
        ball.gameObject.SetActive(false);
        _ballPool.Enqueue(ball);

        _ballsBacked++;
        if (_ballsBacked == 1)
        {
            transform.position = ball.transform.position + Vector3.up * 0.5f;
        }
        else if (_ballsBacked == ballAmount)
        {
            _manager.SpawnBlocks();
        }
    }

}
