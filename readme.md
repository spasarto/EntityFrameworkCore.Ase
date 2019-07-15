# EntityFrameworkCore.Ase #

A very barebones implementation of EntityFrameworkCore. Supports basic CRUD interactions. Lots of untested features.

Most of this implementation was copied from the SqlServer implementation.

Example usage:

```csharp
        class TestDbContext : DbContext
        {
            private readonly string _connString;

            public TestDbContext(string connString)
            {
                _connString = connString;
            }
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);

                optionsBuilder.UseAse(_connString);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                modelBuilder.ApplyConfiguration(new Models.OrderConfiguration());
            }
        }
```