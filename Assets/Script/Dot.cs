using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Dot : MonoBehaviour
{

    Dot enemyTarget=null;
    [SerializeField] float speed = 1f;

    public enum Side
    {
        player,
        enemy
    }
    public Side side;

    private void OnEnable()
    {
        GameManager.gm.AddNewDot(this);
    }

    private void OnDestroy()
    {
     GameManager.gm.RemoveDot(this);
    }

    private void Update()
    {
        if (enemyTarget != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, enemyTarget.transform.position, speed*Time.deltaTime);
        }
        else
        {
            enemyTarget = GameManager.gm.GetTargetDot(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Dot d;
        if (d = collision.gameObject.GetComponent<Dot>())
        {
            if (d.side != side)
            {
                try
                {

                    if (side == Side.player)
                    {
                        Destroy(d.gameObject);
                        Destroy(this.gameObject);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("" + e);
                }
            }
        }

        if (collision.gameObject.GetComponent<Dice>())
        {
            Destroy(this.gameObject);
        }
    }

}