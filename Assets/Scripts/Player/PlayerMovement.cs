using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private bool _usePhysicsMovement = false; // Toggle between physics and transform movement
    
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Animator _animator;
    private Camera _mainCamera;

    private const string _horizontal = "horizontal";
    private const string _vertical = "vertical";
    private const string _lastHorizontal = "lastHorizontal";
    private const string _lastVertical = "lastVertical";

    private Bounds _cameraBounds;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main;
        
        // If not using physics movement, disable gravity
        if (!_usePhysicsMovement)
        {
            _rb.gravityScale = 0f;
        }
    }

    private void Start()
    {
        CalculateCameraBounds();
    }

    private void CalculateCameraBounds()
    {
        var height = _mainCamera.orthographicSize;
        var width = height * _mainCamera.aspect;

        var minX = Globals.WorldBounds.min.x + width;
        var maxX = Globals.WorldBounds.max.x - width;

        var minY = Globals.WorldBounds.min.y + height;
        var maxY = Globals.WorldBounds.max.y - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0.0f),
            new Vector3(maxX, maxY, 0.0f)
        );
    }

    void Update()
    {
        _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        
        // Choose movement method
        if (_usePhysicsMovement)
        {
            // Physics-based movement (affected by collisions, forces, etc.)
            _rb.linearVelocity = _movement * _moveSpeed;
        }
        else
        {
            // Direct transform movement (not affected by physics)
            Vector3 deltaMovement = _movement * _moveSpeed * Time.deltaTime;
            transform.Translate(deltaMovement);
            
            // Stop any residual velocity
            _rb.linearVelocity = Vector2.zero;
        }

        // Update animator
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero) {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
    }

    void LateUpdate()
    {
        // Follow the player
        Vector3 targetPosition = transform.position;
        targetPosition.z = _mainCamera.transform.position.z; // Keep camera's Z position

        // Clamp the camera position within bounds
        targetPosition.x = Mathf.Clamp(targetPosition.x, _cameraBounds.min.x, _cameraBounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, _cameraBounds.min.y, _cameraBounds.max.y);

        // Apply the clamped position to the camera
        _mainCamera.transform.position = targetPosition;
    }
}