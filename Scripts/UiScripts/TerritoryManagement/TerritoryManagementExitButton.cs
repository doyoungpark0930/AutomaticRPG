using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagementExitButton: MonoBehaviour
{

    public TerritoryManagement territoryManagement { get; set; }
    public BuildingGround buildingGround { get; set; }

 

    public void OnClick()
    {


        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        //�������� destroy
        //Destroy(territoryManagement.gameObject);
        UiPool.ReturnObject(territoryManagement.gameObject);
        //�ǹ��������� destroy
       // Destroy(buildingGround.gameObject);
        //���� destory
        UiPool.ReturnObject(gameObject);


    }
}
