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

    [SerializeField] Button GradeButton;
    [SerializeField] Image GradeButtonImage;
    private bool GradeBright = false;
    [SerializeField] Button LevelButton;
    [SerializeField] Image LevelButtonImage;
    private bool LevelBright = false;

    // ��ư�� �̹��� �迭 ����
    [SerializeField] private Button ElementAllButton;
    [SerializeField] private Image ElementAllButtonImages;
    private bool ElementAllButtonBright = true;

    [SerializeField] private Button[] ElementButtons; //Fire, Earth, Water, Wind ��
    [SerializeField] private Image[] ElementButtonImages;
    private bool[] ElementButtonBright = new bool[4]; //false�� 4�� �迭 �ʱ�ȭ

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
            int localIndex = i; //���ٽ� �ܺ� ���� ���� ������
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
    public void initialize() //onEnable��ü
    {
        cameraDrag.enabled = false; //ī�޶� Drag Off
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
    }


   

    public void SlotUpdate(List<CharacterData> CharacterList) //Slot�� �̹������� �ִ´�
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

            var gradeParent = Slot[i].transform.GetChild(8); // Grade ������Ʈ ����
            for (int g = 0; g < gradeParent.childCount; g++)
            {
                var gradeImage = gradeParent.GetChild(g).GetComponent<Image>();
                gradeImage.gameObject.SetActive(g < characterData.Grade); // Grade ���� ���� �̹��� Ȱ��ȭ
            }
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
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
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
