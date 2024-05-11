/*
 * TerritoryManagement.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 영지 관리 UI이다. UIPool에 의해 관리된다.
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{ 
    private GameObject buildingGround; //싱글톤으로 영지 건설 지역 생성
    private CameraZoom cameraZoom; //영지관리 들어오고 나갈 때 ZoomIn,ZoomOut되기 위해서

    private void Awake()
    {
        cameraZoom = Camera.main.GetComponent<CameraZoom>();
    }

    
    public void initialize()
    {
        buildingGround = UiPool.GetObject("BuildingGround");
        buildingGround.transform.SetParent(null);

        cameraZoom.GetZoomOut();
    }
 



    public void ToMainView()
    {
        cameraZoom.GetZoomIn();

        //MainUi 다시 띄우기
        var mainUi = UiPool.GetObject("MainView");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //건물생성지역 destroy
        UiPool.ReturnObject(buildingGround);
        //본인 destory
        UiPool.ReturnObject(gameObject);
    }
}



