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
        print(currentIndex + characterInfo.Name);
    }

    public void initialize() //onEnable대체
    {
        print(currentIndex + characterInfo.Name);
    }

    public void OnClickLeft()
    {
        if (currentIndex > 0) // 왼쪽에 캐릭터가 있는지 확인
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
        if (currentIndex < characterList.Count -1) // 오른쪽에 캐릭터가 있는지 확인
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
