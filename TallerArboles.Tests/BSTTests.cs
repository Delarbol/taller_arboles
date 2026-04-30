using Xunit;
using TallerArboles.Models;

namespace TallerArboles.Tests;

public class BSTTests
{
    [Fact]
    public void InsertarDuplicado_DebeRetornarFallo()
    {
        // Arrange (Preparar)
        var arbol = new ArbolBinario();
        arbol.Insertar("Carpeta_A", true);

        // Act (Ejecutar)
        var resultado = arbol.Insertar("carpeta_a", true); // Caso case-insensitive

        // Assert (Validar)
        Assert.False(resultado.Exitoso);
        Assert.Contains("ya existe", resultado.Mensaje);
    }

    [Fact]
    public void EliminarNodoConDosHijos_DebeMantenerOrdenInorden()
    {
        // Arrange
        var arbol = new ArbolBinario();
        arbol.Insertar("M", true); // Raíz
        arbol.Insertar("F", true); // Hijo Izquierdo
        arbol.Insertar("T", true); // Hijo Derecho

        // Act
        arbol.Eliminar("M"); // Eliminar raíz con dos hijos

        // Assert
        // CAMBIO AQUÍ: Usamos ObtenerListaInorden que fue el que agregamos
        var inorden = arbol.ObtenerListaInorden(); 
        
    
        Assert.Equal("F", inorden[0].Nombre);
        Assert.Equal("T", inorden[1].Nombre);
    }
}