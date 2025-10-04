using ChessProject.DL.Entities;
using ChessProject.DL.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessProject.DAL.Repositories
{
    public class TournamentRepository
    {
        private readonly string _connectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=ChessProject.DB;Trusted_Connection=True;";

        public List<Tournament> GetAll()
        {
            var tournaments = new List<Tournament>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Tournament";
                connection.Open();

                // 1. Turniere ohne Kategorien lesen
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tournaments.Add(MapEntity(reader));
                    }
                }

                // 2. Kategorien nachladen (Reader ist jetzt geschlossen)
                foreach (var t in tournaments)
                {
                    t.Categories = GetCategoriesByTournamentId(t.Id);
                }
            }

            return tournaments;
        }

        private List<TournamentCategory> GetCategoriesByTournamentId(int tournamentId)
        {
            var categories = new List<TournamentCategory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT CategoryId 
                  FROM TournamentCategory
                  WHERE TournamentId = @TournamentId";
                {
                    command.Parameters.AddWithValue("@TournamentId", tournamentId);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add((TournamentCategory)Convert.ToInt32(reader["CategoryId"]));
                        }
                    }
                }

                return categories;
            }
        }

        public void Add(Tournament tournament)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // 1. Turnier speichern
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
                    command.Parameters.AddWithValue("@status", (int)tournament.Status);
                    command.Parameters.AddWithValue("@currentround", tournament.CurrentRound);
                    command.Parameters.AddWithValue("@womenonly", tournament.WomenOnly);
                    command.Parameters.AddWithValue("@registrationdeadline", tournament.RegistrationDeadline);

                    tournament.Id = (int)command.ExecuteScalar();
                }

                // 2. Kategorien speichern
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
            }
        }

        public Tournament? GetOneById(int id)
        {
            Tournament tournament = new Tournament();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT t.Id, t.Name, t.Location, t.MinPlayers, t.MaxPlayers, t.MinElo, t.MaxElo, t.Status, t.CurrentRound, t.WomenOnly, t.RegistrationDeadline, t.CreatedAt, t.UpdatedAt 
                                    FROM Tournament t
                                    WHERE t.Id = @id";

                cmd.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                    tournament = MapEntity(reader);
                }
                tournament.Categories = GetCategoriesByTournamentId(tournament.Id);

                return tournament;
            }
        }


        public bool DeleteTournamentById(int tournamentId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = "DELETE FROM Tournament WHERE [Id] = @id";

                command.Parameters.AddWithValue("id", tournamentId);
                connection.Open();

                int deleted = command.ExecuteNonQuery();
                connection.Close();

                return deleted > 0;
            }
        }
        public Tournament MapEntity(SqlDataReader reader)
        {
            return new Tournament()
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Location = reader["Location"] as string,
                MinPlayers = (int)reader["MinPlayers"],
                MaxPlayers = (int)reader["MaxPlayers"],
                MinElo = reader["MinElo"] as int?,
                MaxElo = reader["MaxElo"] as int?,
                Status = (TournamentStatus)Convert.ToInt32(reader["Status"]),
                CurrentRound = (int)reader["CurrentRound"],
                WomenOnly = (bool)reader["WomenOnly"],
                RegistrationDeadline = (DateTime)reader["RegistrationDeadline"],
                CreatedAt = (DateTime)reader["CreatedAt"],
                UpdatedAt = (DateTime)reader["UpdatedAt"]
            };
        }

        public void StartTournament(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE Tournament
                                        SET Status = @status, CurrentRound = 1, UpdatedAt = SYSDATETIME()
                                        WHERE ID = @id;";
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("status", (int)TournamentStatus.InProgress);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<int> GetUserIdsForTournament(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                // Je recupère la liste des joueurs inscrit dans un tournois
                List<int> userIds = new List<int>();

                command.CommandText = @"SELECT UserId FROM TournamentUser
                                        WHERE TournamentId = @id;";
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userIds.Add((int)reader["UserId"]);
                    }
                }
                return userIds;
            }
        }

        public void CreateEncounter(int tournamentId, int id1, int id2, int round)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Encounter (TournamentId, PlyrWhiteId, PlyrBlackId, Round)
                                        VALUES (@tournamentId, @whiteId, @blackId, @round);";
                command.Parameters.AddWithValue("@tournamentId", tournamentId);
                command.Parameters.AddWithValue("@whiteId", id1);
                command.Parameters.AddWithValue("@blackId", id2);
                command.Parameters.AddWithValue("@round", round);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
        }
    }
}
        
