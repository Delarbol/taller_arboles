namespace TallerArboles.Models;

public enum CasoEliminacion
{
    Ninguno,
    Hoja,
    UnHijo,
    DosHijos
}

public enum TipoRecorrido
{
    Preorden,
    Inorden,
    Postorden,
    PorNiveles
}

public record ResultadoInsercion(
    bool Exitoso,
    string Nombre,
    bool EsCarpeta,
    int Comparaciones,
    string Mensaje);

public record ResultadoBusqueda(
    bool Encontrado,
    Nodo? Nodo,
    int Comparaciones,
    string Mensaje);

public record ResultadoEliminacion(
    bool Exitoso,
    string Nombre,
    CasoEliminacion Caso,
    int Comparaciones,
    string Mensaje);

public record ResultadoActualizacion(
    bool Exitoso,
    string NombreAntiguo,
    string NombreNuevo,
    bool? EsCarpeta,
    int ComparacionesBusqueda,
    int ComparacionesEliminacion,
    int ComparacionesInsercion,
    CasoEliminacion CasoEliminacion,
    string Mensaje);
