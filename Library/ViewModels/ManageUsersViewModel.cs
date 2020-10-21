using System.Collections.Generic;
using Library.Models;

namespace Library.ViewModels
{
    public class ManageUsersViewModel
    {
        public ApplicationUser[] Administrators { get; set; }

        public ApplicationUser[] Everyone { get; set;}
    }
}