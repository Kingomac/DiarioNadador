namespace DiarioNadador.Core;

public class Circuito
{
    public const string EtqCircuito = "circuito";

    private const string EtqDistancia = "distancia";
    private const string EtqLugar = "lugar";
    private const string EtqNotas = "notas";
    private const string EtqUrlMapa = "urlMapa";

    public Circuito(double distancia, string lugar, string notas, string urlMapa)
    {
        Distancia = distancia;
        Lugar = lugar;
        Notas = notas;
        UrlMapa = urlMapa;
    }

    public double Distancia { get; set; }
    public string Lugar { get; set; }
    public string Notas { get; set; }
    public string UrlMapa { get; set; }

    public override string ToString()
    {
        return $"Lugar: {Lugar} Distancia: {Distancia}km Notas: {Notas} Url: {UrlMapa}";
    }
}