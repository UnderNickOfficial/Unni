using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unni.Infrastructure.Database.Repositories;
using Unni.Infrastructure.Logger.Models;

namespace Unni.Infrastructure.Logger.Repositories
{
    public class AppExceptionRepository<Context> : GenericRepository<Context, AppException> where Context : DbContext
    {
        public AppExceptionRepository(Context context) : base(context)
        {
        }

        public async Task<AppException?> FindByIdFull(string exceptionId)
        {
            return await Table.Include(x => x.ExceptionCases).FirstOrDefaultAsync(x => x.Id == exceptionId);
        }
        public async Task<AppException?> FindById(string exceptionId)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == exceptionId);
        }
        public async Task<int> GetCount(string search)
        {
            if (search == "")
                return await Table.CountAsync();
            return await Table.Where(x => x.Id.Contains(search) || x.Message.Contains(search)).CountAsync();
        }
        public async Task<List<AppException>> Get(int page, int count, string search)
        {
            if (search == "")
                return await Table.OrderByDescending(x => x.UpdatedAt != null ? x.UpdatedAt : x.CreatedAt).Skip(count * page).Take(count).Include(x => x.ExceptionCases).AsSplitQuery().ToListAsync();
            return await Table.Where(x => x.Id.Contains(search) || x.Message.Contains(search)).OrderByDescending(x => x.UpdatedAt != null ? x.UpdatedAt : x.CreatedAt).Skip(count * page).Take(count).Include(x => x.ExceptionCases).AsSplitQuery().ToListAsync();
        }
    }
}
