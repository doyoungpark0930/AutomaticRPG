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
        for(int i = 0; i < UiObject.Length; i++)
        {
            //인스턴스화된 객체를 넣자
            var obj = Instantiate(UiObject[i]);
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            poolingObject.Add(UiObject[i].name, obj); //key는 오브젝트이름, value는 오브젝트
            Debug.Log(obj.name);
        }

    }


    public static GameObject GetGameObject(string UiName)
    {
        if(instance.poolingObject.ContainsKey(UiName))
        {
            var obj = instance.poolingObject[UiName];
            obj.transform.SetParent(GameObject.Find("Canvas").transform);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.Log("없는 Ui입니다");
            return null;
        }
    }
}
