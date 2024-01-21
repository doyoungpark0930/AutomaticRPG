using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public void GetZoomOut()
    {
        StartCoroutine(ZoomOut());
    }

    public void GetZoomIn()
    {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ZoomOut() 
    {
        Vector3 startPos= gameObject.transform.position;
        Vector3 targetPos = startPos + new Vector3(0, 10, 0);

        while(gameObject.transform.position.y < targetPos.y)
        {
            gameObject.transform.Translate(Vector3.up * 40 * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.transform.position = targetPos;
    }
    IEnumerator ZoomIn()
    {
        Vector3 startPos = gameObject.transform.position;
        Vector3 targetPos = startPos - new Vector3(0, 10, 0);

        while (gameObject.transform.position.y > targetPos.y)
        {
            gameObject.transform.Translate(Vector3.down * 40 * Time.deltaTime, Space.World);
            yield return null;
        }

        gameObject.transform.position = targetPos;
    }
}
