using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unni.Infrastructure.Logger.Models
{
    public class AppExceptionCase
    {
        public ulong Id { get; set; }
        public string AppExceptionId { get; set; }
        public int? CasesCount { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
