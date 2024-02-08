using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGround : MonoBehaviour
{

    private Vector3 origin; //��ġ �� ��ġ ��ġ

    void Update()
    {
        onClick();

    }

    void onClick() //���콺Ŭ���� Ŭ�� ���� �� ��ġ ������ Ȯ���ϴ� �޼���
    {
        // ���콺 Ŭ�� �Է� ����
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast�� �̿��Ͽ� ������Ʈ�� Ŭ���ߴ��� Ȯ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    origin = Input.mousePosition;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Raycast�� �̿��Ͽ� ������Ʈ�� Ŭ���ߴ��� Ȯ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {

                    if (origin == Input.mousePosition) //��ġ���� ���� �� ��ġ�� ���ٸ�
                    {
                        Debug.Log("Ŭ���Ϸ�");
                    }
                }
            }
        }

    }

}
