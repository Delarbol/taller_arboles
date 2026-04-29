using TallerArboles.Models;
using TallerArboles.Views;

namespace TallerArboles.Controllers;

public class TallerArbolesController
{
    private readonly ArbolBinario _arbol;
    private readonly ConsolaView _vista;

    public TallerArbolesController(ArbolBinario arbol, ConsolaView vista)
    {
        _arbol = arbol;
        _vista = vista;
    }

    public void EjecutarTaller()
    {
        _vista.MostrarTitulo("DocuTrack S.A. - Taller integrador de arboles binarios");

        ConstruirArbolInicial();
        EjecutarBusquedasRapidas();
        EjecutarActualizacionesSelectivas();
        EjecutarEliminacionesSelectivas();
        MostrarRecorridosYMetricasFinales();
    }

    private void ConstruirArbolInicial()
    {
        _vista.MostrarTitulo("1. Construccion del arbol inicial");

        (string Nombre, bool EsCarpeta)[] nodosIniciales =
        [
            ("M_General", true),
            ("F_Finanzas", true),
            ("T_Tecnologia", true),
            ("C_Contratos", true),
            ("H_HojasDeVida", true),
            ("R_Reportes", true),
            ("X_Expedientes", true),
            ("A_Actas.txt", false),
            ("D_Documentos", true),
            ("G_Gastos.xlsx", false),
            ("J_Justificaciones", true),
            ("P_Presupuestos", true),
            ("S_Soporte.log", false),
            ("V_Ventas", true)
        ];

        foreach ((string nombre, bool esCarpeta) in nodosIniciales)
        {
            ResultadoInsercion resultado = _arbol.Insertar(nombre, esCarpeta);
            _vista.MostrarInsercion(resultado);
        }

        _vista.MostrarSubtitulo("Intento de duplicado para dejar clara la politica");
        _vista.MostrarInsercion(_arbol.Insertar("m_general", true));

        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void EjecutarBusquedasRapidas()
    {
        _vista.MostrarTitulo("2. Busquedas rapidas");

        string[] nombres =
        [
            "A_Actas.txt",
            "H_HojasDeVida",
            "S_Soporte.log",
            "X_Expedientes",
            "B_Borrador.docx",
            "Z_ZipFinal.zip"
        ];

        foreach (string nombre in nombres)
        {
            ResultadoBusqueda resultado = _arbol.Buscar(nombre);
            _vista.MostrarBusqueda(nombre, resultado);
        }
    }

    private void EjecutarActualizacionesSelectivas()
    {
        _vista.MostrarTitulo("3. Actualizaciones selectivas");

        ActualizarYMostrar("A_Actas.txt", "B_ActasActualizadas.txt", "Actualizar una hoja");
        ActualizarYMostrar("X_Expedientes", "Y_ExpedientesArchivados", "Actualizar un hijo con un hijo");
        ActualizarYMostrar("M_General", "N_General", "Actualizar la raiz");
    }

    private void EjecutarEliminacionesSelectivas()
    {
        _vista.MostrarTitulo("4. Eliminaciones selectivas");

        EliminarYMostrar("B_ActasActualizadas.txt", "Eliminar una hoja");
        EliminarYMostrar("V_Ventas", "Eliminar un hijo con un hijo");
        EliminarYMostrar("P_Presupuestos", "Eliminar la raiz con dos hijos");
    }

    private void MostrarRecorridosYMetricasFinales()
    {
        _vista.MostrarTitulo("5. Recorridos de verificacion");

        _vista.MostrarRecorrido("Preorden", _arbol.Recorrer(TipoRecorrido.Preorden));
        _vista.MostrarRecorrido("Inorden", _arbol.Recorrer(TipoRecorrido.Inorden));
        _vista.MostrarRecorrido("Postorden", _arbol.Recorrer(TipoRecorrido.Postorden));
        _vista.MostrarRecorrido("Por niveles", _arbol.Recorrer(TipoRecorrido.PorNiveles));

        _vista.MostrarNota("El inorden debe verse alfabetico; por eso confirma visualmente que el BST conserva su orden.");
        _vista.MostrarAltura(_arbol.CalcularAltura());
    }

    private void ActualizarYMostrar(string nombreAntiguo, string nombreNuevo, string titulo)
    {
        _vista.MostrarSubtitulo(titulo);
        ResultadoActualizacion resultado = _arbol.Actualizar(nombreAntiguo, nombreNuevo);
        _vista.MostrarActualizacion(resultado);
        _vista.MostrarArbol(_arbol.Raiz);
    }

    private void EliminarYMostrar(string nombre, string titulo)
    {
        _vista.MostrarSubtitulo(titulo);
        ResultadoEliminacion resultado = _arbol.Eliminar(nombre);
        _vista.MostrarEliminacion(resultado);
        _vista.MostrarArbol(_arbol.Raiz);
    }
}
