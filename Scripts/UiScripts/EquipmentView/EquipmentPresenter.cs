/*
 * EquipmentPresenter.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���â UI�� MVP���� �� Presenter�� �ش��Ѵ�. EquipmentView���� �ʿ��� �����͵��� DataModel�κ��� �޾� �����Ͽ� �������ش�.
 *             EquipmentUI ���ӽ����̽��� Ȱ���Ͽ� �����͸� ���â UI������ ����� �� �ְ� ���ȭ�Ͽ���.
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EquipmentUI
{
    public class SlotData 
    {
        public Sprite EquipmentSprite;
        public Sprite CharacterSprite;
        public int EquipmentLevel;
    }
    public class EquipmentPresenter
    {
        IEquipmentView equipmentView;
        List<SlotData> slotData;
        MyInfo myInfo;
        public EquipmentPresenter(IEquipmentView view)
        {
            equipmentView = view;
        }
        public MyInfo GetMyInfo()
        {
            myInfo = DataModel.instance.myInfo;
            return myInfo;
        }
        public void FilterWeaponsByJobAndSlotUpdate(JobType jobType)
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

            var weaponList = DataModel.instance.MyWeaponList
                         .Where(weapon => weapon.Type == requiredType)
                         .ToList();
            //���Ҵ� �� weaponLIst�� ���� Slot������Ʈ
            equipmentView.SlotUpdate(weaponList);
        }

        public void UpdateEquipmentStats(Weapon weapon)
        {
            var equipmentSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weapon.Name);
            equipmentView.UpdateStats(weapon, equipmentSprite);
        }

        public void OnLevelUpButtonClick(Weapon weapon)
        {
            equipmentView.LevelUp(weapon, DataModel.instance.myInfo);
            DataModel.instance.OnSaveRequested?.Invoke();
        }

        public void OnBreakButtonClick(Weapon weapon)
        {
            //MyWeaponList���� �ش� weapon����
            var weaponIndex = DataModel.instance.MyWeaponList.FindIndex(myWeapon => myWeapon.Name == weapon.Name);
            if (weaponIndex >= 0)
            {
                //�ش� weapon�� �����ϰ� �ִ� ĳ���Ͱ� �ִٸ�, ĳ���� �������⿡ null�� �Ҵ�
                var character = DataModel.instance.MyCharacterList.FirstOrDefault(character => character.EquippedWeapon != null && character.EquippedWeapon.Name == weapon.Name);
                if (character != null)
                {
                    character.EquippedWeapon = null;
                }
                DataModel.instance.MyWeaponList.RemoveAt(weaponIndex);
            }
            else
            {
                Debug.LogError("Can't find weapon");
            }

            equipmentView.BreakEquipment();

            //����
            DataModel.instance.OnSaveRequested?.Invoke();
        }

        public void OnReinforceButtonClick(Weapon weapon)
        {
            equipmentView.EquipmentReinforce(weapon);

            //����
            DataModel.instance.OnSaveRequested?.Invoke();

        }

        //�ش� ��� ���� ���� ������Ʈ
        public void UpdateEquipStatus(Weapon weapon)
        {
            var characterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weapon.EquippedCharacterName);
            equipmentView.UpdateEquipButtonAndEquipStatus(weapon, characterSprite);
        }

        public void SlotImageUpdate(List<Weapon> weaponList)
        {
            // slotData �ʱ�ȭ �� ũ�� ����
            slotData = new List<SlotData>(new SlotData[weaponList.Count]);
            for (int i = 0; i < weaponList.Count; i++)
            {
                slotData[i] = new SlotData();
                slotData[i].EquipmentSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == weaponList[i].Name);
                slotData[i].CharacterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weaponList[i].EquippedCharacterName);
                slotData[i].EquipmentLevel = weaponList[i].Level;
            }
            equipmentView.SlotDataUpdate(slotData);
        }
    }
}


