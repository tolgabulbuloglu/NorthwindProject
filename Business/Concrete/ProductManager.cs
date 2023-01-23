using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {

        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {

            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCategoryCorrect(product.CategoryId),
                                               CheckIfProductNameExists(product.ProductName),
                                               CheckIfCategoryLimitExceeded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Messages.ProductAdded);

        }

        public IDataResult<List<Product>> GetAll()
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductsListed);


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

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult(Messages.ProductUpdated);
        }


        private IResult CheckIfProductCountOfCategoryCategoryCorrect(int categoryId)
        {
            var productsInGivenCategory = _productDal.GetAll(p => p.CategoryId == categoryId);
            if (productsInGivenCategory.Count >= 10)
            {
                return new ErrorResult(Messages.CountOfCategoryExceeded);
            }

            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var productInGivenName = _productDal.GetAll(p => p.ProductName.ToLower() == productName.ToLower()).Any();
            if (productInGivenName)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }

            return new SuccessResult();
        }

        // Eğer mevcut kategori sayısı 15'i geçtiyse sisteme yeni ürün eklenemez (Başka Service enjection'ı için Uydurma kural) 
        private IResult CheckIfCategoryLimitExceeded()
        {
            var result = _categoryService.GetAll().Data.Count();
            if (result > 15)
            {
                return new ErrorResult(Messages.CategoryLimitExceeded);
            }

            return new SuccessResult();
        }
    }
}
