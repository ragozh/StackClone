using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    const float JOYSTICK_LENGTH = 54;
    [SerializeField] Transform _outerJoystick;
    [SerializeField] Transform _innerJoystick;
    [SerializeField] Transform _directionPoint;
    const float MIN_DRAG_VALID = 0.4f;
    Camera _mainCamera;
    Vector3 _startPosition;
    Vector3 _endPosition;
    float _maxDistance;
    bool _touchUI;

    [HideInInspector] public Vector3? _destination;

    void Awake()
    {
        _mainCamera = Camera.main;
        _touchUI = false;
        EnableJoystick(false);
        _directionPoint.gameObject.SetActive(false);
    }

    void Update()
    {
        //MouseController();
        TouchController();
    }

    void MouseController()
    {
        var worldPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            OnBeginDrag(worldPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnDragContinue(worldPosition);
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
                OnDragContinue(worldPosition);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {                
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
        SetJoystickPosition(Input.mousePosition);
        EnableJoystick(true);

        _directionPoint.gameObject.SetActive(true);
    }

    void OnDragContinue(Vector3 worldPosition)
    {
        _endPosition = worldPosition;
        var direction = _endPosition - _startPosition;
        if (direction.magnitude < MIN_DRAG_VALID) return;
        direction.Normalize();

        SetInnerJoystickPosition(direction);
        SetDirectionPoint(direction);

        _destination = transform.position + direction;
    }

    void SetDirectionPoint(Vector3 direction)
    {
        _directionPoint.position = new Vector3(
                     transform.position.x + direction.x * 2,
                     transform.position.y,
                     transform.position.z + direction.z * 2
                );
    }

    void OnEndDrag(Vector3 worldPosition)
    {
        _destination = null;
        EnableJoystick(false);

        _directionPoint.gameObject.SetActive(false);
    }

    void SetJoystickPosition(Vector3 position)
    {        
        _outerJoystick.position = position;
        _innerJoystick.position = position;
    }

    void SetInnerJoystickPosition(Vector3 direction)
    {
        var UIDirection = new Vector3(direction.x, direction.z, 0);
        _innerJoystick.position = _outerJoystick.position + UIDirection * JOYSTICK_LENGTH;
    }

    void EnableJoystick(bool isActive)
    {
        _outerJoystick.gameObject.SetActive(isActive);
        _innerJoystick.gameObject.SetActive(isActive);
    }
}
