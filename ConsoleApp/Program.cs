using System.Diagnostics.CodeAnalysis;
using Maxima.Server.Database.Main;

namespace ConsoleApp
{
    internal static partial class Program
    {
        [SuppressMessage("ReSharper", "ConvertMethodToExpressionBody")]
        private static void ProgramCode()
        {
            WorkWithDb(dataSource: "10.10.15.12", initialCatalog: "Main");
        }

        private static void WorkWithDb([JetBrains.Annotations.NotNull] DbMain db)
        {
            db.SetNoexecOn();
        }
    }
}