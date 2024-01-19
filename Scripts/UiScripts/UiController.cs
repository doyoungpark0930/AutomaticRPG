using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    [SerializeField] GameObject MainUi;

    private void Awake()
    {
        Instantiate(MainUi,GameObject.Find("Canvas").transform);
    }


}
