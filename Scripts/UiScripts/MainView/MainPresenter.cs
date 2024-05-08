using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MainUI
{
    public class MainPresenter
    {
        IMainView mainView;
        MyInfo myInfo;
        public MainPresenter(IMainView view)
        {
            mainView = view;
        }

        public MyInfo GetMyInfo()
        {
            myInfo = DataModel.instance.myInfo;
            return myInfo;
        }
    }
}

