/*
 * UiController.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 게임 실행 후, UIPool에서 MainView를 불러낸다
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{

    private void Start()
    {
        var mainUi = UiPool.GetObject("MainView");
        mainUi.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


}
