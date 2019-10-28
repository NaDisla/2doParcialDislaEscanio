using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using System.Data.Entity;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text.RegularExpressions;

namespace _2doParcialDislaEscanio
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        private static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3);
        }
        static void Main(string[] args)
        {
            Maximize();
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            CategoryData cd = new CategoryData();
            Category CT = new Category();

            ProductData pd = new ProductData();
            Product PD = new Product();

            SupplierData sd = new SupplierData();
            Supplier SP = new Supplier();

            CustomerData cud = new CustomerData();
            Customer CUST = new Customer();

            TerritoryData td = new TerritoryData();
            Territory TR = new Territory();

            RegionData rd = new RegionData();
            Region REG = new Region();

            OrderData od = new OrderData();
            Order ORDER = new Order();

            OrderDetailData odetdat = new OrderDetailData();
            Order_Detail ORDT = new Order_Detail();

            string IDTer, path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            int SEL, SELMenu2, SEL2, SEL3, IDSup, IDCat, IDProd, IDReg, IDFact, x = 0;

            while (x < 3)
            {
                Console.WriteLine("\n ==================== MENÚ PRINCIPAL ====================");
                Console.Write("\n 1) Mantenimiento de tablas");
                Console.Write("\n 2) Exportar una factura en un archivo txt");
                Console.Write("\n 3) Carga de archivo CSV de clientes");
                Console.Write("\n");
                Console.Write("\n Ingrese el dígito de la opción deseada: ");

                while (!int.TryParse(Console.ReadLine(), out SEL))
                {
                    Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                }

                switch (SEL)
                {
                    case 1:
                        Console.WriteLine("\n ====================MANTENIMIENTO DE TABLAS====================");
                        Console.Write("\n Seleccione la tabla a la que desea darle mantenimiento");
                        Console.Write("\n 1) Categorías");
                        Console.Write("\n 2) Territorios");
                        Console.Write("\n 3) Productos");
                        Console.Write("\n");
                        Console.Write("\n Ingrese el dígito de la opción deseada: ");

                        while (!int.TryParse(Console.ReadLine(), out SELMenu2))
                        {
                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                        }

                        switch (SELMenu2)
                        {
                            case 1:
                                Console.WriteLine("\n **************** Bienvenido al mantenimiento de la tabla Categorías ****************");
                                Console.Write("\n ¿Qué desea realizar?");
                                Console.Write("\n 1) Insertar registros");
                                Console.Write("\n 2) Actualizar registros");
                                Console.Write("\n 3) Eliminar registros");
                                Console.Write("\n 4) Mostrar todos los registros");
                                Console.Write("\n");
                                Console.Write("\n Ingrese el dígito de la opción deseada: ");

                                while (!int.TryParse(Console.ReadLine(), out SEL2))
                                {
                                    Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                }

                                switch (SEL2)
                                {
                                    case 1:
                                        Console.Write("\n Ingrese el nombre de la categoría que desea agregar: ");
                                        CT.CategoryName = Console.ReadLine();
                                        while (!Regex.IsMatch(CT.CategoryName, @"[a-zA-Z]"))
                                        {
                                            Console.WriteLine("\n No se permiten números. Intente nuevamente: ");
                                            Console.Write("\n Ingrese el nombre de la categoría que desea agregar: ");
                                            CT.CategoryName = Console.ReadLine();
                                        }
                                        Console.WriteLine("\n Estamos procesando su petición.................\n");
                                        cd.Agregar<Category>(CT);
                                        break;

                                    case 2:
                                        Console.Write("\n Ingrese el código de la categoría que desea actualizar: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDCat))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }
                                        Console.WriteLine("\n Estamos procesando su búsqueda.................\n");
                                        while (!cd.Listado<Category>().Exists(c => c.CategoryID == IDCat))
                                        {
                                            Console.Write("\n Este código no pertenece a ninguna categoría registrada. Intente nuevamente: ");
                                            IDCat = int.Parse(Console.ReadLine());
                                        }
                                        var resultCAT = cd.Listado<Category>().Find(a => a.CategoryID == IDCat);
                                        if (resultCAT != null && cd.Listado<Category>().Exists(c => c.CategoryID == IDCat))
                                        {
                                            var ctNombre = cd.Model.Categories.Where(ct => ct.CategoryID == IDCat).Select(ctn => ctn).FirstOrDefault();
                                            Console.Write("\n Categoría: " + ctNombre.CategoryName);
                                            Console.Write("\n Ingrese el nuevo nombre de esta categoría: ");
                                            CT.CategoryName = Console.ReadLine();
                                            CT.CategoryID = IDCat;
                                            cd.Model.Entry(resultCAT).State = EntityState.Detached;
                                            cd.Actualizar<Category>(CT);
                                        }
                                        break;

                                    case 3:
                                        Console.Write("\n Ingrese el código de la categoría que desea eliminar: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDCat))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }

                                        while (!cd.Listado<Category>().Exists(c => c.CategoryID == IDCat))
                                        {
                                            Console.Write("\n Este código no pertenece a ninguna categoría registrada. Intente nuevamente: ");
                                            IDCat = int.Parse(Console.ReadLine());
                                        }
                                        var resultCT = cd.Listado<Category>().Find(a => a.CategoryID == IDCat);
                                        if (resultCT != null && cd.Listado<Category>().Exists(c => c.CategoryID == IDCat))
                                        {
                                            CT.CategoryID = IDCat;
                                            cd.Model.Entry(resultCT).State = EntityState.Detached;
                                            cd.Eliminar<Category>(CT);
                                        }
                                        break;

                                    case 4:
                                        ConsoleTable TablaCAT = new ConsoleTable("Código", "Nombre");
                                        foreach (Category ListCAT in cd.Listado<Category>())
                                        {
                                            TablaCAT.AddRow(ListCAT.CategoryID, ListCAT.CategoryName);
                                        }
                                        TablaCAT.Write(Format.Alternative);
                                        break;
                                }
                                break;

                            case 2:
                                Console.WriteLine("\n **************** Bienvenido al mantenimiento de la tabla Territorios ****************");
                                Console.Write("\n ¿Qué desea realizar?");
                                Console.Write("\n 1) Insertar registros");
                                Console.Write("\n 2) Actualizar registros");
                                Console.Write("\n 3) Eliminar registros");
                                Console.Write("\n 4) Mostrar todos los registros");
                                Console.Write("\n");
                                Console.Write("\n Ingrese el dígito de la opción deseada: ");

                                while (!int.TryParse(Console.ReadLine(), out SEL2))
                                {
                                    Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                }

                                switch (SEL2)
                                {
                                    case 1:
                                        Console.Write("\n Código: ");
                                        TR.TerritoryID = Console.ReadLine();

                                        Console.Write("\n Nombre: ");
                                        TR.TerritoryDescription = Console.ReadLine();
                                        while (!Regex.IsMatch(TR.TerritoryDescription, @"[a-zA-Z]"))
                                        {
                                            Console.WriteLine("\n No se permiten números. Intente nuevamente: ");
                                            Console.Write("\n Nombre: ");
                                            TR.TerritoryDescription = Console.ReadLine();
                                        }
                                        Console.Write("\n");
                                        Console.Write("\n A continuación se presentan las regiones registradas, favor ingresar el código de la requerida para el nuevo territorio. Presione Enter. ");
                                        Console.ReadLine();
                                        ConsoleTable TablaRegiones = new ConsoleTable("Código", "Nombre");
                                        foreach (Region RG in rd.Listado<Region>())
                                        {
                                            TablaRegiones.AddRow(RG.RegionID, RG.RegionDescription);
                                        }
                                        TablaRegiones.Write(Format.Alternative);
                                        Console.Write("\n");

                                        Console.Write("\n Código de la región: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDReg))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }
                                        while (!rd.Listado<Region>().Exists(c => c.RegionID == IDReg))
                                        {
                                            Console.Write("\n Este código no corresponde a ninguna región registrada. Intente nuevamente: ");
                                            IDReg = int.Parse(Console.ReadLine());
                                        }
                                        if (rd.Listado<Region>().Exists(c => c.RegionID == IDReg))
                                        {
                                            TR.RegionID = IDReg;
                                        }
                                        Console.WriteLine("\n Estamos procesando su petición.................\n");
                                        td.Agregar<Territory>(TR);
                                        break;

                                    case 2:
                                        Console.Write("\n Ingrese el código del territorio que desea actualizar: ");
                                        IDTer = Console.ReadLine();
                                        Console.WriteLine("\n Estamos procesando su búsqueda.................\n");
                                        while (!td.Listado<Territory>().Exists(c => c.TerritoryID == IDTer))
                                        {
                                            Console.Write("\n Este código no pertenece a ningún territorio registrado. Intente nuevamente: ");
                                            IDTer = Console.ReadLine();
                                        }
                                        var resultTER = td.Listado<Territory>().Find(a => a.TerritoryID == IDTer);
                                        if (resultTER != null && td.Listado<Territory>().Exists(c => c.TerritoryID == IDTer))
                                        {
                                            var terNombre = td.Model.Territories.Where(tr => tr.TerritoryID == IDTer).Select(terr => terr).FirstOrDefault();
                                            Console.Write("\n Territorio: " + terNombre.TerritoryDescription);
                                            Console.Write("\n Ingrese el nuevo nombre de este territorio: ");
                                            TR.TerritoryDescription = Console.ReadLine();
                                            TR.TerritoryID = IDTer;
                                            var search = td.Model.Territories.Where(t => t.TerritoryID == IDTer).Select(m => m).FirstOrDefault();
                                            TR.RegionID = search.RegionID;

                                            td.Model.Entry(resultTER).State = EntityState.Detached;
                                            td.Actualizar<Territory>(TR);
                                        }
                                        break;

                                    case 3:
                                        Console.Write("\n Ingrese el código del territorio que desea eliminar: ");
                                        IDTer = Console.ReadLine();
                                        Console.WriteLine("\n Estamos procesando su búsqueda.................\n");
                                        while (!td.Listado<Territory>().Exists(c => c.TerritoryID == IDTer))
                                        {
                                            Console.Write("\n Este código no pertenece a ningún territorio registrado. Intente nuevamente: ");
                                            IDTer = Console.ReadLine();
                                        }
                                        var resultTER2 = td.Listado<Territory>().Find(a => a.TerritoryID == IDTer);
                                        if (resultTER2 != null && td.Listado<Territory>().Exists(c => c.TerritoryID == IDTer))
                                        {
                                            TR.TerritoryID = IDTer;
                                            td.Model.Entry(resultTER2).State = EntityState.Detached;
                                            td.Eliminar<Territory>(TR);
                                        }
                                        break;

                                    case 4:
                                        Console.Write("\n");
                                        ConsoleTable TablaTerritorios = new ConsoleTable("Código", "Nombre", "Región");
                                        foreach (Territory TERT in td.Listado<Territory>())
                                        {
                                            TablaTerritorios.AddRow(TERT.TerritoryID, TERT.TerritoryDescription, TERT.Region.RegionDescription);
                                        }
                                        TablaTerritorios.Write(Format.Alternative);
                                        break;
                                }
                                break;

                            case 3:
                                Console.WriteLine("\n **************** Bienvenido al mantenimiento de la tabla Productos ****************");
                                Console.Write("\n ¿Qué desea realizar?");
                                Console.Write("\n 1) Insertar registros");
                                Console.Write("\n 2) Actualizar registros");
                                Console.Write("\n 3) Eliminar registros");
                                Console.Write("\n 4) Mostrar todos los registros");
                                Console.Write("\n");
                                Console.Write("\n Ingrese el dígito de la opción deseada: ");

                                while (!int.TryParse(Console.ReadLine(), out SEL2))
                                {
                                    Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                }

                                switch (SEL2)
                                {
                                    case 1:
                                        Console.Write("\n Nombre del producto: ");
                                        PD.ProductName = Console.ReadLine();
                                        while (!Regex.IsMatch(PD.ProductName, @"[a-zA-Z]"))
                                        {
                                            Console.WriteLine("\n No se permiten números. Intente nuevamente: ");
                                            Console.Write("\n Nombre del producto: ");
                                            PD.ProductName = Console.ReadLine();
                                        }
                                        Console.Write("\n");
                                        Console.Write("\n A continuación se presentan los suplidores registrados, favor ingresar el código del requerido para el nuevo producto. Presione Enter.");
                                        Console.ReadLine();
                                        ConsoleTable TablaSuplidores = new ConsoleTable("Código", "Nombre", "Teléfono", "Dirección");
                                        foreach (Supplier SUP in sd.Listado<Supplier>())
                                        {
                                            TablaSuplidores.AddRow(SUP.SupplierID, SUP.CompanyName, SUP.Phone, SUP.Address);
                                        }
                                        TablaSuplidores.Write(Format.Alternative);
                                        Console.Write("\n");

                                        Console.Write("\n Código del suplidor: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDSup))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }
                                        while (!pd.Listado<Supplier>().Exists(c => c.SupplierID == IDSup))
                                        {
                                            Console.Write("\n Este código no corresponde a ningún suplidor registrado. Intente nuevamente: ");
                                            IDSup = int.Parse(Console.ReadLine());
                                        }
                                        if (pd.Listado<Supplier>().Exists(s => s.SupplierID == IDSup))
                                        {
                                            PD.SupplierID = IDSup;
                                        }

                                        Console.Write("\n A continuación se presentan las categorías registradas, favor ingresar el código de la requerida para el nuevo producto:");
                                        Console.Write("\n");
                                        ConsoleTable TablaCAT = new ConsoleTable("Código", "Nombre");
                                        foreach (Category ListCAT in cd.Listado<Category>())
                                        {
                                            TablaCAT.AddRow(ListCAT.CategoryID, ListCAT.CategoryName);
                                        }
                                        TablaCAT.Write(Format.Alternative);
                                        Console.Write("\n");

                                        Console.Write("\n Código de la categoría: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDCat))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }

                                        while (!pd.Listado<Category>().Exists(c => c.CategoryID == IDCat))
                                        {
                                            Console.Write("\n Este código no corresponde a ninguna categoría registrada. Intente nuevamente: ");
                                            IDCat = int.Parse(Console.ReadLine());
                                        }
                                        if (pd.Listado<Category>().Exists(s => s.CategoryID == IDCat))
                                        {
                                            PD.CategoryID = IDCat;
                                        }

                                        Console.Write("\n Cantidad: ");
                                        PD.QuantityPerUnit = Console.ReadLine();

                                        Console.Write("\n Precio: ");
                                        PD.UnitPrice = decimal.Parse(Console.ReadLine());

                                        Console.Write("\n Unidades disponibles: ");
                                        PD.UnitsInStock = short.Parse(Console.ReadLine());

                                        Console.Write("\n Unidades pedidas: ");
                                        PD.UnitsOnOrder = short.Parse(Console.ReadLine());

                                        Console.WriteLine("\n Estamos procesando su petición.................\n");
                                        pd.Agregar<Product>(PD);
                                        break;

                                    case 2:
                                        Console.Write("\n Ingrese el código del producto que desea actualizar: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDProd))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }
                                        Console.WriteLine("\n Estamos procesando su búsqueda.................\n");
                                        while (!pd.Listado<Product>().Exists(p => p.ProductID == IDProd))
                                        {
                                            Console.Write("\n Este código no pertenece a ningún producto registrado. Intente nuevamente: ");
                                            IDProd = int.Parse(Console.ReadLine());
                                        }
                                        var resultPROD = pd.Listado<Product>().Find(r => r.ProductID == IDProd);
                                        if (resultPROD != null && cd.Listado<Product>().Exists(d => d.ProductID == IDProd))
                                        {
                                            var pdNombre = pd.Model.Products.Where(pr => pr.ProductID == IDProd).Select(pt => pt).FirstOrDefault();
                                            Console.Write("\n Producto: " + pdNombre.ProductName);
                                            Console.Write("\n");
                                            Console.Write("\n ------------------- ¿Qué desea modificar de este producto? -------------------");
                                            Console.Write("\n 1) Nombre");
                                            Console.Write("\n 2) Unidades disponibles");
                                            Console.Write("\n 3) Precio");
                                            Console.Write("\n Ingrese el dígito de la opción que desea modificar: ");

                                            while (!int.TryParse(Console.ReadLine(), out SEL3))
                                            {
                                                Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                            }

                                            switch (SEL3)
                                            {
                                                case 1:
                                                    Console.Write("\n Ingrese el nuevo nombre de este producto: ");
                                                    PD.ProductName = Console.ReadLine();
                                                    while (!Regex.IsMatch(PD.ProductName, @"[a-zA-Z]"))
                                                    {
                                                        Console.WriteLine("\n No se permiten números. Intente nuevamente: ");
                                                        Console.Write("\n Ingrese el nuevo nombre de este producto: ");
                                                        PD.ProductName = Console.ReadLine();
                                                    }
                                                    PD.ProductID = IDProd;
                                                    var search = pd.Model.Products.Where(p => p.ProductID == IDProd).Select(m => m).FirstOrDefault();
                                                    PD.CategoryID = search.CategoryID;
                                                    PD.Discontinued = search.Discontinued;
                                                    PD.QuantityPerUnit = search.QuantityPerUnit;
                                                    PD.ReorderLevel = search.ReorderLevel;
                                                    PD.SupplierID = search.SupplierID;
                                                    PD.UnitPrice = search.UnitPrice;
                                                    PD.UnitsInStock = search.UnitsInStock;
                                                    PD.UnitsOnOrder = search.UnitsOnOrder;
                                                    pd.Model.Entry(resultPROD).State = EntityState.Detached;
                                                    pd.Actualizar<Product>(PD);
                                                    break;

                                                case 2:
                                                    Console.Write("\n Ingrese la nueva cantidad de unidades disponibles de este producto: ");
                                                    PD.UnitsInStock = short.Parse(Console.ReadLine());
                                                    PD.ProductID = IDProd;
                                                    var search2 = pd.Model.Products.Where(p => p.ProductID == IDProd).Select(m => m).FirstOrDefault();
                                                    PD.ProductName = search2.ProductName;
                                                    PD.CategoryID = search2.CategoryID;
                                                    PD.Discontinued = search2.Discontinued;
                                                    PD.QuantityPerUnit = search2.QuantityPerUnit;
                                                    PD.ReorderLevel = search2.ReorderLevel;
                                                    PD.SupplierID = search2.SupplierID;
                                                    PD.UnitPrice = search2.UnitPrice;
                                                    PD.UnitsOnOrder = search2.UnitsOnOrder;
                                                    pd.Model.Entry(resultPROD).State = EntityState.Detached;
                                                    pd.Actualizar<Product>(PD);
                                                    break;

                                                case 3:
                                                    Console.Write("\n Ingrese el nuevo precio de este producto: ");
                                                    PD.UnitPrice = decimal.Parse(Console.ReadLine());
                                                    PD.ProductID = IDProd;
                                                    var search3 = pd.Model.Products.Where(p => p.ProductID == IDProd).Select(m => m).FirstOrDefault();
                                                    PD.ProductName = search3.ProductName;
                                                    PD.CategoryID = search3.CategoryID;
                                                    PD.Discontinued = search3.Discontinued;
                                                    PD.QuantityPerUnit = search3.QuantityPerUnit;
                                                    PD.ReorderLevel = search3.ReorderLevel;
                                                    PD.SupplierID = search3.SupplierID;
                                                    PD.UnitsInStock = search3.UnitsInStock;
                                                    PD.UnitsOnOrder = search3.UnitsOnOrder;
                                                    pd.Model.Entry(resultPROD).State = EntityState.Detached;
                                                    pd.Actualizar<Product>(PD);
                                                    break;
                                            }

                                        }
                                        break;

                                    case 3:
                                        Console.Write("\n Ingrese el código del producto que desea eliminar: ");
                                        while (!int.TryParse(Console.ReadLine(), out IDProd))
                                        {
                                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                                        }
                                        Console.WriteLine("\n Estamos procesando su búsqueda.................\n");
                                        while (!pd.Listado<Product>().Exists(p => p.ProductID == IDProd))
                                        {
                                            Console.Write("\n Este código no pertenece a ningún producto registrado. Intente nuevamente: ");
                                            IDProd = int.Parse(Console.ReadLine());
                                        }
                                        var resultPROD2 = pd.Listado<Product>().Find(a => a.ProductID == IDProd);
                                        if (resultPROD2 != null && pd.Listado<Product>().Exists(p => p.ProductID == IDProd))
                                        {
                                            PD.ProductID = IDProd;
                                            pd.Model.Entry(resultPROD2).State = EntityState.Detached;
                                            pd.Eliminar<Product>(PD);
                                        }
                                        break;

                                    case 4:
                                        Console.Write("\n");
                                        ConsoleTable TablaMuestra = new ConsoleTable("Código", "Nombre", "Suplidor", "Categoría", "Cantidad", "Precio",
                                "Unidades en Stock", "Unidades pedidas");
                                        foreach (Product PROD in pd.Listado<Product>())
                                        {
                                            TablaMuestra.AddRow(PROD.ProductID, PROD.ProductName, PROD.Supplier.CompanyName, PROD.Category.CategoryName, PROD.QuantityPerUnit,
                                                PROD.UnitPrice, PROD.UnitsInStock, PROD.UnitsOnOrder);
                                        }
                                        TablaMuestra.Write(Format.Minimal);
                                        break;
                                }
                                break;
                        }
                        break;

                    case 2:
                        Console.Write("\n A continuación se presentan todas las facturas creadas. Favor de tomar el código de la deseada para exportar en un archivo txt. Presione Enter.");
                        Console.ReadLine();
                        Console.Write("\n");
                        ConsoleTable TablaDetallesPedidos = new ConsoleTable("Código del pedido", "Producto", "Precio", "Cantidad", "Descuento");
                        foreach (Order_Detail ORDET in odetdat.Listado<Order_Detail>())
                        {
                            TablaDetallesPedidos.AddRow(ORDET.OrderID, ORDET.Product.ProductName, ORDET.UnitPrice, ORDET.Quantity, ORDET.Discount);
                        }
                        TablaDetallesPedidos.Write(Format.Alternative);

                        Console.Write("\n Ingrese el código de la factura que desea exportar: ");
                        while (!int.TryParse(Console.ReadLine(), out IDFact))
                        {
                            Console.Write("\n Sólo se permiten números. Intente nuevamente: ");
                        }
                        var resultFact = odetdat.Listado<Order_Detail>().Find(odt => odt.OrderID== IDFact);
                        if (resultFact != null && odetdat.Listado<Order_Detail>().Exists(ordt => ordt.OrderID == IDFact))
                        {
                            odetdat.TotalProductos = 0;
                            odetdat.TotalFactura = 0;
                            ConsoleTable TablaFacturas = new ConsoleTable("Producto", "Precio", "Cantidad", "Importe");
                            foreach (Order_Detail ORDEN in odetdat.Listado<Order_Detail>().Where(ordt => ordt.OrderID == IDFact))
                            {
                                odetdat.TotalProductos = odetdat.TotalProductos + ORDEN.Quantity;
                                odetdat.Importe = ORDEN.Quantity * ORDEN.UnitPrice;
                                odetdat.TotalFactura = odetdat.TotalFactura + odetdat.Importe;
                                TablaFacturas.AddRow(ORDEN.Product.ProductName, ORDEN.UnitPrice, ORDEN.Quantity, odetdat.Importe);
                            }
                            TablaFacturas.Write(Format.Alternative);
                            Console.Write("\n Total de artículos: ".PadRight(10) + odetdat.TotalProductos + "\t" + "Total a pagar: RD$".PadRight(15) + odetdat.TotalFactura);
                            Console.Write("\n");
                            var custNombre = odetdat.Model.Orders.Where(or => or.OrderID == IDFact).Select(pt => pt).FirstOrDefault();
                            using (StreamWriter Archivo = new StreamWriter(path))
                            {
                                Archivo.Write("\n Cliente: ".PadRight(10) + custNombre.Customer.CompanyName);
                                Archivo.WriteLine("\n");
                                Archivo.WriteLine(TablaFacturas);
                                Archivo.WriteLine("\n");
                                Archivo.Write("\n Total de artículos: " + odetdat.TotalProductos + "\t" + "Total a pagar: RD$" + odetdat.TotalFactura);
                            }
                            Console.Write("\n Seleccione la factura ");
                        }

                        break;

                    case 3:
                        break;
                }
                x = 4;
                if (SEL > 3)
                {
                    Console.Write("\n Este dígito no corresponde al rango permitido. Presione Enter para salir. :) ");
                    Console.ReadKey();
                }
            }
        }
    }
}
