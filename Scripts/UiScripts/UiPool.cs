/*
 * UiPool.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 다중오브젝트 풀링을 이용하여, UI를 관리한다. Dictionary로 UI이름을 통해 빠르게 접근할 수 있다. 모듈화(프리팹화)된 UI들을 게임 실행시 UiPool에 모두 로드한다
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
