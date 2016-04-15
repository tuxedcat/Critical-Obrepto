﻿using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
    public float moveSpeed = 4;
    CharacterController _characterController;
    Vector3 _velocity;
    bool jumping = false;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        InputHandler.instance.onMove = v =>
        {
            v = Quaternion.Euler(0, 0, -transform.eulerAngles.y) * v;
            _velocity.x += v.x / 2;
            _velocity.z += v.y / 2;

            Vector3 xz = _velocity;
            xz.y = 0;
            xz = xz.magnitude > moveSpeed ? xz.normalized * moveSpeed : xz;
            _velocity.x = xz.x;
            _velocity.z = xz.z;
        };
        InputHandler.instance.onStop = () =>
        {
            _velocity.x /= 1.5f;
            _velocity.z /= 1.5f;
        };
        InputHandler.instance.onJump = () =>
        {
            if (jumping == false && _characterController.isGrounded)
            {
                _velocity.y = 6;
                jumping = true;
            }
        };
        InputHandler.instance.onAimMove = v =>
        {
            if (90 < transform.localEulerAngles.x - v.y && transform.localEulerAngles.x - v.y < 270)
                v.y = 0;
            transform.localEulerAngles += new Vector3(-v.y, v.x, 0);
        };
    }

    void Update()
    {
        _velocity += Physics.gravity * Time.deltaTime;
        if (_characterController.isGrounded)
        {
            if (jumping)
                jumping = false;
            else
                _velocity.y = 0;
        }

        _characterController.Move(_velocity * Time.deltaTime);

        if (jumping && _characterController.isGrounded)
            jumping = false;
    }
}