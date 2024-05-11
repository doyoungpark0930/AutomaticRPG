/*
 * MainPresenter.cs
 * 작성자 :박도영
 * 작성일자: 2024/05/11
 * 코드 설명 : 메인 UI의 MVP패턴 중 Presenter에 해당한다. DataModel은 Presenter가 관리한다.
 *
 * email : eofud0930@naver.com
 * phone : 010-9889-1281
 *
 */
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

