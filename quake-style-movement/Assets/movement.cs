using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Rendering;

public class movement : MonoBehaviour
{
    [Range(1, 100)]
    public float sideSpeed = 1;
    [Range(1, 100)]
    public float fdSpeed = 1;
    [Range(0, 100)]
    public float wishSpeed = 1;
    [Range(0, 100)]
    public float accel = 1;
    [Range(0, 100)]
    public float decel = 1;
    [Range(0, 100)]
    public float frict = 1;
    [Range(0, 100)]
    public float jumpForce = 1;
    [Range(0, 100)]
    public float gravity = 1;

    private CharacterController controller;
    private Camera cam;
    private Vector3 wishdir;
    private Vector3 vel;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        vel.y -= gravity * Time.deltaTime;
        if(controller.isGrounded && Input.GetAxis("Jump") > 0) 
        {
            vel.y = jumpForce;
        }
        if(vel.magnitude > 0 && controller.isGrounded)
        {
            applyFriction();
        }
        controller.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
        controller.transform.GetChild(0).transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, 0, 0));
        wishdir = controller.transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal") * sideSpeed, 0, Input.GetAxis("Vertical") * fdSpeed));
        if(wishdir.magnitude > 0)
        {
            wishdir.Normalize();
            accelerate(wishdir, wishSpeed, accel);
        }
        controller.Move(vel * Time.deltaTime);
    }
 
    private void accelerate(Vector3 wishdir, float wishSpeed, float accel)
    {
        float currentSpeed;
        float accelSpeed;
        float addSpeed;

        currentSpeed  = Vector3.Dot(vel, wishdir);
        addSpeed = wishSpeed - currentSpeed;
        accelSpeed = Mathf.Clamp(addSpeed, 0, wishSpeed * accel * Time.deltaTime);
        vel += accelSpeed * wishdir;
    }

    private void applyFriction()
    {
        float speed;
        float newSpeed;
        float control;
        float drop;

        speed = vel.magnitude;
        control = speed < decel ? decel : speed;
        drop = control * frict * Time.deltaTime;

        newSpeed = Mathf.Max(speed - drop, 0);
        newSpeed /= speed;
        vel *= newSpeed;
    }
}
