using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitBar : MonoBehaviour
{
    [SerializeField] Image bar;
    float speed = 0.1f;

    private void Awake()
    {
        bar.enabled = false;
    }

    public void StartTimer(float time)
    {
        bar.enabled = true;
        bar.fillAmount = 0;
        StartCoroutine(Timer(time));
    }

    IEnumerator Timer(float time)
    {
        float step = 1f / (time /speed);

        while(bar.fillAmount<1f)
        {
            bar.fillAmount += step;
            yield return new WaitForSeconds(speed);
        }
        bar.enabled = false;
    }
}
