/*
 * EquipmentView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 장비창 UI의 MVP방식 중 View에 해당한다.캐릭터 장비 정보 보이기, 장비 옵션 변경 및 장착/해제를 돕는 코드이다. EquipmentPresenter로부터 정제된 데이터를 받는다.
 *             캐릭터 중복 착용 방지, 무기 장착/해제, 무기 분해, 무기 업그레이드, 무기 연마 기능이 구현되어있다.
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

    //장비 정보
    [SerializeField] Text EquipmentName;
    [SerializeField] Text EquipmentLevel;
    [SerializeField] Image EquipmentImage;

    //장비 장착 정보
    [SerializeField] GameObject EquipButton;
    [SerializeField] GameObject EquippedCharacterName;

    //무기연마 정보
    [SerializeField] Text ReinforceLevel;
    [SerializeField] Text CurrentReinforce;

    //장비 스텟 정보
    [SerializeField] Text CurrentAttackPower;
    [SerializeField] Text CurrentHealth;

    //장비 분해 정보
    [SerializeField] GameObject EquipmentBreakButton;
    [SerializeField] Text BreakRewardedGold;
    [SerializeField] Text BreakRewardedBread;

    [SerializeField] GameObject LevelUpButton;
    [SerializeField] GameObject ReinforceButton;
    [SerializeField] GameObject NotSelectedImage;

    //MyInfo
    [SerializeField] Text Gold;
    [SerializeField] Text Bread;

    //장비 LevelUp에 요구되는 Gold와 Bread
    [SerializeField] Text NeededGold;
    [SerializeField] Text NeededBread;

    //무기연마에 요구되는 Bread
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

 
    //현재 캐릭터가 장착한 무기가 있다면, 해당 무기의 정보를 띄우는 메서드
    private void UpdateSlotClickStatus()
    {
        if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "")
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

    public void SlotUpdate(List<Weapon> weaponList) //Slot에 이미지들을 넣는다
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

        //클릭한 슬롯만 테두리만 반짝이게
        foreach (GameObject slot in Slot)
        {
            slot.transform.GetChild(0).gameObject.SetActive(false);
        }
        Slot[index].transform.GetChild(0).gameObject.SetActive(true);

        //장비 스텟창 초기화
        equipmentPresenter.UpdateEquipmentStats(weapon);


        //장착버튼에 장착할 무기 할당
        LevelUpButton.GetComponent<Button>().onClick.RemoveAllListeners();
        LevelUpButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentPresenter.OnLevelUpButtonClick(weapon);
        }
        );

        //장비분해 버튼에 무기 할당
        EquipmentBreakButton.GetComponent<Button>().onClick.RemoveAllListeners();
        EquipmentBreakButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            equipmentPresenter.OnBreakButtonClick(weapon);
        }
        );

        //무기연마 버튼에 무기 할당
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
        //장비 스텟 초기화
        EquipmentName.text = weapon.Name;
        EquipmentLevel.text = weapon.Level.ToString();
        EquipmentImage.sprite = equipmentSprite;
        ReinforceLevel.text = weapon.ReinforceLevel.ToString();
        CurrentReinforce.text = weapon.Defense.ToString();
        CurrentAttackPower.text = weapon.Damage.ToString();
        CurrentHealth.text = weapon.Health.ToString();

        //장비 레벨업 버튼 초기화
        NeededGold.text = (150 + weapon.Level * 15).ToString();
        NeededBread.text = ((weapon.Level / 10 + 1) * 14).ToString();

        //장비 분해 보상 초기화(장비의 총 사용한 재화의 0.7배)
        int totalGoldNeeded = 0;
        int totalBreadNeeded = 0;
        for (int level = 1; level <= weapon.Level; level++)
        {
            totalGoldNeeded += 150 + (level * 15);
            totalBreadNeeded += ((level / 10) + 1) * 14;
        }
        //분해로 보상될 재화 초기화
        BreakRewardedGold.text = ((int)(totalGoldNeeded * 0.7f)).ToString();
        BreakRewardedBread.text = ((int)(totalBreadNeeded * 0.7f)).ToString();

        //무기연마에 필요한 재화 초기화
        ReinforceNeededBread.text = (weapon.ReinforceLevel * 100).ToString();
    }

    public void LevelUp(Weapon weapon, MyInfo myInfo)
    {
        if (myInfo.Gold >= int.Parse(NeededGold.text) && myInfo.Bread >= int.Parse(NeededBread.text)) //레벨 업당 Gold와 Bread가 충분하다면
        {
            myInfo.Gold -= int.Parse(NeededGold.text);
            myInfo.Bread -= int.Parse(NeededBread.text);

            //장비 레벨 1업당 스텟 상승
            weapon.Level += 1;
            weapon.Damage += 1;
            weapon.Health += 5;

            //장비 스텟 창 초기화
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

        //weaponList 재할당 및 슬롯업데이트
        equipmentPresenter.FilterWeaponsByJobAndSlotUpdate(characterInfo.Job);
     
        //캐릭터 장비 장착 여부에 따른 Slot클릭 상태
        UpdateSlotClickStatus();

        EventManager.UserInfoUpdated(myInfo);


    }

    

   

    public void EquipmentReinforce(Weapon weapon)
    {

        if (myInfo.Bread >= int.Parse(ReinforceNeededBread.text))
        {
            myInfo.Bread -= int.Parse(ReinforceNeededBread.text);

            //장비의 무기연마 레벨 및 스텟 상승
            weapon.ReinforceLevel += 1;
            weapon.Defense += 1;

            //장비 스텟 창 초기화
            equipmentPresenter.UpdateEquipmentStats(weapon);

            EventManager.UserInfoUpdated(myInfo);
        }
        else
        {
            Debug.Log("Bread is not enough");
        }

       
    }



    //캐릭터장착 여부에 따른 장착상태와 장착버튼 업데이트
    public void UpdateEquipButtonAndEquipStatus(Weapon weapon, Sprite characterSprite)
    {
        
        if (weapon.EquippedCharacterName != "") //이 장비를 장착한 캐릭터가 있다면
        {
            //장착 캐릭터 정보 활성화 및 캐릭터 이미지.이름 할당
            EquippedCharacterName.SetActive(true);
            EquippedCharacterName.transform.GetChild(1).GetComponent<Image>().sprite = characterSprite;
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
