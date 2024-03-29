﻿using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;
        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }


        public IDataResult<List<Category>> GetAll()
        {
            // iş kodları
            var filteredCategories = _categoryDal.GetAll();
            return new SuccessDataResult<List<Category>>(filteredCategories);

        }

        public IDataResult<Category> GetById(int categoryId)
        {
            var filteredCategory = _categoryDal.Get(c => c.CategoryId == categoryId);
            return new SuccessDataResult<Category>(filteredCategory);

        }
    }
}
