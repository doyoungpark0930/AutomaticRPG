/*
 * KnightsView.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 기사단 UI의 MVP패턴 중 View에 해당한다. KnightsPresenter에서 필요한 데이터들을 받고, 모든 캐릭터 정보를 나타내준다
 *             또한 캐릭터를 등급,레벨에 따라 정렬할 수 있고, 직업 및 캐릭터 속성에 따라 필터링 할 수 있다.
 * 
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KnightsUI; //KnightsPresenter 스크립트에 정의되어있음


public class KnightsView : MonoBehaviour, IKnightsView
{
    CameraDrag cameraDrag;

    KnightsPresenter knightsPresenter;

    [SerializeField] GameObject[] Slot;

    [SerializeField] GameObject GradeSortButton;
    private bool GradeSortBright = false;

    [SerializeField] GameObject LevelSortButton;
    private bool LevelSortBright = false;

    // 속성 버튼과 이미지 배열 정의
    [SerializeField] private GameObject ElementAllButton;
    private bool ElementAllButtonBright = true;

    [SerializeField] private GameObject[] ElementButtons; //Fire, Earth, Water, Wind 순
    private bool[] ElementButtonBright = new bool[4]; //false로 4개 배열 초기화

    // 직업 버튼과 이미지 배열 정의
    [SerializeField] private GameObject JobAllButton;
    private bool JobAllButtonBright = true;

    [SerializeField] private GameObject[] JobButtons; // Warrior, Mage, Archer 순 
    private bool[] JobButtonBright = new bool[3]; //false로 3개 배열 초기화

    HashSet<Element> ElementsFilter = new HashSet<Element> { Element.Fire, Element.Earth, Element.Water, Element.Wind };
    HashSet<JobType> JobsFilter = new HashSet<JobType> { JobType.Warrior, JobType.Mage, JobType.Archer };

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        knightsPresenter = new KnightsPresenter(this);

    }
    void Start()
    {
        knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
        AddEventListener();
    }

    public void initialize() //onEnable대체
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
    }

    private void AddEventListener()
    {

        ElementAllButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            ElementAllButtonBright = !ElementAllButtonBright;
            UpdateElementFilter(true);
            ToggleButtonBrightness(ElementAllButton, ref ElementAllButtonBright);
        });

        for (int i = 0; i < ElementButtons.Length; i++)
        {
            int localIndex = i; //람다식 외부 변수 참조 방지용
            ElementButtons[localIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                ElementButtonBright[localIndex] = !ElementButtonBright[localIndex];
                UpdateElementFilter(false);
                ToggleButtonBrightness(ElementButtons[localIndex], ref ElementButtonBright[localIndex]);
            });
        }

        JobAllButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            JobAllButtonBright = !JobAllButtonBright;
            UpdateJobFilter(true);
            ToggleButtonBrightness(JobAllButton, ref JobAllButtonBright);
        });

        for (int i = 0; i < JobButtons.Length; i++)
        {
            int localIndex = i; //람다식 외부 변수 참조 방지용
            JobButtons[localIndex].GetComponent<Button>().onClick.AddListener(() =>
            {
                JobButtonBright[localIndex] = !JobButtonBright[localIndex];
                UpdateJobFilter(false);
                ToggleButtonBrightness(JobButtons[localIndex], ref JobButtonBright[localIndex]);
            });
        }


        LevelSortButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            LevelSortBright = !LevelSortBright;
            knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
            ToggleButtonBrightness(LevelSortButton, ref LevelSortBright);

        });
        GradeSortButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            GradeSortBright = !GradeSortBright;
            knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
            ToggleButtonBrightness(GradeSortButton, ref GradeSortBright);

        });
    }


   

    public void SlotUpdate(List<CharacterData> CharacterList, List<Character> CharacterInfo) //Slot에 이미지들을 넣는다
    {
        foreach(GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var characterData = CharacterList[i];
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(1).GetComponent<Text>().text = characterData.Name; //캐릭터 이름
            Slot[i].transform.GetChild(3).GetComponent<Text>().text = characterData.Level.ToString(); //캐릭터 레벨
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = characterData.ElementSprite; //캐릭터 속성
            Slot[i].transform.GetChild(5).GetComponent<Image>().sprite = characterData.JobSprite; //캐릭터 직업
            Slot[i].transform.GetChild(8).GetComponent<Image>().sprite = characterData.CharacterSprite; //캐릭터 이미지

            var weaponObject = Slot[i].transform.GetChild(6).gameObject; //캐릭터 장착 무기
            if (characterData.WeaponSprite != null)
            {
                weaponObject.SetActive(true);
                weaponObject.GetComponent<Image>().sprite = characterData.WeaponSprite;
            }
            else weaponObject.SetActive(false);

            var armorObject= Slot[i].transform.GetChild(7).gameObject; //캐릭터 장착 방어구
            if (characterData.ArmorSprite != null)
            {
                armorObject.SetActive(true);
                armorObject.GetComponent<Image>().sprite = characterData.ArmorSprite;
            }
            else armorObject.SetActive(false);

            //캐릭터 Grade에 따른 BackGround색 결정
            Image imageComponent = Slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
            switch (characterData.Grade)
            {
                case 1:
                    // 회색
                    imageComponent.color = new Color(91f / 255f, 90f / 255f, 100f / 255f, 110f / 255f);
                    break;
                case 2:
                    // 파란색
                    imageComponent.color = new Color(36f / 255f, 55f / 255f, 189f / 255f, 110f / 255f);
                    break;
                case 3:
                    // 보라색
                    imageComponent.color = new Color(140f / 255f, 34f / 255f, 145f / 255f, 110f / 255f);
                    break;
                default:
                    break;
            }

            //캐릭터 Grade에 따라 별 개수 나타냄
            var gradeParent = Slot[i].transform.GetChild(9); // Grade 오브젝트 접근
            for (int g = 0; g < gradeParent.childCount; g++)
            {
                var gradeImage = gradeParent.GetChild(g).GetComponent<Image>();
                gradeImage.gameObject.SetActive(g < characterData.Grade); // Grade 값에 따라 이미지 활성화
            }

            //각 Slot버튼에 이벤트리스너 할당
            int localIndex = i; //람다식 외부 변수 참조 방지용
            var button = Slot[localIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => ToCharacterInfoView(CharacterInfo , localIndex) ); //해당 캐릭터 정보를 넘김
        }
        

    }


    // 버튼의 밝기를 토글하는 메서드
    private void ToggleButtonBrightness(GameObject button, ref bool isBright)
    {
        Color currentColor = button.GetComponent<Image>().color;
        //투명 값 및 테두리 조절
        if(isBright)
        {
            //밝게
            currentColor.a = 1f;
            //테두리 보이게
            if (button.transform.childCount > 0) button.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        else
        {
            //투명하게
            currentColor.a = 0.5f;
            //테두리 안 보이게
            if (button.transform.childCount > 0) button.transform.GetChild(0).gameObject.SetActive(false);
        }
        currentColor.a = isBright ? 1f : 0.5f;
        button.GetComponent<Image>().color = currentColor;
    }

    private void UpdateElementFilter(bool isAllButtonClicked)
    {
        if(isAllButtonClicked) //All버튼으로 클릭 됐다면
        {
            if(ElementAllButtonBright) 
            {
                //모든 속성 보이도록
                ElementsFilter = new HashSet<Element> { Element.Fire, Element.Earth, Element.Water, Element.Wind };

                //속성 버튼 모두 어둡게
                ElementButtonBright = new bool[4];
                for (int i = 0; i < ElementButtonBright.Length; i++)
                {
                    ToggleButtonBrightness(ElementButtons[i], ref ElementButtonBright[i]);
                }
            }
            else
            {
                ElementsFilter.Clear();
            }
        }
        else //속성으로 클릭 됐다면
        {
            //All버튼 눌러진 상태에서 눌렀을 때
            if(ElementAllButtonBright)
            {
                //All버튼 어둡게하고, 눌러진 속성 버튼에 맞게 해쉬 값 할당
                ElementAllButtonBright = false;
                ToggleButtonBrightness(ElementAllButton, ref ElementAllButtonBright);
                ElementsFilter.Clear();
                for (int i = 0; i < ElementButtonBright.Length; i++)
                {
                    if (ElementButtonBright[i])
                    {
                        ElementsFilter.Add((Element)i);
                    }
                }
            }
            else //All버튼 안 눌러진 상태에서 (속성버튼 1개 이상 눌러진 상태일 때) 눌렀을 때 
            {
                //속성 버튼에 맞게 해쉬 값 할당
                ElementsFilter.Clear();
                for (int i = 0; i < ElementButtonBright.Length; i++)
                {
                    if (ElementButtonBright[i])
                    {
                        ElementsFilter.Add((Element)i);
                    }
                }
            }

        }

        // 필터 적용
        knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
    }

    private void UpdateJobFilter(bool isAllButtonClicked)
    {
        if (isAllButtonClicked) //All버튼으로 클릭 됐다면
        {
            if (JobAllButtonBright)
            {
                //모든 직업 타입 보이도록
                JobsFilter = new HashSet<JobType> { JobType.Warrior, JobType.Mage, JobType.Archer };

                //직업 버튼 모두 어둡게
                JobButtonBright = new bool[3];
                for (int i = 0; i < JobButtonBright.Length; i++)
                {
                    ToggleButtonBrightness(JobButtons[i], ref JobButtonBright[i]);
                }
            }
            else
            {
                JobsFilter.Clear();
            }
        }
        else //직업버튼으로 클릭 됐다면
        {
            //All버튼 눌러진 상태에서 눌렀을 때
            if (JobAllButtonBright)
            {
                //All버튼 어둡게하고, 눌러진 직업 버튼에 맞게 해쉬 값 할당
                JobAllButtonBright = false;
                ToggleButtonBrightness(JobAllButton, ref JobAllButtonBright);
                JobsFilter.Clear();
                for (int i = 0; i < JobButtonBright.Length; i++)
                {
                    if (JobButtonBright[i])
                    {
                        JobsFilter.Add((JobType)i);
                    }
                }
            }
            else //All버튼 안 눌러진 상태에서 (직업버튼 1개 이상 눌러진 상태일 때) 눌렀을 때 
            {
                //직업 버튼에 맞게 해쉬 값 할당
                JobsFilter.Clear();
                for (int i = 0; i < JobButtonBright.Length; i++)
                {
                    if (JobButtonBright[i])
                    {
                        JobsFilter.Add((JobType)i);
                    }
                }
            }

        }

        // 필터 적용
        knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
    }

    public void ToCharacterInfoView(List<Character> characterList, int currentIndex)
    {
        var characterInfoView = UiPool.GetObject("CharacterInfoView");
        characterInfoView.GetComponent<CharacterInfoView>().SetCharacterInfo(characterList, currentIndex);
        characterInfoView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        characterInfoView.GetComponent<CharacterInfoView>().initialize();
        UiPool.ReturnObject(gameObject);
    }

    public void OnExitButtonClick()
    {

        //MainUi 다시 띄우기
        var mainUi = UiPool.GetObject("MainView");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainView>().initialize();

        //본인 destory
        UiPool.ReturnObject(gameObject);
        
    }


}
