using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGround : MonoBehaviour
{
    float clickStartTime;

    void Update()
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
                    // ������Ʈ�� Ŭ������ �� ���¸� true�� �����ϰ� ���� �ð��� ����
                    clickStartTime = Time.time;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            // Raycast�� �̿��Ͽ� ������Ʈ�� Ŭ���ߴ��� Ȯ��
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // ������Ʈ�� Ŭ������ �� ���¸� true�� �����ϰ� ���� �ð��� ����
                    if(Time.time -clickStartTime <=1.0f)
                    {
                        Debug.Log("Ŭ�� �Ϸ�");
                    }

                }
            }
        }

    }

}
