using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagementExitButton: MonoBehaviour
{
    [SerializeField] GameObject MainUi;
    public TerritoryManagement territoryManagement { get; set; }
    public BuildingGround buildingGround { get; set; }

 

    public void OnClick()
    {
   
        
        Instantiate(MainUi, GameObject.Find("Canvas").transform);

        //영지관리 destroy
        Destroy(territoryManagement.gameObject);
        //건물생성지역 destroy
        Destroy(buildingGround.gameObject);
        //본인 destory
        Destroy(gameObject);


    }
}
