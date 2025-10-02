using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ChessProject.DAL.Repositories
{
    public class TournamentRepository
    {
        private readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=ChessProject.DB;Trusted_Connection=True;";

        public List<Tournament> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Tournament";

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tournament> tournaments = new List<Tournament>();
                    while (reader.Read())
                    {
                        tournaments.Add(MapTournament(reader, connection));
                    }

                    return tournaments;
                }
            }
        }

        private Tournament MapTournament(SqlDataReader reader, SqlConnection connection)
        {
            int tournamentId = (int)reader["Id"];

            return new Tournament()
            {
                Id = tournamentId,
                Name = (string)reader["Name"],
                Location = reader["Location"] as string,
                MinPlayers = (int)reader["MinPlayers"],
                MaxPlayers = (int)reader["MaxPlayers"],
                MinElo = reader["MinElo"] as int?,
                MaxElo = reader["MaxElo"] as int?,
                CurrentRound = (int)reader["CurrentRound"],
                WomenOnly = (bool)reader["WomenOnly"],
                RegistrationDeadline = (DateTime)reader["RegistrationDeadline"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = (DateTime)reader["UpdatedAt"],

                Categories = GetCategoriesByTournamentId(tournamentId, connection)
            };
        }

        public List<TournamentCategory> GetCategoriesByTournamentId(int tournamentId, SqlConnection connection)
        {
            var categories = new List<TournamentCategory>();

            using (var cmd = new SqlCommand(
                @"SELECT CategoryId 
                  FROM TournamentCategory
                  WHERE TournamentId = @TournamentId", connection))
            {
                cmd.Parameters.AddWithValue("@TournamentId", tournamentId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add((TournamentCategory)(int)reader["CategoryId"]);
                    }
                }
            }

            return categories;
        }

        public void Add(Tournament tournament)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [Tournament]
                                                (Name, Location, MinPlayers, MaxPlayers, MinElo, MaxElo, Status, CurrentRound, WomenOnly, RegistrationDeadline, CreatedAt, UpdatedAt)
                                            OUTPUT INSERTED.Id
                                            VALUES
                                                (@name, @location, @minplayers, @maxplayers, @minelo, @maxelo, @status, @currentround, @womenonly, @registrationdeadline, SYSDATETIME(), SYSDATETIME());";

                    command.Parameters.AddWithValue("@name", tournament.Name);
                    command.Parameters.AddWithValue("@location", (object?)tournament.Location ?? DBNull.Value);
                    command.Parameters.AddWithValue("@minplayers", tournament.MinPlayers);
                    command.Parameters.AddWithValue("@maxplayers", tournament.MaxPlayers);
                    command.Parameters.AddWithValue("@minelo", (object?)tournament.MinElo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@maxelo", (object?)tournament.MaxElo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@status", tournament.Status);
                    command.Parameters.AddWithValue("@currentround", tournament.CurrentRound);
                    command.Parameters.AddWithValue("@womenonly", tournament.WomenOnly);
                    command.Parameters.AddWithValue("@registrationdeadline", tournament.RegistrationDeadline);

                    
                    tournament.Id = (int)command.ExecuteScalar();
                }
                if (tournament.Categories != null && tournament.Categories.Any())
                {
                    foreach (var category in tournament.Categories)
                    {
                        using (SqlCommand categoryCmd = connection.CreateCommand())
                        {
                            categoryCmd.CommandText = @"
                        INSERT INTO TournamentCategory (TournamentId, CategoryId)
                        VALUES (@tournamentId, @categoryId)";

                            categoryCmd.Parameters.AddWithValue("@tournamentId", tournament.Id);
                            categoryCmd.Parameters.AddWithValue("@categoryId", (int)category);

                            categoryCmd.ExecuteNonQuery();
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
