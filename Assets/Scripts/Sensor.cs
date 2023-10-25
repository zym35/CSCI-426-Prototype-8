using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sensor : MonoBehaviour
{
    public float blinkInterval = 1f;
    public Color defaultColor;
    public Color illuminatedColor = Color.green;
    public float holdTime = 3;
    public LineRenderer line;
    public bool isOn;

    private SpriteRenderer spriteRenderer;
    private float _onCounter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

    private void Update()
    {
        if (LightManager.Instance.IlluminatedByAnyLight(transform.position, out _))
        {
            if (!isOn)
            {
                isOn = true;
            
                spriteRenderer.color = illuminatedColor;
                spriteRenderer.enabled = true;
            }
            
            _onCounter = holdTime;
        }

        if (isOn)
        {
            _onCounter -= Time.deltaTime;
            if (_onCounter < 0)
            {
                isOn = false;
                spriteRenderer.color = defaultColor;
                spriteRenderer.enabled = true;
            }
        }

        line.startColor = isOn ? illuminatedColor : defaultColor;
        line.endColor = isOn ? illuminatedColor : defaultColor;
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            if (!isOn)
            {
                yield return new WaitForSeconds(blinkInterval);
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }
            else
            {
                spriteRenderer.enabled = true;
                yield return null;
            }
        }
    }
}