using CharacterInfoUI;
public interface ICharacterInfoView
{
    public void UpdateCharacterInfo(InfoData infodata);
    public void CharacterViewMyInfoUpdate(MyInfo myinfo);

    public void LevelUp(MyInfo myinfo);
}