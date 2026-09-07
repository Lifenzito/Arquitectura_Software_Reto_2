# Comparación de comportamiento AS-IS vs TO-BE — Reto 2

Base de comparación: rama `linea-base-as-is` (código final del Reto 1, sin un
solo cambio) contra la rama de implementación del TO-BE.

## Tabla 1 — Casos heredados

Mismas entradas, misma redirección, comparación con `diff -u`.

| Caso | Resultado | Diferencias |
| --- | --- | --- |
| 01 arranque y login correcto | Idéntico | 0 |
| 02 login incorrecto | Idéntico | 0 |
| 03 listar productos | Idéntico | 0 |
| 04 buscar producto existente | Idéntico | 0 |
| 05 buscar producto inexistente | Idéntico | 0 |
| 06 buscar subcadena múltiple | Idéntico | 0 |
| 07 venta con stock suficiente | Idéntico | 0 |
| 08 venta con stock insuficiente | Idéntico | 0 |
| 09 acumular puntos | Idéntico | 0 |
| 10 ver alertas | Idéntico | 0 |
| 11 salir | Idéntico | 0 |
| 12 recorrido de demostración (opción 8) | Idéntico | 0 |

**12 de 12 idénticos.** El caso 12 es el más exigente: la opción 8 dejó de
construir los procedimientos con `new` y ahora los toma del repositorio
cargado desde `productos-sc2.txt`, y aun así imprime exactamente lo mismo, en
el mismo orden y con los mismos precios y duraciones.

## Tabla 2 — Casos nuevos

Estos casos no existen en el AS-IS: recorren opciones nuevas o funcionalidad
nueva, y su diferencia es un **cambio autorizado**, no una regresión.

| Caso | Recorre | Resultado |
| --- | --- | --- |
| 13 listado y búsqueda por fábrica | Factory Method: los 16 productos del catálogo se construyen desde el registro `tipo → fábrica + esquema` | Idéntico al AS-IS (la ruta cambia, la salida no) |
| 14 convenios de las cinco entidades | Strategy: opción 9, las cinco `IConvenio` con su porcentaje inyectado y un cliente sin convenio | Difiere: opción nueva |
| 15 los cuatro eventos | Observer: stock, vencimiento, puntos y movimiento con sus cuatro observadores | Idéntico al AS-IS |
| 16 SC-2 desde archivo | Opción 10: procedimientos cargados desde `productos-sc2.txt` y venta sin descuento de stock | Difiere: opción nueva |

## Color en consola real

La redirección a archivo elimina las secuencias ANSI. Se repitió la corrida
bajo pseudo-terminal (`script -qec`, `TERM=xterm-256color`) en ambas versiones:
las capturas son **idénticas byte a byte incluyendo los códigos de color**
(`ESC[91m` rojo para stock, `ESC[93m` amarillo para vencimiento, reset al
final de cada bloque). Los cuatro `ServicioNotificacion` con `ConsoleColor`
por constructor reproducen exactamente lo que hacían las lambdas del AS-IS.

## Segunda corrida: ajuste de fidelidad al TO-BE

Tras alinear el código con el diagrama (se retiró `TipoEntidad` de `IConvenio`,
`RepositoryProducto` volvió a recibir el `Dictionary<string, IProductoFactory>`
por constructor sin `RegistrarProducto`, y `IClienteFactory`/`IUsuarioFactory`
pasaron a `Crear(Dictionary<string,string>)`) se repitieron las dos tablas:

- Compilación: 0 errores, 0 advertencias.
- Casos heredados: **12 de 12 idénticos** al AS-IS.
- Casos nuevos: los cuatro dan salida **byte a byte igual** a la de la primera
  corrida del TO-BE; el ajuste no movió una sola línea.
- Color bajo pseudo-terminal en los casos 01, 10 y 12: idéntico al AS-IS.

El caso 12 vuelve a ser el crítico: sigue imprimiendo
`Cliente: Carlos - Convenio: UPB (Universidad)`, ahora resuelto por el
`ToString()` de `ConvenioUniversidad` en vez de una propiedad del contrato.

## Conclusión

La refactorización a Factory Method, Strategy y Observer **preserva el
comportamiento observable al 100 %**: texto, orden, formato, color y hasta el
stock negativo de la venta excesiva. Las únicas diferencias provienen de las
dos opciones nuevas de demostración (9 y 10) y de la SC-2, no del rediseño.
