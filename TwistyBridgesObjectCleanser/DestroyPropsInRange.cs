using UnityEngine;
using Random = UnityEngine.Random;

namespace TwistyBridgesObjectCleanser;

public sealed class DestroyPropsInRange : MonoBehaviour, IScheduledUpdateBehaviour
{
    private const float MinUpdateInterval = 2;
    
    public string[] classIdsToDestroy;
    public float radius;

    private float _radiusSqr;

    private float _timeNextUpdate;

    private void Start()
    {
        _radiusSqr = radius * radius;
    }

    private void LazyUpdate()
    {
        if (!isActiveAndEnabled) return;

        if (LargeWorld.main.streamer.cellManager.IsProcessing()) return;
        
        if (!LargeWorld.main.streamer.IsReady()) return;

        var allEntities = UniqueIdentifier.identifiers;

        float checkRadius = _radiusSqr * (transform.localScale.x * transform.localScale.x);
        foreach (var entity in allEntities.Values)
        {
            if (entity != null)
            {
                ProcessEntity(entity, checkRadius);
            }
        }
    }

    private void ProcessEntity(UniqueIdentifier identifier, float radiusSqr)
    {
        if (!identifier.gameObject.activeInHierarchy)
            return;
        if ((transform.position - identifier.transform.position).sqrMagnitude > radiusSqr)
            return;
        foreach (var classId in classIdsToDestroy)
        {
            if (string.Equals(identifier.ClassId, classId, System.StringComparison.Ordinal))
            {
                SafeDestroy(identifier.gameObject);
                return;
            }
        }
    }

    private void SafeDestroy(GameObject go)
    {
        if (go.GetComponent<LiveMixin>())
            return;
        if (go.transform.parent != null && go.transform.parent.GetComponent<ChildObjectIdentifier>())
            return;
        var rb = go.GetComponent<Rigidbody>();
        if (rb != null && !rb.isKinematic)
            return;
        if (go.GetComponentInParent<Player>())
            return;
        go.AddComponent<DestroyAfterDelay>().delay = 0.1f; // this makes sure the object actually 'exists' for a moment
    }

    public string GetProfileTag()
    {
        return "DestroyPropsInRange";
    }

    public void ScheduledUpdate()
    {
        if (Time.time > _timeNextUpdate)
        {
            _timeNextUpdate = Time.time + MinUpdateInterval + Random.value;
            LazyUpdate();
        }
    }

    private void OnEnable()
    {
        UpdateSchedulerUtils.Register(this);
    }

    private void OnDisable()
    {
        UpdateSchedulerUtils.Deregister(this);
    }

    public int scheduledUpdateIndex { get; set; }
}