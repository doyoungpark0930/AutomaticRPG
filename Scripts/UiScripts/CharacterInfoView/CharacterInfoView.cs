using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CharacterInfoUI;



public class CharacterInfoView : MonoBehaviour, ICharacterInfoView
{
    CharacterInfoPresenter characterInfoPresenter;

    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;

    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;

    [SerializeField] GameObject[] stars; //Grade�̹��� �迭

    //CharacterInfo
    [SerializeField] Text GradeText;
    [SerializeField] Image GradeColor;
    [SerializeField] Text NameText;
    [SerializeField] Text ElementText;
    [SerializeField] Image ElementImage;
    [SerializeField] Text JobText;
    [SerializeField] Image JobImage;
    [SerializeField] Image WeaponImage;
    [SerializeField] Image ArmorImage;
    [SerializeField] Button WeaponButton;
    [SerializeField] Button ArmorButton;

    //CharacterStats
    [SerializeField] Text LevelText;
    [SerializeField] Text CombatPowerText;
    [SerializeField] Text AttackPowerText;
    [SerializeField] Text HealthText;
    [SerializeField] Text DefenseText;
    [SerializeField] Text AttackSpeedText;

    //MyInfo
    [SerializeField] Text Exp;
    [SerializeField] Text Gold;

    //LevelUp��ư
    [SerializeField] Button LevelUpButton;

    //LevelUp�� �䱸�Ǵ� Exp�� Gold
    [SerializeField] Text NeededExp;
    [SerializeField] Text NeededGold;

    private void Awake()
    {
        characterInfoPresenter = new CharacterInfoPresenter(this);

        LeftButton.onClick.AddListener(OnLeftButtonClick);
        RightButton.onClick.AddListener(OnRightButtonClick);
        LevelUpButton.onClick.AddListener(characterInfoPresenter.OnLevelUpButtonClick);
        WeaponButton.onClick.AddListener(ToEquipmentView);

    }

    public void initialize() //onEnable��ü
    {
        SetCharacterInfo(characterList, currentIndex);
    }

    public void CharacterViewMyInfoUpdate(MyInfo myInfo) //UserInfo������Ʈ�ϴ� �Ŵ����� �Ҵ��� ��
    {
        Exp.text = myInfo.Exp.ToString();
        Gold.text = myInfo.Gold.ToString();

        NeededExp.text = ((characterInfo.Level / 10 + 1) * 14).ToString();
        NeededGold.text = (100 + characterInfo.Level * 10).ToString();

    }


    public void SetCharacterInfo(List<Character> characterList, int index)
    {
        if (index < 0 || index >= characterList.Count)
        {
            Debug.LogError("Invalid character index");
            return;
        }
        this.characterList = characterList;
        this.characterInfo = characterList[index];
        this.currentIndex = index;
        characterInfoPresenter.UpdateView(characterInfo);
    }


    public void UpdateCharacterInfo(InfoData infodata)
    {
        //��� �̹��� �� ��� �� ������Ʈ
        UpdateGradeImage(); 
        GradeColorUpdate();

        //Element�� Job ������Ʈ
        JobText.text = infodata.JobName;
        JobImage.sprite = infodata.JobSprite;
        ElementText.text = infodata.ElementName;
        ElementImage.sprite = infodata.ElementSprite;

        //ĳ���� �̸� �� ���� ������Ʈ
        NameText.text = characterInfo.Name;
        LevelText.text = characterInfo.Level.ToString();

        //���� �̹����� �� �̹��� ������Ʈ
        WeaponImage.sprite = infodata.WeaponSprite;
        WeaponImage.color = WeaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

        ArmorImage.sprite = infodata.ArmorSprite;
        ArmorImage.color = ArmorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

        //ĳ���� ���� ������Ʈ
        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.AttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

        //UserInfo �̺�Ʈ�Ŵ��� ����
        EventManager.UserInfoUpdated(infodata.myinfo);

    }


    private void UpdateGradeImage() //grade�� ���� �� �߾� ����
    {
        // ���� ��� ���� ��Ȱ��ȭ
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        // �׷� ����, ��޿� ���� ���� Ȱ��ȭ�ϰ� �߾ӿ� ��ġ.
        int activeStars = characterInfo.grade; 
        float starWidth = stars[0].GetComponent<RectTransform>().sizeDelta.x; // �ϳ��� �� �ʺ�.
        float totalWidth = starWidth * activeStars; // Ȱ��ȭ�� ���� �� �ʺ�.
        float startPosition = -(totalWidth / 2) + (starWidth / 2); // ���ʿ������� ���� ��ġ

        for (int i = 0; i < activeStars; i++)
        {
            stars[i].SetActive(true);
            // �� ���� ���� ��ġ(x)�� ����
            stars[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition + (i * starWidth), stars[i].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
    
    private void GradeColorUpdate()
    {
        switch (characterInfo.grade)
        {
            case 1:
                GradeText.text = "�Ϲ�";
                GradeColor.color = new Color(100f/255f, 100f/255f, 100f/255f);
                break;
            case 2:
                GradeText.text = "����";
                GradeColor.color = new Color(25f/255f, 50f/255f, 75f/255f);
                break;
            case 3:
                GradeText.text = "����ũ";
                GradeColor.color = new Color(120f / 255f, 50f / 255f, 140f / 255f);
                break;
            default:
                Debug.LogError("Invalid grade value :" + characterInfo.grade);
                break;


        }
    } //grade�� ���� ��� �� ����



    public void LevelUp(MyInfo myInfo)
    {
       if(myInfo.Gold>= int.Parse(NeededGold.text) && myInfo.Exp >= int.Parse(NeededExp.text)) //���� ���� Gold�� Exp�� ����ϴٸ�
        {
            myInfo.Gold -= int.Parse(NeededGold.text);
            myInfo.Exp -= int.Parse(NeededExp.text);

            OnLevelUp();
            EventManager.UserInfoUpdated(myInfo);

        }
        else
        {
            Debug.Log("Gold or Exp is not enough");
        }
    }

    private void OnLevelUp()
    {
        //���� 1���� ���� ���
        characterInfo.Level++;
        characterInfo.AttackPower += 1;
        characterInfo.Defense += 1;
        characterInfo.Health += 10;

        //���� �ؽ�Ʈ �ʱ�ȭ
        LevelText.text = characterInfo.Level.ToString();
        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.AttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();
    }

    public void OnLeftButtonClick() //���� ĳ���� ������ �̵�
    {
        if (currentIndex > 0) // ���ʿ� ĳ���Ͱ� �ִ��� Ȯ��
        {
            currentIndex--;
        }
        else
        {
            currentIndex = characterList.Count - 1;
        }
        SetCharacterInfo(characterList, currentIndex);

    }

    public void OnRightButtonClick() //������ ĳ���� ������ �̵�
    {
        if (currentIndex < characterList.Count - 1) // �����ʿ� ĳ���Ͱ� �ִ��� Ȯ��
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        SetCharacterInfo(characterList, currentIndex);
    }

    public void ToKnightsView() //KnightsVIew�� �̵�
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();

        UiPool.ReturnObject(gameObject);

    }

    public void ToEquipmentView()
    {
        var equipmentView = UiPool.GetObject("EquipmentView");
        equipmentView.GetComponent<CharacterInfoView>().SetCharacterInfo(characterList, currentIndex);
        equipmentView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        equipmentView.GetComponent<EquipmentView>().initialize();

        UiPool.ReturnObject(gameObject);
    }
}
