using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{ 
    private GameObject buildingGround; //�̱������� ���� �Ǽ� ���� ����
    private CameraMove cameraMove;

    private void Awake()
    {
        cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
    }

    public void initialize()
    {
        buildingGround = UiPool.GetObject("BuildingGround");
        buildingGround.transform.SetParent(null);

        cameraMove.GetZoomOut();
    }

    

    public void OnExitButtonClick()
    {
        cameraMove.GetZoomIn();

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //�ǹ��������� destroy
        UiPool.ReturnObject(buildingGround);
        //���� destory
        UiPool.ReturnObject(gameObject);
    }
}



