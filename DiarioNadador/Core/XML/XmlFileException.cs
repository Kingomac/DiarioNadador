using System;

namespace DiarioNadador.Core.XML;

public class XmlFileException : Exception
{
    public enum Operation
    {
        Load,
        Save,
        Unknown
    }

    public XmlFileException(Exception innerException) : base("Error de fichero XML", innerException)
    {
    }

    public string FilePath { get; init; } = "";
    public Operation OperationType { get; init; } = Operation.Unknown;
}