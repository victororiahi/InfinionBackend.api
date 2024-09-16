using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinionBackend.Infrastructure.Utitlities
{
    public class Page<T>
    {
        public long PageSize { get; }
        public long PageNumber { get; }
        public long TotalSize { get; }
        public T[] Items { get; set; }

        public Page(T[] items, long totalSize, long pageNumber, long pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            TotalSize = totalSize;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Items = items;
        }
    }
    public class PagedEnumerable<T> : PageInfo
    {
        public IEnumerable<T> Data { get; set; }

        public PagedEnumerable(IEnumerable<T> items, int totalSize, int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            Size = totalSize;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = items;
        }
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Size { get; set; }
    }
}
