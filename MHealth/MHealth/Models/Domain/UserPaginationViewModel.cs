﻿namespace MHealth.Models.Domain
{
    public class UserPaginationViewModel
    {
        public List<UserModel>? Users { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

    }
}
