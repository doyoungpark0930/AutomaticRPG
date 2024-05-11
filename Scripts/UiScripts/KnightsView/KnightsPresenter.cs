/*
 * KnightsView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���� UI�� MVP���� �� Presenter�� �ش��Ѵ�. KnightsView���� �ʿ��� �����͵��� DataModel�κ��� �޾� �����Ͽ� �������ش�.
 *             Linq�� �̿��Ͽ� ���,������ ���� ���� �� ĳ���� �Ӽ�, ������ ���� ���͸� �ϴ� ����� �ִ�. 
 *             KnightsUI ���ӽ����̽��� Ȱ���Ͽ� �����͸� ���� UI������ ����� �� �ְ� ���ȭ�Ͽ���.
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
        List<Character> myCharacterList; //Model�� ���� ���� MyCharacterList
        bool LevelFilter = false; 
        bool GradeFilter = false;
        HashSet<Element> ElementsFilter;
        HashSet<JobType> JobsFilter;

        public KnightsPresenter(IKnightsView knightsView)
        {
            KnightsView = knightsView;
        }

        // ��ư ��� ���¿� ���� ����
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
            // Element�� Job�� ���� ���͸�
            List<Character> filteredCharacters = myCharacterList
                    .Where(character =>
                        (ElementsFilter.Count == 4 || ElementsFilter.Contains(character.Element)) && (JobsFilter.Count == 3 || JobsFilter.Contains(character.Job))  )
                    .ToList();

            // Grade�� Level������ ����
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

            // View���� ������ �� ��ȯ
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
