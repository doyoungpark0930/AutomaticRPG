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
        List<Character> myCharacterList; //Model�� ���� ���� MyCharacterList
        bool LevelSort = false; 
        bool GradeSort = false;

        public KnightsPresenter(IKnightsView knightsView)
        {
            KnightsView = knightsView;
        }

        // ��ư ��� ���¿� ���� ����
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
            // ���⿡ ���� ������ �߰�
            if (GradeSort && LevelSort)
            {
                // ������� ���� �����ϰ�, �� ������ ������ ����
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Grade)
                    .ThenByDescending(c => c.Level)
                    .ToList();
            }
            else if (GradeSort)
            {
                // ��޸� ����Ͽ� ����
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Grade)
                    .ToList();
            }
            else if (LevelSort)
            {
                // ������ ����Ͽ� ����
                characterDataList = characterDataList
                    .OrderByDescending(c => c.Level)
                    .ToList();
            }

            KnightsView.SlotUpdate(characterDataList);
        }


    
    }

}
