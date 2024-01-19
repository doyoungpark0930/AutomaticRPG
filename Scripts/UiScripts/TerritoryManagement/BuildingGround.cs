using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGround : MonoBehaviour
{
    //터치하면 debug.log뜨도록
    bool isClicking;
    float clickStartTime;

    void Update()
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
                    // 오브젝트를 클릭했을 때 상태를 true로 설정하고 시작 시간을 저장
                    isClicking = true;
                    clickStartTime = Time.time;
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            // Raycast를 이용하여 오브젝트를 클릭했는지 확인
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    // 오브젝트를 클릭했을 때 상태를 true로 설정하고 시작 시간을 저장
                    isClicking = false;
                    if(Time.time -clickStartTime <=1.0f)
                    {
                        Debug.Log("클릭 완료");
                    }

                }
            }
        }

    }

}
