using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DiarioNadador.Core;

public class DiarioEntrenamiento : IDictionary<DateOnly, DiaEntrenamiento>, IXmlSerializable
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
        if (array == null) throw new ArgumentNullException(nameof(array), "El arreglo de destino no puede ser nulo.");

        if (arrayIndex < 0 || arrayIndex >= array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "Índice de arreglo no válido.");

        if (array.Length - arrayIndex < _diasEntrenamiento.Count)
            throw new ArgumentException(
                "El arreglo de destino no es lo suficientemente grande para contener los elementos.", nameof(array));

        foreach (var kvp in _diasEntrenamiento) array[arrayIndex++] = kvp;
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

    public XmlSchema? GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var keySerializer = new XmlSerializer(typeof(DateTime));
        var valueSerializer = new XmlSerializer(typeof(DiaEntrenamiento));

        if (reader.IsEmptyElement)
        {
            reader.Read();
            return;
        }

        reader.ReadStartElement("DiarioEntrenamiento");

        while (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.ReadStartElement("KeyValuePair");

            reader.ReadStartElement("Key");
            var key = (DateTime)keySerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("Value");
            var value = (DiaEntrenamiento)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadEndElement();

            _diasEntrenamiento.Add(DateOnly.FromDateTime(key), value);
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        var keySerializer = new XmlSerializer(typeof(DateTime));
        var valueSerializer = new XmlSerializer(typeof(DiaEntrenamiento));

        foreach (var key in _diasEntrenamiento.Keys)
        {
            writer.WriteStartElement("KeyValuePair");

            writer.WriteStartElement("Key");
            keySerializer.Serialize(writer, key.ToDateTime(TimeOnly.MinValue));
            writer.WriteEndElement();

            writer.WriteStartElement("Value");
            valueSerializer.Serialize(writer, _diasEntrenamiento[key]);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}