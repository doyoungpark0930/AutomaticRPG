using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{
    
    [SerializeField] TerritoryManagementExitButton territoryManagementExitButton; //뒤로가기 버튼을 생성한다.


    [SerializeField] BuildingGround buildingGround; //싱글톤으로 영지 건설 지역 생성
    private void Start()
    {

        var tmp = Instantiate(territoryManagementExitButton,GameObject.Find("Canvas").transform);
        tmp.territoryManagement = this;

        //영지 반복문으로 한 4개정도 생성하도록 해보자
        //buildingGround 인스턴스화 및 TerritoryExitButton에 넘기기
        var tmp1 = Instantiate(buildingGround);
        tmp.buildingGround = tmp1;
    }
}
