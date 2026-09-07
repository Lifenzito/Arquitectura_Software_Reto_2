# Recorrido del TO-BE elemento por elemento y diferencias documentales

## 1. Correspondencia con el diagrama TO-BE

Fuente: `diagramas/Actividad3-1B-TO-BE-completo-v2.drawio`.

### Interfaces (15 en el diagrama)

`IClienteFactory`, `IClienteRepository`, `IConvenio`, `IEvento`,
`IMovimientoRepository`, `IProductoConStock`, `IProductoFactory`,
`IRepositoryProducto`, `IRepositoryUsuario`, `IServicioAutenticacion`,
`IServicioNotificacion`, `IUsuarioFactory`, `IValidador`, `IVencimiento`,
`IVerificador`.

**Las 15 existen en el código con el mismo nombre.**

### Enumeraciones (3)

`MaterialEnvase`, `TipoBeneficio`, `TipoRelleno`. **Las 3 existen.**
`TipoConvenio` fue eliminado, como exige el TO-BE.

### Clases abstractas (4)

`Persona`, `Procedimiento`, `Producto`, `Proveedor`. **Las 4 existen.**

### Clases concretas

Dominio: `Cliente`, `Usuario`, `Movimiento`, `Medicamento`,
`MedicamentoCapsula`, `MedicamentoLiquido`, `Cosmetico`, `Comestible`,
`Inyectologia`, `CuracionBasica`, `CambioVendaje`, `Laboratorio`, `Marca`.
Fábricas: las siete de producto más `ClienteFactory` y `UsuarioFactory`.
Repositorios: `RepositoryProducto`, `ClienteRepository`, `RepositoryUsuario`,
`MovimientoRepository`.
Servicios: `ServicioProducto`, `ServicioCliente`, `ServicioUsuario`,
`ServicioMovimiento`, `ServicioAutenticacion`, `ServicioDescuento`,
`ServicioNotificacion`.
Convenios: `ConvenioEmpresa`, `ConvenioBanco`, `ConvenioCooperativa`,
`ConvenioMutual`, `ConvenioUniversidad`.
Eventos: `EventoStockMinimo`, `EventoVencimiento`, `EventoPuntos`,
`EventoMovimiento`.
Verificadores y validadores: `VerificadorStock`, `VerificadorVencimiento`,
`ValidadorCliente`, `ValidadorProducto`.

**Todas existen con el mismo nombre.**

### Elementos del diagrama que NO quedaron en el código

Ninguno.

### Elementos del código que NO están en el diagrama

Uno solo: el archivo de datos `AppFarmaciaConsola/productos-sc2.txt`, fixture
de la SC-2. No se creó ninguna clase ni interfaz auxiliar: el esquema de
columnas se representa con `string[]`, no con un tipo nuevo.

## 2. Diferencias que deben corregirse en los documentos

Ordenadas por documento. Ninguna es un defecto del código: todas provienen de
decisiones tomadas y aprobadas durante la implementación.

### Diagrama TO-BE (`Actividad3-1B-TO-BE-completo-v2.drawio`)

| # | Elemento | Qué dice el diagrama | Qué quedó en el código | Corrección |
| --- | --- | --- | --- | --- |
| D-1 | `IConvenio` | `NombreEntidad`, `TipoBeneficio`, `CalcularBeneficio` | añade `TipoEntidad: string` | Agregar la propiedad. Sin ella no se puede imprimir `UPB (Universidad)` sin resucitar `TipoConvenio` |
| D-2 | `RepositoryProducto` | solo `- fabricas: Dictionary` | añade `- esquemas: Dictionary<string,string[]>` y `+ RegistrarProducto(tipo, fabrica, esquema)` | Agregar campo y método |
| D-3 | `IRepositoryProducto` | `CargarDesdeArchivo`, `ObtenerProductos` | añade `RegistrarProducto(tipo, fabrica, esquema)` | Agregar el método al contrato |
| D-4 | `ServicioNotificacion` | solo `EnviarNotificacion(mensaje)` | añade `- color: ConsoleColor` y constructor que la recibe | Agregar campo y constructor |
| D-5 | `IClienteFactory` / `IUsuarioFactory` | `Crear(datos: Dictionary<string,string>)` | `Crear(nombre, cedula, telefono, correo[, usuario, contrasena])` | Corregir la firma: clientes y usuarios tienen esquema fijo, el diccionario solo aporta ruido |
| D-6 | «archivo» | `productos.txt`, `clientes.txt`, `usuarios.txt`, `convenios.txt` | añade `productos-sc2.txt` | Agregar el nodo y su flecha «lee» hacia el segundo `RepositoryProducto` |
| D-7 | `Program.cs` | «crea las 9 fábricas» | 9 tipos de fábrica, 12 instancias: las tres de procedimiento se registran también en el repositorio de la SC-2 | Precisar «nueve tipos de fábrica; las de procedimiento se registran en dos repositorios» |

