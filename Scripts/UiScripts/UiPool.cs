using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            var obj = Instantiate(UiObject[i]);
            obj.SetActive(false);
            obj.transform.SetParent(transform);

            poolingObject.Add(UiObject[i].name, obj); //key는 오브젝트이름, value는 오브젝트
        }
        
    }


    public static GameObject GetObject(string UiName)
    {
        if(instance.poolingObject.ContainsKey(UiName))
        {
            var obj = instance.poolingObject[UiName];
            instance.poolingObject.Remove(UiName);
            obj.transform.SetParent(GameObject.Find("Canvas").transform);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            Debug.LogError("없는 Ui입니다. 인스턴스화 할 오브젝트의 이름을 다시 확인해주세요.");
            return null;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        if(!instance.poolingObject.ContainsKey(obj.name.Substring(0, obj.name.Length - 7))) //Substring은 name 뒤에 붙은 (Clone)제거 
        {
            instance.poolingObject.Add(obj.name.Substring(0, obj.name.Length - 7), obj);
        }
        else
        {
            Debug.LogError("이미 존재하는 key값 입니다.");
        }
    }
}
