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

        //영지관리 destroy
        //Destroy(territoryManagement.gameObject);
        UiPool.ReturnObject(territoryManagement.gameObject);
        //건물생성지역 destroy
       // Destroy(buildingGround.gameObject);
        //본인 destory
        UiPool.ReturnObject(gameObject);


    }
}
