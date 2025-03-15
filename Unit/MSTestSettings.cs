using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)] // Executa testes de classes diferentes em paralelo

[TestClass]
public class TestSetup
{
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        try
        {
            context.WriteLine("Inicializando MSTest para Testes Unitários");

            // Exemplo de possível configuração global, como conexão com banco de dados fake
            // DatabaseTestHelper.InitializeTestDatabase();

            context.WriteLine("Configuração inicial concluída.");
        }
        catch (Exception ex)
        {
            context.WriteLine($"Erro na configuração dos testes: {ex.Message}");
            throw;
        }
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        try
        {
            // Exemplo de cleanup global
            // DatabaseTestHelper.CleanupTestDatabase();

            Console.WriteLine("Cleanup dos testes concluído.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante cleanup dos testes: {ex.Message}");
        }
    }
}
