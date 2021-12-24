using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    public event Action<int> stopMove;

    [SerializeField] private List<DiceSide> sides;
    [SerializeField] private Rigidbody rigidbody;
    public bool isPushed;

    private void Awake()
    {
        if (rigidbody == null)
            rigidbody = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(isPushed)
        {
            CheckIfStop();
        }
    }

    void CheckIfStop()
    {
        if (rigidbody.velocity.magnitude < 0.1f&& GetDiceNum()!=0)
        {
            if(stopMove!=null)
            stopMove.Invoke(GetDiceNum());
            isPushed = false;
        }
    }

    public void Push(Vector3 v)
    {
        rigidbody.velocity = v;
        isPushed = true;
    }

    public int GetDiceNum()
    {
        foreach(DiceSide side in sides)
        {
            if(side.isDown)
            {
                return side.sideNum;
            }
        }
        return 0;
    }
}
