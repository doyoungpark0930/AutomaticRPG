using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class KnightsUi : MonoBehaviour
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //�̰� �ڵ�ȭ�ؾ��ҵ�.. �ϴ� mvc�� �Ѵ�
    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    void Start()
    {
        //���⼭ slot�� �����Ϳ����� �� �޾ƹ�����. �׸��� slot��ȭ ������ ���� �޼��� ���� �� �κи� ��ȭ
    }
    public void initialize()
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
        ShowCharacterSlot();
        
    }

    /*�̰� slot���� mvp�������� �ƴϸ� scriptable Object�� �ؼ� ������ ���������� �ڵ� ����
     * allCharacterList�� ���� �°� slot���� �׸���, myCharacterList�� ���� �ڹ��� ��� => �̰� �ϴ� allCharaceList�� MyCharacterList��
     * ���и� ���ָ� �Ǵϱ� ���߿� ����
     * */
    private void ShowCharacterSlot() 
    {
        //�ϴ� MyCharacterList��ŭ slot�����ϴ°ɷ�����
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

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainUi>().initialize();

        //���� destory
        UiPool.ReturnObject(gameObject);
    }


}
