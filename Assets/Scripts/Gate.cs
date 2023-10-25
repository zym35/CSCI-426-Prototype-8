using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public List<Sensor> sensors;
    public Transform gate1, gate2, center;
    public float openTime, openDistance;
    public bool opened;

    private Vector3 _startPos1, _startPos2;

    private void Start()
    {
        _startPos1 = gate1.position;
        _startPos2 = gate2.position;
    }

    private void Update()
    {
        bool allOn = true;
        foreach (Sensor sensor in sensors)
        {
            allOn &= sensor.isOn;
        }

        if (allOn)
        {
            if (!opened)
                Open();
        }
        else
        {
            if (opened)
                Close();
        }
    }

    private void Open()
    {
        Vector3 dir1 = Vector3.Normalize(gate1.position - center.position);
        Vector3 dir2 = Vector3.Normalize(gate2.position - center.position);
        gate1.transform.DOMove(gate1.position + dir1 * openDistance, openTime);
        gate2.transform.DOMove(gate2.position + dir2 * openDistance, openTime).onComplete += delegate { AstarPath.active.Scan(); };
        opened = true;
    }

    private void Close()
    {
        gate1.transform.DOMove(_startPos1, openTime);
        gate2.transform.DOMove(_startPos2, openTime).onComplete += delegate { AstarPath.active.Scan(); };
        opened = false;
    }
}