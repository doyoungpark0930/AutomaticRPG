using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{

    [SerializeField] BuildingGround buildingGround; //싱글톤으로 영지 건설 지역 생성

    public void initialize()
    {
        var territoryManagementExitButton = UiPool.GetObject("TerritoryManagementExitButton");
        territoryManagementExitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(999, 455);
        territoryManagementExitButton.GetComponent<TerritoryManagementExitButton>().territoryManagement = this;

        //영지 반복문으로 한 4개정도 생성하도록 해보자
        //buildingGround 인스턴스화 및 TerritoryExitButton에 넘기기
        //var tmp1 = Instantiate(buildingGround);
        //tmp.buildingGround = tmp1;
    }
}
