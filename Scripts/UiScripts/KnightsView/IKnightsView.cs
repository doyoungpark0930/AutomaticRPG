/*
 * IKnightsView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 장비창 UI의 MVP방식중 View의 인터페이스에 해당한다
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
