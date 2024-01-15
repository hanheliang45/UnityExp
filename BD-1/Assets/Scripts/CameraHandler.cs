using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vc;

    private float targetOrthographicSize;
    void Start()
    {
        targetOrthographicSize = vc.m_Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Zoom();
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, y, 0).normalized;
        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    void Zoom()
    {
        float scrollValue = Input.mouseScrollDelta.y;
        if ((vc.m_Lens.OrthographicSize > 40 && scrollValue < 0)
            || (vc.m_Lens.OrthographicSize < 10 && scrollValue > 0)
            || (vc.m_Lens.OrthographicSize <= 40 && vc.m_Lens.OrthographicSize >= 10))
        {
            targetOrthographicSize = vc.m_Lens.OrthographicSize + scrollValue;
        }
        float zoomSpeed = 40f;
        vc.m_Lens.OrthographicSize = Mathf.Lerp(
            vc.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }


}
