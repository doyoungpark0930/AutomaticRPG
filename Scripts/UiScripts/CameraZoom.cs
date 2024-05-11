/*
 * CameraZoom.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 메인 뷰와 영지관리 뷰 이동시 카메라를 ZoomIn,ZoomOut하는 기능을 담당한다
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public void GetZoomOut()
    {
        StartCoroutine(ZoomOut());
    }

    public void GetZoomIn()
    {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomOut() 
    {
        Vector3 startPos= gameObject.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 10, 0);

        while(gameObject.transform.position.y < targetPos.y)
        {
            gameObject.transform.Translate(Vector3.up * 40 * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.transform.position = targetPos;
    }
    IEnumerator ZoomIn()
    {
        Vector3 startPos = gameObject.transform.position;
        Vector3 targetPos = startPos - new Vector3(0, 10, 0);

        while (gameObject.transform.position.y > targetPos.y)
        {
            gameObject.transform.Translate(Vector3.down * 40 * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.transform.position = targetPos;
    }
}
