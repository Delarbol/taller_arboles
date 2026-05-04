using Xunit;
using TallerArboles.Models;

namespace TallerArboles.Tests;

public class BSTTests
{
    [Fact]
    public void InsertarDuplicado_DebeRetornarFallo()
    {
        
        var arbol = new ArbolBinario();
        arbol.Insertar("Carpeta_A", true);

        
        var resultado = arbol.Insertar("carpeta_a", true); 

        
        Assert.False(resultado.Exitoso);
        Assert.Contains("ya existe", resultado.Mensaje);
    }

    [Fact]
    public void EliminarNodoConDosHijos_DebeMantenerOrdenInorden()
    {
        
        var arbol = new ArbolBinario();
        arbol.Insertar("M", true); 
        arbol.Insertar("F", true); 
        arbol.Insertar("T", true); 

        
        arbol.Eliminar("M"); 

        
        
        var inorden = arbol.ObtenerListaInorden(); 
        
    
        Assert.Equal("F", inorden[0].Nombre);
        Assert.Equal("T", inorden[1].Nombre);
    }
}