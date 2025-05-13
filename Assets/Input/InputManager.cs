using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // just call InputManager.Movement
    public static Vector2 Movement;

    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Movement"];
    }

    private void Update() {
        Movement = _moveAction.ReadValue<Vector2>();
    }
}
