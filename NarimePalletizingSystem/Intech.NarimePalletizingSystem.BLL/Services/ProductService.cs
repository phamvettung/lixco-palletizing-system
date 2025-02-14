using Intech.NarimePalletizingSystem.DAL.Entities;
using Intech.NarimePalletizingSystem.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.NarimePalletizingSystem.BLL.Services
{
    public class ProductService
    {
        private ProductRepository _repository = new();

        public List<Product> GetAllProducts()
        {
            try
            {
                return _repository.FindAll();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product GetProductByModel(int model)
        {
            try
            {
                Product product = _repository.FindByModel(model);
                if (product != null) return product;
                else return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void CreateProduct(Product product)
        {
            try
            {
                Product prod = _repository.FindByProductCode(product.ProductCode);
                if (prod == null)
                    _repository.Create(product);
                else
                    throw new Exception("Sản phẩm đã tồn tại trong hệ thống.");
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product UpdateProduct(Product product)
        {
            try
            {
                Product prod = _repository.FindByProductCode(product.ProductCode);
                if (prod == null)
                    throw new Exception("Sản phẩm không tồn tại trong hệ thống.");
                else
                {
                    _repository.Update(product);
                    return prod;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public Product DeleteProduct(string productCode)
        {
            try
            {
                Product prod = _repository.FindByProductCode(productCode);
                if (prod == null)
                    throw new Exception("Sản phẩm không tồn tại trong hệ thống.");
                else
                {
                    _repository.Delete(productCode);
                    return prod;
                }
               
            }
            catch (Exception e)
            {

                throw e;
            }
        }

    }
}
