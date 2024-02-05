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
    private float verticalMomentum = 0;
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

        Jump();

        CalculateVelocity();
        transform.Translate(velocity * Time.deltaTime * moveSpeed, Space.World);

        transform.Rotate(Vector3.up * mouseHorizontal * 3);
        cam.Rotate(Vector3.right * -mouseVertical * 3);

    }

    private void FixedUpdate()
    {
        
    }

    void Jump()
    {
        if (!jumpRequest)
        {
            return;
        }
        jumpRequest = false;

        isGround = false;
        verticalMomentum = jumpForce;
    }

    void CalculateVelocity()
    {
        velocity = transform.forward * vertical + transform.right * horizontal;
        velocity += Vector3.down * gravity * Time.deltaTime * 5;
        float original_y = velocity.y;
        velocity.y = checkDownSpeed(velocity.y);


        if (verticalMomentum > -gravity)
        {
            verticalMomentum -= Time.fixedDeltaTime * gravity;
        }

        velocity = (transform.forward * vertical + transform.right * horizontal)
                * Time.fixedDeltaTime 
                * (isSprinting ? sprintSpeed : moveSpeed);

        velocity += Vector3.up * verticalMomentum * Time.fixedDeltaTime;

        if (velocity.z > 0 && checkFront(true))
        {
            velocity.z = 0;
        }
        if (velocity.z < 0 && checkFront(false))
        {
            velocity.z = 0;
        }
        if (velocity.x > 0 && checkRight(true))
        {
            velocity.x = 0;
        }
        if (velocity.x < 0 && checkRight(false))
        {
            velocity.x = 0;
        }
        if (velocity.y > 0)
        {
            velocity.y = checkUpSpeed(velocity.y);
        }
        if (velocity.y < 0)
        {
            velocity.y = checkDownSpeed(velocity.y);
        }

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
        if (world.CheckForVoxel(transform.position.x - playerWidth,
            transform.position.y + downSpeed, transform.position.z - playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
            transform.position.y + downSpeed, transform.position.z - playerWidth) ||
            world.CheckForVoxel(transform.position.x - playerWidth,
            transform.position.y + downSpeed, transform.position.z + playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
            transform.position.y + downSpeed, transform.position.z + playerWidth))
        {
            isGround = true;
            return 0;
        }
       
        isGround = false;
        return downSpeed;
    }

    float checkUpSpeed(float upSpeed)
    {
        if (world.CheckForVoxel(transform.position.x - playerWidth,
                transform.position.y + 2f + upSpeed, transform.position.z - playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
                transform.position.y + 2f + upSpeed, transform.position.z - playerWidth) ||
            world.CheckForVoxel(transform.position.x - playerWidth,
                transform.position.y + 2f + upSpeed, transform.position.z + playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
                transform.position.y + 2f + upSpeed, transform.position.z + playerWidth))
        {
            return 0;
        }

        return upSpeed;
    }

    bool checkFront(bool isFront)
    {
        int frontOrBack = isFront ? 1 : -1;
        if (
            world.CheckForVoxel(transform.position.x - playerWidth,
                transform.position.y , transform.position.z + playerWidth * frontOrBack) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
                transform.position.y, transform.position.z + playerWidth * frontOrBack) ||
            world.CheckForVoxel(transform.position.x - playerWidth,
                transform.position.y + 1f, transform.position.z + playerWidth * frontOrBack) ||
            world.CheckForVoxel(transform.position.x + playerWidth,
                transform.position.y + 1f, transform.position.z + playerWidth * frontOrBack)
            )
        {
            return true;
        }
        return false;
    }

    bool checkRight(bool isRight)
    {
        int rightOrLeft = isRight ? 1 : -1;
        if (
            world.CheckForVoxel(transform.position.x + playerWidth * rightOrLeft,
                transform.position.y, transform.position.z + playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth * rightOrLeft,
                transform.position.y, transform.position.z - playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth * rightOrLeft,
                transform.position.y + 1f, transform.position.z + playerWidth) ||
            world.CheckForVoxel(transform.position.x + playerWidth * rightOrLeft,
                transform.position.y + 1f, transform.position.z - playerWidth)
            )
        {
            return true;
        }
        return false;
    }
}
