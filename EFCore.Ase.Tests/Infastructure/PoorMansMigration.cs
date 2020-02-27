using AdoNetCore.AseClient;

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
                    cmd.CommandText = "create table test_order (id int, name varchar(50), guid_id varchar(36))";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into test_order (id, name, guid_id) values (1, 'asdf', 'FA7D2349-87D6-4178-A6B5-F07D8293589A')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into test_order (id, name, guid_id) values (2, 'b', 'EA7D2349-87D6-4178-A6B5-F07D8293589A')";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "insert into test_order (id, name, guid_id) values (3, 'b', 'DA7D2349-87D6-4178-A6B5-F07D8293589A')";
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
