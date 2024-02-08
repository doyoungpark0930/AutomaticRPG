using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnightsUi : MonoBehaviour
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //이거 자동화해야할듯.. 일단 mvc로 한다
    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    public void initialize()
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        ShowCharacterSlot();
        
    }

    /*이거 slot개수 mvp형식으로 아니면 scriptable Object로 해서 데이터 개수에따라 자동 생성
     * allCharacterList의 수에 맞게 slot생성 그리고, myCharacterList에 따라 자물쇠 잠금
     * */
    private void ShowCharacterSlot() 
    {
        //일단 MyCharacterList만큼 slot생성하는걸로하자
        for(int i = 0; i < DataManager.MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = DataManager.MyCharacterList[i].Name;
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
