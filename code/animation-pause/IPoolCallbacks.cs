public interface IPoolCallbacks
{
    // Called when the object is (re)spawned from the pool (or enabled)
    void OnPoolSpawn();

    // Called right before the object is returned to the pool (or disabled)
    void OnPoolDespawn();
}