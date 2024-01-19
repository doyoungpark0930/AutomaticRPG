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

        //�������� destroy
        Destroy(territoryManagement.gameObject);
        //�ǹ��������� destroy
        Destroy(buildingGround.gameObject);
        //���� destory
        Destroy(gameObject);


    }
}
