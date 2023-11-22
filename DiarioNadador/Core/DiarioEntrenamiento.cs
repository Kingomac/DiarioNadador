using System;
using System.Collections;
using System.Collections.Generic;

namespace DiarioNadador.Core;

public class DiarioEntrenamiento : IDictionary<DateOnly, DiaEntrenamiento>
{
    private readonly Dictionary<DateOnly, DiaEntrenamiento> _diasEntrenamiento = new();

    public IEnumerator<KeyValuePair<DateOnly, DiaEntrenamiento>> GetEnumerator()
    {
        return _diasEntrenamiento.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(KeyValuePair<DateOnly, DiaEntrenamiento> item)
    {
        _diasEntrenamiento.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        _diasEntrenamiento.Clear();
    }

    public bool Contains(KeyValuePair<DateOnly, DiaEntrenamiento> item)
    {
        return _diasEntrenamiento.ContainsKey(item.Key) && _diasEntrenamiento[item.Key].Equals(item.Value);
    }

    public void CopyTo(KeyValuePair<DateOnly, DiaEntrenamiento>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(KeyValuePair<DateOnly, DiaEntrenamiento> item)
    {
        return _diasEntrenamiento.Remove(item.Key);
    }

    public int Count => _diasEntrenamiento.Count;
    public bool IsReadOnly => false;

    public void Add(DateOnly key, DiaEntrenamiento value)
    {
        _diasEntrenamiento.Add(key, value);
    }

    public bool ContainsKey(DateOnly key)
    {
        return _diasEntrenamiento.ContainsKey(key);
    }

    public bool Remove(DateOnly key)
    {
        return _diasEntrenamiento.Remove(key);
    }

    public bool TryGetValue(DateOnly key, out DiaEntrenamiento value)
    {
        return _diasEntrenamiento.TryGetValue(key, out value);
    }

    public DiaEntrenamiento this[DateOnly key]
    {
        get => _diasEntrenamiento[key];
        set => _diasEntrenamiento[key] = value;
    }

    public ICollection<DateOnly> Keys => _diasEntrenamiento.Keys;
    public ICollection<DiaEntrenamiento> Values => _diasEntrenamiento.Values;
}