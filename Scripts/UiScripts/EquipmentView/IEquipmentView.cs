/*
 * IEquipmentView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���â UI�� MVP��� �� View�� �������̽��� �ش��Ѵ�
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