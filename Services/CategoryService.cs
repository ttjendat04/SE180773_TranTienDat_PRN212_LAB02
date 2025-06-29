using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using Repositories;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository iCategoryReporisoty;

        public List<Category> GetCategories()
        {
            return iCategoryReporisoty.GetCategories();
        }
        
        public CategoryService()
        {
            iCategoryReporisoty = new CategoryRepository();
        }
    }
}
