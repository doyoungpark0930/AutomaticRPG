/*
 * ICharacterInfoView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 캐릭터 정보UI의 MVP패턴 중 View의 인터페이스에 해당한다.
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