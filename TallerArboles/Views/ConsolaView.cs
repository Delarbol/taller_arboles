using TallerArboles.Models;

namespace TallerArboles.Views;

public class ConsolaView
{
    public void MostrarTitulo(string titulo)
    {
        Console.WriteLine();
        Console.WriteLine("============================================================");
        Console.WriteLine(titulo);
        Console.WriteLine("============================================================");
    }

    public void MostrarSubtitulo(string titulo)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {titulo} ---");
    }

    public void MostrarInsercion(ResultadoInsercion resultado)
    {
        string tipo = resultado.EsCarpeta ? "Carpeta" : "Archivo";
        string estado = resultado.Exitoso ? "OK" : "RECHAZADO";
        Console.WriteLine($"{estado,-10} {tipo,-8} {resultado.Nombre,-24} | Comparaciones: {resultado.Comparaciones} | {resultado.Mensaje}");
    }

    public void MostrarBusqueda(string etiqueta, ResultadoBusqueda resultado)
    {
        string estado = resultado.Encontrado ? "Hallado" : "No hallado";
        string tipo = resultado.Nodo is null ? "-" : ObtenerTipo(resultado.Nodo);
        Console.WriteLine($"{etiqueta,-22} => {estado,-10} | Tipo: {tipo,-8} | Comparaciones: {resultado.Comparaciones} | {resultado.Mensaje}");
    }

    public void MostrarActualizacion(ResultadoActualizacion resultado)
    {
        string estado = resultado.Exitoso ? "OK" : "RECHAZADA";
        string tipo = resultado.EsCarpeta is null ? "-" : resultado.EsCarpeta.Value ? "Carpeta" : "Archivo";
        Console.WriteLine($"{estado,-10} {resultado.NombreAntiguo} -> {resultado.NombreNuevo} ({tipo})");
        Console.WriteLine($"Busqueda: {resultado.ComparacionesBusqueda}, eliminacion: {resultado.ComparacionesEliminacion}, insercion: {resultado.ComparacionesInsercion}");
        Console.WriteLine($"Caso al eliminar: {TraducirCaso(resultado.CasoEliminacion)} | {resultado.Mensaje}");
    }

    public void MostrarEliminacion(ResultadoEliminacion resultado)
    {
        string estado = resultado.Exitoso ? "OK" : "RECHAZADA";
        Console.WriteLine($"{estado,-10} {resultado.Nombre,-24} | Caso: {TraducirCaso(resultado.Caso),-18} | Comparaciones: {resultado.Comparaciones} | {resultado.Mensaje}");
    }

    public void MostrarArbol(Nodo? raiz)
    {
        Console.WriteLine();
        Console.WriteLine("Diagrama ASCII del arbol:");

        if (raiz is null)
        {
            Console.WriteLine("(arbol vacio)");
            return;
        }

        Console.WriteLine(FormatearNodo(raiz));
        ImprimirHijos(raiz, string.Empty);
    }

    public void MostrarRecorrido(string nombre, IReadOnlyList<Nodo> nodos)
    {
        string texto = nodos.Count == 0
            ? "(sin nodos)"
            : string.Join(" -> ", nodos.Select(nodo => $"{nodo.Nombre} [{ObtenerTipo(nodo)}]"));

        Console.WriteLine($"{nombre,-12}: {texto}");
    }

    public void MostrarAltura(int altura)
    {
        Console.WriteLine();
        Console.WriteLine($"Altura final del arbol: {altura}");
    }

    public void MostrarNota(string mensaje)
    {
        Console.WriteLine(mensaje);
    }

    private static void ImprimirHijos(Nodo nodo, string prefijo)
    {
        List<Nodo> hijos = new();

        if (nodo.Izquierdo is not null)
        {
            hijos.Add(nodo.Izquierdo);
        }

        if (nodo.Derecho is not null)
        {
            hijos.Add(nodo.Derecho);
        }

        for (int i = 0; i < hijos.Count; i++)
        {
            bool esUltimo = i == hijos.Count - 1;
            Nodo hijo = hijos[i];
            string conector = esUltimo ? "└── " : "├── ";
            Console.WriteLine($"{prefijo}{conector}{FormatearNodo(hijo)}");

            string nuevoPrefijo = prefijo + (esUltimo ? "    " : "│   ");
            ImprimirHijos(hijo, nuevoPrefijo);
        }
    }

    private static string FormatearNodo(Nodo nodo)
    {
        return $"{nodo.Nombre} [{ObtenerTipo(nodo)}]";
    }

    private static string ObtenerTipo(Nodo nodo)
    {
        return nodo.EsCarpeta ? "Carpeta" : "Archivo";
    }

    private static string TraducirCaso(CasoEliminacion caso)
    {
        return caso switch
        {
            CasoEliminacion.Hoja => "Hoja",
            CasoEliminacion.UnHijo => "Un hijo",
            CasoEliminacion.DosHijos => "Dos hijos",
            _ => "No aplica"
        };
    }
}
