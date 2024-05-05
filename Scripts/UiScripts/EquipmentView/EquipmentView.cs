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

    public void SlotUpdate() //Slot�� �̹������� �ִ´�
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

            
            //�� Slot��ư�� �̺�Ʈ������ �Ҵ�
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
            var button = Slot[localIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => EquipWeapon(weapon)); //�ش� ĳ���� ������ �ѱ�
            

        }


    }

    // ���� �Ҵ� �޼���
    public void EquipWeapon(Weapon weapon)
    {
        if (characterInfo.EquippedWeapon != null && characterInfo.EquippedWeapon.Name != "") //���� ĳ���Ͱ� ������ ���Ⱑ �ִٸ�
        {
            //���� ���� ���� ����.
            characterInfo.EquippedWeapon = null;
        }

        if (weapon.EquippedCharacterName != "") //�ش� ���⸦ ������ ĳ���Ͱ� �ִٸ�
        {
            // �̹� �ٸ� ĳ���Ϳ� �Ҵ�� ������ �� ĳ������ ���⸦ ����(���� �ߺ� ���� ����)
            var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.Name == weapon.EquippedCharacterName);
            if(character!=null) character.EquippedWeapon = null;
        }

        // �� ĳ���Ϳ� �� ���⸦ �Ҵ�
        characterInfo.EquippedWeapon = weapon;
        weapon.EquippedCharacterName = characterInfo.Name;  // ���⿡ ���ο� �Ҵ� ĳ���� ����

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
