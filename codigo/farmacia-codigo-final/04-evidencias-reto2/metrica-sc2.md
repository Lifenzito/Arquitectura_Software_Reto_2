# Métrica de la SC-2 — Anexo B

**SC-2: venta de servicios de inyectología, curaciones básicas y cambios de
vendaje.**

Las clases `Inyectologia`, `CuracionBasica`, `CambioVendaje`, la abstracción
`Procedimiento` y sus tres fábricas **ya existían en el AS-IS**: no se cuentan
como clases nuevas. Lo que faltaba era su integración real, que es lo que se
midió.

## Qué se midió

Archivos que hay que tocar para que los tres procedimientos entren por el
flujo normal de carga y puedan venderse.

| | Arquitectura AS-IS | Arquitectura TO-BE |
| --- | --- | --- |
| Clases de dominio nuevas | 0 | 0 |
| Fábricas nuevas | 0 | 0 |
| **Clases de biblioteca modificadas** | **2** (`RepositoryProducto` para admitir un esquema de 4 columnas + `Program.cs` con el bloque manual del `case 8`) | **0** |
| Archivos de datos nuevos | 1 | 1 (`productos-sc2.txt`) |
| Líneas de ensamblaje en el Composition Root | bloque de creación manual con `new` y fábricas locales | 3 líneas de `RegistrarProducto` + 1 de carga |
| Archivos del proyecto tocados | `RepositoryProducto.cs`, `Program.cs`, `productos.txt` o fixture | `Program.cs`, `productos-sc2.txt`, `.csproj` |

## Resultado

**Cero clases de `BibFarmacia` modificadas.** La integración completa de la
SC-2 se resolvió con:

1. `AppFarmaciaConsola/productos-sc2.txt` — 3 filas de datos.
2. `AppFarmaciaConsola/Program.cs` — un segundo `RepositoryProducto`, tres
   registros `tipo → fábrica + esquema` y una llamada de carga.
3. `AppFarmaciaConsola.csproj` — copiar el fixture al directorio de salida.

En el AS-IS lo mismo exigía modificar `RepositoryProducto` (la selección de
fábrica y el parseo posicional asumían el esquema de seis columnas) y dejar en
`Program.cs` un bloque de `new` manual dentro del `case 8`.

## Por qué la cuenta cambia

El registro `tipo → (fábrica, esquema)` de Factory Method saca del repositorio
tanto la elección de la fábrica como el conocimiento de las columnas. Un tipo
de producto con un esquema distinto —cuatro columnas en vez de seis— deja de
ser un cambio en el repositorio y pasa a ser una línea de registro. Eso es lo
que hace que la SC-2 cueste 0 clases modificadas en lugar de 2.

## Comportamiento verificado

- La opción 8 (recorrido de demostración) muestra los mismos tres
  procedimientos, en el mismo orden y con los mismos precios y duraciones que
  antes: el caso 12 de caracterización sale **idéntico byte a byte**.
- La opción 10 vende un procedimiento y registra el movimiento.
- No se descuenta stock: `Procedimiento` no implementa `IProductoConStock`.
- No hay condicional de reserva que construya los procedimientos a mano: si el
  fixture falta, la lista queda vacía, igual que la carga parcial silenciosa
  del resto del sistema.
