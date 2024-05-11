/*
 * KnightsView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���� UI�� MVP���� �� View�� �ش��Ѵ�. KnightsPresenter���� �ʿ��� �����͵��� �ް�, ��� ĳ���� ������ ��Ÿ���ش�
 *             ���� ĳ���͸� ���,������ ���� ������ �� �ְ�, ���� �� ĳ���� �Ӽ��� ���� ���͸� �� �� �ִ�.
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
using KnightsUI; //KnightsPresenter ��ũ��Ʈ�� ���ǵǾ�����


public class KnightsView : MonoBehaviour, IKnightsView
{
    CameraDrag cameraDrag;

    KnightsPresenter knightsPresenter;

    [SerializeField] GameObject[] Slot;

    [SerializeField] GameObject GradeSortButton;
    private bool GradeSortBright = false;

    [SerializeField] GameObject LevelSortButton;
    private bool LevelSortBright = false;

    // �Ӽ� ��ư�� �̹��� �迭 ����
    [SerializeField] private GameObject ElementAllButton;
    private bool ElementAllButtonBright = true;

    [SerializeField] private GameObject[] ElementButtons; //Fire, Earth, Water, Wind ��
    private bool[] ElementButtonBright = new bool[4]; //false�� 4�� �迭 �ʱ�ȭ

    // ���� ��ư�� �̹��� �迭 ����
    [SerializeField] private GameObject JobAllButton;
    private bool JobAllButtonBright = true;

    [SerializeField] private GameObject[] JobButtons; // Warrior, Mage, Archer �� 
    private bool[] JobButtonBright = new bool[3]; //false�� 3�� �迭 �ʱ�ȭ

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

    public void initialize() //onEnable��ü
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
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
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
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
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
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


   

    public void SlotUpdate(List<CharacterData> CharacterList, List<Character> CharacterInfo) //Slot�� �̹������� �ִ´�
    {
        foreach(GameObject slot in Slot)
        {
            slot.SetActive(false);
        }
        for (int i = 0; i < CharacterList.Count; i++)
        {
            var characterData = CharacterList[i];
            Slot[i].SetActive(true);
            Slot[i].transform.GetChild(1).GetComponent<Text>().text = characterData.Name; //ĳ���� �̸�
            Slot[i].transform.GetChild(3).GetComponent<Text>().text = characterData.Level.ToString(); //ĳ���� ����
            Slot[i].transform.GetChild(4).GetComponent<Image>().sprite = characterData.ElementSprite; //ĳ���� �Ӽ�
            Slot[i].transform.GetChild(5).GetComponent<Image>().sprite = characterData.JobSprite; //ĳ���� ����
            Slot[i].transform.GetChild(8).GetComponent<Image>().sprite = characterData.CharacterSprite; //ĳ���� �̹���

            var weaponObject = Slot[i].transform.GetChild(6).gameObject; //ĳ���� ���� ����
            if (characterData.WeaponSprite != null)
            {
                weaponObject.SetActive(true);
                weaponObject.GetComponent<Image>().sprite = characterData.WeaponSprite;
            }
            else weaponObject.SetActive(false);

            var armorObject= Slot[i].transform.GetChild(7).gameObject; //ĳ���� ���� ��
            if (characterData.ArmorSprite != null)
            {
                armorObject.SetActive(true);
                armorObject.GetComponent<Image>().sprite = characterData.ArmorSprite;
            }
            else armorObject.SetActive(false);

            //ĳ���� Grade�� ���� BackGround�� ����
            Image imageComponent = Slot[i].transform.GetChild(0).GetChild(0).GetComponent<Image>();
            switch (characterData.Grade)
            {
                case 1:
                    // ȸ��
                    imageComponent.color = new Color(91f / 255f, 90f / 255f, 100f / 255f, 110f / 255f);
                    break;
                case 2:
                    // �Ķ���
                    imageComponent.color = new Color(36f / 255f, 55f / 255f, 189f / 255f, 110f / 255f);
                    break;
                case 3:
                    // �����
                    imageComponent.color = new Color(140f / 255f, 34f / 255f, 145f / 255f, 110f / 255f);
                    break;
                default:
                    break;
            }

            //ĳ���� Grade�� ���� �� ���� ��Ÿ��
            var gradeParent = Slot[i].transform.GetChild(9); // Grade ������Ʈ ����
            for (int g = 0; g < gradeParent.childCount; g++)
            {
                var gradeImage = gradeParent.GetChild(g).GetComponent<Image>();
                gradeImage.gameObject.SetActive(g < characterData.Grade); // Grade ���� ���� �̹��� Ȱ��ȭ
            }

            //�� Slot��ư�� �̺�Ʈ������ �Ҵ�
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
            var button = Slot[localIndex].GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => ToCharacterInfoView(CharacterInfo , localIndex) ); //�ش� ĳ���� ������ �ѱ�
        }
        

    }


    // ��ư�� ��⸦ ����ϴ� �޼���
    private void ToggleButtonBrightness(GameObject button, ref bool isBright)
    {
        Color currentColor = button.GetComponent<Image>().color;
        //���� �� �� �׵θ� ����
        if(isBright)
        {
            //���
            currentColor.a = 1f;
            //�׵θ� ���̰�
            if (button.transform.childCount > 0) button.transform.GetChild(0).gameObject.SetActive(true);
            
        }
        else
        {
            //�����ϰ�
            currentColor.a = 0.5f;
            //�׵θ� �� ���̰�
            if (button.transform.childCount > 0) button.transform.GetChild(0).gameObject.SetActive(false);
        }
        currentColor.a = isBright ? 1f : 0.5f;
        button.GetComponent<Image>().color = currentColor;
    }

    private void UpdateElementFilter(bool isAllButtonClicked)
    {
        if(isAllButtonClicked) //All��ư���� Ŭ�� �ƴٸ�
        {
            if(ElementAllButtonBright) 
            {
                //��� �Ӽ� ���̵���
                ElementsFilter = new HashSet<Element> { Element.Fire, Element.Earth, Element.Water, Element.Wind };

                //�Ӽ� ��ư ��� ��Ӱ�
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
        else //�Ӽ����� Ŭ�� �ƴٸ�
        {
            //All��ư ������ ���¿��� ������ ��
            if(ElementAllButtonBright)
            {
                //All��ư ��Ӱ��ϰ�, ������ �Ӽ� ��ư�� �°� �ؽ� �� �Ҵ�
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
            else //All��ư �� ������ ���¿��� (�Ӽ���ư 1�� �̻� ������ ������ ��) ������ �� 
            {
                //�Ӽ� ��ư�� �°� �ؽ� �� �Ҵ�
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

        // ���� ����
        knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
    }

    private void UpdateJobFilter(bool isAllButtonClicked)
    {
        if (isAllButtonClicked) //All��ư���� Ŭ�� �ƴٸ�
        {
            if (JobAllButtonBright)
            {
                //��� ���� Ÿ�� ���̵���
                JobsFilter = new HashSet<JobType> { JobType.Warrior, JobType.Mage, JobType.Archer };

                //���� ��ư ��� ��Ӱ�
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
        else //������ư���� Ŭ�� �ƴٸ�
        {
            //All��ư ������ ���¿��� ������ ��
            if (JobAllButtonBright)
            {
                //All��ư ��Ӱ��ϰ�, ������ ���� ��ư�� �°� �ؽ� �� �Ҵ�
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
            else //All��ư �� ������ ���¿��� (������ư 1�� �̻� ������ ������ ��) ������ �� 
            {
                //���� ��ư�� �°� �ؽ� �� �Ҵ�
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

        // ���� ����
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

        //MainUi �ٽ� ����
        var mainUi = UiPool.GetObject("MainView");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mainUi.GetComponent<MainView>().initialize();

        //���� destory
        UiPool.ReturnObject(gameObject);
        
    }


}
