using AssignmentTask.Entity.Models;
using AssignmentTask.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentTask.Repository.Interface
{
    public interface ILibrary
    {
        HomeDataTableModel getBooks(string searchName, int page, int pageSize);

        List<string> getCityFromBorrow(string name);

        BookPopupViewModel getSingleBook(int id);

        void addBookDetail(BookPopupViewModel book);

        void updateBookDetail(BookPopupViewModel book);

        void removeBookDetail(int id);
    }
}
