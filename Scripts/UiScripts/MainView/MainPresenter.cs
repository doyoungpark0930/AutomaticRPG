using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainUI
{
    public class MainPresenter
    {
        IMainView mainView;

        public MainPresenter(IMainView view)
        {
            mainView = view;
            EventManager.OnUserInfoUpdated += mainView.MainViewMyInfoUpdate;
        }
        public void UserInfoUpdate()
        {
            EventManager.UserInfoUpdated(DataModel.instance.myInfo);
        }
    }
}

