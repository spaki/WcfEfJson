using System.Collections.Generic;

namespace WcfEf.Model
{
    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
