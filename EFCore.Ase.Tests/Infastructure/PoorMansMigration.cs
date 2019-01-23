using AdoNetCore.AseClient;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFrameworkCore.Ase.Tests.Infastructure
{
    public class PoorMansMigration
    {
        private readonly AseOptions _options;

        public PoorMansMigration(AseOptions options)
        {
            _options = options;
        }

        private bool _up;
        public void Up()
        {
            try
            {
                using (var conn = new AseConnection(_options.ConnectionString))
                {
                    conn.Open();

                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "create table test_order (id int, name varchar(50))";
                    cmd.ExecuteNonQuery();
                    
                    cmd.CommandText = "insert into test_order (id, name) values (1, 'asdf')";
                    cmd.ExecuteNonQuery();
                }
            }
            catch
            {
                if (_up)
                    throw;

                _up = true;
                Down();
                Up();
            }
            finally
            {
                _up = false;
            }
        }

        public void Down()
        {
            using (var conn = new AseConnection(_options.ConnectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "drop table test_order";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
