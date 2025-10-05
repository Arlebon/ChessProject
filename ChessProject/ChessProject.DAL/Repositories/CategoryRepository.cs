using ChessProject.DL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=ChessProject.DB;Trusted_Connection=True;";

        public List<Category> GetAll()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Category;";

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(MapEntity(reader));
                    }
                    return categories;
                }
            }
        }

        public Category? GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT * FROM Category
                                    WHERE Id = @id";

                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    return MapEntity(reader);
                }
            }
        }

        public Category MapEntity(SqlDataReader reader)
        {
            return new Category()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                MinAge = (int)reader["MinAge"],
                MaxAge = (int)reader["MaxAge"],
            };
        }
    }
}
