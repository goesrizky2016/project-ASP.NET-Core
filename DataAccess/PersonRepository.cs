using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ExportDemo.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace ExportDemo.DataAccess
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string? _connectionString;

        public PersonRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Person> GetPersons()
        {
            var persons = new List<Person>();

            using (var conn = new SqlConnection(_connectionString))
            {
                using var cmd = new SqlCommand("sp_GetPersonsWithCTE", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var person = new Person
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nama = reader.GetString(reader.GetOrdinal("Nama")),
                        Umur = reader.GetInt32(reader.GetOrdinal("Umur"))
                    };

                    persons.Add(person);
                }
            }

            return persons;
        }
    }
}
