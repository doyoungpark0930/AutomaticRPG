using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoView : MonoBehaviour
{
    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;

    [SerializeField] Button LeftButton;
    [SerializeField] Button RightButton;

    [SerializeField] GameObject[] stars; //Grade이미지 배열

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

    //LevelUp버튼
    [SerializeField] Button LevelUpButton;
    //LevelUp에 요구되는 Exp와 Gold
    [SerializeField] Text NeededExp;
    [SerializeField] Text NeededGold;

    private void Awake()
    {
        LeftButton.onClick.AddListener(OnLeftButtonClick);
        RightButton.onClick.AddListener(OnRightButtonClick);
        LevelUpButton.onClick.AddListener(OnLevelUpButtonClick);

        EventManager.OnUserInfoUpdated += CharacterViewMyInfoUpdate;
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
        UpdateCharacterInfo();
    }
    private void UpdateCharacterInfo()
    {
        UpdateGradeImage();
        GradeColorUpdate();
        ElementAndJobUpdate();
        NameText.text = characterInfo.Name;
        LevelText.text = characterInfo.Level.ToString();
        WeaponImage.sprite = DataModel.instance.WeaponSprite.FirstOrDefault(sprite => sprite.name == characterInfo.EquippedWeapon.Name);
        WeaponImage.color = WeaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

        ArmorImage.sprite = DataModel.instance.ArmorSprite.FirstOrDefault(sprite => sprite.name == characterInfo.EquippedArmor.Name);
        ArmorImage.color = ArmorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;


        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.AttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

        EventManager.UserInfoUpdated();

    }

    public void initialize() //onEnable대체
    {
        SetCharacterInfo(characterList, currentIndex);
    }
    private void CharacterViewMyInfoUpdate()
    {
        Exp.text = DataModel.instance.myInfo.Exp.ToString();
        Gold.text = DataModel.instance.myInfo.Gold.ToString();

        NeededExp.text = ((characterInfo.Level / 10 + 1) * 14).ToString();
        NeededGold.text = (100 + characterInfo.Level * 10).ToString();

    }



    private void UpdateGradeImage() //grade에 따른 별 중앙 정렬
    {
        // 먼저 모든 별을 비활성화
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        // 그런 다음, 등급에 따라 별을 활성화하고 중앙에 배치.
        int activeStars = characterInfo.grade; 
        float starWidth = stars[0].GetComponent<RectTransform>().sizeDelta.x; // 하나의 별 너비.
        float totalWidth = starWidth * activeStars; // 활성화된 별의 총 너비.
        float startPosition = -(totalWidth / 2) + (starWidth / 2); // 왼쪽에서부터 별을 배치

        for (int i = 0; i < activeStars; i++)
        {
            stars[i].SetActive(true);
            // 각 별의 수평 위치(x)를 조정
            stars[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition + (i * starWidth), stars[i].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
    
    private void GradeColorUpdate()
    {
        switch (characterInfo.grade)
        {
            case 1:
                GradeText.text = "일반";
                GradeColor.color = new Color(100f/255f, 100f/255f, 100f/255f);
                break;
            case 2:
                GradeText.text = "에픽";
                GradeColor.color = new Color(25f/255f, 50f/255f, 75f/255f);
                break;
            case 3:
                GradeText.text = "유니크";
                GradeColor.color = new Color(120f / 255f, 50f / 255f, 140f / 255f);
                break;
            default:
                Debug.LogError("Invalid grade value :" + characterInfo.grade);
                break;


        }
    }

    private void ElementAndJobUpdate()
    {
        switch(characterInfo.Job)
        {
            case JobType.Warrior:
                JobText.text = "전사";
                JobImage.sprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Job.ToString());
                break;
            case JobType.Mage:
                JobText.text = "마법사";
                JobImage.sprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Job.ToString());
                break;
            case JobType.Archer:
                JobText.text = "궁수";
                JobImage.sprite = DataModel.instance.JobSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Job.ToString());
                break;
            default:
                Debug.LogError("Invalid JobType :" + characterInfo.Job);
                break;
        }
        switch(characterInfo.Element)
        {
            case Element.Fire:
                ElementText.text = "불";
                ElementImage.sprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Element.ToString());
                break;
            case Element.Earth:
                ElementText.text = "대지";
                ElementImage.sprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Element.ToString());
                break;
            case Element.Water:
                ElementText.text = "물";
                ElementImage.sprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Element.ToString());
                break;
            case Element.Wind:
                ElementText.text = "바람";
                ElementImage.sprite = DataModel.instance.ElementSprite.FirstOrDefault(sprite => sprite.name == characterInfo.Element.ToString());
                break;
            default:
                break;
        }
    }
    public void OnLeftButtonClick()
    {
        if (currentIndex > 0) // 왼쪽에 캐릭터가 있는지 확인
        {
            currentIndex--;
        }
        else
        {
            currentIndex = characterList.Count - 1;
        }
        SetCharacterInfo(characterList, currentIndex);

    }

    public void OnRightButtonClick()
    {
        if (currentIndex < characterList.Count -1) // 오른쪽에 캐릭터가 있는지 확인
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        SetCharacterInfo(characterList, currentIndex);
    }


    private void OnLevelUp()
    {
        //스텟Up
        characterInfo.Level++;
        characterInfo.AttackPower += 1;
        characterInfo.Defense += 1;
        characterInfo.Health += 10;

        //스텟 텍스트 초기화
        LevelText.text = characterInfo.Level.ToString();
        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.AttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

        EventManager.UserInfoUpdated();
    }

    public void OnLevelUpButtonClick()
    {
      //MyInfo에서 차감
      //저장
       if(DataModel.instance.myInfo.Gold>= int.Parse(NeededGold.text) && DataModel.instance.myInfo.Exp >= int.Parse(NeededExp.text))
        {
            DataModel.instance.myInfo.Gold -= int.Parse(NeededGold.text);
            DataModel.instance.myInfo.Exp -= int.Parse(NeededExp.text);

            OnLevelUp();
            DataModel.instance.OnSaveRequested?.Invoke();

        }
        else
        {
            Debug.Log("Gold or Exp is not enough");
        }
    }
    public void ToKnightsView()
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();

        UiPool.ReturnObject(gameObject);

    }

}
