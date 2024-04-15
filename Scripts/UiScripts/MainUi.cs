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
    public void initialize() //OnEnable은 오브젝트 풀에서 첫 생성시 발동 되므로 위험하니 initilize()를 사용한다
    {
        cameraDrag.enabled = true; //카메라 Drag On
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
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();
        UiPool.ReturnObject(gameObject);
    }

    public void OnAdvantureButton()
    {
        Debug.Log("Click OnAdvantureButton");
    }
}
