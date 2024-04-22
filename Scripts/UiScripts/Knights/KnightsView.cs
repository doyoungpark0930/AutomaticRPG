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
    [SerializeField] Image GradeImage;
    private bool GradeBright = false;
    [SerializeField] Button LevelButton;
    [SerializeField] Image LevelImage;
    private bool LevelBright = false;

    HashSet<Element> ElementsFilter = new HashSet<Element> { Element.Fire, Element.Water };

    void Awake()
    {
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        knightsPresenter = new KnightsPresenter(this);

    }
    void Start()
    {
        knightsPresenter.UpdateByFlags(LevelBright, GradeBright,ElementsFilter);

        LevelButton.onClick.AddListener(() =>
        {
            LevelBright = !LevelBright;
            knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
            ToggleButtonBrightness(LevelImage, ref LevelBright);
         
        });
        GradeButton.onClick.AddListener(() =>
        {
            GradeBright = !GradeBright;
            knightsPresenter.UpdateByFlags(LevelBright, GradeBright, ElementsFilter);
            ToggleButtonBrightness(GradeImage, ref GradeBright);

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
