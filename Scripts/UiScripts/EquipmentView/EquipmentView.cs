/*
 * EquipmentView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���â UI�� MVP��� �� View�� �ش��Ѵ�.ĳ���� ��� ���� ���̱�, ��� �ɼ� ���� �� ����/������ ���� �ڵ��̴�. EquipmentPresenter�κ��� ������ �����͸� �޴´�.
 *             ĳ���� �ߺ� ���� ����, ���� ����/����, ���� ����, ���� ���׷��̵�, ���� ���� ����� �����Ǿ��ִ�.
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using EquipmentUI;


public class EquipmentView : MonoBehaviour, IEquipmentView
{
    EquipmentPresenter equipmentPresenter;

    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;
    private List<Weapon> weaponList;

    private MyInfo myInfo;

    [SerializeField] GameObject[] Slot;

    //��� ����
    [SerializeField] Text EquipmentName;
    [SerializeField] Text EquipmentLevel;
    [SerializeField] Image EquipmentImage;

    //��� ���� ����
    [SerializeField] GameObject EquipButton;
    [SerializeField] GameObject EquippedCharacterName;

    //���⿬�� ����
    [SerializeField] Text ReinforceLevel;
    [SerializeField] Text CurrentReinforce;

    //��� ���� ����
    [SerializeField] Text CurrentAttackPower;
    [SerializeField] Text CurrentHealth;

    //��� ���� ����
    [SerializeField] GameObject EquipmentBreakButton;
    [SerializeField] Text BreakRewardedGold;
    [SerializeField] Text BreakRewardedBread;

    [SerializeField] GameObject LevelUpButton;
    [SerializeField] GameObject ReinforceButton;
    [SerializeField] GameObject NotSelectedImage;

    //MyInfo
    [SerializeField] Text Gold;
    [SerializeField] Text Bread;

    //��� LevelUp�� �䱸�Ǵ� Gold�� Bread
    [SerializeField] Text NeededGold;
    [SerializeField] Text NeededBread;

    //���⿬���� �䱸�Ǵ� Bread
    [SerializeField] Text ReinforceNeededBread;


    private void Awake()
    {
        equipmentPresenter = new EquipmentPresenter(this);
        myInfo = equipmentPresenter.GetMyInfo();
        EventManager.OnUserInfoUpdated += EquipmentViewMyInfoUpdate;
    }
    public void EquipmentViewMyInfoUpdate(MyInfo myInfo)
    {
        Gold.text = myInfo.Gold.ToString();
        Bread.text = myInfo.Bread.ToString();

    }

    public void SetCharacterInfo(List<Character> characterList, int index)
    {
        if (index < 0 || index >= characterList.Count)
        {
            Debug.LogError("Invalid character index");
            return;
        }
        this.characterList = characterList;
        this.characterInfo = characterList[index];
        this.currentIndex = index;

        equipmentPresenter.FilterWeaponsByJobAndSlotUpdate(characterInfo.Job);
    }

    public void initialize()
    {
        UpdateSlotClickStatus();
        EventManager.UserInfoUpdated(myInfo);
    }

 
    //���� ĳ���Ͱ� ������ ���Ⱑ �ִٸ�, �ش� ������ ������ ���� �޼���
    private void UpdateSlotClickStatus()
    {
        if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "")
        {
            // weaponList���� ���� ������ ������ �ε����� ã��
            int equippedWeaponIndex = weaponList.FindIndex(weapon => weapon.Name == characterInfo.EquippedWeapon.Name);
            if (equippedWeaponIndex >= 0)
            {
                OnSlotClick(characterInfo.EquippedWeapon, equippedWeaponIndex);
            }
            else
            {
                Debug.LogError("Equipped weapon not found in the weapon list");
            }
        }
        else //������ ���Ⱑ ���ٸ�
        {
            NotSelectedImage.SetActive(true);
            LevelUpButton.SetActive(false);
            ReinforceButton.SetActive(false);

            //���� �׵θ� ��� ��Ӱ�
            foreach (GameObject slot in Slot)
            {
                slot.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

    }

    public void SlotUpdate(List<Weapon> weaponList) //Slot�� �̹������� �ִ´�
    {
        this.weaponList = weaponList;

        foreach (GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < weaponList.Count; i++)
        {
            Slot[i].SetActive(true);
            var weapon = weaponList[i];

            //�� Slot��ư�� �̺�Ʈ������ �Ҵ�
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
            var button = Slot[localIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                OnSlotClick(weapon,localIndex);
            }
            );
            
        }
        equipmentPresenter.SlotImageUpdate(weaponList);

    }
    public void SlotDataUpdate(List<SlotData> slotData)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            Slot[i].transform.GetChild(2).GetComponent<Image>().sprite = slotData[i].EquipmentSprite;
            Slot[i].transform.GetChild(3).GetComponent<Text>().text = slotData[i].EquipmentLevel.ToString();
            var characterImage = Slot[i].transform.GetChild(4).GetComponent<Image>();
            characterImage.sprite = slotData[i].CharacterSprite;
            characterImage.color = characterImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;
        }


    }
    private void OnSlotClick(Weapon weapon,int index)
    {
        NotSelectedImage.SetActive(false);
        LevelUpButton.SetActive(true);
        ReinforceButton.SetActive(true);

        //Ŭ���� ���Ը� �׵θ��� ��¦�̰�
        foreach (GameObject slot in Slot)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }
        Slot[index].transform.GetChild(0).gameObject.SetActive(true);

        //��� ����â �ʱ�ȭ
        equipmentPresenter.UpdateEquipmentStats(weapon);


        //������ư�� ������ ���� �Ҵ�
        LevelUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        LevelUpButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentPresenter.OnLevelUpButtonClick(weapon);
        }
        );

        //������ ��ư�� ���� �Ҵ�
        EquipmentBreakButton.GetComponent<Button>().onClick.RemoveAllListeners();
        EquipmentBreakButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentPresenter.OnBreakButtonClick(weapon);
        }
        );

        //���⿬�� ��ư�� ���� �Ҵ�
        ReinforceButton.GetComponent<Button>().onClick.RemoveAllListeners();
        ReinforceButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentPresenter.OnReinforceButtonClick(weapon);
        }
        );

        equipmentPresenter.UpdateEquipStatus(weapon);

    }
    public void UpdateStats(Weapon weapon, Sprite equipmentSprite)
    {
        //��� ���� �ʱ�ȭ
        EquipmentName.text = weapon.Name;
        EquipmentLevel.text = weapon.Level.ToString();
        EquipmentImage.sprite = equipmentSprite;
        ReinforceLevel.text = weapon.ReinforceLevel.ToString();
        CurrentReinforce.text = weapon.Defense.ToString();
        CurrentAttackPower.text = weapon.Damage.ToString();
        CurrentHealth.text = weapon.Health.ToString();

        //��� ������ ��ư �ʱ�ȭ
        NeededGold.text = (150 + weapon.Level * 15).ToString();
        NeededBread.text = ((weapon.Level / 10 + 1) * 14).ToString();

        //��� ���� ���� �ʱ�ȭ(����� �� ����� ��ȭ�� 0.7��)
        int totalGoldNeeded = 0;
        int totalBreadNeeded = 0;
        for (int level = 1; level <= weapon.Level; level++)
        {
            totalGoldNeeded += 150 + (level * 15);
            totalBreadNeeded += ((level / 10) + 1) * 14;
        }
        //���ط� ����� ��ȭ �ʱ�ȭ
        BreakRewardedGold.text = ((int)(totalGoldNeeded * 0.7f)).ToString();
        BreakRewardedBread.text = ((int)(totalBreadNeeded * 0.7f)).ToString();

        //���⿬���� �ʿ��� ��ȭ �ʱ�ȭ
        ReinforceNeededBread.text = (weapon.ReinforceLevel * 100).ToString();
    }

    public void LevelUp(Weapon weapon, MyInfo myInfo)
    {
        if (myInfo.Gold >= int.Parse(NeededGold.text) && myInfo.Bread >= int.Parse(NeededBread.text)) //���� ���� Gold�� Bread�� ����ϴٸ�
        {
            myInfo.Gold -= int.Parse(NeededGold.text);
            myInfo.Bread -= int.Parse(NeededBread.text);

            //��� ���� 1���� ���� ���
            weapon.Level += 1;
            weapon.Damage += 1;
            weapon.Health += 5;

            //��� ���� â �ʱ�ȭ
            equipmentPresenter.UpdateEquipmentStats(weapon);

            equipmentPresenter.SlotImageUpdate(weaponList);

            EventManager.UserInfoUpdated(myInfo);

        }
        else
        {
            Debug.Log("Gold or Bread is not enough");
        }
    }

    public void BreakEquipment()
    {
        myInfo.Gold += int.Parse(BreakRewardedGold.text);
        myInfo.Bread += int.Parse(BreakRewardedBread.text);

        //weaponList ���Ҵ� �� ���Ծ�����Ʈ
        equipmentPresenter.FilterWeaponsByJobAndSlotUpdate(characterInfo.Job);
     
        //ĳ���� ��� ���� ���ο� ���� SlotŬ�� ����
        UpdateSlotClickStatus();

        EventManager.UserInfoUpdated(myInfo);


    }

    

   

    public void EquipmentReinforce(Weapon weapon)
    {

        if (myInfo.Bread >= int.Parse(ReinforceNeededBread.text))
        {
            myInfo.Bread -= int.Parse(ReinforceNeededBread.text);

            //����� ���⿬�� ���� �� ���� ���
            weapon.ReinforceLevel += 1;
            weapon.Defense += 1;

            //��� ���� â �ʱ�ȭ
            equipmentPresenter.UpdateEquipmentStats(weapon);

            EventManager.UserInfoUpdated(myInfo);
        }
        else
        {
            Debug.Log("Bread is not enough");
        }

       
    }



    //ĳ�������� ���ο� ���� �������¿� ������ư ������Ʈ
    public void UpdateEquipButtonAndEquipStatus(Weapon weapon, Sprite characterSprite)
    {
        
        if (weapon.EquippedCharacterName != "") //�� ��� ������ ĳ���Ͱ� �ִٸ�
        {
            //���� ĳ���� ���� Ȱ��ȭ �� ĳ���� �̹���.�̸� �Ҵ�
            EquippedCharacterName.SetActive(true);
            EquippedCharacterName.transform.GetChild(1).GetComponent<Image>().sprite = characterSprite;
            EquippedCharacterName.transform.GetChild(2).GetComponent<Text>().text = weapon.EquippedCharacterName;

            if (weapon.EquippedCharacterName == characterInfo.Name) //�� ��� ������ ĳ���Ͱ� ���� ĳ���Ͷ��
            {
                //���� ��ư ����������
                Image imageComponent = EquipButton.GetComponent<Image>(); 
                imageComponent.color = new Color(150f / 255f, 50f / 255f, 50f / 255f, 110f / 255f);

                //������ư �ؽ�Ʈ ������ �� "��������"��
                EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                EquipButton.transform.GetChild(0).GetComponent<Text>().text = "��������";
            }
            else //�� ��� ������ ĳ���Ͱ� �ٸ� ĳ���Ͷ��
            {
                //���� ��ư ���������
                Image imageComponent = EquipButton.GetComponent<Image>();
                imageComponent.color = new Color(236f / 255f, 198f / 255f, 124f / 255f, 110f / 255f);

                //������ư �ؽ�Ʈ ��� �� "����"����
                EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                EquipButton.transform.GetChild(0).GetComponent<Text>().text = "����";
            }

        }
        else
        {
            //���� ĳ���� ���� ��Ȱ��ȭ
            EquippedCharacterName.SetActive(false);

            //���� ��ư ���������
            Image imageComponent = EquipButton.GetComponent<Image>(); 
            imageComponent.color = new Color(236f / 255f, 198f / 255f, 124f / 255f, 110f / 255f);

            //������ư �ؽ�Ʈ ��� �� "����"����
            EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            EquipButton.transform.GetChild(0).GetComponent<Text>().text = "����";
        }

        //������ư�� ������ ���� �Ҵ�
        EquipButton.GetComponent<Button>().onClick.RemoveAllListeners();
        EquipButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            EquipOrOffWeapon(weapon);
        }
        );
    }

    // ���� �Ҵ� �޼���
    public void EquipOrOffWeapon(Weapon weapon)
    {
        
        if(EquipButton.transform.GetChild(0).GetComponent<Text>().text == "����") //���� ���� ��ư�̶��,�׳��������� ��Ű��, ������ Slot�� ���� ��ư�� �ʱ�ȭ
        {
            if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "") //���� ĳ���Ͱ� ������ ���Ⱑ �ִٸ�
            {
                //���� �������⿡�� ĳ���� �̸� ����
                var EquippedWeapon = DataModel.instance.MyWeaponList.FirstOrDefault(weapon => weapon.Name == characterInfo.EquippedWeapon.Name);
                if (EquippedWeapon != null) EquippedWeapon.EquippedCharacterName = "";

                //���� ĳ������������ �������� ����
                characterInfo.EquippedWeapon = null;

            }

            if (weapon.EquippedCharacterName != "") //�ش� ���⸦ ������ ĳ���Ͱ� �ִٸ�
            {
                // �̹� �ٸ� ĳ���Ϳ� �Ҵ�� ������ �� ĳ������ ���⸦ ����(���� �ߺ� ���� ����)
                var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
                if (character != null) character.EquippedWeapon = null;
            }

            // �� ĳ���Ϳ� �� ���⸦ �Ҵ�
            characterInfo.EquippedWeapon = weapon;
            weapon.EquippedCharacterName = characterInfo.Name;  // ���⿡ ���ο� �Ҵ� ĳ���� ����

        }
        else if(EquipButton.transform.GetChild(0).GetComponent<Text>().text == "��������")//�������� ��ư�̶��
        {
            //��������
            var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
            if (character != null) character.EquippedWeapon = null;
            weapon.EquippedCharacterName = "";
        }
        else
        {
            Debug.LogError("Check Again ������ư text");
        }


        equipmentPresenter.SlotImageUpdate(weaponList);
        equipmentPresenter.UpdateEquipStatus(weapon);


        DataModel.instance.OnSaveRequested?.Invoke();

    }




    
    public void ToCharacterInfoView()
    {
        var characterInfoView = UiPool.GetObject("CharacterInfoView");
        characterInfoView.GetComponent<CharacterInfoView>().SetCharacterInfo(characterList, currentIndex);
        characterInfoView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        characterInfoView.GetComponent<CharacterInfoView>().initialize();
        UiPool.ReturnObject(gameObject);
    }
}
