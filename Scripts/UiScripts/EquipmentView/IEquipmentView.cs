/*
 * IEquipmentView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 장비창 UI의 MVP방식 중 View의 인터페이스에 해당한다
 * 
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using EquipmentUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEquipmentView
{
    public void EquipmentViewMyInfoUpdate(MyInfo myInfo);
    public void SlotUpdate(List<Weapon> weaponList);
    public void UpdateStats(Weapon weapon, Sprite equipmentSprite);

    public void LevelUp(Weapon weapon, MyInfo myInfo);

    public void BreakEquipment();

    public void EquipmentReinforce(Weapon weapon);

    public void UpdateEquipButtonAndEquipStatus(Weapon weapon, Sprite characterSprite);

    public void SlotDataUpdate(List<SlotData> slotData);
}