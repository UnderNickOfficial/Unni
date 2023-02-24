using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unni.Infrastructure.Database.Repositories;
using Unni.Infrastructure.Logger.Models;
using Unni.Infrastructure.Logger.Repositories;
using Unni.Infrastructure.Logger.Services.Interfaces;

namespace Unni.Infrastructure.Logger.Services
{
    public class DatabaseLoggerService<Context> : IDatabaseLoggerService<Context> where Context : DbContext
    {
        private readonly Context context;

        public DatabaseLoggerService(Context context)
        {
            this.context = context;
        }
        public async Task CreateAsync(AppException exception, string? key = null) 
        {
            var appExceptionRepository = new AppExceptionRepository<Context>(context);
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(exception.Message.ToString()));
                exception.Id = BitConverter.ToString(bytes).Replace("-", String.Empty);
            }
            var realException = await appExceptionRepository.FindByIdFull(exception.Id);
            if (realException == null)
            {
                await InternalCreate(exception, appExceptionRepository, key);
            }
            else
            {
                await InternalUpdate(exception, realException, appExceptionRepository, key);
            }
        }

        private async Task InternalUpdate(AppException exception, AppException realException, AppExceptionRepository<Context> appExceptionRepository, string? key = null)
        {
            var realExceptionCase = realException.ExceptionCases?.FirstOrDefault(x => x.Key == key);
            var appExceptionCaseRepository = context.Set<AppExceptionCase>();
            if (realExceptionCase != null)
            {
                realExceptionCase.CasesCount += 1;
                realExceptionCase.UpdatedAt = DateTime.UtcNow;
                appExceptionCaseRepository.Update(realExceptionCase);
            }
            else
            {
                appExceptionCaseRepository.Add(CreateAppExceptionCase(exception, key));
            }
            realException.UpdatedAt = DateTime.UtcNow;
            appExceptionRepository.Update(realException);
            await context.SaveChangesAsync();
        }

        private async Task InternalCreate(AppException exception, AppExceptionRepository<Context> appExceptionRepository, string? key = null)
        {
            var realException = new AppException
            {
                CreatedAt = DateTime.UtcNow,
                Id = exception.Id,
                StackTrace = exception.StackTrace,
                InnerException = exception.InnerException,
                Message = exception.Message
            };
            var appExceptionCaseRepository = context.Set<AppExceptionCase>();
            appExceptionCaseRepository.Add(CreateAppExceptionCase(exception, key));
            appExceptionRepository.Insert(realException);
            await context.SaveChangesAsync();
        }

        private AppExceptionCase CreateAppExceptionCase(AppException realException, string? key = null)
        {
            var realExceptionCase = new AppExceptionCase()
            {
                AppExceptionId = realException.Id,
                Key = key,
                CreatedAt = DateTime.UtcNow,
                CasesCount = 1
            };
            return realExceptionCase;
        }

        public async Task<UnniResult> DeleteAsync(string exceptionId)
        {
            try
            {
                var appExceptionRepository = new AppExceptionRepository<Context>(context);
                var realException = await appExceptionRepository.FindByIdFull(exceptionId);
                if (realException == null)
                    return new UnniResult(false);
                DbSet<AppExceptionCase> dbSetCase = context.Set<AppExceptionCase>();
                foreach (var exCase in realException.ExceptionCases)
                {
                    dbSetCase.Remove(exCase);
                }
                appExceptionRepository.Delete(realException);
                await context.SaveChangesAsync();
                return new UnniResult();
            }
            catch (Exception ex)
            {
                return new UnniResult(ex);
            }
        }

        public async Task<UnniResult<AppException?>> FindByIdAsync(string exceptionId)
        {

            try
            {
                var appExceptionRepository = new AppExceptionRepository<Context>(context);
                var realException = await appExceptionRepository.FindByIdFull(exceptionId);
                return new UnniResult<AppException?>(realException);
            }
            catch (Exception ex)
            {
                return new UnniResult<AppException?>(ex);
            }
        }

        public async Task<UnniResult<List<AppException>>> GetAsync(int page, int count, string search)
        {
            try
            {
                var appExceptionRepository = new AppExceptionRepository<Context>(context);
                return new UnniResult<List<AppException>>(await appExceptionRepository.Get(page, count, search));
            }
            catch (Exception ex)
            {
                return new UnniResult<List<AppException>>(ex);
            }
        }

        public async Task<UnniResult<int>> GetCountAsync(string search)
        {
            try
            {
                var appExceptionRepository = new AppExceptionRepository<Context>(context);
                return new UnniResult<int>(await appExceptionRepository.GetCount(search));
            }
            catch (Exception ex)
            {
                return new UnniResult<int>(ex);
            }
        }
    }
}
