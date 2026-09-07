using BibFarmacia.Clases;
using BibFarmacia.Convenios;
using BibFarmacia.Enum;
using BibFarmacia.Eventos;
using BibFarmacia.Factories;
using BibFarmacia.Interfaces;
using BibFarmacia.Repositorios;
using BibFarmacia.Servicios;
using BibFarmacia.Verificadores;

Console.Title = "Sistema Farmacia";

// Esquemas de columnas de productos.txt. Agregar un tipo nuevo solo exige su
// clase, su fabrica y una linea de registro aqui: RepositoryProducto no cambia.
string[] esquemaConStock =
    new[]
    {
        "nombre",
        "precio",
        "stock",
        "stockMinimo",
        "fechaVencimiento",
        "proveedor"
    };

string[] esquemaMedicamentoLiquido =
    new[]
    {
        "nombre",
        "precio",
        "stock",
        "stockMinimo",
        "fechaVencimiento",
        "proveedor",
        "materialEnvase",
        "mililitros"
    };

string[] esquemaProcedimiento =
    new[]
    {
        "nombre",
        "precio",
        "duracionMinutos",
        "proveedor"
    };

// Registro de fabricas por discriminador y su esquema de columnas. Agregar
// un tipo nuevo solo exige su clase, su fabrica y una linea en cada mapa:
// RepositoryProducto no cambia.
Dictionary<string, IProductoFactory> fabricasProducto =
    new Dictionary<string, IProductoFactory>
    {
        ["medicamento_capsula"] = new MedicamentoCapsulaFactory(),
        ["medicamento_liquido"] = new MedicamentoLiquidoFactory(),
        ["cosmetico"] = new CosmeticoFactory(),
        ["comestible"] = new ComestibleFactory(),
        ["inyectologia"] = new InyectologiaFactory(),
        ["curacion_basica"] = new CuracionBasicaFactory(),
        ["cambio_vendaje"] = new CambioVendajeFactory()
    };

Dictionary<string, string[]> esquemasProducto =
    new Dictionary<string, string[]>
    {
        ["medicamento_capsula"] = esquemaConStock,
        ["medicamento_liquido"] = esquemaMedicamentoLiquido,
        ["cosmetico"] = esquemaConStock,
        ["comestible"] = esquemaConStock,
        ["inyectologia"] = esquemaProcedimiento,
        ["curacion_basica"] = esquemaProcedimiento,
        ["cambio_vendaje"] = esquemaProcedimiento
    };

IRepositoryProducto repositoryProducto =
    new RepositoryProducto(
        fabricasProducto,
        esquemasProducto);

// SC-2: los procedimientos entran por el mismo flujo de carga y por el mismo
// tipo de repositorio, con su propio fixture para no alterar productos.txt.
Dictionary<string, IProductoFactory> fabricasProcedimiento =
    new Dictionary<string, IProductoFactory>
    {
        ["inyectologia"] = new InyectologiaFactory(),
        ["curacion_basica"] = new CuracionBasicaFactory(),
        ["cambio_vendaje"] = new CambioVendajeFactory()
    };

Dictionary<string, string[]> esquemasProcedimiento =
    new Dictionary<string, string[]>
    {
        ["inyectologia"] = esquemaProcedimiento,
        ["curacion_basica"] = esquemaProcedimiento,
        ["cambio_vendaje"] = esquemaProcedimiento
    };

IRepositoryProducto repositoryProcedimiento =
    new RepositoryProducto(
        fabricasProcedimiento,
        esquemasProcedimiento);

repositoryProcedimiento.CargarDesdeArchivo(
    "productos-sc2.txt");

IClienteRepository clienteRepository =
    new ClienteRepository(
        new ClienteFactory());

IRepositoryUsuario repositoryUsuario =
    new RepositoryUsuario(
        new UsuarioFactory());

IMovimientoRepository movimientoRepository =
    new MovimientoRepository();

