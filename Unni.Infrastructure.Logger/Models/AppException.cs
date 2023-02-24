using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unni.Infrastructure.Logger.Models
{
    public class AppException
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public virtual List<AppExceptionCase> ExceptionCases { get; set; }
    }
}
