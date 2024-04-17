using System;

public class EventManager
{
    public static event Action OnUserInfoUpdated;

    public static void UserInfoUpdated()
    {
        OnUserInfoUpdated?.Invoke();
    }
}