namespace Service.ManagerVPS.DTO.OtherModels
{
    public class QueryResponseModel<T>
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Items { get; set; }
        = Enumerable.Empty<T>();
        public QueryResponseModel(int page, int pageSize, IQueryable<T> items)
        {
            this.Total = items.Count();
            this.Page = page;
            this.PageSize = pageSize;
            this.Items = items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
