/*
 * TerritoryManagement.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���� ���� UI�̴�. UIPool�� ���� �����ȴ�.
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
    private GameObject buildingGround; //�̱������� ���� �Ǽ� ���� ����
    private CameraZoom cameraZoom; //�������� ������ ���� �� ZoomIn,ZoomOut�Ǳ� ���ؼ�

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

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainView");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //�ǹ��������� destroy
        UiPool.ReturnObject(buildingGround);
        //���� destory
        UiPool.ReturnObject(gameObject);
    }
}