EventoStockMinimo eventoStock =
    new EventoStockMinimo();

EventoVencimiento eventoVencimiento =
    new EventoVencimiento();

EventoPuntos eventoPuntos =
    new EventoPuntos();

EventoMovimiento eventoMovimiento =
    new EventoMovimiento();

List<IVerificador> verificadores =
    new List<IVerificador>
    {
        new VerificadorStock(eventoStock),
        new VerificadorVencimiento(
            eventoVencimiento)
    };

ServicioCliente servicioCliente =
    new ServicioCliente(
        clienteRepository,
        eventoPuntos);

ServicioUsuario servicioUsuario =
    new ServicioUsuario(
        repositoryUsuario);

ServicioMovimiento servicioMovimiento =
    new ServicioMovimiento(
        movimientoRepository,
        eventoMovimiento);

IServicioAutenticacion servicioAutenticacion =
    new ServicioAutenticacion(
        repositoryUsuario);

// Los porcentajes son un parametro comercial: se leen una sola vez aqui,
// en el Composition Root, y se inyectan a cada estrategia.
Dictionary<string, decimal> porcentajesConvenio =
    LeerPorcentajesConvenio("convenios.txt");

IConvenio convenioUniversidad =
    new ConvenioUniversidad(
        "UPB",
        Porcentaje("Universidad"));

List<IConvenio> convenios =
    new List<IConvenio>
    {
        new ConvenioEmpresa(
            "Sofka",
            Porcentaje("Empresa")),
        new ConvenioBanco(
            "Bancolombia",
            Porcentaje("Banco")),
        new ConvenioCooperativa(
            "Coomeva",
            Porcentaje("Cooperativa")),
        new ConvenioMutual(
            "Mutual Ser",
            Porcentaje("Mutual")),
        convenioUniversidad
    };

ServicioDescuento servicioDescuento =
    new ServicioDescuento();

decimal Porcentaje(string tipoEntidad)
{
    return porcentajesConvenio.TryGetValue(
        tipoEntidad,
        out decimal porcentaje)
        ? porcentaje
        : 0;
}

static Dictionary<string, decimal> LeerPorcentajesConvenio(
    string ruta)
{
    Dictionary<string, decimal> porcentajes =
        new Dictionary<string, decimal>();

    try
    {
        if (!File.Exists(ruta))
        {
            return porcentajes;
        }

        foreach (string linea in
            File.ReadAllLines(ruta))
        {
            string[] datos =
                linea.Split(';');

            porcentajes[datos[0]] =
                decimal.Parse(datos[1]);
        }

        return porcentajes;
    }
    catch (Exception)
    {
        return porcentajes;
    }
}

List<IEvento> eventos =
    new List<IEvento>
    {
        eventoStock,
        eventoVencimiento,
        eventoPuntos,
        eventoMovimiento
    };

ServicioProducto servicioProducto =
    new ServicioProducto(
        repositoryProducto,
        verificadores,
        eventos);

// ================= EVENTOS =================

IServicioNotificacion notificacionStock =
    new ServicioNotificacion(
        ConsoleColor.Red);

IServicioNotificacion notificacionVencimiento =
    new ServicioNotificacion(
        ConsoleColor.Yellow);

IServicioNotificacion notificacionPuntos =
    new ServicioNotificacion(
        ConsoleColor.Green);

IServicioNotificacion notificacionMovimiento =
    new ServicioNotificacion(
        ConsoleColor.Cyan);

eventoStock.StockMinimo +=
    notificacionStock.EnviarNotificacion;

eventoVencimiento.Vencimiento +=
    notificacionVencimiento.EnviarNotificacion;

eventoPuntos.PuntosAcumulados +=
    notificacionPuntos.EnviarNotificacion;

eventoMovimiento.MovimientoRegistrado +=
    notificacionMovimiento.EnviarNotificacion;

// ================= CARGA TXT =================

