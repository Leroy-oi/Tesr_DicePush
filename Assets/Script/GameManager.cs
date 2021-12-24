using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public List<Dot> playerDots= new List<Dot>();
    public List<Dot> enemyDots = new List<Dot>();

    private void Awake()
    {
        if (gm == null)
            gm = this;
    }

    public void AddNewDot(Dot dot)
    {
        switch (dot.side)
        {
            case Dot.Side.player:
                playerDots.Add(dot);
                break;
            case Dot.Side.enemy:
                enemyDots.Add(dot);
                break;
        }
    }
    public void RemoveDot(Dot dot)
    {
        switch (dot.side)
        {
            case Dot.Side.player:
                playerDots.Remove(dot);
                break;
            case Dot.Side.enemy:
                enemyDots.Remove(dot);
                break;
        }
    }


    public Dot GetTargetDot(Dot dot)
    {
        switch (dot.side)
        {
            case Dot.Side.player:
                return ClosestDot(dot, enemyDots);
            case Dot.Side.enemy:
                return ClosestDot(dot, playerDots);
        }
        return null;
    }

    Dot ClosestDot(Dot looker, List<Dot> target)
    {

        float dist = 100f;
        Dot dot = null;
        if(target.Count>0)
        foreach (Dot d in target)
        {
            if ((looker.transform.position - d.transform.position).magnitude < dist)
            {
                dist = (looker.transform.position - d.transform.position).magnitude;
                dot = d;
            }
        }
        return dot;
    }
}