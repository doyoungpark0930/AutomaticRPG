using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataChangeTest : MonoBehaviour
{
   
    public void OnClickButton()
    {
        DataManager.instance.MyCharacterList[1].Level++;
    }
}
