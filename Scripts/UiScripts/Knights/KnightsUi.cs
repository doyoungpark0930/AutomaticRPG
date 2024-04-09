using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class KnightsUi : MonoBehaviour, ICharacterListObserver
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //이거 자동화해야할듯.. 일단 mvc로 한다

    public void OnCharacterListUpdated()
    {
        // MyCharacterList의 변경 사항을 기반으로 UI 업데이트
        ShowCharacterSlot();
    }

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    void Start()
    {
        //KnightsUI스크립트를 옵저버에 등록한다
        DataManager.instance.RegisterObserver(this);
        //Slot이미지를 첫 실행시에 다 받는다. 
        ShowCharacterSlot();
    }
    public void initialize() //onEnable대체
    {
        cameraDrag.enabled = false; //카메라 Drag Off
    }
   

    /*이거 slot개수 mvp형식으로 아니면 scriptable Object로 해서 데이터 개수에따라 자동 생성
     * allCharacterList의 수에 맞게 slot생성 그리고, myCharacterList에 따라 자물쇠 잠금 => 이건 일단 allCharaceList와 MyCharacterList를
     * 구분만 해주면 되니까 나중에 생각
     * */
    private void ShowCharacterSlot() //Slot에 이미지들을 넣는다
    {
        //일단 MyCharacterList만큼 slot생성하는걸로하자
        for(int i = 0; i < DataManager.instance.MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite =
                DataManager.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].Element.ToString());
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = 
                DataManager.instance.JobSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].Job.ToString());

            if(DataManager.instance.MyCharacterList[i].EquippedWeapon.Name != "") //MyCharacterList[i]에 무기가 배정됐다면
            {
                //무기 이미지를 slot의 무기 sprite에 배정한다
                Slot[i].transform.GetChild(5).GetComponent<Image>().sprite =
                    DataManager.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].EquippedWeapon.Name);
            }
            else //무기가 배정되지 않았다면, 알파값을 0f로 조정하여 투명하게 만든다
            {
                Image weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>();

                Color color = weaponImage.color;
                color.a = 0f;
                weaponImage.color = color;

            }

            if (DataManager.instance.MyCharacterList[i].EquippedArmor.Name != "") //MyCharacterList[i]에 방어구가 배정됐다면
            {
                //방어구 이미지를 slot의 방어구 sprite에 배정한다
                Slot[i].transform.GetChild(6).GetComponent<Image>().sprite =
                    DataManager.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].EquippedArmor.Name);
            }
            else //방어구가 배정되지 않았다면, 알파값을 0f로 조정하여 투명하게 만든다
            {
                Image armorImage = Slot[i].transform.GetChild(6).GetComponent<Image>();

                Color color = armorImage.color;
                color.a = 0f;
                armorImage.color = color;
            }


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
