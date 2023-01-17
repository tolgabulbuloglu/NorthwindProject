using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public IResult Add(Product product)
        {
            if (product.ProductName.Length<2)
            {
                return new ErrorResult(Messages.ProductNameInvalid);
            }
            
            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);
        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour==22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.ProductsListed);
                
            
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int categoryId)
        {
            var filteredProducts = _productDal.GetAll(p => p.CategoryId == categoryId);
            return new SuccessDataResult<List<Product>>(filteredProducts, Messages.ProductsListed);
                
        }

        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            var filteredProducts = _productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max);
            return new SuccessDataResult<List<Product>>(filteredProducts);
        }

        public IDataResult<Product> GetById(int productId)
        {
            var filteredProduct = _productDal.Get(p => p.ProductId == productId);
            return new SuccessDataResult<Product>(filteredProduct);
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            var filteredProductDetails = _productDal.GetProductDetails();
            return new SuccessDataResult<List<ProductDetailDto>>(filteredProductDetails);
        }
    }
}
