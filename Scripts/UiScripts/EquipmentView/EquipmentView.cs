using CharacterInfoUI;
using KnightsUI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    public void SlotUpdate(List<CharacterData> CharacterList, List<Character> CharacterInfo) //Slot에 이미지들을 넣는다
    {
        foreach (GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var characterData = CharacterList[i];
            
        }


    }

    public void ToCharacterInfoView()
    {
        var characterInfoView = UiPool.GetObject("CharacterInfoView");
        characterInfoView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        characterInfoView.GetComponent<CharacterInfoView>().initialize();
        UiPool.ReturnObject(gameObject);
    }
}
