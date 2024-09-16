﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinionBackend.Infrastructure.Utitlities
{
    public static class Extensions
    {
        public static Page<T> ToPageList<T>(this IEnumerable<T> query, int pageNumber, int pageSize)
        {
            var count = query.Count();
            int offset = (pageNumber - 1) * pageSize;
            var items = query.Skip(offset).Take(pageSize).ToArray();
            return new Page<T>(items, count, pageNumber, pageSize);
        }
    }

    public static async Task<Page<T>> ToPageListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = await query.CountAsync();
        int offset = (pageNumber - 1) * pageSize;
        var items = await query.Skip(offset).Take(pageSize).ToArrayAsync();
        return new Page<T>(items, count, pageNumber, pageSize);
    }
}
