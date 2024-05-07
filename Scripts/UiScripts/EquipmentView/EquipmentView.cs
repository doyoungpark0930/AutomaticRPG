using KnightsUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;
    private List<Weapon> weaponList;

    [SerializeField] GameObject[] Slot;

    [SerializeField] Text EquipmentNameText;
    [SerializeField] Text EquipmentLevel;
    [SerializeField] Image EquipmentImage;
    [SerializeField] GameObject EquipButton;
    [SerializeField] GameObject EquippedCharacterName;
    [SerializeField] Text CurrentReinforce;
    [SerializeField] Text CurrentAttackPower;
    [SerializeField] Text NextAttackPower;
    [SerializeField] Text CurrentHealth;
    [SerializeField] Text NextHealth;
    [SerializeField] Button BreakButton;
    [SerializeField] Text BreakReward; //����*�󸶸�ŭ�� ��
    [SerializeField] GameObject LevelUpButton;
    [SerializeField] GameObject ReinforceButton;
    [SerializeField] GameObject NotSelectedImage;
    public void initialize()
    {
        //���� ĳ���Ͱ� ������ ���Ⱑ �ִٸ�, �ش� ������ ������ ����
        if (characterInfo.EquippedWeapon!=null && characterInfo.EquippedWeapon.Name != "") 
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

        FilterWeaponsByJob(characterInfo.Job);
        InitialSlotUpdate();
    }


    private void FilterWeaponsByJob(JobType jobType)
    {
        WeaponType requiredType;
        switch (jobType)
        {
            case JobType.Warrior:
                requiredType = WeaponType.Sword;
                break;
            case JobType.Mage:
                requiredType = WeaponType.Staff;
                break;
            case JobType.Archer:
                requiredType = WeaponType.Bow;
                break;
            default:
                Debug.LogError("Unknown job type: " + jobType);
                return;
        }

        weaponList = DataModel.instance.MyWeaponList
                     .Where(weapon => weapon.Type == requiredType)
                     .ToList();
    }

    public void InitialSlotUpdate() //Slot�� �̹������� �ִ´�
    {
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
        SlotImageUpdate();

    }
    private void SlotImageUpdate()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            Slot[i].transform.GetChild(2).GetComponent<Image>().sprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weaponList[i].Name);

            var characterImage = Slot[i].transform.GetChild(4).GetComponent<Image>();
            characterImage.sprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weaponList[i].EquippedCharacterName);
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

        //��� ���� �ʱ�ȭ
        EquipmentNameText.text = weapon.Name;
        EquipmentLevel.text = weapon.Level.ToString();
        EquipmentImage.sprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weapon.Name);
        CurrentReinforce.text = weapon.Defense.ToString();
        CurrentAttackPower.text = weapon.Damage.ToString();
        CurrentHealth.text = weapon.Health.ToString();

        
        UpdateEquipButton(weapon);

    }

    //ĳ�������� ���ο� ���� ������ư ������Ʈ(���� or ��������)
    private void UpdateEquipButton(Weapon weapon)
    {
        
        if (weapon.EquippedCharacterName != "") //�� ��� ������ ĳ���Ͱ� �ִٸ�
        {
            //���� ĳ���� ���� Ȱ��ȭ �� ĳ���� �̹���.�̸� �Ҵ�
            EquippedCharacterName.SetActive(true);
            EquippedCharacterName.transform.GetChild(1).GetComponent<Image>().sprite =
                DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weapon.EquippedCharacterName);
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


        SlotImageUpdate();
        UpdateEquipButton(weapon);


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
