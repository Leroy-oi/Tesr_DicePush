using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSide : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    public int sideNum=0;
    public bool isDown;

    private void OnTriggerEnter(Collider other)
    {
        if ((layer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            isDown = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((layer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            isDown = false;
        }
    }
}
