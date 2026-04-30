using Microsoft.EntityFrameworkCore;
using ReniecRamos.Models;
using System.Reflection.Emit;
namespace ReniecRamos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ubigeo> Ubigeos { get; set; }
        public DbSet<CivilStatus> CivilStatuses { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Procedure> Procedures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para evitar el ciclo en BirthPlace
            modelBuilder.Entity<Citizen>()
                .HasOne(c => c.BirthPlace)
                .WithMany()
                .HasForeignKey(c => c.BirthUbigeoId)
                .OnDelete(DeleteBehavior.Restrict); // <--- ESTO ES LO IMPORTANTE

            // Configuración para evitar el ciclo en ResidencePlace
            modelBuilder.Entity<Citizen>()
                .HasOne(c => c.ResidencePlace)
                .WithMany()
                .HasForeignKey(c => c.CurrentUbigeoId)
                .OnDelete(DeleteBehavior.Restrict); // <--- ESTO ES LO IMPORTANTE
        

            // 1. Seed de Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Administrator" },
                new Role { RoleId = 2, Name = "Assistant" }
            );

            // 2. Seed de Tipos de Documento
            modelBuilder.Entity<DocumentType>().HasData(
                new DocumentType { DocumentTypeId = 1, Description = "Documento Nacional de Identidad", Abbreviation = "DNI" },
                new DocumentType { DocumentTypeId = 2, Description = "Pasaporte", Abbreviation = "PAS" },
                new DocumentType { DocumentTypeId = 3, Description = "Carnet de Extranjería", Abbreviation = "CE" }
            );

            // 3. Seed de Estados Civiles
            modelBuilder.Entity<CivilStatus>().HasData(
                new CivilStatus { CivilStatusId = 1, Description = "Soltero(a)" },
                new CivilStatus { CivilStatusId = 2, Description = "Casado(a)" },
                new CivilStatus { CivilStatusId = 3, Description = "Divorciado(a)" },
                new CivilStatus { CivilStatusId = 4, Description = "Viudo(a)" }
            );

            // 4. Seed de 10 Ubigeos (Códigos reales INEI)
            modelBuilder.Entity<Ubigeo>().HasData(
                new Ubigeo { UbigeoId = "150101", Department = "Lima", Province = "Lima", District = "Lima" },
                new Ubigeo { UbigeoId = "150122", Department = "Lima", Province = "Lima", District = "Miraflores" },
                new Ubigeo { UbigeoId = "150142", Department = "Lima", Province = "Lima", District = "Surquillo" },
                new Ubigeo { UbigeoId = "070101", Department = "Callao", Province = "Callao", District = "Callao" },
                new Ubigeo { UbigeoId = "070106", Department = "Callao", Province = "Callao", District = "Ventanilla" },
                new Ubigeo { UbigeoId = "040101", Department = "Arequipa", Province = "Arequipa", District = "Arequipa" },
                new Ubigeo { UbigeoId = "080101", Department = "Cusco", Province = "Cusco", District = "Cusco" },
                new Ubigeo { UbigeoId = "130101", Department = "La Libertad", Province = "Trujillo", District = "Trujillo" },
                new Ubigeo { UbigeoId = "140101", Department = "Lambayeque", Province = "Chiclayo", District = "Chiclayo" },
                new Ubigeo { UbigeoId = "190101", Department = "Pasco", Province = "Pasco", District = "Chaupimarca" }
            );
            // 5. Seed de Usuarios (Contraseña "123" para ambos con un Hash estático)
            // Este hash ya contiene el salt y corresponde a "123"
            string staticHash = "$2a$11$m9N98XzY9.KstHlG.G8v7u8Vv5Zp6M8Lp7S6R5Q4P3O2N1M0L9K8J";
            byte[] passBytes = System.Text.Encoding.UTF8.GetBytes(staticHash);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Username = "jhonAd",
                    FullName = "Jhon Administrador",
                    PasswordHash = passBytes,
                    RoleId = 1,
                    IsActive = true
                },
                new User
                {
                    UserId = 2,
                    Username = "jhonAsis",
                    FullName = "Jhon Asistente",
                    PasswordHash = passBytes,
                    RoleId = 2,
                    IsActive = true
                }
           
            );
        }

    }
}
    
    

