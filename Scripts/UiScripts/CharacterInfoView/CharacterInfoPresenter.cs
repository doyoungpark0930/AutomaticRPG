/*
 * CharacterInfoPresenter.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 캐릭터 정보UI의 MVP패턴 중 Presenter에 해당한다. CharacterInfoVIew에 필요한 데이터들을 DataModel에서 읽고 정제하여 전달해준다
 *             CharacterInfoUI 네임스페이스를 활용하여 데이터를 캐릭터 정보 UI에서만 사용할 수 있게 모듈화하였다.
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

namespace CharacterInfoUI
{
    public class InfoData 
    {
        public Sprite WeaponSprite;
        public Sprite ArmorSprite;
        public Sprite JobSprite;
        public Sprite ElementSprite;
        public Sprite CharacterSprite;
        public string JobName;
        public string ElementName;

    }
    public class CharacterInfoPresenter
    {
        ICharacterInfoView characterInfoView;

        InfoData infoData = new InfoData();
        MyInfo myInfo;

        public CharacterInfoPresenter(ICharacterInfoView view)
        {
            characterInfoView = view;
        }
        public MyInfo GetMyInfo()
        {
            myInfo = DataModel.instance.myInfo; //유저 info할당(Gold및 Exp)
            return myInfo;
        }

        public void UpdateView(Character character)
        {

            //Character가 장착하고있는 무기 및 방어구 스프라이트 할당
            infoData.WeaponSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => character.EquippedWeapon != null && sprite.name == character.EquippedWeapon.Name);
            infoData.ArmorSprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => character.EquippedArmor != null && sprite.name == character.EquippedArmor.Name);
            infoData.CharacterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == character.Name);

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

