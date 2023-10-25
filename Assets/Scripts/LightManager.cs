﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    public List<Light2D> lights;
    public Light2D playerLight;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var l in FindObjectsOfType<Light2D>())
        {
            lights.Add(l);
        }
    }

    public bool IlluminatedByPlayerLight(Vector3 position, float rangePercent = 1)
    {
        bool notBlock = false;
        bool inAngle = false;
        bool inRange = false;

        var l2pos = position - playerLight.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(playerLight.transform.position, l2pos.normalized, l2pos.magnitude, 1 << LayerMask.NameToLayer("Obstacle"));
        if (hit.collider == null)
        {
            notBlock = true;
        }

        Vector3 lightDirection = playerLight.transform.up;
        float dotProduct = Vector3.Dot(l2pos.normalized, lightDirection);
        inAngle = Mathf.Acos(dotProduct) < Mathf.Deg2Rad * playerLight.pointLightInnerAngle;

        inRange = l2pos.magnitude < playerLight.pointLightOuterRadius * rangePercent;

        return notBlock && inAngle && inRange;
    }
    
    public bool IlluminatedByAnyLight(Vector3 position, float rangePercent = 1)
    {
        foreach (Light2D l in lights)
        {
            bool notBlock = false;
            bool inAngle = false;
            bool inRange = false;

            var l2pos = position - l.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(l.transform.position, l2pos.normalized, l2pos.magnitude, 1 << LayerMask.NameToLayer("Obstacle"));
            if (hit.collider == null)
            {
                notBlock = true;
            }

            Vector3 lightDirection = l.transform.up;
            float dotProduct = Vector3.Dot(l2pos.normalized, lightDirection);
            inAngle = Mathf.Acos(dotProduct) < Mathf.Deg2Rad * l.pointLightInnerAngle;

            inRange = l2pos.magnitude < l.pointLightOuterRadius * rangePercent;

            if (notBlock && inAngle && inRange)
                return true;
        }

        return false;
    }
}