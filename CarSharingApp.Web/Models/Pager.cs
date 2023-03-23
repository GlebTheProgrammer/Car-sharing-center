namespace CarSharingApp.Web.Models
{
    public sealed class Pager
    {
        public Pager()
        {

        }

        public Pager(int totalItems, int page, int pageSize = 6)
        {
            int totalPages = (int)Math.Ceiling(totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 3;
            int endPage = currentPage + 2;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;

                if (endPage > 6)
                    startPage = endPage - 5;
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
    }
}
