using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

[assembly: Parallelize(Scope = ExecutionScope.ClassLevel)] // Permite paralelismo por classe

[TestClass]
public class TestSetup
{
    [AssemblyInitialize]
    public static void AssemblyInit(TestContext context)
    {
        try
        {
            context.WriteLine("Inicializando MSTest para Testes de Integração");

            // Verifica se os testes de integração devem rodar
            if (Environment.GetEnvironmentVariable("RUN_INTEGRATION_TESTS") != "true")
            {
                context.WriteLine("Testes de integração estão desativados.");
                return;
            }

            // Configuração global, ex: iniciar banco de dados fake
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
            // DatabaseTestHelper.CloseConnections();

            Console.WriteLine("Cleanup dos testes concluído.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro durante cleanup dos testes: {ex.Message}");
        }
    }
}
