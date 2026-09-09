using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class FrozenAnimatorCache : MonoBehaviour, IPoolCallbacks
{
    [Header("Cache")]
    [SerializeField] private bool includeInactive = true;

    [Tooltip("Enable only if animators are genuinely added/removed at runtime.")]
    [SerializeField] private bool recacheEveryPoolSpawn = false;

    [Header("Pooling")]
    [SerializeField] private bool resetToDefaultsOnPoolSpawn = true;

    private Animator[] _anims;
    private float[] _defaultSpeeds;
    private float[] _resumeSpeeds;
    private bool _cached;
    private int _pauseCount;

    private readonly List<Animator> _animScratch = new(16);

    public bool IsPaused => _pauseCount > 0;

    private void Awake() => CacheIfNeeded(force: true);

    public void CacheIfNeeded(bool force = false)
    {
        if (_cached && !force) return;

        _animScratch.Clear();
        GetComponentsInChildren(includeInactive, _animScratch);

        if (_animScratch.Count == 0)
        {
            _anims = System.Array.Empty<Animator>();
            _defaultSpeeds = System.Array.Empty<float>();
            _resumeSpeeds = System.Array.Empty<float>();
            _cached = true;
            return;
        }

        int count = _animScratch.Count;
        if (_anims == null || _anims.Length != count)
            _anims = new Animator[count];

        for (int i = 0; i < count; i++)
            _anims[i] = _animScratch[i];

        _defaultSpeeds = EnsureFloatArray(_defaultSpeeds, count);
        _resumeSpeeds = EnsureFloatArray(_resumeSpeeds, count);

        for (int i = 0; i < count; i++)
            _defaultSpeeds[i] = _anims[i] ? _anims[i].speed : 1f;

        _animScratch.Clear();
        _cached = true;
    }

    public void Pause()
    {
        CacheIfNeeded();

        _pauseCount++;
        if (_pauseCount > 1) return;

        for (int i = 0; i < _anims.Length; i++)
        {
            Animator animator = _anims[i];
            _resumeSpeeds[i] = animator ? animator.speed : 1f;
            if (animator && animator.speed != 0f)
                animator.speed = 0f;
        }
    }

    public void Resume()
    {
        if (_pauseCount <= 0) return;

        _pauseCount--;
        if (_pauseCount > 0) return;

        for (int i = 0; i < _anims.Length; i++)
        {
            Animator animator = _anims[i];
            if (!animator) continue;

            float target = i < _resumeSpeeds.Length
                ? _resumeSpeeds[i]
                : _defaultSpeeds[i];

            if (!Mathf.Approximately(animator.speed, target))
                animator.speed = target;
        }
    }

    public void ResetToDefaults()
    {
        CacheIfNeeded();
        _pauseCount = 0;

        for (int i = 0; i < _anims.Length; i++)
        {
            Animator animator = _anims[i];
            if (!animator) continue;

            float target = i < _defaultSpeeds.Length ? _defaultSpeeds[i] : 1f;
            if (!Mathf.Approximately(animator.speed, target))
                animator.speed = target;
        }
    }

    public void RecacheAndPause()
    {
        CacheIfNeeded(force: true);
        Pause();
    }

    public static FrozenAnimatorCache GetOrAdd(GameObject go)
    {
        if (!go) return null;
        if (!go.TryGetComponent(out FrozenAnimatorCache cache))
            cache = go.AddComponent<FrozenAnimatorCache>();
        return cache;
    }

    public void OnPoolSpawn()
    {
        if (recacheEveryPoolSpawn)
            CacheIfNeeded(force: true);
        else
            CacheIfNeeded();

        if (resetToDefaultsOnPoolSpawn)
            ResetToDefaults();
    }

    public void OnPoolDespawn() => _pauseCount = 0;

    private static float[] EnsureFloatArray(float[] array, int length)
        => array != null && array.Length == length ? array : new float[length];
}
