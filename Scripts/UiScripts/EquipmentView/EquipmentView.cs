using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentView : MonoBehaviour
{
    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;
    private List<Weapon> weaponList;

    [SerializeField] GameObject[] Slot;
    public void initialize()
    {

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
        SlotUpdate();
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

    public void SlotUpdate() //Slot에 이미지들을 넣는다
    {
        foreach (GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < weaponList.Count; i++)
        {
            Slot[i].SetActive(true);
            var weapon = weaponList[i];
            Slot[i].transform.GetChild(1).GetComponent<Image>().sprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weapon.Name);

            
            //각 Slot버튼에 이벤트리스너 할당
            int localIndex = i; //람다식 외부 변수 참조 방지용
            var button = Slot[localIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => EquipWeapon(weapon)); //해당 캐릭터 정보를 넘김
            

        }


    }

    // 무기 할당 메서드
    public void EquipWeapon(Weapon weapon)
    {
        if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "") //현재 캐릭터가 장착한 무기가 있다면
        {
            //현재 장착 무기 제거.
            characterInfo.EquippedWeapon = null;
        }

        if (weapon.EquippedCharacterName != "") //해당 무기를 장착한 캐릭터가 있다면
        {
            // 이미 다른 캐릭터에 할당된 무기라면 그 캐릭터의 무기를 제거(무기 중복 착용 방지)
            var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
            if(character!=null) character.EquippedWeapon = null;
        }

        // 이 캐릭터에 새 무기를 할당
        characterInfo.EquippedWeapon = weapon;
        weapon.EquippedCharacterName = characterInfo.Name;  // 무기에 새로운 할당 캐릭터 설정

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
