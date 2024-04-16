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
        List<Character> myCharacterList; //Model�� ���� ���� MyCharacterList
        List<CharacterData> characterDataList;  //KnightsView�� �Ѱ��� �����͸���Ʈ

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
                    //���� �ؿ� ��� ������ KnightsView�� ���ؼ� �����ϰ�, KnightsView�� slot setadtive�� ��� ó������. ������ slot�� ���� �󸶳� ���� Ȯ��
                    WeaponSprite = character.EquippedWeapon.Name != "" ? DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == character.EquippedWeapon.Name) : null,
                    ArmorSprite = character.EquippedArmor.Name != "" ? DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == character.EquippedArmor.Name) : null
                };
                characterDataList.Add(characterData);
            }
            KnightsView.SlotUpdate(characterDataList);


        }

    }

}
