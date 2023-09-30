namespace MHealth.Models.Domain
{
    public class BookingPaginationViewModel
    {
        public List<BookingViewModel> Bookings { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
