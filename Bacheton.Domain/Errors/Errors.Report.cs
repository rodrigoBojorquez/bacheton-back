using ErrorOr;

namespace Bacheton.Domain.Errors;

public static partial class Errors
{
    public static class Report
    {
        public static Error NotFound => 
            Error.NotFound("Report.NotFound", "Reporte no encontrado");
    }
}