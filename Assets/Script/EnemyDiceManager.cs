using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiceManager : DiceManager
{
    [SerializeField] float xDist=2f, yDist=2f;
    RaycastHit hit;
    private void Start()
    {
        line.StartTrajectory(startLineTransform, flyTime);
    }
    private void Update()
    {
        switch (stage)
        {
            case Stage.waitMDown:
                {
                        stage = Stage.waitMUp;
                        line.gameObject.SetActive(true);
                     
                }
                break;

            case Stage.waitMUp:
                MakeEnemyStep();
                line.SetVelocity(cv);
                stage = Stage.waitDice;
                line.isActive = false;
                PushDice();
                break;

        }

        if(stage!= Stage.pause)
        line.DrawCursor(hit);
        line.SetVelocity(cv);
    }

 void  MakeEnemyStep()
    {
        float x = Random.Range(-xDist, xDist);
        float y = Random.Range(-yDist, yDist);
        hit = new RaycastHit();
        hit.point = new Vector3(x, 0.2f, y);
        cv = CalculateVelocity (new Vector3(x, 0.2f, y), startLineTransform.position, flyTime);
    }

}
