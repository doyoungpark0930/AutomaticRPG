using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUi : MonoBehaviour
{
    [SerializeField] GameObject TerritoryManagement; 
    public void OnTerritoryManagementButton()
    {
        Instantiate(TerritoryManagement); //�����Ǽ� ������Ʈ �ν��Ͻ�ȭ
        Destroy(gameObject);    //�̰� Ǯ�� ������ҵ�

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
