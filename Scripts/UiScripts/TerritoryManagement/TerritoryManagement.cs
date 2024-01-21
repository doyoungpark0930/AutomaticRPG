using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{ 
    private GameObject buildingGround; //싱글톤으로 영지 건설 지역 생성
    private CameraZoom cameraZoom;

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

    

    public void OnExitButtonClick()
    {
        cameraZoom.GetZoomIn();

        //MainUi 다시 띄우기
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //건물생성지역 destroy
        UiPool.ReturnObject(buildingGround);
        //본인 destory
        UiPool.ReturnObject(gameObject);
    }
}



