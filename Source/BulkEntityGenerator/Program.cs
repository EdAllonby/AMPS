using System;
using System.Threading.Tasks;
using log4net;

namespace BulkEntityGenerator
{
    /// <summary>
    /// Bombard the Server with generated clients. Mainly for concurrency and stress testing.
    /// </summary>
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            LoginUser(1);

            // LoginUsers(50);

            Log.Debug("Start");

            Console.ReadKey();
        }

        private static void LoginUsers(int count)
        {
            Parallel.For(0, count, LoginUser);
        }

        private static void LoginUser(int i)
        {
            var generator = new Generator($"generatedUser{i}");
            generator.LoginUser();

            generator.BootstrapCompleted += (s, e) => Parallel.For(0, 10000, j => { generator.CreateTask(); });
        }
    }
}