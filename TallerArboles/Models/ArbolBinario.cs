namespace TallerArboles.Models;

public class ArbolBinario
{
    // El enunciado pide ignorar mayúsculas y minúsculas. Uso cultura invariante
    // para que la comparación no dependa del idioma configurado en Windows.
    private static readonly StringComparer ComparadorNombres = StringComparer.InvariantCultureIgnoreCase;

    public Nodo? Raiz { get; private set; }

    public ResultadoInsercion Insertar(string nombre, bool esCarpeta)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return new ResultadoInsercion(false, nombre, esCarpeta, 0, "No se inserta: el nombre está vacío.");
        }

        string nombreLimpio = nombre.Trim();

        if (Raiz is null)
        {
            Raiz = new Nodo(nombreLimpio, esCarpeta);
            return new ResultadoInsercion(true, nombreLimpio, esCarpeta, 0, "Insertado como raíz.");
        }

        int comparaciones = 0;
        Nodo actual = Raiz;

        while (true)
        {
            comparaciones++;
            int comparacion = ComparadorNombres.Compare(nombreLimpio, actual.Nombre);

            if (comparacion == 0)
            {
                return new ResultadoInsercion(false, nombreLimpio, esCarpeta, comparaciones, "No se inserta: ya existe un nodo con ese nombre.");
            }

            if (!actual.EsCarpeta)
            {
                return new ResultadoInsercion(false, nombreLimpio, esCarpeta, comparaciones, "No se inserta: un archivo debe mantenerse como hoja.");
            }

            if (comparacion < 0)
            {
                if (actual.Izquierdo is null)
                {
                    actual.Izquierdo = new Nodo(nombreLimpio, esCarpeta);
                    return new ResultadoInsercion(true, nombreLimpio, esCarpeta, comparaciones, "Insertado por el lado izquierdo.");
                }

                actual = actual.Izquierdo;
            }
            else
            {
                if (actual.Derecho is null)
                {
                    actual.Derecho = new Nodo(nombreLimpio, esCarpeta);
                    return new ResultadoInsercion(true, nombreLimpio, esCarpeta, comparaciones, "Insertado por el lado derecho.");
                }

                actual = actual.Derecho;
            }
        }
    }

    public ResultadoBusqueda Buscar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return new ResultadoBusqueda(false, null, 0, "No se busca: el nombre está vacío.");
        }

        string nombreLimpio = nombre.Trim();
        int comparaciones = 0;
        Nodo? actual = Raiz;

        while (actual is not null)
        {
            comparaciones++;
            int comparacion = ComparadorNombres.Compare(nombreLimpio, actual.Nombre);

            if (comparacion == 0)
            {
                return new ResultadoBusqueda(true, actual, comparaciones, "Nodo encontrado.");
            }

            actual = comparacion < 0 ? actual.Izquierdo : actual.Derecho;
        }

        return new ResultadoBusqueda(false, null, comparaciones, "Nodo no encontrado.");
    }

    public ResultadoActualizacion Actualizar(string nombreAntiguo, string nombreNuevo)
    {
        if (string.IsNullOrWhiteSpace(nombreAntiguo) || string.IsNullOrWhiteSpace(nombreNuevo))
        {
            return new ResultadoActualizacion(false, nombreAntiguo, nombreNuevo, null, 0, 0, 0, CasoEliminacion.Ninguno, "No se actualiza: hay un nombre vacío.");
        }

        string antiguoLimpio = nombreAntiguo.Trim();
        string nuevoLimpio = nombreNuevo.Trim();
        ResultadoBusqueda busquedaAntiguo = Buscar(antiguoLimpio);

        if (!busquedaAntiguo.Encontrado || busquedaAntiguo.Nodo is null)
        {
            return new ResultadoActualizacion(false, antiguoLimpio, nuevoLimpio, null, busquedaAntiguo.Comparaciones, 0, 0, CasoEliminacion.Ninguno, "No se actualiza: el nodo original no existe.");
        }

        if (ComparadorNombres.Equals(antiguoLimpio, nuevoLimpio))
        {
            return new ResultadoActualizacion(false, antiguoLimpio, nuevoLimpio, busquedaAntiguo.Nodo.EsCarpeta, busquedaAntiguo.Comparaciones, 0, 0, CasoEliminacion.Ninguno, "No se actualiza: el nombre nuevo es igual al anterior.");
        }

        ResultadoBusqueda busquedaNuevo = Buscar(nuevoLimpio);

        if (busquedaNuevo.Encontrado)
        {
            int comparacionesBusqueda = busquedaAntiguo.Comparaciones + busquedaNuevo.Comparaciones;
            return new ResultadoActualizacion(false, antiguoLimpio, nuevoLimpio, busquedaAntiguo.Nodo.EsCarpeta, comparacionesBusqueda, 0, 0, CasoEliminacion.Ninguno, "No se actualiza: el nombre nuevo ya existe.");
        }

        bool eraCarpeta = busquedaAntiguo.Nodo.EsCarpeta;

        // La guía pide hacerlo así: eliminar primero y luego insertar el nuevo nombre.
        ResultadoEliminacion eliminacion = Eliminar(antiguoLimpio);
        ResultadoInsercion insercion = Insertar(nuevoLimpio, eraCarpeta);

        bool exitoso = eliminacion.Exitoso && insercion.Exitoso;
        string mensaje = exitoso
            ? "Actualizado usando la estrategia Eliminar + Insertar."
            : "La actualización quedó incompleta; revisar las operaciones internas.";

        return new ResultadoActualizacion(
            exitoso,
            antiguoLimpio,
            nuevoLimpio,
            eraCarpeta,
            busquedaAntiguo.Comparaciones + busquedaNuevo.Comparaciones,
            eliminacion.Comparaciones,
            insercion.Comparaciones,
            eliminacion.Caso,
            mensaje);
    }

    public ResultadoEliminacion Eliminar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return new ResultadoEliminacion(false, nombre, CasoEliminacion.Ninguno, 0, "No se elimina: el nombre está vacío.");
        }

        string nombreLimpio = nombre.Trim();
        int comparaciones = 0;
        Nodo? nodoEliminado;
        CasoEliminacion caso;
        bool eliminado;

        (Raiz, eliminado, nodoEliminado, caso) = EliminarNodo(Raiz, nombreLimpio, ref comparaciones);

        if (!eliminado)
        {
            return new ResultadoEliminacion(false, nombreLimpio, CasoEliminacion.Ninguno, comparaciones, "No se elimina: el nodo no existe.");
        }

        string mensaje = caso switch
        {
            CasoEliminacion.Hoja => "Eliminado como hoja.",
            CasoEliminacion.UnHijo => "Eliminado como nodo con un solo hijo.",
            CasoEliminacion.DosHijos => "Eliminado como nodo con dos hijos usando el sucesor.",
            _ => "Eliminado."
        };

        return new ResultadoEliminacion(true, nodoEliminado!.Nombre, caso, comparaciones, mensaje);
    }

    public IReadOnlyList<Nodo> Recorrer(TipoRecorrido tipo)
    {
        List<Nodo> nodos = new();

        switch (tipo)
        {
            case TipoRecorrido.Preorden:
                RecorrerPreorden(Raiz, nodos);
                break;
            case TipoRecorrido.Inorden:
                RecorrerInorden(Raiz, nodos);
                break;
            case TipoRecorrido.Postorden:
                RecorrerPostorden(Raiz, nodos);
                break;
            case TipoRecorrido.PorNiveles:
                RecorrerPorNiveles(nodos);
                break;
        }

        return nodos;
    }

    public int CalcularAltura()
    {
        return CalcularAltura(Raiz);
    }

    public List<Nodo> ObtenerListaInorden()
    {
        var lista = new List<Nodo>();
        LlenarListaInorden(Raiz, lista);
        return lista;
    }

    private static (Nodo? NuevoNodo, bool Eliminado, Nodo? NodoEliminado, CasoEliminacion Caso) EliminarNodo(
        Nodo? actual,
        string nombre,
        ref int comparaciones)
    {
        if (actual is null)
        {
            return (null, false, null, CasoEliminacion.Ninguno);
        }

        comparaciones++;
        int comparacion = ComparadorNombres.Compare(nombre, actual.Nombre);

        if (comparacion < 0)
        {
            (actual.Izquierdo, bool eliminado, Nodo? nodoEliminado, CasoEliminacion caso) = EliminarNodo(actual.Izquierdo, nombre, ref comparaciones);
            return (actual, eliminado, nodoEliminado, caso);
        }

        if (comparacion > 0)
        {
            (actual.Derecho, bool eliminado, Nodo? nodoEliminado, CasoEliminacion caso) = EliminarNodo(actual.Derecho, nombre, ref comparaciones);
            return (actual, eliminado, nodoEliminado, caso);
        }

        Nodo copiaDelNodoEliminado = new(actual.Nombre, actual.EsCarpeta);

        if (actual.Izquierdo is null && actual.Derecho is null)
        {
            return (null, true, copiaDelNodoEliminado, CasoEliminacion.Hoja);
        }

        if (actual.Izquierdo is null)
        {
            return (actual.Derecho, true, copiaDelNodoEliminado, CasoEliminacion.UnHijo);
        }

        if (actual.Derecho is null)
        {
            return (actual.Izquierdo, true, copiaDelNodoEliminado, CasoEliminacion.UnHijo);
        }

        // Para dos hijos elegí el sucesor: el menor nodo del subárbol derecho.
        Nodo sucesor = ObtenerMinimo(actual.Derecho);
        actual.Nombre = sucesor.Nombre;
        actual.EsCarpeta = sucesor.EsCarpeta;
        (actual.Derecho, _, _, _) = EliminarNodo(actual.Derecho, sucesor.Nombre, ref comparaciones);

        return (actual, true, copiaDelNodoEliminado, CasoEliminacion.DosHijos);
    }

    private static Nodo ObtenerMinimo(Nodo actual)
    {
        while (actual.Izquierdo is not null)
        {
            actual = actual.Izquierdo;
        }

        return actual;
    }

    private static void RecorrerPreorden(Nodo? actual, List<Nodo> nodos)
    {
        if (actual is null)
        {
            return;
        }

        nodos.Add(actual);
        RecorrerPreorden(actual.Izquierdo, nodos);
        RecorrerPreorden(actual.Derecho, nodos);
    }

    private static void RecorrerInorden(Nodo? actual, List<Nodo> nodos)
    {
        if (actual is null)
        {
            return;
        }

        RecorrerInorden(actual.Izquierdo, nodos);
        nodos.Add(actual);
        RecorrerInorden(actual.Derecho, nodos);
    }

    private static void RecorrerPostorden(Nodo? actual, List<Nodo> nodos)
    {
        if (actual is null)
        {
            return;
        }

        RecorrerPostorden(actual.Izquierdo, nodos);
        RecorrerPostorden(actual.Derecho, nodos);
        nodos.Add(actual);
    }

    private void RecorrerPorNiveles(List<Nodo> nodos)
    {
        if (Raiz is null)
        {
            return;
        }

        Queue<Nodo> pendientes = new();
        pendientes.Enqueue(Raiz);

        while (pendientes.Count > 0)
        {
            Nodo actual = pendientes.Dequeue();
            nodos.Add(actual);

            if (actual.Izquierdo is not null)
            {
                pendientes.Enqueue(actual.Izquierdo);
            }

            if (actual.Derecho is not null)
            {
                pendientes.Enqueue(actual.Derecho);
            }
        }
    }

    private static int CalcularAltura(Nodo? actual)
    {
        if (actual is null)
        {
            return 0;
        }

        return 1 + Math.Max(CalcularAltura(actual.Izquierdo), CalcularAltura(actual.Derecho));
    }
    private void LlenarListaInorden(Nodo? nodo, List<Nodo> lista)
    {
        if (nodo == null) return;
        LlenarListaInorden(nodo.Izquierdo, lista);
        lista.Add(nodo);
        LlenarListaInorden(nodo.Derecho, lista);
    }
}
