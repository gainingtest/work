using System.Collections.Generic;

namespace Com.EnjoyCodes.DapperRepositories.Entities
{
    public class Page<T>
    {
        public List<T> DataList { get; set; }

        public int Count { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
