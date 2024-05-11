/*
 * MainView.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���� UI�� MVP���� �� View�� �ش��Ѵ�. DataModel�� Presenter�� �����Ѵ�. �ٸ� View�� �Ѿ�� ����� �ִ�.
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainUI;


public class MainView : MonoBehaviour, IMainView
{
    MainPresenter mainPresenter;
    private MyInfo myInfo;
    CameraDrag cameraDrag;
    [SerializeField] Text NickName;
    [SerializeField] Text Progress;
    [SerializeField] Text Bread;
    [SerializeField] Text Exp;
    [SerializeField] Text Gold;

    private void Awake()
    {
        mainPresenter = new MainPresenter(this);
        myInfo = mainPresenter.GetMyInfo();
        EventManager.OnUserInfoUpdated += MainViewMyInfoUpdate;
        EventManager.UserInfoUpdated(myInfo);
        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        NickName.text= DataModel.instance.myInfo.nickName;
;
    }
    public void initialize() //OnEnable�� ������Ʈ Ǯ���� ù ������ �ߵ� �ǹǷ� �����ϴ� initilize()�� ����Ѵ�
    {
        cameraDrag.enabled = true; //ī�޶� Drag On
    }
    public void MainViewMyInfoUpdate(MyInfo myInfo)
    {
        Progress.text = myInfo.Progress;
        Bread.text = myInfo.Bread.ToString();
        Exp.text = myInfo.Exp.ToString();
        Gold.text =myInfo.Gold.ToString();

    }
    public void ToTerritoryManagement()
    {
        var territoryManagement = UiPool.GetObject("TerritoryManagement");
        territoryManagement.transform.position = Vector2.zero;
        territoryManagement.GetComponent<TerritoryManagement>().initialize();
        UiPool.ReturnObject(gameObject);

    }

    public void ToKnightsView()
    {
        var knightsView = UiPool.GetObject("KnightsView");
        knightsView.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        knightsView.GetComponent<KnightsView>().initialize();
        UiPool.ReturnObject(gameObject);
    }

    public void ToAdvantureView()
    {
        Debug.Log("Click OnAdvantureButton");
    }
}
