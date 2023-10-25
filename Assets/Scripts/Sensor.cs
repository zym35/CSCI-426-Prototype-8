using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sensor : MonoBehaviour
{
    public float blinkInterval = 1f;
    public Color defaultColor;
    public Color illuminatedColor = Color.green;
    public Gate gate;
    public float holdTime = 3;
    public LineRenderer line;

    private SpriteRenderer spriteRenderer;
    private bool _isOn;
    private float _onCounter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Blink());
    }

    private void Update()
    {
        if (LightManager.Instance.IlluminatedByAnyLight(transform.position))
        {
            if (!_isOn)
            {
                _isOn = true;
            
                spriteRenderer.color = illuminatedColor;
                spriteRenderer.enabled = true;
            
                gate.Open();
            }
            
            _onCounter = holdTime;
        }

        if (_isOn)
        {
            _onCounter -= Time.deltaTime;
            if (_onCounter < 0)
            {
                _isOn = false;
                spriteRenderer.color = defaultColor;
                spriteRenderer.enabled = true;
                gate.Close();
            }
        }

        line.startColor = _isOn ? illuminatedColor : defaultColor;
        line.endColor = _isOn ? illuminatedColor : defaultColor;
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            if (!_isOn)
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