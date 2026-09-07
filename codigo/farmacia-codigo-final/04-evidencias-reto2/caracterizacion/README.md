# Caracterización AS-IS / TO-BE — Reto 2

Evidencia del Entregable 4.2. Compara la salida de consola del código AS-IS
(rama `linea-base-as-is`) contra el código refactorizado con Factory Method,
Strategy, Observer y la SC-2.

## Estructura

| Carpeta | Contenido |
| --- | --- |
| `entradas/` | Un `.in` por caso, con las teclas que se digitan |
| `as-is/` | Salidas capturadas sobre la línea base sin modificar |
| `tobe/` | Salidas capturadas sobre el código refactorizado |
| `diff/` | `diff -u` de cada par; archivo vacío = salidas idénticas |

## Cómo se ejecuta

```bash
cd codigo/farmacia-codigo-final
./04-evidencias-reto2/caracterizacion/ejecutar-casos.sh tobe
```

El script compila en Release, copia los `.txt` al directorio de salida y
ejecuta cada entrada con redirección:

```bash
dotnet AppFarmaciaConsola.dll < entradas/caso-NN.in > tobe/caso-NN.out 2>&1
```

Para regenerar las salidas AS-IS hay que situarse en un worktree de la rama
`linea-base-as-is` y ejecutar el mismo script con el destino `as-is`.

## Casos

### Heredados del Reto 1 (deben quedar idénticos)

| Caso | Qué ejercita |
| --- | --- |
| 01 | Arranque, login correcto, alertas de stock y de vencimiento |
| 02 | Login con credenciales incorrectas |
| 03 | Listar todos los productos |
| 04 | Buscar un producto existente |
| 05 | Buscar un producto inexistente |
| 06 | Buscar por subcadena con varias coincidencias |
| 07 | Venta con stock suficiente |
| 08 | Venta con stock insuficiente (queda negativo) |
| 09 | Acumular puntos a un cliente |
| 10 | Ver alertas desde el menú |
| 11 | Salir |
| 12 | Recorrido de demostración (opción 8, no listada en el menú) |

### Nuevos del Reto 2 (cambio autorizado)

| Caso | Patrón recorrido |
| --- | --- |
| 13 | Factory Method: catálogo construido por las siete fábricas desde el registro |
| 14 | Strategy: las cinco entidades de convenio y el cliente sin convenio (opción 9) |
| 15 | Observer: los cuatro eventos con sus cuatro `ServicioNotificacion` |
| 16 | SC-2: procedimientos cargados desde `productos-sc2.txt` y vendidos (opción 10) |

## Verificación de color en consola real

La redirección a archivo suprime las secuencias ANSI, así que el color se
verificó por separado ejecutando ambas versiones bajo una pseudo-terminal:

```bash
script -qec "TERM=xterm-256color dotnet AppFarmaciaConsola.dll < entrada.in" /dev/null
```

Las dos capturas resultan **idénticas byte a byte, secuencias de color
incluidas**: `ESC[91m` (rojo) en las alertas de stock, `ESC[93m` (amarillo) en
las de vencimiento, y `ESC[0m` de reset después de cada bloque.
