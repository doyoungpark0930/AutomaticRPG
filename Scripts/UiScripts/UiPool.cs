/*
 * UiPool.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���߿�����Ʈ Ǯ���� �̿��Ͽ�, UI�� �����Ѵ�. Dictionary�� UI�̸��� ���� ������ ������ �� �ִ�. ���ȭ(������ȭ)�� UI���� ���� ����� UiPool�� ��� �ε��Ѵ�
 * 
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
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

            poolingObject.Add(UiObject[i].name, obj); //key�� ������Ʈ�̸�, value�� ������Ʈ
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
            Debug.LogError("���� Ui�Դϴ�. �ν��Ͻ�ȭ �� ������Ʈ�� �̸��� �ٽ� Ȯ�����ּ���.");
            return null;
        }
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        if(!instance.poolingObject.ContainsKey(obj.name.Substring(0, obj.name.Length - 7))) //Substring�� name �ڿ� ���� (Clone)���� 
        {
            instance.poolingObject.Add(obj.name.Substring(0, obj.name.Length - 7), obj);
        }
        else
        {
            Debug.LogError("�̹� �����ϴ� key�� �Դϴ�.");
        }
    }
}
