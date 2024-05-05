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

    [SerializeField] Button GradeSortButton;
    [SerializeField] Image GradeSortButtonImage;
    private bool GradeSortBright = false;

    [SerializeField] Button LevelSortButton;
    [SerializeField] Image LevelSortButtonImage;
    private bool LevelSortBright = false;

    // �Ӽ� ��ư�� �̹��� �迭 ����
    [SerializeField] private Button ElementAllButton;
    [SerializeField] private Image ElementAllButtonImages;
    private bool ElementAllButtonBright = true;

    [SerializeField] private Button[] ElementButtons; //Fire, Earth, Water, Wind ��
    [SerializeField] private Image[] ElementButtonImages;
    private bool[] ElementButtonBright = new bool[4]; //false�� 4�� �迭 �ʱ�ȭ

    // ���� ��ư�� �̹��� �迭 ����
    [SerializeField] private Button JobAllButton;
    [SerializeField] private Image JobAllButtonImages;
    private bool JobAllButtonBright = true;

    [SerializeField] private Button[] JobButtons; // Warrior, Mage, Archer �� 
    [SerializeField] private Image[] JobButtonImages;
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

        ElementAllButton.onClick.AddListener(() =>
        {
            ElementAllButtonBright = !ElementAllButtonBright;
            UpdateElementFilter(true);
            ToggleButtonBrightness(ElementAllButtonImages, ref ElementAllButtonBright);
        });

        for (int i = 0; i < ElementButtons.Length; i++)
        {
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
            ElementButtons[localIndex].onClick.AddListener(() =>
            {
                ElementButtonBright[localIndex] = !ElementButtonBright[localIndex];
                UpdateElementFilter(false);
                ToggleButtonBrightness(ElementButtonImages[localIndex], ref ElementButtonBright[localIndex]);
            });
        }

        JobAllButton.onClick.AddListener(() =>
        {
            JobAllButtonBright = !JobAllButtonBright;
            UpdateJobFilter(true);
            ToggleButtonBrightness(JobAllButtonImages, ref JobAllButtonBright);
        });

        for (int i = 0; i < JobButtons.Length; i++)
        {
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
            JobButtons[localIndex].onClick.AddListener(() =>
            {
                JobButtonBright[localIndex] = !JobButtonBright[localIndex];
                UpdateJobFilter(false);
                ToggleButtonBrightness(JobButtonImages[localIndex], ref JobButtonBright[localIndex]);
            });
        }


        LevelSortButton.onClick.AddListener(() =>
        {
            LevelSortBright = !LevelSortBright;
            knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
            ToggleButtonBrightness(LevelSortButtonImage, ref LevelSortBright);

        });
        GradeSortButton.onClick.AddListener(() =>
        {
            GradeSortBright = !GradeSortBright;
            knightsPresenter.UpdateByFlags(LevelSortBright, GradeSortBright, ElementsFilter, JobsFilter);
            ToggleButtonBrightness(GradeSortButtonImage, ref GradeSortBright);

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

            var weaponImage = Slot[i].transform.GetChild(6).GetComponent<Image>();  //ĳ���� ���� ����
            weaponImage.sprite = characterData.WeaponSprite;
            weaponImage.color = weaponImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

            var armorImage = Slot[i].transform.GetChild(7).GetComponent<Image>(); //ĳ���� ���� ��
            armorImage.sprite = characterData.ArmorSprite;
            armorImage.color = armorImage.sprite == null ? new Color(1, 1, 1, 0) : Color.white;

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
    private void ToggleButtonBrightness(Image buttonImage, ref bool isBright)
    {
        Color currentColor = buttonImage.color;
        //���� �� ����
        currentColor.a = isBright ? 1f : 0.5f;
        buttonImage.color = currentColor;
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
                    ToggleButtonBrightness(ElementButtonImages[i], ref ElementButtonBright[i]);
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
                    ToggleButtonBrightness(JobButtonImages[i], ref JobButtonBright[i]);
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
                ToggleButtonBrightness(JobAllButtonImages, ref JobAllButtonBright);
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
