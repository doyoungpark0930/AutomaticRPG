/*
 * MainView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 드래그로 화면을 이동하는 기능을 담당한다
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    private Vector3 dragOrigin; // 드래그 시작 지점
    private bool isDragging = false; // 드래그 중인지 여부

    private float dragSpeed = 0.8f; // 드래그 속도

    public Vector3 minPosition; // 허용되는 최소 위치
    public Vector3 maxPosition; // 허용되는 최대 위치

    private Vector3 newPosition;

    private Vector3 difference;

    void LateUpdate()
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


