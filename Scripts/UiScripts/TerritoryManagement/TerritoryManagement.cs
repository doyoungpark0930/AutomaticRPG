using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{

    [SerializeField] BuildingGround buildingGround; //�̱������� ���� �Ǽ� ���� ����

    public void initialize()
    {
        var territoryManagementExitButton = UiPool.GetObject("TerritoryManagementExitButton");
        territoryManagementExitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(999, 455);
        territoryManagementExitButton.GetComponent<TerritoryManagementExitButton>().territoryManagement = this;

        //���� �ݺ������� �� 4������ �����ϵ��� �غ���
        //buildingGround �ν��Ͻ�ȭ �� TerritoryExitButton�� �ѱ��
        //var tmp1 = Instantiate(buildingGround);
        //tmp.buildingGround = tmp1;
    }
}
