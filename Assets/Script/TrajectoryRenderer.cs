using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineVisual;
    [SerializeField] private int segmentsCount = 10;
    Transform startTransform;
    float flyTime;

    [SerializeField] private GameObject cursor;

    public bool isActive;

    void Awake()
    {
        lineVisual.positionCount = segmentsCount + 1;
    }

    public void StartTrajectory(Transform start, float time)
    {
        cursor.gameObject.SetActive(true);
        startTransform = start;
        isActive = true;
    }

    public void StopTrajectory()
    {
        cursor.gameObject.SetActive(false);
        isActive = false;
        this.gameObject.SetActive(false);
    }

    public void DrawCursor(RaycastHit hit)
    {
        cursor.gameObject.SetActive(true);
        cursor.transform.position = hit.point + (Vector3.up * 0.1f+new Vector3(0,0.15f,0));
    }

    public void SetVelocity(Vector3 vector)
    {
        VisualizeLine(vector, cursor.transform.position);
        transform.rotation = Quaternion.LookRotation(vector);
    }

    void VisualizeLine(Vector3 cv, Vector3 target)
    {
        for (int i = 0; i < segmentsCount; i++)
        {
            Vector3 pos = CalculatePosInTime(cv, (i / (float)segmentsCount) * flyTime);
            lineVisual.SetPosition(i, pos);
        }
        lineVisual.SetPosition(segmentsCount, target);
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = startTransform.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + startTransform.position.y;
        result.y = sY;
        return result;
    }
}