using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterInfoUI
{
    public class InfoData //���߿� �̸� �ٲ����
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
            myInfo = DataModel.instance.myInfo; //���� info�Ҵ�(Gold�� Exp)
            return myInfo;
        }

        public void UpdateView(Character character)
        {

            //Character�� �����ϰ��ִ� ���� �� �� ��������Ʈ �Ҵ�
            infoData.WeaponSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => character.EquippedWeapon != null && sprite.name == character.EquippedWeapon.Name);
            infoData.ArmorSprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => character.EquippedArmor != null && sprite.name == character.EquippedArmor.Name);
            infoData.CharacterSprite = DataModel.instance.CharacterSprite.FirstOrDefault(sprite => sprite.name == character.Name);

            //Character������ �Ӽ��� �°� �̸��� ����,�Ӽ� ��������Ʈ �Ҵ�
            switch (character.Job)
            {
                case JobType.Warrior:
                    infoData.JobName = "����";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                case JobType.Mage:
                    infoData.JobName = "������";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                case JobType.Archer:
                    infoData.JobName = "�ü�";
                    infoData.JobSprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == character.Job.ToString());
                    break;
                default:
                    Debug.LogError("Invalid JobType :" + character.Job);
                    break;
            }

            switch (character.Element)
            {
                case Element.Fire:
                    infoData.ElementName = "��";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Earth:
                    infoData.ElementName = "����";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Water:
                    infoData.ElementName = "��";
                    infoData.ElementSprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == character.Element.ToString());
                    break;
                case Element.Wind:
                    infoData.ElementName = "�ٶ�";
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

