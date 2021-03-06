﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public float sensX;
    public float sensY;

    private GameObject player;
    private Vector2 smoothedVel;
    private Vector2 currentLook;
    private float curTilt = 0;
    private float wishTilt = 0;
    private float delta;
    private float wishFov = 90;
    private float addedFov = 90;
    private float fov = 90;
    public Camera cam;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        curTilt = transform.localEulerAngles.z;
        player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        addedFov = Mathf.Clamp(rb.velocity.magnitude - 3.44f, 0f, 180f - fov);
        fov = Mathf.Lerp(fov, wishFov + addedFov, Time.deltaTime);
        cam.fieldOfView = fov;
        curTilt = transform.localEulerAngles.z;
        if(curTilt < 180)
        {
            delta = Mathf.Abs(wishTilt - curTilt);
        }
        else
        {
            delta = Mathf.Abs(360 - wishTilt - curTilt);
        }
        delta /= 2;
        curTilt = Mathf.LerpAngle(curTilt, wishTilt, Time.deltaTime * delta);
        RotateCamera();
    }

    void RotateCamera()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseInput.x *= sensX;
        mouseInput.y *= sensY;

        currentLook.x += mouseInput.x;
        currentLook.y = Mathf.Clamp(currentLook.y += mouseInput.y, -90, 90);

        transform.localRotation = Quaternion.AngleAxis(-currentLook.y, Vector3.right);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, curTilt);
        player.transform.localRotation = Quaternion.AngleAxis(currentLook.x, player.transform.up);
    }



    #region Setters

    public void SetTilt(float newVal)
    {
        wishTilt = newVal;
    }

    public void SetXSens(float newVal)
    {
        sensX = newVal;
    }

    public void SetYSens(float newVal)
    {
        sensY = newVal;
    }

    public void SetFov(float newVal)
    {
        wishFov = newVal;
    }
    #endregion
}
