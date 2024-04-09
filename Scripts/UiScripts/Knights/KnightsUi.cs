using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class KnightsUi : MonoBehaviour, ICharacterListObserver
{
    CameraDrag cameraDrag;
    [SerializeField] GameObject[] Slot; //�̰� �ڵ�ȭ�ؾ��ҵ�.. �ϴ� mvc�� �Ѵ�

    public void OnCharacterListUpdated()
    {
        // MyCharacterList�� ���� ������ ������� UI ������Ʈ
        ShowCharacterSlot();
    }

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
    }
    void Start()
    {
        //KnightsUI��ũ��Ʈ�� �������� ����Ѵ�
        DataManager.instance.RegisterObserver(this);
        //Slot�̹����� ù ����ÿ� �� �޴´�. 
        ShowCharacterSlot();
    }
    public void initialize() //onEnable��ü
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
    }
   

    /*�̰� slot���� mvp�������� �ƴϸ� scriptable Object�� �ؼ� ������ ���������� �ڵ� ����
     * allCharacterList�� ���� �°� slot���� �׸���, myCharacterList�� ���� �ڹ��� ��� => �̰� �ϴ� allCharaceList�� MyCharacterList��
     * ���и� ���ָ� �Ǵϱ� ���߿� ����
     * */
    private void ShowCharacterSlot() //Slot�� �̹������� �ִ´�
    {
        //�ϴ� MyCharacterList��ŭ slot�����ϴ°ɷ�����
        for(int i = 0; i < DataManager.instance.MyCharacterList.Count; i++)
        {
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = DataManager.instance.MyCharacterList[i].Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite =
                DataManager.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].Element.ToString());
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = 
                DataManager.instance.JobSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].Job.ToString());

            if(DataManager.instance.MyCharacterList[i].EquippedWeapon.Name != "") //MyCharacterList[i]�� ���Ⱑ �����ƴٸ�
            {
                //���� �̹����� slot�� ���� sprite�� �����Ѵ�
                Slot[i].transform.GetChild(5).GetComponent<Image>().sprite =
                    DataManager.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].EquippedWeapon.Name);
            }
            else //���Ⱑ �������� �ʾҴٸ�, ���İ��� 0f�� �����Ͽ� �����ϰ� �����
            {
                Image weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>();

                Color color = weaponImage.color;
                color.a = 0f;
                weaponImage.color = color;

            }

            if (DataManager.instance.MyCharacterList[i].EquippedArmor.Name != "") //MyCharacterList[i]�� ���� �����ƴٸ�
            {
                //�� �̹����� slot�� �� sprite�� �����Ѵ�
                Slot[i].transform.GetChild(6).GetComponent<Image>().sprite =
                    DataManager.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == DataManager.instance.MyCharacterList[i].EquippedArmor.Name);
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
