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
    [SerializeField] Text BreakReward; //레벨*얼마만큼의 빵
    [SerializeField] GameObject LevelUpButton;
    [SerializeField] GameObject ReinforceButton;
    [SerializeField] GameObject NotSelectedImage;
    public void initialize()
    {
        //현재 캐릭터가 장착한 무기가 있다면, 해당 무기의 정보를 띄운다
        if (characterInfo.EquippedWeapon!=null && characterInfo.EquippedWeapon.Name != "") 
        {
            // weaponList에서 현재 장착한 무기의 인덱스를 찾기
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
        else //장착한 무기가 없다면
        {
            NotSelectedImage.SetActive(true);
            LevelUpButton.SetActive(false);
            ReinforceButton.SetActive(false);

            //슬롯 테두리 모두 어둡게
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

    public void InitialSlotUpdate() //Slot에 이미지들을 넣는다
    {
        foreach (GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < weaponList.Count; i++)
        {
            Slot[i].SetActive(true);
            var weapon = weaponList[i];

            //각 Slot버튼에 이벤트리스너 할당
            int localIndex = i; //람다식 외부 변수 참조 방지용
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

        //클릭한 슬롯만 테두리만 반짝이게
        foreach (GameObject slot in Slot)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }
        Slot[index].transform.GetChild(0).gameObject.SetActive(true);

        //장비 스텟 초기화
        EquipmentNameText.text = weapon.Name;
        EquipmentLevel.text = weapon.Level.ToString();
        EquipmentImage.sprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weapon.Name);
        CurrentReinforce.text = weapon.Defense.ToString();
        CurrentAttackPower.text = weapon.Damage.ToString();
        CurrentHealth.text = weapon.Health.ToString();

        
        UpdateEquipButton(weapon);

    }

    //캐릭터장착 여부에 따른 장착버튼 업데이트(장착 or 장착해제)
    private void UpdateEquipButton(Weapon weapon)
    {
        
        if (weapon.EquippedCharacterName != "") //이 장비를 장착한 캐릭터가 있다면
        {
            //장착 캐릭터 정보 활성화 및 캐릭터 이미지.이름 할당
            EquippedCharacterName.SetActive(true);
            EquippedCharacterName.transform.GetChild(1).GetComponent<Image>().sprite =
                DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weapon.EquippedCharacterName);
            EquippedCharacterName.transform.GetChild(2).GetComponent<Text>().text = weapon.EquippedCharacterName;

            if (weapon.EquippedCharacterName == characterInfo.Name) //이 장비를 장착한 캐릭터가 현재 캐릭터라면
            {
                //장착 버튼 붉은색으로
                Image imageComponent = EquipButton.GetComponent<Image>(); 
                imageComponent.color = new Color(150f / 255f, 50f / 255f, 50f / 255f, 110f / 255f);

                //장착버튼 텍스트 검은색 및 "장착해제"로
                EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                EquipButton.transform.GetChild(0).GetComponent<Text>().text = "장착해제";
            }
            else //이 장비를 장착한 캐릭터가 다른 캐릭터라면
            {
                //장착 버튼 노란색으로
                Image imageComponent = EquipButton.GetComponent<Image>();
                imageComponent.color = new Color(236f / 255f, 198f / 255f, 124f / 255f, 110f / 255f);

                //장착버튼 텍스트 흰색 및 "장착"으로
                EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
                EquipButton.transform.GetChild(0).GetComponent<Text>().text = "장착";
            }

        }
        else
        {
            //장착 캐릭터 정보 비활성화
            EquippedCharacterName.SetActive(false);

            //장착 버튼 노란색으로
            Image imageComponent = EquipButton.GetComponent<Image>(); 
            imageComponent.color = new Color(236f / 255f, 198f / 255f, 124f / 255f, 110f / 255f);

            //장착버튼 텍스트 흰색 및 "장착"으로
            EquipButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;
            EquipButton.transform.GetChild(0).GetComponent<Text>().text = "장착";
        }

        //장착버튼에 장착할 무기 할당
        EquipButton.GetComponent<Button>().onClick.RemoveAllListeners();
        EquipButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            EquipOrOffWeapon(weapon);
        }
        );
    }

    // 무기 할당 메서드
    public void EquipOrOffWeapon(Weapon weapon)
    {
        
        if(EquipButton.transform.GetChild(0).GetComponent<Text>().text == "장착") //장착 해제 버튼이라면,그냥장착해제 시키고, 나머지 Slot의 장착 버튼만 초기화
        {
            if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "") //현재 캐릭터가 장착한 무기가 있다면
            {
                //기존 장착무기에서 캐릭터 이름 제거
                var EquippedWeapon = DataModel.instance.MyWeaponList.FirstOrDefault(weapon => weapon.Name == characterInfo.EquippedWeapon.Name);
                if (EquippedWeapon != null) EquippedWeapon.EquippedCharacterName = "";

                //현재 캐릭터정보에서 장착무기 제거
                characterInfo.EquippedWeapon = null;

            }

            if (weapon.EquippedCharacterName != "") //해당 무기를 장착한 캐릭터가 있다면
            {
                // 이미 다른 캐릭터에 할당된 무기라면 그 캐릭터의 무기를 제거(무기 중복 착용 방지)
                var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
                if (character != null) character.EquippedWeapon = null;
            }

            // 이 캐릭터에 새 무기를 할당
            characterInfo.EquippedWeapon = weapon;
            weapon.EquippedCharacterName = characterInfo.Name;  // 무기에 새로운 할당 캐릭터 설정

        }
        else if(EquipButton.transform.GetChild(0).GetComponent<Text>().text == "장착해제")//장착해제 버튼이라면
        {
            //장착해제
            var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
            if (character != null) character.EquippedWeapon = null;
            weapon.EquippedCharacterName = "";
        }
        else
        {
            Debug.LogError("Check Again 장착버튼 text");
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
