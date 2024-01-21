using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{

    public void OnTerritoryManagementButton()
    {
        var territoryManagement = UiPool.GetObject("TerritoryManagement");
        territoryManagement.GetComponent<TerritoryManagement>().initialize();
        UiPool.ReturnObject(gameObject);

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
