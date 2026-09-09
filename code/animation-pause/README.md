# FrozenAnimatorCache — ReapKeep code sample

A selected Unity component from Garrett Goodwin's ReapKeep project. The two C# files are unchanged from the supplied September 2026 project snapshot. This sample covers local Animator speed control and pool lifecycle cleanup; the rest of the game source is private.

## What it demonstrates

- Cache child Animator references and reuse arrays during normal operation.
- Count overlapping Pause/Resume calls: only the first pause captures speeds, and only the final matching resume restores them.
- Restore the captured speeds, including values other than 1.
- Reset pause state and cached default speeds on pooled reuse when resetToDefaultsOnPoolSpawn is enabled (the default).
- Avoid a per-frame Update loop in this component.

## Files

- FrozenAnimatorCache.cs: component, cache, nested pause counter, and reset behavior.
- IPoolCallbacks.cs: the two-method contract used by the project's pool. An interface does not invoke its callbacks automatically.

## Integration

1. Copy both scripts into a Unity project with support for the C# syntax used here (including target-typed new). If the project already defines IPoolCallbacks, use its compatible definition instead of importing a duplicate.
2. Add FrozenAnimatorCache to the root of a hierarchy containing Animator components. It discovers child animators in Awake; includeInactive defaults to true.
3. Have each effect call Pause() when it starts and Resume() when it ends. The counter relies on balanced calls.
4. Have your pool invoke OnPoolDespawn() before returning the object and OnPoolSpawn() before the next use. OnPoolDespawn clears the counter; the default OnPoolSpawn path also restores the cached default speeds.
5. For a non-pooled object, pair calls normally and call ResetToDefaults() when you need to discard pending pauses for a fresh use.

Example: Pause(), Pause(), Resume() leaves IsPaused true and speeds at zero. The second Resume() restores the original speeds. Additional Resume() calls while unpaused are ignored.

## Scope and tradeoffs

This component changes Animator.speed. It does not pause physics, movement scripts, audio, or the whole game. Coordinate with any other system that writes to Animator.speed while this cache controls it.

The default cache assumes a stable hierarchy. If animators are added or removed, recache while unpaused and after restoring intended defaults; a forced recache captures the speeds present at that moment. RecacheAndPause() should not be used to add a nested pause to an already paused cache.

GetOrAdd() is a setup convenience; routine pause/resume calls should reuse an existing component reference. Disabling resetToDefaultsOnPoolSpawn makes restoration the caller's responsibility.

No frame-rate improvement is claimed for this isolated sample. It has been source-reviewed for this portfolio package, but it has not been compiled or run in Unity as a standalone package here.

## References

- Case study: https://garrettgoodwin.github.io/projects/reapkeep/
- Annotated sample: https://garrettgoodwin.github.io/code/animation-pause/
- Unity Animator.speed: https://docs.unity3d.com/ScriptReference/Animator-speed.html
- Unity Component.GetComponentsInChildren: https://docs.unity3d.com/ScriptReference/Component.GetComponentsInChildren.html

Copyright Garrett Goodwin. Published for portfolio review; no additional redistribution license is granted by this package.
