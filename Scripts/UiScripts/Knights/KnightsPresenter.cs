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
    }
    public class KnightsPresenter
    {

       

        IKnightsView KnightsView;
        List<Character> myCharacterList; //Model로 부터 받을 MyCharacterList
        List<CharacterData> characterDataList;  //KnightsView로 넘겨줄 데이터리스트

        public KnightsPresenter(IKnightsView knightsView)
        {
            KnightsView = knightsView;
        }

        public void ViewUpdate()
        {
            myCharacterList = DataModel.instance.MyCharacterList;

            List<CharacterData> characterDataList = new List<CharacterData>();
            foreach (var character in myCharacterList)
            {
                var characterData = new CharacterData
                {
                    Name = character.Name,
                    Level = character.Level,
                    ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString()),
                    JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString()),
                    //여기 밑에 어떻게 들어가는지 KnightsView와 비교해서 공부하고, KnightsView에 slot setadtive는 어떻게 처리될지. 갯수는 slot에 비해 얼마나 들어갈지 확인
                    WeaponSprite = character.EquippedWeapon.Name != "" ? DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == character.EquippedWeapon.Name) : null,
                    ArmorSprite = character.EquippedArmor.Name != "" ? DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == character.EquippedArmor.Name) : null
                };
                characterDataList.Add(characterData);
            }
            KnightsView.SlotUpdate(characterDataList);


        }

    }

}
