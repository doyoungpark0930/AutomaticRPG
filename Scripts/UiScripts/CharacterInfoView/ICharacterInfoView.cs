/*
 * ICharacterInfoView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ĳ���� ����UI�� MVP���� �� View�� �������̽��� �ش��Ѵ�.
 * 
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using CharacterInfoUI;
public interface ICharacterInfoView
{
    public void UpdateCharacterInfo(InfoData infodata);
    public void CharacterViewMyInfoUpdate(MyInfo myinfo);

    public void LevelUp(MyInfo myinfo);
}