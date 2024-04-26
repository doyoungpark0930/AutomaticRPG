using KnightsUI;
using System;
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

    [SerializeField] GameObject[] stars; //Grade�̹��� �迭

    private void Awake()
    {
        LeftButton.onClick.AddListener(OnClickLeft);
        RightButton.onClick.AddListener(OnClickRight);
    }
    public void SetCharacterInfo(List<Character> characterList)
    {
        this.characterList = characterList;
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
        UpdateGradeImage(characterInfo.grade);
        print(currentIndex + characterInfo.Name);
    }

    private void UpdateGradeImage(int grade) //grade�� ���� �� �߾� ����
    {
        // ���� ��� ���� ��Ȱ��ȭ
        foreach (var star in stars)
        {
            star.SetActive(false);
        }

        // �׷� ����, ��޿� ���� ���� Ȱ��ȭ�ϰ� �߾ӿ� ��ġ.
        int activeStars = grade; 
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
    public void initialize() //onEnable��ü
    {
        print(currentIndex + characterInfo.Name);
    }

    public void OnClickLeft()
    {
        if (currentIndex > 0) // ���ʿ� ĳ���Ͱ� �ִ��� Ȯ��
        {
            currentIndex--;
            SetCharacterInfo(characterList, currentIndex);
        }
        else
        {
            currentIndex = characterList.Count - 1;
            SetCharacterInfo(characterList, currentIndex);
        }
    }

    public void OnClickRight()
    {
        if (currentIndex < characterList.Count -1) // �����ʿ� ĳ���Ͱ� �ִ��� Ȯ��
        {
            currentIndex++;
            SetCharacterInfo(characterList, currentIndex);
        }
        else
        {
            currentIndex = 0;
            SetCharacterInfo(characterList, currentIndex);
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
