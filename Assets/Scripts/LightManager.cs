using System;
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
    
    public bool IlluminatedByAnyLight(Vector3 position, out Transform closest, float rangePercent = 1, Light2D exclude = null)
    {
        foreach (Light2D l in lights)
        {
            if (exclude != null && l == exclude)
            {
                continue;
            }
            

            var l2pos = position - l.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(l.transform.position, l2pos.normalized, l2pos.magnitude, LayerMask.GetMask("Obstacle"));
            if (hit.collider != null)
            {
                continue;
            }

            Vector3 lightDirection = l.transform.up;
            float dotProduct = Vector3.Dot(l2pos.normalized, lightDirection);
            bool inAngle = Mathf.Acos(dotProduct) < Mathf.Deg2Rad * l.pointLightInnerAngle;
            if (!inAngle)
            {
                continue;
            }

            bool inRange = l2pos.magnitude < l.pointLightOuterRadius * rangePercent;
            if (!inRange)
            {
                continue;
            }

            closest = l.transform;
            return true;
        }

        closest = null;
        return false;
    }
}