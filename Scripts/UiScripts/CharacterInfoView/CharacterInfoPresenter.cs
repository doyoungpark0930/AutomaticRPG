using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class InfoData //나중에 이름 바꿔야함
{
    public Sprite WeaponSprite;
    public Sprite ArmorSprite;
    public Sprite JobSprite;
    public Sprite ElementSprite;
    public string JobName;
    public string ElementName;
}
public class CharacterInfoPresenter
{
    ICharacterInfoView characterInfoView;

    InfoData infoData = new InfoData();
    public CharacterInfoPresenter(ICharacterInfoView view)
    {
        characterInfoView = view;
    }

    public void UpdateView(Character character)
    {
        infoData.WeaponSprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == character.EquippedWeapon.Name);
        infoData.ArmorSprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == character.EquippedArmor.Name);

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
}

