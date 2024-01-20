using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiPool : MonoBehaviour
{
    public static UiPool instance = null;
    [SerializeField] GameObject[] UiObject;
    private Dictionary<string, GameObject> poolingObject = new Dictionary<string, GameObject>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        Initialize();
    }

    private void Initialize()
    {
        poolingObject.Add("MainUi", UiObject[0]);
        poolingObject.Add("TerritoryManagement", UiObject[1]);
        poolingObject.Add("TerritoryManagementExitButton", UiObject[2]);
        poolingObject.Add("BuildingGround", UiObject[3]);
    }

    void Start()
    {
        if(poolingObject.ContainsKey("MainUi"))
        {
            var obj = Instantiate(poolingObject["MainUi"], GameObject.Find("Canvas").transform);
            obj.SetActive(false);
            obj.transform.SetParent(transform);
        }
    }

    //�ϴ� getmain�޼��常����. �׸����߿� �̸��ٲ㼭 getobject���ϰ� ����ġ�Լ�����
    public static GameObject GetGameObject(string UiName)
    {
        if(instance.poolingObject.ContainsKey(UiName))
        {
            return null;
        }
        else
        {
            Debug.Log("���� Ui�Դϴ�");
            return null;
        }
    }
}
