/*
 * EquipmentPresenter.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 장비창 UI의 MVP패턴 중 Presenter에 해당한다. EquipmentView에서 필요한 데이터들을 DataModel로부터 받아 정제하여 전달해준다.
 *             EquipmentUI 네임스페이스를 활용하여 데이터를 장비창 UI에서만 사용할 수 있게 모듈화하였다.
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
            //재할당 된 weaponLIst에 따라 Slot업데이트
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
            //MyWeaponList에서 해당 weapon제거
            var weaponIndex = DataModel.instance.MyWeaponList.FindIndex(myWeapon => myWeapon.Name == weapon.Name);
            if (weaponIndex >= 0)
            {
                //해당 weapon을 장착하고 있는 캐릭터가 있다면, 캐릭터 장착무기에 null값 할당
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

            //저장
            DataModel.instance.OnSaveRequested?.Invoke();
        }

        public void OnReinforceButtonClick(Weapon weapon)
        {
            equipmentView.EquipmentReinforce(weapon);

            //저장
            DataModel.instance.OnSaveRequested?.Invoke();

        }

        //해당 장비 장착 상태 업데이트
        public void UpdateEquipStatus(Weapon weapon)
        {
            var characterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == weapon.EquippedCharacterName);
            equipmentView.UpdateEquipButtonAndEquipStatus(weapon, characterSprite);
        }

        public void SlotImageUpdate(List<Weapon> weaponList)
        {
            // slotData 초기화 및 크기 설정
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


