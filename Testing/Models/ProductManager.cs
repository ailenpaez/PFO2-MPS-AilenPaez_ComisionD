using System;
using System.Collections.Generic;
using System.Linq;

namespace Testing.Models
{
    public class ProductManager
    {
        private List<Product> _products;

        public ProductManager()
        {
            _products = new List<Product>();
        }

        // 🔹 Agregar producto
        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo");
            }

            // 🔹 Nueva validación: evitar productos duplicados por ID o nombre
            if (_products.Any(p => p.Id == product.Id ||
                                   p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("Ya existe un producto con el mismo ID o nombre.");
            }

            _products.Add(product);
        }

        // 🔹 Calcular precio total con impuestos
        public decimal CalculateTotalPrice(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo");
            }

            decimal taxRate = product.Category == "Electrónica" ? 0.10m : 0.05m;
            decimal totalPrice = product.Price + (product.Price * taxRate);

            return totalPrice;
        }

        // 🔹 Buscar producto por nombre
        public Product? FindProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre no puede estar vacío");
            }

            return _products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // 🔹 Obtener todos los productos
        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}
