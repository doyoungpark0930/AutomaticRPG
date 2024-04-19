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
        public int Grade;
    }
    public class KnightsPresenter
    {
        IKnightsView KnightsView;
        List<Character> myCharacterList; //Model로 부터 받을 MyCharacterList
        bool LevelSort = false; 
        bool GradeSort = false;

        public KnightsPresenter(IKnightsView knightsView)
        {
            KnightsView = knightsView;
        }

        // 버튼 토글 상태에 따라 정렬
        public void UpdateByFlags(bool level, bool grade)
        {
            LevelSort = level;
            GradeSort = grade;
            SortAndUpdateView();
        }
        private List<CharacterData> DataOrganize() 
        {
            myCharacterList = DataModel.instance.MyCharacterList;
            List<CharacterData> characterDataList = myCharacterList
                .Select(character => new CharacterData
                {
                    Name = character.Name,
                    Level = character.Level,
                    ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString()),
                    JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString()),
                    WeaponSprite = character.EquippedWeapon.Name != "" ? DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == character.EquippedWeapon.Name) : null,
                    ArmorSprite = character.EquippedArmor.Name != "" ? DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == character.EquippedArmor.Name) : null,
                    Grade = character.Grade
                }).ToList();

            return characterDataList;
        }
        private void SortAndUpdateView()
        {

            List<CharacterData> characterDataList = DataOrganize();
            // 여기에 정렬 로직을 추가
            if (GradeSort && LevelSort)
            {
                // 등급으로 먼저 정렬하고, 그 다음에 레벨로 정렬
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Grade)
                    .ThenByDescending(c => c.Level)
                    .ToList();
            }
            else if (GradeSort)
            {
                // 등급만 고려하여 정렬
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Grade)
                    .ToList();
            }
            else if (LevelSort)
            {
                // 레벨만 고려하여 정렬
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Level)
                    .ToList();
            }

            KnightsView.SlotUpdate(characterDataList);
        }


    
    }

}
