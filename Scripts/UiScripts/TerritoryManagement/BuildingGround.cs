using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGround : MonoBehaviour
{
    //��ġ�ϸ� debug.log�ߵ���
    bool isClicking;
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
                    isClicking = true;
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
                    isClicking = false;
                    if(Time.time -clickStartTime <=1.0f)
                    {
                        Debug.Log("Ŭ�� �Ϸ�");
                    }

                }
            }
        }

    }

}
