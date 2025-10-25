using NUnit.Framework;
using Testing.Models;

namespace Testing.Tests
{
    [TestFixture]
    public class ProductTests
    {
        private ProductManager _productManager = null!;

        [SetUp]
        public void SetUp()
        {
            _productManager = new ProductManager();
        }

        [Test]
        public void CrearProducto_ConDatosValidos_CreaCorrectamente()
        {
            // Arrange
            int expectedId = 1;
            string expectedName = "notebook";
            decimal expectedPrice = 1000m;
            string expectedCategory = "Electrónica";

            // Act
            var product = new Product(expectedId, expectedName, expectedPrice, expectedCategory);

            // Assert
            Assert.That(product.Id, Is.EqualTo(expectedId), "El ID no coincide.");
            Assert.That(product.Name, Is.EqualTo(expectedName), "El nombre no coincide.");
            Assert.That(product.Price, Is.EqualTo(expectedPrice), "El precio no coincide.");
            Assert.That(product.Category, Is.EqualTo(expectedCategory), "La categoría no coincide.");
        }

        [Test]
        public void AddProduct_AgregaProductoCorrectamente()
        {
            // Arrange
            var producto = new Product(1, "Teclado", 1500m, "Electrónica");

            // Act
            _productManager.AddProduct(producto);
            var productos = _productManager.GetAllProducts();

            // Assert
            Assert.That(productos.Count, Is.EqualTo(1), "Debería haber al menos un producto");
            Assert.That(productos[0].Name, Is.EqualTo("Teclado"), "El nombre del producto no coincide");
        }

        [Test]
        public void FindProductByName_ConVariosProductos_EncuentraCorrectamente()
        {
            // Arrange
            var producto1 = new Product(1, "Televisor", 800m, "Electrónica");
            var producto2 = new Product(2, "Arroz", 50m, "Alimentos");
            var producto3 = new Product(3, "Mouse", 25m, "Electrónica");

            _productManager.AddProduct(producto1);
            _productManager.AddProduct(producto2);
            _productManager.AddProduct(producto3);

            // Act
            var productoEncontrado = _productManager.FindProductByName("Arroz");

            // Assert
            Assert.That(productoEncontrado, Is.Not.Null, "El producto debería ser encontrado.");
            Assert.That(productoEncontrado.Id, Is.EqualTo(2), "El ID no coincide.");
            Assert.That(productoEncontrado.Price, Is.EqualTo(50m), "El precio no coincide.");
            Assert.That(productoEncontrado.Category, Is.EqualTo("Alimentos"), "La categoría no coincide.");
        }

        [Test]
        public void CalculateTotalPrice_Electronica_CalculaCorrectamenteImpuesto10Porciento()
        {
            // arrange
            var producto = new Product(1, "Celular", 1000m, "Electrónica");
            decimal precioEsperado = 1100m; // 1000 + 10% = 1100

            // act
            decimal precioTotal = _productManager.CalculateTotalPrice(producto);

            // assert
            Assert.That(precioTotal, Is.EqualTo(precioEsperado),
                "El precio total con impuesto del 10% no es correcto.");
        }

        [Test]
        public void CalculateTotalPrice_Alimentos_CalculaCorrectamenteImpuesto5Porciento()
        {
            // arrange
            var producto = new Product(1, "Leche", 200m, "Alimentos");
            decimal precioEsperado = 210m; 

            // act
            decimal precioTotal = _productManager.CalculateTotalPrice(producto);

            // assert
            Assert.That(precioTotal, Is.EqualTo(precioEsperado),
                "El precio total con impuesto del 5% no es correcto");
        }

        [Test]
        public void CrearProducto_ConPrecioNegativo_LanzaExcepcion()
        {
            // arrange
            int id = 1;
            string name = "Producto inválido";
            decimal price = -100m;
            string category = "Electrónica";

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Product(id, name, price, category)
            );

            Assert.That(ex.Message, Does.Contain("negativo"));
        }

        [Test]
        public void CrearProducto_ConCategoriaInvalida_LanzaExcepcion()
        {
            // arrange
            int id = 1;
            string name = "Producto";
            decimal price = 100m;
            string category = "Ropa"; // otra cat

            // act + assert
            var ex = Assert.Throws<ArgumentException>(() =>
                new Product(id, name, price, category)
            );

            Assert.That(ex.Message, Does.Contain("categoría"));
        }

        [Test]
        public void FindProductByName_ProductoNoExiste_RetornaNull()
        {
            // arrange
            var producto = new Product(1, "Teclado", 100m, "Electrónica");
            _productManager.AddProduct(producto);

            // act
            var productoEncontrado = _productManager.FindProductByName("Monitor");

            // assert
            Assert.That(productoEncontrado, Is.Null, "No debería encontrar el producto");
        }

        [Test]
        public void CalculateTotalPrice_MultiplesCategorias_CalculaCorrectamente()
        {
            // arrange
            var productoElectronica = new Product(1, "Tablet", 500m, "Electrónica");
            var productoAlimento = new Product(2, "Pan", 100m, "Alimentos");

            // act
            decimal precioElectronica = _productManager.CalculateTotalPrice(productoElectronica);
            decimal precioAlimento = _productManager.CalculateTotalPrice(productoAlimento);

            // assert
            Assert.That(precioElectronica, Is.EqualTo(550m),
                "Precio de Electrónica incorrecto");
            Assert.That(precioAlimento, Is.EqualTo(105m),
                "Precio de Alimentos incorrecto");
        }

        [Test]
        public void AddProduct_ProductoNulo_LanzaExcepcion()
        {
            // act & assert
            Assert.Throws<ArgumentNullException>(() =>
                _productManager.AddProduct(null!)
            );
        }
    }
}