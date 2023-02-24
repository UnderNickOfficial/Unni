using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unni.Infrastructure.Logger.Models;

namespace Unni.Infrastructure.Logger.Services.Interfaces
{
    public interface IDatabaseLoggerService<Context> where Context : DbContext
    {
        public Task CreateAsync(AppException exception, string? key = null);
    }
}
