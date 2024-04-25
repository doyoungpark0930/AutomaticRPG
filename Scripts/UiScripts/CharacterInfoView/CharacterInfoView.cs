using KnightsUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfoView : MonoBehaviour
{
    Character characterInfo;

    public void SetCharacterInfo(Character characterInfo)
    {
        this.characterInfo = characterInfo;
    }

    public void initialize() //onEnable¥Î√º
    {
        print(characterInfo.Name);
    }

    public void ToKnightsView()
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();

        UiPool.ReturnObject(gameObject);

    }

}
