using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using KnightsUI; //KnightsPresenter 스크립트에 정의되어있음

public interface IKnightsView
{
    void SlotUpdate(List<CharacterData> MyCharacterList);
}
public class KnightsView : MonoBehaviour, IKnightsView
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot;

    KnightsPresenter knightsPresenter;

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        knightsPresenter = new KnightsPresenter(this);
        
    }
    void Start()
    {
        //Slot이미지를 첫 실행시에 다 받는다. 
        knightsPresenter.ViewUpdate();
    }
    public void initialize() //onEnable대체
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        knightsPresenter.ViewUpdate();
    }
   

    public void SlotUpdate(List<CharacterData> MyCharacterList) //Slot에 이미지들을 넣는다
    {
        //일단 MyCharacterList만큼 slot생성하는걸로하자
        for(int i = 0; i < MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = MyCharacterList[i].Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = MyCharacterList[i].Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite =
                DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].Element.ToString());
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = 
                DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].Job.ToString());

            if(DataModel.instance.MyCharacterList[i].EquippedWeapon.Name != "") //MyCharacterList[i]에 무기가 배정됐다면
            {
                //무기 이미지를 slot의 무기 sprite에 배정한다
                Slot[i].transform.GetChild(5).GetComponent<Image>().sprite =
                    DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].EquippedWeapon.Name);
            }
            else //무기가 배정되지 않았다면, 알파값을 0f로 조정하여 투명하게 만든다
            {
                Image weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>();

                Color color = weaponImage.color;
                color.a = 0f;
                weaponImage.color = color;

            }

            if (DataModel.instance.MyCharacterList[i].EquippedArmor.Name != "") //MyCharacterList[i]에 방어구가 배정됐다면
            {
                //방어구 이미지를 slot의 방어구 sprite에 배정한다
                Slot[i].transform.GetChild(6).GetComponent<Image>().sprite =
                    DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].EquippedArmor.Name);
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
