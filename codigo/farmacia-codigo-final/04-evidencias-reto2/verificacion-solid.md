# Verificación SOLID contra el código implementado

Se recorre celda por celda la matriz del Entregable 4.1
(`entregables/Actividad4-1-matriz-verificacion-SOLID.xlsx`) y se confronta con
el código ya implementado. Las rutas son relativas a
`codigo/farmacia-codigo-final/`.

## Factory Method

| Principio | Matriz 4.1 | Verificación en el código | Estado |
| --- | --- | --- | --- |
| SRP | Refuerza | `ClienteRepository` lee el archivo y delega la construcción en `clienteFactory.Crear(...)` (`BibFarmacia/Repositorios/ClienteRepository.cs:62`); `RepositoryUsuario` igual (`RepositoryUsuario.cs:64`). Ninguno ejecuta ya `new Cliente(...)` ni `new Usuario(...)`. | Confirmado |
| OCP | Tensionado pero compensado | `RepositoryProducto` resuelve por clave (`fabricas[tipo]`, `RepositoryProducto.cs:98`) sin `switch`, sin cadena de `if` y sin `First(f => f.Tipo == tipo)`. **La tensión que la matriz dejaba abierta quedó resuelta**: el esquema de columnas se registra junto a la fábrica en el Composition Root (`AppFarmaciaConsola/Program.cs:53-90`), así que agregar un tipo cuesta clase + fábrica + una línea de registro, y el repositorio no se toca. | Confirmado y mejorado |
| LSP | Tensionado pero compensado | Sigue siendo cierto: cada fábrica exige claves distintas del diccionario y un diccionario incompleto falla en ejecución, no al compilar. Se compensa porque la única fuente de claves es el esquema registrado junto a la fábrica: quien registra el par no puede desalinearlos por accidente, y una clave duplicada se rechaza con excepción (`RepositoryProducto.cs:38-52`). | Confirmado |
| ISP | Refuerza | `IProductoFactory` quedó con un solo método y sin la propiedad `Tipo` (`BibFarmacia/Interfaces/IProductoFactory.cs`). `IClienteFactory` e `IUsuarioFactory` declaran un método cada una. | Confirmado |
| DIP | Refuerza | Los tres repositorios dependen de abstracciones de fábrica recibidas por constructor (`ClienteRepository.cs:15-22`, `RepositoryUsuario.cs:15-22`, `RepositoryProducto.cs:38`). Las concretas solo se nombran en `Program.cs`. | Confirmado |

## Strategy

| Principio | Matriz 4.1 | Verificación en el código | Estado |
| --- | --- | --- | --- |
| SRP | Tensionado pero compensado | **Mejor de lo previsto**: la matriz temía que la lectura de `convenios.txt` se repartiera entre las cinco estrategias. En el código ninguna estrategia abre el archivo: `Program.cs` lo lee una sola vez (`Program.cs:164-167`, `LeerPorcentajesConvenio`) e inyecta el porcentaje por constructor. Cada estrategia tiene un solo motivo de cambio: su regla de cálculo. | Corrige la matriz |
| OCP | Refuerza | Una entidad nueva es una clase que implementa `IConvenio` más una línea en `Program.cs`; no hay clase base ni enumeración que tocar (`TipoConvenio` fue eliminado). | Confirmado |
| LSP | Refuerza | Los cinco convenios se sustituyen entre sí desde `Cliente.Convenio` y `ServicioDescuento` no nota diferencia (`ServicioDescuento.cs:18-26`). Ninguno implementa un `AplicaA` que no le corresponda. | Confirmado |
| ISP | Refuerza | `IConvenio` declara `NombreEntidad`, `TipoEntidad`, `TipoBeneficio` y `CalcularBeneficio` (`Interfaces/IConvenio.cs:13-19`). **Diferencia con la matriz y el diagrama**: se añadió `TipoEntidad` para preservar la salida `UPB (Universidad)` sin resucitar `TipoConvenio`. | Confirmado con ajuste |
| DIP | Refuerza | `Cliente.Convenio` es `IConvenio?` y `ServicioDescuento` ya no recibe `List<IEntidadConvenio>`: delega en la abstracción que trae el cliente. | Confirmado |

## Observer

| Principio | Matriz 4.1 | Verificación en el código | Estado |
| --- | --- | --- | --- |
| SRP | Refuerza | `ServicioCliente` y `ServicioMovimiento` reciben su evento por constructor y lo guardan privado; ya no lo crean ni lo exponen. | Confirmado |
| OCP | Refuerza | Los eventos publican con `event`/`delegate`; agregar un observador es suscribir otra `IServicioNotificacion` en `Program.cs`, sin tocar el evento ni el verificador. | Confirmado |
| LSP | Tensionado pero compensado | Sigue siendo cierto: `IEvento.Disparar(object)` se conservó intacto y cada evento castea por dentro, así que un emparejamiento equivocado no lo detecta el compilador. Se compensa porque el único punto donde se emparejan evento y verificador es el Composition Root. | Confirmado |
| ISP | Neutro | `IServicioNotificacion` tiene un solo método y todos los observadores lo usan. | Confirmado |
| DIP | Refuerza | `VerificadorStock` y `VerificadorVencimiento` dependen de `IEvento` (`Verificadores/VerificadorStock.cs:14-19`), y los cuatro observadores se referencian como `IServicioNotificacion`, nunca como `ServicioNotificacion`. | Confirmado |

## Resumen

15 celdas verificadas. Ninguna se degradó. Dos quedaron mejor que lo previsto
—Factory Method/OCP y Strategy/SRP— y una requiere ajuste documental:
Strategy/ISP, por la propiedad `TipoEntidad` añadida a `IConvenio`.
