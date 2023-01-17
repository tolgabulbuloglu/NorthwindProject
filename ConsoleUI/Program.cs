

using Business.Concrete;
using DataAccess.Concrete.EntityFramework;

ProductTest();

//CategoryTest();







static void ProductTest()
{
    ProductManager productManager = new ProductManager(new EfProductDal());

    var result = productManager.GetAllByUnitPrice(50, 150);

    if (result.Success)
    {
        foreach (var product in result.Data)
        {
            Console.WriteLine(product.ProductName);
        }
    }
    else
    {
        Console.WriteLine(result.Message);
    }

    Console.WriteLine("----------------");




    foreach (var product in productManager.GetProductDetails().Data)
    {
        Console.WriteLine(product.ProductName + " /// " +product.CategoryName);
    }


}

static void CategoryTest()
{
    CategoryManager categoryManager = new CategoryManager(new EfCategoryDal());


    foreach (var category in categoryManager.GetAll())
    {
        Console.WriteLine("Kategori Id: {0}, Kategori Adı: {1}", category.CategoryId, category.CategoryName);
    }

    Console.WriteLine("-------------------");
    Console.WriteLine(categoryManager.GetById(8).CategoryName);
}