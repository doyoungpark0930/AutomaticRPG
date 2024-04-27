using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Text GradeText;
    [SerializeField] Image GradeColor;
    [SerializeField] Text NameText;
    [SerializeField] Text ElementText;
    [SerializeField] Image ElementImage;
    [SerializeField] Text JobText;
    [SerializeField] Image JobImage;
    [SerializeField] Text LevelText;
    [SerializeField] Text CombatPowerText;
    [SerializeField] Text AttackPowerText;
    [SerializeField] Text HealthText;
    [SerializeField] Text DefenseText;
    [SerializeField] Text AttackSpeedText;

    private void Awake()
    {
        LeftButton.onClick.AddListener(OnClickLeft);
        RightButton.onClick.AddListener(OnClickRight);
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
        UpdateGradeImage(characterInfo.grade);
        GradeColorUpdate(characterInfo.grade);
        NameText.text = characterInfo.Name;
        ElementText.text = characterInfo.Element.ToString();
        JobText.text = characterInfo.Job.ToString();
        LevelText.text = characterInfo.Level.ToString();

        CombatPowerText.text = characterInfo.CombatPower.ToString();
        AttackPowerText.text = characterInfo.AttackPower.ToString();
        HealthText.text = characterInfo.Health.ToString();
        DefenseText.text = characterInfo.Defense.ToString();
        AttackSpeedText.text = characterInfo.AttackSpeed.ToString();

    }

    public void initialize() //onEnable대체
    {
        SetCharacterInfo(characterList, currentIndex);
    }
    private void UpdateGradeImage(int grade) //grade에 따른 별 중앙 정렬
    {
        // 먼저 모든 별을 비활성화
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        // 그런 다음, 등급에 따라 별을 활성화하고 중앙에 배치.
        int activeStars = grade; 
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
    
    private void GradeColorUpdate(int grade)
    {
        switch (grade)
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
                Debug.LogError("Invalid grade value :" + grade);
                break;


        }
    }

    public void OnClickLeft()
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

    public void OnClickRight()
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

    public void ToKnightsView()
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();

        UiPool.ReturnObject(gameObject);

    }

}
