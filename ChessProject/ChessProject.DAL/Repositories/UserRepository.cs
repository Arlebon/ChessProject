using ChessProject.DL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessProject.DAL.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=ChessProject.DB;Trusted_Connection=True;";

        public void Add(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO [User](Username, Email, [Password], BirthDate, Gender, ELO, [Role])
                                    VALUES(@username, @email, @password, @birthDate, @gender, @elo, @role);";

                cmd.Parameters.AddWithValue("@username", user.UserName);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@password", user.Password);
                cmd.Parameters.AddWithValue("@birthDate", user.BirthDate);
                cmd.Parameters.AddWithValue("@gender", user.Gender.ToString());
                cmd.Parameters.AddWithValue("@elo", user.ELO);
                cmd.Parameters.AddWithValue("@role", user.Role.ToString());

                connection.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public bool ExistByUserName(string userName)
        {
            int count;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(*) FROM [User]
                                    WHERE Username = @userName;";

                cmd.Parameters.AddWithValue("@userName", userName);

                connection.Open();

                count = (int)cmd.ExecuteScalar();
            }
            return count != 0;
        }

        public bool ExistByEmail(string email)
        {
            int count;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT COUNT(*) FROM [User]
                                    WHERE Email = @email;";

                cmd.Parameters.AddWithValue("@userName", email);

                connection.Open();

                count = (int)cmd.ExecuteScalar();
            }
            return count != 0;
        }
    }
}
