using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterInfoUI
{
    public class InfoData //나중에 이름 바꿔야함
    {
        public Sprite WeaponSprite;
        public Sprite ArmorSprite;
        public Sprite JobSprite;
        public Sprite ElementSprite;
        public string JobName;
        public string ElementName;

        public MyInfo myinfo;
    }
    public class CharacterInfoPresenter
    {
        ICharacterInfoView characterInfoView;

        InfoData infoData = new InfoData();
        public CharacterInfoPresenter(ICharacterInfoView view)
        {
            characterInfoView = view;
            EventManager.OnUserInfoUpdated += characterInfoView.CharacterViewMyInfoUpdate; //userInfo데이터 업데이트 이벤트매니저에 할당
        }


        public void UpdateView(Character character)
        {
            infoData.myinfo = DataModel.instance.myInfo; //유저 info할당(Gold및 Exp)

            //Character가 장착하고있는 무기 및 방어구 스프라이트 할당
            infoData.WeaponSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => character.EquippedWeapon != null && sprite.name == character.EquippedWeapon.Name);
            infoData.ArmorSprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => character.EquippedArmor != null && sprite.name == character.EquippedArmor.Name);

            //Character직업과 속성에 맞게 이름과 직업,속성 스프라이트 할당
            switch (character.Job)
            {
                case JobType.Warrior:
                    infoData.JobName = "전사";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                case JobType.Mage:
                    infoData.JobName = "마법사";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                case JobType.Archer:
                    infoData.JobName = "궁수";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                default:
                    Debug.LogError("Invalid JobType :" + character.Job);
                    break;
            }

            switch (character.Element)
            {
                case Element.Fire:
                    infoData.ElementName = "불";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Earth:
                    infoData.ElementName = "대지";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Water:
                    infoData.ElementName = "물";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Wind:
                    infoData.ElementName = "바람";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                default:
                    break;
            }
            characterInfoView.UpdateCharacterInfo(infoData);
        }

        public void OnLevelUpButtonClick()
        {
            characterInfoView.LevelUp(DataModel.instance.myInfo);
            DataModel.instance.OnSaveRequested?.Invoke();
        }
    }

}

