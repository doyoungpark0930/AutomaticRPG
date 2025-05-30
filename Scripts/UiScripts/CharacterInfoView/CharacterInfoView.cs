/*
 * CharacterInfoView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 캐릭터 정보UI의 MVP패턴 중 View에 해당한다. CharacterInfoPresenter에 필요한 데이터들을 받고, 캐릭터 정보를 나타내준다
 *             캐릭터 정보 및 스텟을 나타내고, 레벨업 기능 및 장비UI(EquipmentView)로 넘어가는 기능이 있다
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
using UnityEngine.UI;
using CharacterInfoUI;



public class CharacterInfoView : MonoBehaviour, ICharacterInfoView
{
    CharacterInfoPresenter characterInfoPresenter;

    private List<Character> characterList;
    private Character characterInfo;
    private int currentIndex;

    private MyInfo myInfo;

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
    [SerializeField] Image CharacterImage;
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

    //LevelUp버튼
    [SerializeField] Button LevelUpButton;

    //LevelUp에 요구되는 Exp와 Gold
    [SerializeField] Text NeededExp;
    [SerializeField] Text NeededGold;

    private void Awake()
    {
        characterInfoPresenter = new CharacterInfoPresenter(this);
        myInfo = characterInfoPresenter.GetMyInfo();
        EventManager.OnUserInfoUpdated += CharacterViewMyInfoUpdate; //userInfo데이터 업데이트 이벤트매니저에 할당

        LeftButton.onClick.AddListener(OnLeftButtonClick);
        RightButton.onClick.AddListener(OnRightButtonClick);
        LevelUpButton.onClick.AddListener(characterInfoPresenter.OnLevelUpButtonClick);
        WeaponButton.onClick.AddListener(ToEquipmentView);

    }

    public void initialize() //onEnable대체
    {
        SetCharacterInfo(characterList, currentIndex);
    }

    public void CharacterViewMyInfoUpdate(MyInfo myInfo) //UserInfo업데이트하는 매니저에 할당할 것
    {
        Exp.text = myInfo.Exp.ToString();
        Gold.text = myInfo.Gold.ToString();

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
        //등급 이미지 및 배너 색 업데이트
        UpdateGradeImage(); 
        GradeColorUpdate();

        //Element와 Job 업데이트
        JobText.text = infodata.JobName;
        JobImage.sprite = infodata.JobSprite;
        ElementText.text = infodata.ElementName;
        ElementImage.sprite = infodata.ElementSprite;

        //캐릭터 이름 및 레벨 업데이트
        NameText.text = characterInfo.Name;
        LevelText.text = characterInfo.Level.ToString();

        //이미지 업데이트
        CharacterImage.sprite = infodata.CharacterSprite;

        WeaponImage.sprite = infodata.WeaponSprite;
        WeaponImage.color = WeaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

        ArmorImage.sprite = infodata.ArmorSprite;
        ArmorImage.color = ArmorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

        //캐릭터 스텟 업데이트
        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.TotalAttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

        //레벨업에 필요한 Exp와 Gold텍스트 초기화
        NeededExp.text = ((characterInfo.Level / 10 + 1) * 14).ToString();
        NeededGold.text = (100 + characterInfo.Level * 10).ToString();

        //UserInfo 이벤트매니저 구동
        EventManager.UserInfoUpdated(myInfo);

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
    } //grade에 따른 배너 색 변경



    public void LevelUp(MyInfo myInfo)
    {
       if(myInfo.Gold>= int.Parse(NeededGold.text) && myInfo.Exp >= int.Parse(NeededExp.text)) //레벨 업당 Gold와 Exp가 충분하다면
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
        //레벨 1업당 스텟 상승
        characterInfo.Level++;
        characterInfo.BaseAttackPower += 1; //이렇게하면안됨.AttackPower구할 때 다합산한 값이 나와버림
        characterInfo.Defense += 1;
        characterInfo.Health += 10;

        //스텟 텍스트 초기화
        LevelText.text = characterInfo.Level.ToString();
        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.TotalAttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

        //레벨업에 필요한 Exp와 Gold텍스트 초기화
        NeededExp.text = ((characterInfo.Level / 10 + 1) * 14).ToString();
        NeededGold.text = (100 + characterInfo.Level * 10).ToString();
    }

    public void OnLeftButtonClick() //왼쪽 캐릭터 정보로 이동
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

    public void OnRightButtonClick() //오른쪽 캐릭터 정보로 이동
    {
        if (currentIndex < characterList.Count - 1) // 오른쪽에 캐릭터가 있는지 확인
        {
            currentIndex++;
        }
        else
        {
            currentIndex = 0;
        }
        SetCharacterInfo(characterList, currentIndex);
    }

    public void ToKnightsView() //KnightsVIew로 이동
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();

        UiPool.ReturnObject(gameObject);

    }

    public void ToEquipmentView()
    {
        var equipmentView = UiPool.GetObject("EquipmentView");
        equipmentView.GetComponent<EquipmentView>().SetCharacterInfo(characterList, currentIndex);
        equipmentView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        equipmentView.GetComponent<EquipmentView>().initialize();

        UiPool.ReturnObject(gameObject);
    }
}
