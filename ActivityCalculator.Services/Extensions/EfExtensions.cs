using ActivityCalculator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityCalculator.Services.Extensions
{
    public static class EfExtensions
    {
        public static Task<T> GetByIdAsync<T>(this IQueryable<T> query, long id) where T : Entity
        {
            return query.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
