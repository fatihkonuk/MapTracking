using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapTracking.Models;
using Npgsql;
using WebApi.Services.Interfaces;

namespace WebApi.Services
{
    public class DbService(IConfiguration configuration) : IDbService
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

        public async Task<List<Point>> GetAll()
        {
            try
            {
                var points = new List<Point>();

                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand("SELECT id, name, pointx, pointy FROM points", connection);
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    points.Add(new Point
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PointX = reader.GetDouble(2),
                        PointY = reader.GetDouble(3)
                    });
                }

                return points;
            }
            catch (Exception ex)
            {
                throw new Exception("Tüm noktaları getirirken bir hata oluştu", ex);
            }
        }

        public async Task<Point?> GetById(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand("SELECT id, name, pointx, pointy FROM points WHERE id = @id", connection);
                command.Parameters.AddWithValue("id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Point
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PointX = reader.GetDouble(2),
                        PointY = reader.GetDouble(3)
                    };
                }
                else
                {
                    throw new InvalidOperationException("Belirtilen ID'ye sahip nokta bulunamadı");
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message ?? "Noktayı getirirken bir hata oluştu";
                throw new Exception(message, ex);
            }
        }

        public async Task<Point> Add(Point point)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand(
                    "INSERT INTO points (name, pointx, pointy) VALUES (@name, @pointx, @pointy) RETURNING id, name, pointx, pointy",
                    connection);

                command.Parameters.AddWithValue("name", point.Name);
                command.Parameters.AddWithValue("pointx", point.PointX);
                command.Parameters.AddWithValue("pointy", point.PointY);

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new Point
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PointX = reader.GetDouble(2),
                        PointY = reader.GetDouble(3)
                    };
                }
                else
                {
                    throw new InvalidOperationException("Nokta eklenirken bir hata oluştu");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Nokta eklenirken bir hata oluştu", ex);
            }
        }

        public async Task<Point?> UpdateById(int id, Point point)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand(
                    "UPDATE points SET name = @name, pointx = @pointx, pointy = @pointy WHERE id = @id RETURNING id, name, pointx, pointy",
                    connection);
                command.Parameters.AddWithValue("name", point.Name);
                command.Parameters.AddWithValue("pointx", point.PointX);
                command.Parameters.AddWithValue("pointy", point.PointY);
                command.Parameters.AddWithValue("id", id);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new Point
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PointX = reader.GetDouble(2),
                        PointY = reader.GetDouble(3)
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Nokta güncellenirken bir hata oluştu", ex);
            }
        }

        public async Task<bool> DeleteById(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                using var command = new NpgsqlCommand("DELETE FROM points WHERE id = @id", connection);
                command.Parameters.AddWithValue("id", id);
                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Nokta silinirken bir hata oluştu", ex);
            }
        }
    }
}
