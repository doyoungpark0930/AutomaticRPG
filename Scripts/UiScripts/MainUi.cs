using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{
    [SerializeField] GameObject TerritoryManagement; 
    public void OnTerritoryManagementButton()
    {
        Instantiate(TerritoryManagement); //영지건설 오브젝트 인스턴스화
        Destroy(gameObject);    //이거 풀링 해줘야할듯

    }

    public void OnKnightsButton()
    {
        Debug.Log("click OnKnightsButton");
    }

    public void OnAdvantureButton()
    {
        Debug.Log("Click OnAdvantureButton");
    }
}
