using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{
    CameraDrag cameraDrag;

    private void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>(); 
    }
    public void OnTerritoryManagementButton()
    {
        var territoryManagement = UiPool.GetObject("TerritoryManagement");
        territoryManagement.transform.position = Vector2.zero;
        territoryManagement.GetComponent<TerritoryManagement>().initialize();
        UiPool.ReturnObject(gameObject);

    }

    public void OnKnightsButton()
    {
        Debug.Log("click OnKnightsButton");
        cameraDrag.enabled = !cameraDrag.enabled;
    }

    public void OnAdvantureButton()
    {
        Debug.Log("Click OnAdvantureButton");
    }
}
