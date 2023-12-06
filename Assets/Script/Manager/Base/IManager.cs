using UnityEngine;

public interface IManager<TKey,TValue> where TValue : IManagedItem<TKey>
{

    void Register(TKey _key, TValue _value);
    void Unregister(TKey _key);
    bool Exist(TKey _key);
    TValue Get(TKey _key);
}
