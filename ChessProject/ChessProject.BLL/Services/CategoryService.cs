using ChessProject.DAL.Repositories;
using ChessProject.DL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.BLL.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = _categoryRepository.GetAll();

            return categories;
        }

        public Category GetCategoryById(int id)
        {
            Category? category = _categoryRepository.GetById(id);

            if (category == null)
            {
                throw new Exception("Category doesn't exist");
            }
            return category;
        }
    }
}
