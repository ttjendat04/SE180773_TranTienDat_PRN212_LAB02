using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Services
{
    public interface IProductService
    {
        public void DeleteProduct(Product p);
        public Product GetProductById(int id);
        public List<Product> GetProducts();
        public void SaveProduct(Product p);
        public void UpdateProduct(Product p);
    }
}
