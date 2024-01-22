using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    private Vector3 dragOrigin; // �巡�� ���� ����
    private bool isDragging = false; // �巡�� ������ ����

    private float dragSpeed = 1.0f; // �巡�� �ӵ�

    public Vector3 minPosition; // ���Ǵ� �ּ� ��ġ
    public Vector3 maxPosition; // ���Ǵ� �ִ� ��ġ

    private Vector3 newPosition;

    private Vector3 difference;

    void Update()
    {
        Drag();
    }

    void Drag()
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

            newPosition = transform.position;

            // ��ġ�� �����Ͽ� �ּ� �� �ִ� �� ���� �ӹ������� ��
            newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
            newPosition.z = Mathf.Clamp(newPosition.z, minPosition.z, maxPosition.z);

            transform.position = newPosition;
        }
    }
}


