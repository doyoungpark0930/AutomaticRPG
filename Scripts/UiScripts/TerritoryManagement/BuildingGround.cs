using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGround : MonoBehaviour
{

    private Vector3 origin; //터치 시 터치 위치

    void Update()
    {
        onClick();

    }

    void onClick() //마우스클릭과 클릭 땠을 때 위치 같은지 확인하는 메서드
    {
        // 마우스 클릭 입력 감지
        if (Input.GetMouseButtonDown(0))
        {
            // Raycast를 이용하여 오브젝트를 클릭했는지 확인
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
            // Raycast를 이용하여 오브젝트를 클릭했는지 확인
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {

                    if (origin == Input.mousePosition) //터치에서 땠을 떄 위치가 같다면
                    {
                        Debug.Log("클릭완료");
                    }
                }
            }
        }

    }

}
