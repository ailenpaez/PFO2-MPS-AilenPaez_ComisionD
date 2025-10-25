namespace Testing.Models
{
    public class ProductManager
    {
        private List<Product> _products;

        public ProductManager()
        {
            _products = new List<Product>();
        }

        // addproduct
        public void AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo");
            }

            _products.Add(product);
        }

        // totalprice
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

        // encontrar producto por nombre
    
        public Product? FindProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("El nombre no puede estar vacío");
            }

            return _products.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // obtener todos los productos 
        public List<Product> GetAllProducts()
        {
            return _products;
        }
    }
}