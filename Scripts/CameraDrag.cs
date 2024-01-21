using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    private Vector3 dragOrigin; // �巡�� ���� ����
    private bool isDragging = false; // �巡�� ������ ����

    public float dragSpeed = 1.0f; // �巡�� �ӵ�

    Vector3 difference;

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // ���콺 ��ư�� ������ �� �巡�� ����
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            isDragging = true;
        }

        // ���콺 ��ư�� ������ �� �巡�� ����
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // �巡�� ���� �� ī�޶� �̵�
        if (isDragging)
        {
            difference = dragOrigin - Input.mousePosition;
            dragOrigin = Input.mousePosition;

            // �巡�� ���⿡ ���� ī�޶� �̵�
            transform.Translate(new Vector3(difference.x, difference.y, 0) * dragSpeed * Time.deltaTime);
        }
    }
}