Console.ForegroundColor =
    ConsoleColor.DarkGreen;

Console.WriteLine(
    "Cargando información del sistema...\n");

Console.ResetColor();

Console.WriteLine(
    servicioProducto.CargarDesdeArchivo(
        "productos.txt"));

Console.WriteLine(
    servicioCliente.Cargar(
        "clientes.txt"));

Console.WriteLine(
    servicioUsuario.Cargar(
        "usuarios.txt"));

Console.WriteLine();

// ================= LOGIN =================

Console.ForegroundColor =
    ConsoleColor.Blue;

Console.WriteLine(
    "=========== LOGIN ===========");

Console.ResetColor();

Console.Write("Usuario: ");
string user =
    Console.ReadLine()!;

Console.Write("Contraseña: ");
string password =
    Console.ReadLine()!;

bool login =
    servicioAutenticacion.Login(
        user,
        password);

if (!login)
{
    Console.ForegroundColor =
        ConsoleColor.Red;

    Console.WriteLine(
        "\nAcceso denegado");

    Console.ResetColor();

    return;
}

Console.ForegroundColor =
    ConsoleColor.Green;

Console.WriteLine(
    "\nLogin correcto");

Console.ResetColor();

// ================= ALERTAS =================

servicioProducto.Verificar();

// ================= MENÚ =================

int opcion = 0;

