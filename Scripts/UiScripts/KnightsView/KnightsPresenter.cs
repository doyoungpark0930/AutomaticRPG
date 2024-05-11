/*
 * KnightsView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 기사단 UI의 MVP패턴 중 Presenter에 해당한다. KnightsView에서 필요한 데이터들을 DataModel로부터 받아 정제하여 전달해준다.
 *             Linq언어를 이용하여 등급,레벨에 따라 정렬 및 캐릭터 속성, 직업에 따라 필터링 하는 기능이 있다. 
 *             KnightsUI 네임스페이스를 활용하여 데이터를 기사단 UI에서만 사용할 수 있게 모듈화하였다.
 * 
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace KnightsUI
{
    public class CharacterData
    {
        public string Name;
        public int Level;
        public Sprite ElementSprite;
        public Sprite JobSprite;
        public Sprite WeaponSprite;
        public Sprite ArmorSprite;
        public Sprite CharacterSprite;
        public int Grade;
    }
    public class KnightsPresenter
    {

        IKnightsView KnightsView;
        List<Character> myCharacterList; //Model로 부터 받을 MyCharacterList
        bool LevelFilter = false; 
        bool GradeFilter = false;
        HashSet<Element> ElementsFilter;
        HashSet<JobType> JobsFilter;

        public KnightsPresenter(IKnightsView knightsView)
        {
            KnightsView = knightsView;
        }

        // 버튼 토글 상태에 따라 정렬
        public void UpdateByFlags(bool level, bool grade, HashSet<Element> Elements, HashSet<JobType> Jobs)
        {
            LevelFilter = level;
            GradeFilter = grade;
            ElementsFilter = Elements;
            JobsFilter = Jobs;
            SortAndUpdateView();
        }
   
   
        private void SortAndUpdateView()
        {
            myCharacterList = DataModel.instance.MyCharacterList;
            // Element와 Job에 따라 필터링
            List<Character> filteredCharacters = myCharacterList
                    .Where(character =>
                        (ElementsFilter.Count == 4 || ElementsFilter.Contains(character.Element)) && (JobsFilter.Count == 3 || JobsFilter.Contains(character.Job))  )
                    .ToList();

            // Grade와 Level에따른 정렬
            if (GradeFilter && LevelFilter)
            {
                filteredCharacters = filteredCharacters
                    .OrderByDescending(c => c.Grade)
                    .ThenByDescending(c => c.Level)
                    .ToList();
            }
            else if (GradeFilter)
            {
                filteredCharacters = filteredCharacters
                    .OrderByDescending(c => c.Grade)
                    .ToList();
            }
            else if (LevelFilter)
            {
                filteredCharacters = filteredCharacters
                    .OrderByDescending(c => c.Level)
                    .ToList();
            }

            // View로의 데이터 모델 변환
            List<CharacterData> characterDataList = filteredCharacters
                .Select(character => new CharacterData
                {
                    Name = character.Name,
                    Level = character.Level,
                    ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString()),
                    JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString()),
                    WeaponSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => character.EquippedWeapon != null && sprite.name == character.EquippedWeapon.Name),
                    ArmorSprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => character.EquippedArmor != null && sprite.name == character.EquippedArmor.Name) ,
                    CharacterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == character.Name),
                    Grade = character.Grade
                }).ToList();

            KnightsView.SlotUpdate(characterDataList, filteredCharacters);
        }
        
    }

}
