using NUnit.Framework;
using Testing.Models;

namespace Testing.Tests
{
    [TestFixture]
    public class ProductTests
    {
        private ProductManager _productManager = new ProductManager();

        [SetUp]
        public void SetUp()
        {
            _productManager = new ProductManager();
        }

        [Test]
        public void CrearProducto_ConDatosValidos_CreaCorrectamente()
        {
            int expectedId = 1;
            string expectedName = "notebook";
            decimal expectedPrice = 1000m;
            string expectedCategory = "Electrónica";

            var product = new Product(expectedId, expectedName, expectedPrice, expectedCategory);

            Assert.That(product.Id, Is.EqualTo(expectedId));
            Assert.That(product.Name, Is.EqualTo(expectedName));
            Assert.That(product.Price, Is.EqualTo(expectedPrice));
            Assert.That(product.Category, Is.EqualTo(expectedCategory));
        }

        [Test]
        public void AddProduct_AgregaProductoCorrectamente()
        {
            var producto = new Product(1, "Teclado", 1500m, "Electrónica");

            _productManager.AddProduct(producto);
            var productos = _productManager.GetAllProducts();

            Assert.That(productos.Count, Is.EqualTo(1));
            Assert.That(productos[0].Name, Is.EqualTo("Teclado"));
        }

        [Test]
        public void FindProductByName_ConVariosProductos_EncuentraCorrectamente()
        {
            var producto1 = new Product(1, "Televisor", 800m, "Electrónica");
            var producto2 = new Product(2, "Arroz", 50m, "Alimentos");
            var producto3 = new Product(3, "Mouse", 25m, "Electrónica");

            _productManager.AddProduct(producto1);
            _productManager.AddProduct(producto2);
            _productManager.AddProduct(producto3);

            var productoEncontrado = _productManager.FindProductByName("Arroz");

            Assert.That(productoEncontrado, Is.Not.Null);
            Assert.That(productoEncontrado.Id, Is.EqualTo(2));
            Assert.That(productoEncontrado.Price, Is.EqualTo(50m));
            Assert.That(productoEncontrado.Category, Is.EqualTo("Alimentos"));
        }

        [Test]
        public void CalculateTotalPrice_Electronica_CalculaCorrectamenteImpuesto10Porciento()
        {
            var producto = new Product(1, "Celular", 1000m, "Electrónica");
            decimal precioEsperado = 1100m;

            decimal precioTotal = _productManager.CalculateTotalPrice(producto);

            Assert.That(precioTotal, Is.EqualTo(precioEsperado));
        }

        [Test]
        public void CalculateTotalPrice_Alimentos_CalculaCorrectamenteImpuesto5Porciento()
        {
            var producto = new Product(1, "Leche", 200m, "Alimentos");
            decimal precioEsperado = 210m;

            decimal precioTotal = _productManager.CalculateTotalPrice(producto);

            Assert.That(precioTotal, Is.EqualTo(precioEsperado));
        }

        [Test]
        public void CrearProducto_ConPrecioNegativo_LanzaExcepcion()
        {
            int id = 1;
            string name = "Producto inválido";
            decimal price = -100m;
            string category = "Electrónica";

            var ex = Assert.Throws<ArgumentException>(() =>
                new Product(id, name, price, category)
            );

            Assert.That(ex.Message, Does.Contain("negativo"));
        }

        [Test]
        public void CrearProducto_ConCategoriaInvalida_LanzaExcepcion()
        {
            int id = 1;
            string name = "Producto";
            decimal price = 100m;
            string category = "Ropa";

            var ex = Assert.Throws<ArgumentException>(() =>
                new Product(id, name, price, category)
            );

            Assert.That(ex.Message, Does.Contain("categoría"));
        }

        [Test]
        public void FindProductByName_ProductoNoExiste_RetornaNull()
        {
            var producto = new Product(1, "Teclado", 100m, "Electrónica");
            _productManager.AddProduct(producto);

            var productoEncontrado = _productManager.FindProductByName("Monitor");

            Assert.That(productoEncontrado, Is.Null);
        }

        [Test]
        public void CalculateTotalPrice_MultiplesCategorias_CalculaCorrectamente()
        {
            var productoElectronica = new Product(1, "Tablet", 500m, "Electrónica");
            var productoAlimento = new Product(2, "Pan", 100m, "Alimentos");

            decimal precioElectronica = _productManager.CalculateTotalPrice(productoElectronica);
            decimal precioAlimento = _productManager.CalculateTotalPrice(productoAlimento);

            Assert.That(precioElectronica, Is.EqualTo(550m));
            Assert.That(precioAlimento, Is.EqualTo(105m));
        }

        [Test]
        public void AddProduct_ProductoNulo_LanzaExcepcion()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _productManager.AddProduct(null!)
            );
        }

        // adicional producto duplicado
        [Test]
        public void AddProduct_ProductoDuplicado_LanzaExcepcion()
        {
            var producto1 = new Product(1, "Teclado", 100m, "Electrónica");
            var producto2 = new Product(1, "Teclado", 150m, "Electrónica");

            _productManager.AddProduct(producto1);

            var ex = Assert.Throws<InvalidOperationException>(() =>
                _productManager.AddProduct(producto2)
            );

            Assert.That(ex.Message, Does.Contain("mismo ID"));
        }

        // adicional case sensitive
        [Test]
        public void FindProductByName_DistintosCasing_EncuentraCorrectamente()
        {
            var producto = new Product(1, "Televisor", 800m, "Electrónica");
            _productManager.AddProduct(producto);

            var resultado = _productManager.FindProductByName("teLeVisor");

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado!.Name, Is.EqualTo("Televisor"));
        }

        // adicional lista empty
        [Test]
        public void FindProductByName_ListaVacia_RetornaNull()
        {
            var resultado = _productManager.FindProductByName("Algo");

            Assert.That(resultado, Is.Null);
        }
    }
}
