using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KnightsUI; //KnightsPresenter 스크립트에 정의되어있음

public interface IKnightsView
{
    void SlotUpdate(List<CharacterData> CharacterList);
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
        knightsPresenter.ViewUpdate();
    }
    public void initialize() //onEnable대체
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        knightsPresenter.ViewUpdate();
    }
   

    public void SlotUpdate(List<CharacterData> CharacterList) //Slot에 이미지들을 넣는다
    {
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var characterData = CharacterList[i];
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = characterData.Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = characterData.Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite = characterData.ElementSprite;
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = characterData.JobSprite;

            var weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>();
            weaponImage.sprite = characterData.WeaponSprite;
            weaponImage.color = weaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

            var armorImage = Slot[i].transform.GetChild(6).GetComponent<Image>();
            armorImage.sprite = characterData.ArmorSprite;
            armorImage.color = armorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;
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
