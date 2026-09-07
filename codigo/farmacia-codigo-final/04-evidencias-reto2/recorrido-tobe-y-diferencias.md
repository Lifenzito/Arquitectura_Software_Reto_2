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

## 2. Diferencias con los documentos

Decisión del equipo: los documentos y el diagrama no se modifican. El código se
ajustó al TO-BE dibujado. Estado de las cinco diferencias detectadas:

### Diagrama TO-BE (`Actividad3-1B-TO-BE-completo-v2.drawio`)

| # | Elemento | Qué dice el diagrama | Estado | Cómo se resolvió |
| --- | --- | --- | --- | --- |
| D-1 | `IConvenio` | `NombreEntidad`, `TipoBeneficio`, `CalcularBeneficio` | **Corregido** | Se retiró `TipoEntidad` del contrato. Cada ConcreteStrategy sobrescribe `ToString()` con su propia etiqueta y `Program.cs` imprime `{cliente.Convenio}`: la salida `UPB (Universidad)` se conserva por polimorfismo, sin condicional central ni dependencia de clases concretas |
| D-2 | `RepositoryProducto` | solo `- fabricas: Dictionary` | **Corregido** | Se eliminó `RegistrarProducto`. El repositorio recibe por constructor el `Dictionary<string, IProductoFactory>` dibujado; el esquema de columnas queda como campo privado de configuración, sin tipo ni interfaz nuevos. OCP se mantiene: agregar un tipo es una entrada en cada mapa del Composition Root, el repositorio no cambia |
| D-3 | `IRepositoryProducto` | `CargarDesdeArchivo`, `ObtenerProductos` | **Corregido** | El contrato quedó con esos dos métodos exactos |
| D-4 | `ServicioNotificacion` | solo `EnviarNotificacion(mensaje)` | **Sin diferencia de contrato** | El `ConsoleColor` llega por constructor: es configuración de construcción del Composition Root, no un miembro del contrato dibujado. El diagrama no dibuja constructores de ninguna clase |
| D-5 | `IClienteFactory` / `IUsuarioFactory` | `Crear(datos: Dictionary<string,string>)` | **Corregido** | Las dos firmas quedaron exactamente así; `ClienteRepository` y `RepositoryUsuario` arman el diccionario al leer su archivo |
| D-6 | `productos-sc2.txt` | no aparece | **Sin diferencia arquitectónica** | Es un archivo de datos fixture de la SC-2, no un componente: no introduce clases, interfaces ni relaciones. El diagrama tampoco dibuja los archivos de evidencia |

Única diferencia estructural que subsiste: `Program.cs` instancia nueve tipos de
fábrica en doce objetos, porque las tres de procedimiento se registran también
en el repositorio de la SC-2. No cambia ningún contrato ni relación dibujada.

### Tabla 3.2 (`Actividad3-2-tabla-cambio-estructural.xlsx`)

| # | Fila | Qué dice | Qué quedó | Corrección |
| --- | --- | --- | --- | --- |
| T-1 | E-37 `ServicioNotificacion` | «Program.cs la instancia **una vez** y la suscribe a los cuatro eventos» | un solo tipo de ConcreteObserver con **cuatro instancias**, cada una con su `ConsoleColor` | Diferencia real que **no puede eliminarse**: con una sola instancia los cuatro eventos imprimirían del mismo color y cambiaría la salida observable. Se defiende en la sustentación como precisión de E-37, no como desvío del patrón |
| T-2 | E-37 | no menciona el prefijo `[NOTIFICACION]` en el TO-BE | el prefijo se retiró para preservar la salida | Declararlo explícitamente como cambio necesario de preservación |
| T-3 | E-27 `convenios.txt` | «lo lee cada ConcreteStrategy» | lo lee **solo** `Program.cs`, que inyecta el porcentaje | Corregir el lector: el Composition Root, no las estrategias |
| T-4 | E-20 a E-24 | «contiene su propia regla de descuento **y la lectura de su porcentaje**» | contiene la regla; el porcentaje llega por constructor | Quitar la lectura de las cinco filas |
| T-5 | E-13 `RepositoryProducto` | «guarda `fabricas: Dictionary<string, IProductoFactory>` y pide la fábrica al registro» | se cumple literalmente; el esquema de columnas es un campo privado adicional | **Sin corrección**: la tabla describe la relación arquitectónica, no los campos privados |
| T-6 | E-09 / E-11 | `Crear(datos: Dictionary<string,string>)` | se cumple literalmente | **Corregido en el código** |
| T-7 | E-16 `IConvenio` | sin `TipoEntidad` | se cumple literalmente | **Corregido en el código** |
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
| M-3 | Strategy / ISP | Sin corrección: `IConvenio` quedó con los tres miembros de la matriz |

Detalle celda por celda en `verificacion-solid.md`.

## 3. Deuda declarada que se conserva a propósito

No se corrigieron, porque hacerlo cambiaría la conducta observable:
venta con stock negativo, validadores creados pero no conectados,
contraseñas en texto plano, `int.Parse` sin validar en el menú
(`FormatException` con entrada no numérica), formato de los `.txt`, y las
opciones 8, 9 y 10 que no se listan en el menú.
