using backend.Context;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    // Carga inicial de dados para facilitar os testes:
    // - cria o usuario padrao (admin / Admin@123)
    // - cria alguns clientes, motoristas e veiculos de exemplo
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            // Garante que o banco esta atualizado com as migrations.
            await context.Database.MigrateAsync();

            // Usuario padrao.
            const string usuarioPadrao = "admin";
            const string senhaPadrao = "Admin@123";

            if (await userManager.FindByNameAsync(usuarioPadrao) == null)
            {
                var usuario = new IdentityUser
                {
                    UserName = usuarioPadrao,
                    Email = "admin@transportadora.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(usuario, senhaPadrao);
            }

            // Clientes de exemplo.
            if (!await context.Clientes.AnyAsync())
            {
                context.Clientes.AddRange(
                    new Cliente { Nome = "Comercial Silva", Documento = "12.345.678/0001-00", Email = "contato@silva.com", Telefone = "11999990000" },
                    new Cliente { Nome = "Distribuidora Norte", Documento = "98.765.432/0001-11", Email = "vendas@norte.com", Telefone = "11888880000" }
                );
            }

            // Motoristas de exemplo.
            if (!await context.Motoristas.AnyAsync())
            {
                context.Motoristas.AddRange(
                    new Motorista { Nome = "João Pereira", Cpf = "111.222.333-44", Telefone = "11977770000" },
                    new Motorista { Nome = "Carlos Souza", Cpf = "555.666.777-88", Telefone = "11966660000" }
                );
            }

            // Veiculos de exemplo.
            if (!await context.Veiculos.AnyAsync())
            {
                context.Veiculos.AddRange(
                    new Veiculo { Placa = "ABC1D23", Modelo = "Sprinter", Marca = "Mercedes", CapacidadeCargaKg = 1500 },
                    new Veiculo { Placa = "EFG4H56", Modelo = "Daily", Marca = "Iveco", CapacidadeCargaKg = 2000 }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
