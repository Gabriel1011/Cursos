using System;
using System.Linq;
using CursoEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CursoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        private static readonly ILoggerFactory _logger = LoggerFactory.Create(p=> p.AddConsole());
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder){
            optionsBuilder
            .UseLoggerFactory(_logger)
            .EnableSensitiveDataLogging()
            .UseSqlServer("Server=localhost,1433;Database=EFCore;User Id=sa;Password=efcore@2021;",
            p => p.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(2),
                errorNumbersToAdd: null
            )
            .MigrationsHistoryTable("aplicacao_migracoes"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedadesEsquecidas(modelBuilder);
        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder){

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var propriedades = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var propriedade in propriedades)
                {
                    if(string.IsNullOrEmpty(propriedade.GetColumnType()) && !propriedade.GetMaxLength().HasValue)
                    {
                        //propriedade.SetMaxLength(100);
                        propriedade.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}
