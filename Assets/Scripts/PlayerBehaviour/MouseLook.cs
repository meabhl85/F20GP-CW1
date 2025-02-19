﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSenitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    public bool mouseEnabled = true;

    void Start()
    {
        //Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (mouseEnabled)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSenitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSenitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
