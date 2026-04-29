using TallerArboles.Controllers;
using TallerArboles.Models;
using TallerArboles.Views;

// Main queda lo más limpio posible: solo arma las piezas MVC y entrega el control.
// Toda la historia del taller se ejecuta desde el controlador.
ArbolBinario arbol = new();
ConsolaView vista = new();
TallerArbolesController controller = new(arbol, vista);

controller.EjecutarTaller();
