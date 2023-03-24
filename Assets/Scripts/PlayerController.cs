using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _walkingSpeed = 7.5f;
    [SerializeField] private float _runningSpeed = 11.5f;
    [SerializeField] private float _gravity = 20.0f;
    [SerializeField] private float _lookSpeed = 2.0f;
    [SerializeField] private float _lookXLimit = 45.0f;
    private GameManager _gameManager;

    private float _rotationX = 0;
    private CharacterController _characterController;
    private Vector3 _moveDirection = Vector3.zero;

    private bool _canMove = true;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!_gameManager.Playing)
            return;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = _canMove ? (isRunning ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = _canMove ? (isRunning ? _runningSpeed : _walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = _moveDirection.y;
        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
        _moveDirection.y = movementDirectionY;

        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
        if (_canMove)
        {
            _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
            _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
        }
    }
}