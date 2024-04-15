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
    public void initialize() //OnEnable�� ������Ʈ Ǯ���� ù ������ �ߵ� �ǹǷ� �����ϴ� initilize()�� ����Ѵ�
    {
        cameraDrag.enabled = true; //ī�޶� Drag On
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
