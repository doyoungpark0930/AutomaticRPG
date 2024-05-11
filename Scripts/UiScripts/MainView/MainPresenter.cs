/*
 * MainPresenter.cs
 * �ۼ��� :�ڵ���
 * �ۼ�����: 2024/05/11
 * �ڵ� ���� : ���� UI�� MVP���� �� Presenter�� �ش��Ѵ�. DataModel�� Presenter�� �����Ѵ�.
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

