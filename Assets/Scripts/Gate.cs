using System;
using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform gate1, gate2, center;
    public float openTime, openDistance;
    public bool opened;

    private Vector3 _startPos1, _startPos2;

    private void Start()
    {
        _startPos1 = gate1.position;
        _startPos2 = gate2.position;
    }

    public void Open()
    {
        if (opened) return;
        Vector3 dir1 = Vector3.Normalize(gate1.position - center.position);
        Vector3 dir2 = Vector3.Normalize(gate2.position - center.position);
        gate1.transform.DOMove(gate1.position + dir1 * openDistance, openTime);
        gate2.transform.DOMove(gate2.position + dir2 * openDistance, openTime);
        opened = true;
    }

    public void Close()
    {
        if (!opened) return;
        gate1.transform.DOMove(_startPos1, openTime);
        gate2.transform.DOMove(_startPos2, openTime);
        opened = false;
    }
}