using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainUI;


public class MainView : MonoBehaviour, IMainView
{
    MainPresenter mainPresenter;

    CameraDrag cameraDrag;
    [SerializeField] Text NickName;
    [SerializeField] Text Progress;
    [SerializeField] Text Bread;
    [SerializeField] Text Exp;
    [SerializeField] Text Gold;

    private void Awake()
    {
        mainPresenter = new MainPresenter(this);

        cameraDrag = Camera.main.GetComponent<CameraDrag>();
        NickName.text= DataModel.instance.myInfo.nickName;

        mainPresenter.UserInfoUpdate();
    }
    public void initialize() //OnEnable은 오브젝트 풀에서 첫 생성시 발동 되므로 위험하니 initilize()를 사용한다
    {
        cameraDrag.enabled = true; //카메라 Drag On
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
