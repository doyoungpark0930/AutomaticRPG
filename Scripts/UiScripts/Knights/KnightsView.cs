using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using KnightsUI; //KnightsPresenter ��ũ��Ʈ�� ���ǵǾ�����

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
        //Slot�̹����� ù ����ÿ� �� �޴´�. 
        knightsPresenter.ViewUpdate();
    }
    public void initialize() //onEnable��ü
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
        knightsPresenter.ViewUpdate();
    }
   

    public void SlotUpdate(List<CharacterData> MyCharacterList) //Slot�� �̹������� �ִ´�
    {
        //�ϴ� MyCharacterList��ŭ slot�����ϴ°ɷ�����
        for(int i = 0; i < MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = MyCharacterList[i].Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = MyCharacterList[i].Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite =
                DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].Element.ToString());
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = 
                DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].Job.ToString());

            if(DataModel.instance.MyCharacterList[i].EquippedWeapon.Name != "") //MyCharacterList[i]�� ���Ⱑ �����ƴٸ�
            {
                //���� �̹����� slot�� ���� sprite�� �����Ѵ�
                Slot[i].transform.GetChild(5).GetComponent<Image>().sprite =
                    DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].EquippedWeapon.Name);
            }
            else //���Ⱑ �������� �ʾҴٸ�, ���İ��� 0f�� �����Ͽ� �����ϰ� �����
            {
                Image weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>();

                Color color = weaponImage.color;
                color.a = 0f;
                weaponImage.color = color;

            }

            if (DataModel.instance.MyCharacterList[i].EquippedArmor.Name != "") //MyCharacterList[i]�� ���� �����ƴٸ�
            {
                //�� �̹����� slot�� �� sprite�� �����Ѵ�
                Slot[i].transform.GetChild(6).GetComponent<Image>().sprite =
                    DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == DataModel.instance.MyCharacterList[i].EquippedArmor.Name);
            }
            else //���� �������� �ʾҴٸ�, ���İ��� 0f�� �����Ͽ� �����ϰ� �����
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

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainUi");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainUi>().initialize();

        //���� destory
        UiPool.ReturnObject(gameObject);
    }


}