while (opcion != 7)
{
    Console.ForegroundColor =
        ConsoleColor.Magenta;

    Console.WriteLine("\n==============================");
    Console.WriteLine("      SISTEMA FARMACIA");
    Console.WriteLine("==============================");

    Console.ResetColor();

    Console.WriteLine("1. Ver productos");
    Console.WriteLine("2. Ver clientes");
    Console.WriteLine("3. Buscar producto");
    Console.WriteLine("4. Registrar venta");
    Console.WriteLine("5. Acumular puntos");
    Console.WriteLine("6. Ver alertas");
    Console.WriteLine("7. Salir");

    Console.Write("\nSeleccione opción: ");

    opcion =
        int.Parse(Console.ReadLine()!);

    switch (opcion)
    {
        case 1:

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n===== PRODUCTOS =====");

            Console.ResetColor();

            Console.WriteLine(
                "Nombre\t\tStock\tPrecio");

            Console.WriteLine(
                "-----------------------------------");

            foreach (var producto in
                servicioProducto.ObtenerProductos())
            {
                Console.WriteLine(
                    $"{producto.Nombre}\t\t" +
                    $"{(producto as IProductoConStock)?.Stock}\t" +
                    $"{producto.Precio}");
            }

            break;

        case 2:

            Console.ForegroundColor =
                ConsoleColor.Green;

            Console.WriteLine(
                "\n===== CLIENTES =====");

            Console.ResetColor();

            foreach (var cliente in
                servicioCliente.ObtenerClientes())
            {
                Console.WriteLine(
                    $"{cliente.Nombre} - " +
                    $"Puntos: {cliente.Puntos}");
            }

            break;

        case 3:

            Console.Write(
                "\nIngrese nombre producto: ");

            string nombre =
                Console.ReadLine()!;

            var productoBuscado =
                servicioProducto
                .ObtenerProductos()
                .FirstOrDefault(p =>
                    p.Nombre.ToLower()
                    .Contains(nombre.ToLower()));

            if (productoBuscado != null)
            {
                Console.WriteLine(
                    $"\nProducto: " +
                    $"{productoBuscado.Nombre}");

                Console.WriteLine(
                    $"Precio: " +
                    $"{productoBuscado.Precio}");

                Console.WriteLine(
                    $"Stock: " +
                    $"{(productoBuscado as IProductoConStock)?.Stock}");
            }
            else
            {
                Console.WriteLine(
                    "\nProducto no encontrado");
            }

            break;

        case 4:

            Console.Write(
                "\nNombre producto: ");

            string nombreVenta =
                Console.ReadLine()!;

            var productoVenta =
                servicioProducto
                .ObtenerProductos()
                .FirstOrDefault(p =>
                    p.Nombre.ToLower()
                    .Contains(
                        nombreVenta.ToLower()));

            if (productoVenta != null)
            {
                Console.Write(
                    "Cantidad: ");

                int cantidad =
                    int.Parse(
                        Console.ReadLine()!);

                if (productoVenta is
                    IProductoConStock productoConStock)
                {
                    productoConStock.Stock -=
                        cantidad;
                }

                Movimiento venta =
                    new Movimiento(
                        DateTime.Now,
                        cantidad,
                        "Venta",
                        productoVenta);

                servicioMovimiento
                    .RegistrarMovimiento(
                        venta);

                Console.WriteLine(
                    "\nVenta registrada");
            }
            else
            {
                Console.WriteLine(
                    "\nProducto no encontrado");
            }

            break;

        case 5:

            Console.Write(
                "\nNombre cliente: ");

            string nombreCliente =
                Console.ReadLine()!;

            var clientePuntos =
                servicioCliente
                .ObtenerClientes()
                .FirstOrDefault(c =>
                    c.Nombre.ToLower()
                    .Contains(
                        nombreCliente.ToLower()));

            if (clientePuntos != null)
            {
                Console.Write(
                    "Puntos: ");

                int puntos =
                    int.Parse(
                        Console.ReadLine()!);

                servicioCliente
                    .AcumularPuntos(
                        clientePuntos,
                        puntos);
            }
            else
            {
                Console.WriteLine(
                    "\nCliente no encontrado");
            }

            break;

        case 6:

            Console.WriteLine(
                "\nVerificando alertas...");

            servicioProducto
                .Verificar();

            break;

        case 8:

            // Recorrido de demostracion. No se lista en el menu para no
            // alterar la salida observable de las opciones originales.

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n===== DEMOSTRACIÓN =====");

            Console.ResetColor();

            List<Procedimiento> procedimientos =
                repositoryProcedimiento
                .ObtenerProductos()
                .OfType<Procedimiento>()
                .ToList();

            Console.WriteLine(
                "\n--- Catálogo por tipo ---");

            foreach (var producto in
                servicioProducto.ObtenerProductos())
            {
                Console.WriteLine(
                    $"{producto.GetType().Name}\t" +
                    $"{producto.Nombre}\t" +
                    $"{producto.Precio}\t" +
                    $"Proveedor: " +
                    $"{producto.Proveedor.Nombre}");
            }

            foreach (var procedimiento in
                procedimientos)
            {
                Console.WriteLine(
                    $"{procedimiento.GetType().Name}\t" +
                    $"{procedimiento.Nombre}\t" +
                    $"{procedimiento.Precio}\t" +
                    $"Duración: " +
                    $"{procedimiento.DuracionMinutos} min");
            }

            Console.WriteLine(
                "\n--- Venta de producto con stock ---");

            var productoDemo =
                servicioProducto
                .ObtenerProductos()
                .OfType<IProductoConStock>()
                .First();

            Console.WriteLine(
                $"Stock antes: " +
                $"{productoDemo.Stock}");

            productoDemo.Stock -= 1;

            servicioMovimiento
                .RegistrarMovimiento(
                    new Movimiento(
                        DateTime.Now,
                        1,
                        "Venta",
                        (Producto)productoDemo));

            Console.WriteLine(
                $"Stock después: " +
                $"{productoDemo.Stock}");

            Console.WriteLine(
                "\n--- Venta de procedimiento ---");

            Procedimiento procedimientoDemo =
                procedimientos[0];

            servicioMovimiento
                .RegistrarMovimiento(
                    new Movimiento(
                        DateTime.Now,
                        1,
                        "Venta",
                        procedimientoDemo));

            Console.WriteLine(
                $"{procedimientoDemo.Nombre} no " +
                $"implementa IProductoConStock: " +
                $"no hay stock que descontar");

            Console.WriteLine(
                "\n--- Descuento por convenio ---");

            var clienteDemo =
                servicioCliente
                .ObtenerClientes()
                .First();

            clienteDemo.Convenio =
                convenioUniversidad;

            decimal descuento =
                servicioDescuento
                .CalcularDescuento(
                    procedimientoDemo.Precio,
                    clienteDemo);

            Console.WriteLine(
                $"Cliente: {clienteDemo.Nombre} - " +
                $"Convenio: {clienteDemo.Convenio}");

            Console.WriteLine(
                $"Precio: {procedimientoDemo.Precio} - " +
                $"Descuento: {descuento} - " +
                $"Total: " +
                $"{procedimientoDemo.Precio - descuento}");

            break;

        case 10:

            // Demostracion de SC-2. Tampoco se lista en el menu.

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n===== DEMOSTRACIÓN SC-2 =====");

            Console.ResetColor();

            Console.WriteLine(
                "\n--- Procedimientos cargados desde " +
                "productos-sc2.txt ---");

            foreach (Producto procedimientoCargado in
                repositoryProcedimiento
                .ObtenerProductos())
            {
                Console.WriteLine(
                    $"{procedimientoCargado.GetType().Name}\t" +
                    $"{procedimientoCargado.Nombre}\t" +
                    $"{procedimientoCargado.Precio}\t" +
                    $"Proveedor: " +
                    $"{procedimientoCargado.Proveedor.Nombre}\t" +
                    $"Duración: " +
                    $"{((Procedimiento)procedimientoCargado).DuracionMinutos} min");
            }

            Console.WriteLine(
                "\n--- Venta de procedimiento cargado ---");

            Producto procedimientoSc2 =
                repositoryProcedimiento
                .ObtenerProductos()
                .First();

            servicioMovimiento
                .RegistrarMovimiento(
                    new Movimiento(
                        DateTime.Now,
                        1,
                        "Venta",
                        procedimientoSc2));

            Console.WriteLine(
                $"{procedimientoSc2.Nombre} no " +
                $"implementa IProductoConStock: " +
                $"no hay stock que descontar");

            break;

        case 9:

            // Demostracion de Strategy. Tampoco se lista en el menu para no
            // alterar la salida observable de las opciones originales.

            Console.ForegroundColor =
                ConsoleColor.Cyan;

            Console.WriteLine(
                "\n===== DEMOSTRACIÓN CONVENIOS =====");

            Console.ResetColor();

            Cliente clienteConvenios =
                servicioCliente
                .ObtenerClientes()
                .First();

            decimal precioBase = 100000m;

            foreach (IConvenio convenio in convenios)
            {
                clienteConvenios.Convenio =
                    convenio;

                decimal beneficio =
                    servicioDescuento
                    .CalcularDescuento(
                        precioBase,
                        clienteConvenios);

                Console.WriteLine(
                    $"{convenio}\t" +
                    $"{convenio.TipoBeneficio}\t" +
                    $"Precio: {precioBase}\t" +
                    $"Descuento: {beneficio}\t" +
                    $"Total: {precioBase - beneficio}");
            }

            clienteConvenios.Convenio = null;

            decimal sinConvenio =
                servicioDescuento
                .CalcularDescuento(
                    precioBase,
                    clienteConvenios);

            Console.WriteLine(
                $"Sin convenio\t" +
                $"Precio: {precioBase}\t" +
                $"Descuento: {sinConvenio}\t" +
                $"Total: {precioBase - sinConvenio}");

            break;

        case 7:

            Console.ForegroundColor =
                ConsoleColor.Red;

            Console.WriteLine(
                "\nSaliendo del sistema...");

            Console.ResetColor();

            break;

        default:

            Console.WriteLine(
                "\nOpción inválida");

            break;
    }
}

Console.WriteLine(
    "\nFIN DEL SISTEMA");