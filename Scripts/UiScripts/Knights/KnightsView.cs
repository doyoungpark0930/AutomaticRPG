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

    [SerializeField] Button GradeButton;
    [SerializeField] Image GradeButtonImage;
    private bool GradeBright = false;
    [SerializeField] Button LevelButton;
    [SerializeField] Image LevelButtonImage;
    private bool LevelBright = false;

    // 버튼과 이미지 배열 정의
    [SerializeField] private Button ElementAllButton;
    [SerializeField] private Image ElementAllButtonImages;
    private bool ElementAllButtonBright = true;

    [SerializeField] private Button[] ElementButtons; //Fire, Earth, Water, Wind 순
    [SerializeField] private Image[] ElementButtonImages;
    private bool[] ElementButtonBright = new bool[4]; //false로 4개 배열 초기화

    HashSet<Element> ElementsFilter = new HashSet<Element> { Element.Fire, Element.Earth, Element.Water, Element.Wind };

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        knightsPresenter = new KnightsPresenter(this);

    }
    void Start()
    {
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);

        ElementAllButton.onClick.AddListener(() =>
        {
            ElementAllButtonBright = !ElementAllButtonBright;
            UpdateElementFilter(true) ;
            ToggleButtonBrightness(ElementAllButtonImages, ref ElementAllButtonBright);
        });

        for (int i = 0; i < ElementButtons.Length; i++)
        {
            int localIndex = i; //람다식 외부 변수 참조 방지용
            ElementButtons[localIndex].onClick.AddListener(() =>
            {
                ElementButtonBright[localIndex] = !ElementButtonBright[localIndex];
                UpdateElementFilter(false);
                ToggleButtonBrightness(ElementButtonImages[localIndex], ref ElementButtonBright[localIndex]);
            });
        }


        LevelButton.onClick.AddListener(() =>
        {
            LevelBright = !LevelBright;
            knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
            ToggleButtonBrightness(LevelButtonImage, ref LevelBright);
         
        });
        GradeButton.onClick.AddListener(() =>
        {
            GradeBright = !GradeBright;
            knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
            ToggleButtonBrightness(GradeButtonImage, ref GradeBright);

        });
    }
    public void initialize() //onEnable대체
    {
        cameraDrag.enabled = false; //카메라 Drag Off
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
    }


   

    public void SlotUpdate(List<CharacterData> CharacterList) //Slot에 이미지들을 넣는다
    {
        foreach(GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var characterData = CharacterList[i];
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(0).GetComponent<Text>().text = characterData.Name;
            Slot[i].transform.GetChild(2).GetComponent<Text>().text = characterData.Level.ToString();
            Slot[i].transform.GetChild(3).GetComponent<Image>().sprite = characterData.ElementSprite;
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = characterData.JobSprite;

            var weaponImage = Slot[i].transform.GetChild(5).GetComponent<Image>(); 
            weaponImage.sprite = characterData.WeaponSprite;
            weaponImage.color = weaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

            var armorImage = Slot[i].transform.GetChild(6).GetComponent<Image>();
            armorImage.sprite = characterData.ArmorSprite;
            armorImage.color = armorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

            var gradeParent = Slot[i].transform.GetChild(8); // Grade 오브젝트 접근
            for (int g = 0; g < gradeParent.childCount; g++)
            {
                var gradeImage = gradeParent.GetChild(g).GetComponent<Image>();
                gradeImage.gameObject.SetActive(g < characterData.Grade); // Grade 값에 따라 이미지 활성화
            }
        }
        

    }


    // 버튼의 밝기를 토글하는 메서드
    private void ToggleButtonBrightness(Image buttonImage, ref bool isBright)
    {
        Color currentColor = buttonImage.color;
        //투명 값 조절
        currentColor.a = isBright ? 1f : 0.5f;
        buttonImage.color = currentColor;
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
                    ToggleButtonBrightness(ElementButtonImages[i], ref ElementButtonBright[i]);
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
                ToggleButtonBrightness(ElementAllButtonImages, ref ElementAllButtonBright);
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
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
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
