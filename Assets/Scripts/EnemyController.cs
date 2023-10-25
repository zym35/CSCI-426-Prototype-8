using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float sightRange = 10f;
    public float chargeRate = 0.1f;
    public float dechargeRate = 0.1f;
    public float maxRadius = 1f;
    public float charge2Radius, charge2Intensity;
    public float explodeThreshold;
    public AIDestinationSetter setter;
    public AIPath path;
    public Transform lastSeenPosition;
    public GameObject remain;
    
    private float _chargeAmount = 0f;
    private Light2D _glow;
    private Transform _chargingSource;

    private void Start()
    {
        _glow = GetComponentInChildren<Light2D>();
        lastSeenPosition = new GameObject("lastPlayerPos_" + name).transform;
        lastSeenPosition.position = transform.position;
    }

    private void Update()
    {
        _chargingSource = null;
        if (CanSeePlayer() && LightManager.Instance.IlluminatedByAnyLight(player.position, out _))
        {
            lastSeenPosition.position = player.position;
        }
        ChaseTransform(lastSeenPosition);

        LightManager.Instance.IlluminatedByAnyLight(transform.position, out _chargingSource, 1, _glow);

        if (_chargingSource != null)
        {
            float factor = Time.deltaTime * (1f / Vector3.Distance(transform.position, _chargingSource.position));
            _chargeAmount += chargeRate * factor;

            if (_chargeAmount > explodeThreshold)
            {
                _glow.intensity = 1.2f;
                _glow.pointLightInnerRadius = 0;
                _glow.transform.SetParent(null);
                Destroy(gameObject);
                Instantiate(remain, transform.position, Quaternion.identity);
                return;
            }
        }
        else
        {
            if (_chargeAmount > 0)
                _chargeAmount -= dechargeRate * Time.deltaTime;
        }
        
        _glow.intensity = Mathf.Clamp(_chargeAmount * charge2Intensity, 0.05f, 5);
        _glow.pointLightInnerRadius = Mathf.Clamp(_chargeAmount * charge2Radius, 0, maxRadius);
        _glow.pointLightOuterRadius = Mathf.Clamp(_chargeAmount * charge2Radius + 1, 1, maxRadius + 1);
    }

    private void ChaseTransform(Transform t)
    {
        if (!path.enabled)
            path.enabled = true;
        setter.target = t;
    }

    private void Stop()
    {
        setter.target = null;
        if (path.enabled)
            path.enabled = false;
    }

    bool CanSeePlayer()
    {
        var l2pos = transform.position - player.position;
        RaycastHit2D hit = Physics2D.Raycast(player.position, l2pos.normalized, l2pos.magnitude, LayerMask.GetMask("Obstacle"));
        return hit.collider == null;
    }

    bool IsCloseToPlayer(float distance)
    {
        return Vector3.Distance(transform.position, player.position) <= distance;
    }
}
