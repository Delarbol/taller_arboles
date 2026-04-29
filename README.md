# Taller integrador - Arboles binarios en C#

Proyecto académico para la empresa ficticia DocuTrack S.A. El programa gestiona carpetas y archivos en un árbol binario de búsqueda usando el nombre como clave.

## Integrantes y responsabilidades

- Camilo De la Cruz: implementación del modelo BST, controlador de casos de uso, vista de consola y documentación.
- Sebastián Cardona: 

## Requisitos para ejecutar

- .NET SDK 9.0 o superior.

## Ejecución

```powershell
dotnet run --project .\TallerArboles\TallerArboles.csproj
```

## Arquitectura MVC

- `Models`: contiene `Nodo`, `ArbolBinario` y resultados de operaciones. No usa `Console`.
- `Views`: contiene `ConsolaView`, responsable de imprimir resultados, métricas y el árbol en ASCII.
- `Controllers`: contiene `TallerArbolesController`, que secuencia los casos pedidos por el taller.
- `Program.cs`: solo crea modelo, vista y controlador; no tiene lógica de negocio.

## Decisiones del BST

- La clave es `Nombre`.
- La comparación ignora mayúsculas y minúsculas usando cultura invariante.
- Los duplicados se rechazan y se reportan en consola.
- Las actualizaciones se hacen como pide el enunciado: primero eliminar el nombre anterior y luego insertar el nuevo.
- Cuando se elimina un nodo con dos hijos, se reemplaza por su sucesor, es decir, el menor nodo del subárbol derecho.

## Recorrido que confirma el orden

El recorrido **inorden** confirma que el árbol sigue respetando la propiedad del BST, porque visita primero el subárbol izquierdo, luego la raíz y por último el subárbol derecho. Si el árbol está correcto, los nombres salen en orden alfabético.

## Checklist del taller

- Construcción inicial con 14 nombres mezclando carpetas y archivos.
- Intento de duplicado reportado.
- Impresión ASCII del árbol inicial.
- 6 búsquedas con número de comparaciones.
- 3 actualizaciones: hoja, nodo con un hijo y raíz.
- 3 eliminaciones: hoja, nodo con un hijo y raíz con dos hijos.
- Recorridos preorden, inorden, postorden y por niveles.
- Altura final del árbol.
