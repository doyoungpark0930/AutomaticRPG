using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{ 
    private GameObject buildingGround; //�̱������� ���� �Ǽ� ���� ����
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

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //�ǹ��������� destroy
        UiPool.ReturnObject(buildingGround);
        //���� destory
        UiPool.ReturnObject(gameObject);
    }
}



