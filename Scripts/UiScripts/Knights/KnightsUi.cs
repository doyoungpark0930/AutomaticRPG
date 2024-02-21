using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class KnightsUi : MonoBehaviour
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //이거 자동화해야할듯.. 일단 mvc로 한다
    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    void Start()
    {
        //여기서 slot을 데이터에따라 다 받아버리기. 그리고 slot변화 있을때 따로 메서드 만들어서 그 부분만 변화
    }
    public void initialize()
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        ShowCharacterSlot();
        
    }

    /*이거 slot개수 mvp형식으로 아니면 scriptable Object로 해서 데이터 개수에따라 자동 생성
     * allCharacterList의 수에 맞게 slot생성 그리고, myCharacterList에 따라 자물쇠 잠금 => 이건 일단 allCharaceList와 MyCharacterList를
     * 구분만 해주면 되니까 나중에 생각
     * */
    private void ShowCharacterSlot() 
    {
        //일단 MyCharacterList만큼 slot생성하는걸로하자
        for(int i = 0; i < DataManager.instance.MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Level.ToString();
           Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = DataManager.instance.JobSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].Job.ToString());


        }

    }
    public void OnExitButtonClick()
    {

        //MainUi 다시 띄우기
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainUi>().initialize();

        //본인 destory
        UiPool.ReturnObject(gameObject);
    }


}
