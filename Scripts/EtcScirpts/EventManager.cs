using System;

public class EventManager
{
    public static event Action<MyInfo> OnUserInfoUpdated;

    public static void UserInfoUpdated(MyInfo myinfo)
    {
        OnUserInfoUpdated?.Invoke(myinfo);
    }
}