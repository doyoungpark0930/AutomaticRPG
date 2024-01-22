using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    private Vector3 dragOrigin; // 드래그 시작 지점
    private bool isDragging = false; // 드래그 중인지 여부

    private float dragSpeed = 1.0f; // 드래그 속도

    public Vector3 minPosition; // 허용되는 최소 위치
    public Vector3 maxPosition; // 허용되는 최대 위치

    private Vector3 newPosition;

    private Vector3 difference;

    void Update()
    {
        Drag();
    }

    void Drag()
    {
        // 마우스 버튼이 눌렸을 때 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        // 마우스 버튼이 떼졌을 때 드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 드래그 중일 때 카메라 이동
        if (isDragging)
        {
            difference = dragOrigin - Input.mousePosition;
            dragOrigin = Input.mousePosition;

            // 드래그 방향에 따라 카메라 이동
            transform.Translate(new Vector3(difference.x, difference.y, 0) * dragSpeed * Time.deltaTime);

            newPosition = transform.position;

            // 위치를 제한하여 최소 및 최대 값 내에 머무르도록 함
            newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minPosition.z, maxPosition.z);

            transform.position = newPosition;
        }
    }
}


