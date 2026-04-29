namespace TallerArboles.Models;

public class Nodo
{
    public Nodo(string nombre, bool esCarpeta)
    {
        Nombre = nombre;
        EsCarpeta = esCarpeta;
    }

    public string Nombre { get; set; }
    public bool EsCarpeta { get; set; }
    public Nodo? Izquierdo { get; set; }
    public Nodo? Derecho { get; set; }
}