### Tabla 3.2 (`Actividad3-2-tabla-cambio-estructural.xlsx`)

| # | Fila | Qué dice | Qué quedó | Corrección |
| --- | --- | --- | --- | --- |
| T-1 | E-37 `ServicioNotificacion` | «Program.cs la instancia **una vez** y la suscribe a los cuatro eventos» | un solo tipo de ConcreteObserver con **cuatro instancias**, cada una con su `ConsoleColor` | Reescribir: cuatro instancias configuradas (rojo stock, amarillo vencimiento, verde puntos, cian movimiento) |
| T-2 | E-37 | no menciona el prefijo `[NOTIFICACION]` en el TO-BE | el prefijo se retiró para preservar la salida | Declararlo explícitamente como cambio necesario de preservación |
| T-3 | E-27 `convenios.txt` | «lo lee cada ConcreteStrategy» | lo lee **solo** `Program.cs`, que inyecta el porcentaje | Corregir el lector: el Composition Root, no las estrategias |
| T-4 | E-20 a E-24 | «contiene su propia regla de descuento **y la lectura de su porcentaje**» | contiene la regla; el porcentaje llega por constructor | Quitar la lectura de las cinco filas |
| T-5 | E-13 `RepositoryProducto` | no menciona el registro de esquemas | tiene `RegistrarProducto(tipo, fabrica, esquema)` y el diccionario de esquemas | Añadir el mecanismo de registro |
| T-6 | E-09 / E-11 | `Crear(datos: Dictionary<string,string>)` | firma con parámetros explícitos | Corregir la firma |
| T-7 | E-16 `IConvenio` | sin `TipoEntidad` | con `TipoEntidad` | Añadir la propiedad |
| T-8 | E-38 `Program.cs` | no menciona la SC-2 | registra un segundo `RepositoryProducto` y lo carga desde `productos-sc2.txt` | Añadir el ensamblaje de la SC-2 |

### Fichas 3.3 (`Actividad3-3-fichas-patrones.md`)

| # | Ficha | Corrección |
| --- | --- | --- |
| F-1 | Strategy | Dice que el porcentaje «debe leerse desde un solo sitio»: ya se cumple, es `Program.cs`. Actualizar de deuda declarada a decisión implementada |
| F-2 | Factory Method | Documentar la opción adoptada para el diccionario: esquema de columnas registrado junto a la fábrica; `ColumnasSinTipo = 6` y `TipoPorDefecto = "medicamento_capsula"` para las filas heredadas sin discriminador |
| F-3 | Observer | Documentar las cuatro instancias con color por constructor y la desaparición del prefijo `[NOTIFICACION]` |

### Matriz 4.1 (`Actividad4-1-matriz-verificacion-SOLID.xlsx`)

| # | Celda | Corrección |
| --- | --- | --- |
| M-1 | Factory Method / OCP | La tensión declarada («la conversión a claves aún no está definida») quedó resuelta: el esquema vive en el registro, no en el repositorio. Reescribir la evidencia |
| M-2 | Strategy / SRP | La tensión declarada (la lectura repartida entre las cinco estrategias) no ocurrió: ninguna estrategia abre el archivo. Puede pasar a «Refuerza» |
| M-3 | Strategy / ISP | Añadir `TipoEntidad` a la lista de miembros de `IConvenio` |

Detalle celda por celda en `verificacion-solid.md`.

## 3. Deuda declarada que se conserva a propósito

No se corrigieron, porque hacerlo cambiaría la conducta observable:
venta con stock negativo, validadores creados pero no conectados,
contraseñas en texto plano, `int.Parse` sin validar en el menú
(`FormatException` con entrada no numérica), formato de los `.txt`, y las
opciones 8, 9 y 10 que no se listan en el menú.
