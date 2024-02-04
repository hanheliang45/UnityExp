using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] World world;

    private float playerWidth = 0.3f;
    private float playerHeight = 1.9f;

    private Transform cam;

    private float horizontal;
    private float vertical;
    private float mouseHorizontal;
    private float mouseVertical;
    private Vector3 velocity;
    private float veticalMomentum = 0;
    private bool jumpRequest;

    private bool isGround = true;
    private bool isSprinting = true;

    void Start()
    {
        cam = transform.Find("Main Camera");
    }

    void Update()
    {
        GetPlayerInputs();

        velocity = transform.forward * vertical + transform.right * horizontal;
        velocity += Vector3.down * gravity * Time.deltaTime * 5;
        float original_y = velocity.y;
        velocity.y = checkDownSpeed(velocity.y);
        Debug.Log("original_y:" + original_y + " new " + velocity.y);

        transform.Rotate(Vector3.up * mouseHorizontal * 3);
        cam.Rotate(Vector3.right * -mouseVertical * 3);

        transform.Translate(velocity * Time.deltaTime * moveSpeed, Space.World);
    }

    void GetPlayerInputs()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        mouseHorizontal = Input.GetAxis("Mouse X");
        mouseVertical = Input.GetAxis("Mouse Y");

        if (Input.GetButtonDown("Fire3"))
        { 
            isSprinting = true;
        }
        if (Input.GetButtonUp("Fire3"))
        {
            isSprinting = false;
        }

        if (isGround && Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }

    }

    float checkDownSpeed(float downSpeed)
    {
        if (world.CheckForVoxel(transform.position.x, 
            transform.position.y + downSpeed, transform.position.z))
        {
            isGround = true;
            return 0;
        }
        if (world.CheckForVoxel(transform.position.x - playerWidth,
            transform.position.y + downSpeed, transform.position.z - playerWidth))
        {
            isGround = true;
            return 0;
        }
        if (world.CheckForVoxel(transform.position.x + playerWidth,
            transform.position.y + downSpeed, transform.position.z - playerWidth))
        {
            isGround = true;
            return 0;
        }
        if (world.CheckForVoxel(transform.position.x - playerWidth,
            transform.position.y + downSpeed, transform.position.z + playerWidth))
        {
            isGround = true;
            return 0;
        }
        if (world.CheckForVoxel(transform.position.x + playerWidth,
            transform.position.y + downSpeed, transform.position.z + playerWidth))
        {
            isGround = true;
            return 0;
        }
        isGround = false;
        return downSpeed;
    }
}
