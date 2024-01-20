using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManagement : MonoBehaviour
{
    
    [SerializeField] TerritoryManagementExitButton territoryManagementExitButton; //�ڷΰ��� ��ư�� �����Ѵ�.


    [SerializeField] BuildingGround buildingGround; //�̱������� ���� �Ǽ� ���� ����
    private void Start()
    {

        var tmp = Instantiate(territoryManagementExitButton,GameObject.Find("Canvas").transform);
        tmp.territoryManagement = this;

        //���� �ݺ������� �� 4������ �����ϵ��� �غ���
        //buildingGround �ν��Ͻ�ȭ �� TerritoryExitButton�� �ѱ��
        var tmp1 = Instantiate(buildingGround);
        tmp.buildingGround = tmp1;
    }
}
