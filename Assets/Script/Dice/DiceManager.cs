using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiceManager : MonoBehaviour
{
    [SerializeField] Dice dicePrefab;
    Dice diceNow;

    [SerializeField] protected TrajectoryRenderer line;
    [SerializeField] protected Transform startLineTransform;
    [SerializeField] protected Vector3 startDicePos;
    Vector3 targetPos;
    protected Vector3 cv = new Vector3(0,0,0);
    protected Camera camera;
    [SerializeField] protected LayerMask layer;

    [SerializeField] protected float waitTime = 3f;
    [SerializeField] protected float flyTime = 1f;
    [SerializeField] protected Stage stage;

    [SerializeField] WaitBar waitBar;

    [Header ("Создание сфер")]
    [SerializeField] protected Dot dotPref;
    [SerializeField] protected float dist=1f;

public enum Stage
    {
        waitMDown,
        waitMUp,
        waitDice,
        pause
    }

    void Awake()
    {
        camera = Camera.main;
        line.StopTrajectory();
        PlaceDice();
    }

    private void Update()
    {

        switch (stage)
        {
            case Stage.waitMDown:
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        stage = Stage.waitMUp;
                        line.gameObject.SetActive(true);
                        line.StartTrajectory(startLineTransform, flyTime);
                    }

                }
                break;

            case Stage.waitMUp:
                if (Input.GetMouseButtonUp(0))
                {
                    stage = Stage.waitDice;
                    line.isActive = false;
                    PushDice();
                    break;
                }
                line.SetVelocity(CalculateMousePos());
                break;
        }
    }

    protected  Vector3 CalculateMousePos()
    {
        targetPos = Input.mousePosition; //??
        Ray camRay = camera.ScreenPointToRay(targetPos);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            line.DrawCursor(hit);
          cv  = CalculateVelocity(hit.point, startLineTransform.position, flyTime);
        }
        return cv;
    }

    void PlaceDice()
    {
        if (diceNow != null)
            Destroy(diceNow.gameObject);
        diceNow = Instantiate(dicePrefab.gameObject, startDicePos, Quaternion.identity).GetComponent<Dice>();
    }

    protected void PushDice()
    {
        diceNow.stopMove += DiceStopped;
        diceNow.Push(cv);
    }

    protected IEnumerator WaitPouse()
    {
        stage = Stage.pause;
        waitBar.StartTimer(waitTime);
        yield return new WaitForSeconds(waitTime);
        stage = Stage.waitMDown;
    }


    protected Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }


    protected void DiceStopped(int num)
    {
        diceNow.stopMove -= DiceStopped;
        line.StopTrajectory();//gameObject.SetActive(false);
        StartCoroutine(WaitPouse());
        CreteDots(num, diceNow.transform.position);
        PlaceDice();
    }

    void DiceStopped()
    {
        diceNow.stopMove -= DiceStopped;
        line.StopTrajectory();// line.gameObject.SetActive(false);
        StartCoroutine(WaitPouse());
        PlaceDice();
    }

    protected void  CreteDots(int count, Vector3 pos)
    {
        List<GameObject> dots=new List<GameObject>();
        for (int i = 0; i< count; i++)
        {
            Vector3 v = GetNewPos(pos);
            while (IsNear(dots, v))
            {
                v = GetNewPos(pos);
            }

            Instantiate(dotPref.gameObject, v, Quaternion.identity);
        }
    }

    Vector3 GetNewPos(Vector3 pos)
    {
        float a = Random.Range(-180f, 180f);
        float r = Random.Range(0, dist);
        return new Vector3(pos.x + Mathf.Cos(a) * r, pos.y-0.1f , pos.z + Mathf.Sin(a) * r);

    }

    bool IsNear(List<GameObject> objects, Vector3 pos)
    {
        foreach (GameObject go in objects)
        {
            if ((go.transform.position - pos).magnitude < 0.7f)
                return true;
        }
        return false;
    }

    public void DestroyDice()
    {
        DiceStopped();
    }
}