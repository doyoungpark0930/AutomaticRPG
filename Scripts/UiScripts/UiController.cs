using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    //[SerializeField] GameObject MainUi;

    private void Start()
    {
        //Instantiate(MainUi,GameObject.Find("Canvas").transform);
        UiPool.GetGameObject("MainUi");
    }


}
