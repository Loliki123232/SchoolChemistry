using Npgsql;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SchoolChemistry.Models;

namespace SchoolChemistry.DataAccess
{
    public class DatabaseService
    {
        private readonly string _connectionString;
        
        public DatabaseService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ChemistryDb"].ConnectionString;
        }
        /// <summary>
        /// Возвращает список всех тем из базы данных, упорядоченных по идентификатору.
        /// </summary>
        /// <returns>Коллекция объектов <see cref="Theme"/>, представляющих все темы.</returns>
        public IEnumerable<Theme> GetAllThemes()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            return conn.Query<Theme>("SELECT id, name, content FROM themes ORDER BY id");
        }
        /// <summary>
        /// Возвращает список всех химических элементов из базы данных, упорядоченных по атомному номеру.
        /// </summary>
        /// <returns>Коллекция объектов <see cref="ChemicalElement"/>, содержащих полную информацию об элементах.</returns>
        public IEnumerable<ChemicalElement> GetAllElements()
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM elements ORDER BY atomic_number";
            var result = new List<ChemicalElement>();

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                var element = new ChemicalElement
                {
                    Id = reader.GetInt32(0),
                    Symbol = reader.GetString(1),
                    Name = reader.GetString(2),
                    AtomicNumber = reader.GetInt32(3),      // atomic_number
                    AtomicMass = reader.GetDecimal(4),       // atomic_mass
                    GroupNumber = reader.GetInt32(5),        // group_number
                    Period = reader.GetInt32(6),             // period
                    Description = reader.IsDBNull(7) ? "" : reader.GetString(7)
                };
                result.Add(element);
            }

            return result;
        }
        /// <summary>
        /// Выполняет поиск химического элемента по его символу или названию (без учёта регистра).
        /// </summary>
        /// <param name="query">Строка поиска: может быть символом элемента (например, "H" или "h") или названием (например, "Hydrogen").</param>
        /// <returns>Объект <see cref="ChemicalElement"/>, соответствующий критериям поиска, или <c>null</c>, если элемент не найден.</returns>
        public ChemicalElement GetElementBySymbolOrName(string query)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM elements WHERE LOWER(symbol) = LOWER(@query) OR LOWER(name) = LOWER(@query)";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("query", query);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var element = new ChemicalElement
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Symbol = reader.GetString(reader.GetOrdinal("symbol")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    AtomicNumber = reader.GetInt32(reader.GetOrdinal("atomic_number")),
                    AtomicMass = reader.GetDecimal(reader.GetOrdinal("atomic_mass")),
                    GroupNumber = reader.IsDBNull(reader.GetOrdinal("group_number")) ? null : reader.GetInt32(reader.GetOrdinal("group_number")),
                    Period = reader.IsDBNull(reader.GetOrdinal("period")) ? null : reader.GetInt32(reader.GetOrdinal("period")),
                    Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description"))
                };

                return element;
            }

            return null;
        }
    }
}
