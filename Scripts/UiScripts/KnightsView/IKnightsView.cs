/*
 * IKnightsView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���â UI�� MVP����� View�� �������̽��� �ش��Ѵ�
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using KnightsUI;
using System.Collections.Generic;

public interface IKnightsView
{
    void SlotUpdate(List<CharacterData> CharacterList,List<Character> CharacterInfoList);
}
