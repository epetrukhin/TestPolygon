using System.Diagnostics.CodeAnalysis;
using Maxima.Server.Database.Main;

namespace ConsoleApp
{
    internal static partial class Program
    {
        [SuppressMessage("ReSharper", "ConvertMethodToExpressionBody")]
        private static void ProgramCode()
        {

            return;

//            WorkWithDb(dataSource: ".", initialCatalog: "Test");
//            WorkWithDb(dataSource: "10.10.15.112" /* Эталон */, initialCatalog: "Main");
            WorkWithDb(dataSource: "AppServer-QA2", initialCatalog: "Main", TraceLevel.Normal);
        }

        private static void WorkWithDb([JetBrains.Annotations.NotNull] DbMain db)
        {
            db.SetNoexecOn();
        }
    }
}