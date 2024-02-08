using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KnightsUi : MonoBehaviour
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //�̰� �ڵ�ȭ�ؾ��ҵ�.. �ϴ� mvc�� �Ѵ�
    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    public void initialize()
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
        ShowCharacterSlot();
        
    }

    /*�̰� slot���� mvp�������� �ƴϸ� scriptable Object�� �ؼ� ������ ���������� �ڵ� ����
     * allCharacterList�� ���� �°� slot���� �׸���, myCharacterList�� ���� �ڹ��� ���
     * */
    private void ShowCharacterSlot() 
    {
        //�ϴ� MyCharacterList��ŭ slot�����ϴ°ɷ�����
        for(int i = 0; i < DataManager.MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = DataManager.MyCharacterList[i].Name;
        }

    }
    public void OnExitButtonClick()
    {

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainUi>().initialize();

        //���� destory
        UiPool.ReturnObject(gameObject);
    }


}
