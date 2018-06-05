using System;
using System.Data.SqlClient;
using ConsoleApp.Helpers;
using LinqToDB.Data;
using Maxima.Server.Database.Main;

namespace ConsoleApp
{
    internal static partial class Program
    {
        private static void Trace([JetBrains.Annotations.NotNull] TraceInfo traceInfo)
        {
            switch (traceInfo.TraceInfoStep)
            {
                case TraceInfoStep.AfterExecute:
                case TraceInfoStep.Error:
                case TraceInfoStep.Completed:
                    break;
                default:
                    return;
            }

            $"{traceInfo.TraceInfoStep} ({traceInfo.TraceLevel})".WriteWarning();
            if (traceInfo.Exception != null)
                DumpException(traceInfo.Exception);

            if (!string.IsNullOrWhiteSpace(traceInfo.SqlText))
                traceInfo.SqlText.WriteLine();

            traceInfo.ExecutionTime?.Dump(nameof(traceInfo.ExecutionTime));
            traceInfo.RecordsAffected?.Dump(nameof(traceInfo.RecordsAffected));
            SeparatorLine();
        }

        private static void SetNoexecOn([JetBrains.Annotations.NotNull] this DataConnection db) => db.Execute("set noexec on");
        private static void SetNoexecOff([JetBrains.Annotations.NotNull] this DataConnection db) => db.Execute("set noexec off");

        private static void WorkWithDb(string dataSource, string initialCatalog)
        {
            DbMain.AddTraceListener(Trace);

            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource         = dataSource,
                InitialCatalog     = initialCatalog,
                IntegratedSecurity = true,
                ApplicationName    = $"Linq2DbTest_{Environment.MachineName}"
            }.ToString();

            connectionString.Dump("Connection string");
            SeparatorLine();

            using (var db = new DbMain(connectionString))
            {
                WorkWithDb(db);
            }
        }
    }
}