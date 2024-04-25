using AssignmentTask.Entity.Data;
using AssignmentTask.Entity.Models;
using AssignmentTask.Entity.ViewModels;
using AssignmentTask.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentTask.Repository.Implement
{
    public class Library : ILibrary
    {
        private readonly LibraryDbContext _library;

        public Library(LibraryDbContext context)
        {
            _library = context;
        }

        public HomeDataTableModel getBooks(string searchName, int page, int pageSize)
        {
            List<BookPopupViewModel> books = _library.Books
                .Include(x => x.Borrower)
                .Where(x => searchName == null || x.Bookname.ToLower().Contains(searchName.ToLower()))
                .Select(x => new BookPopupViewModel
                {
                    id = x.Id,
                    BookName = x.Bookname,
                    AuthorName = x.Author,
                    BorrowerName = x.Borrowername,
                    DateOfIssue = x.Dateofissue.Value.Date,
                    Genere = x.Genere,
                    City = x.City,
                })
                .AsEnumerable()
                .OrderBy(x => x.id).ToList();
            HomeDataTableModel data = new HomeDataTableModel { BookModelList = books.Skip((page * pageSize)).Take(5).ToList(), totalRecord = books.Count() };
            return data;
        }

        public List<string> getCityFromBorrow(string name)
        {
            return _library.Borrowers.Where(x => name == null || x.City.ToLower().Contains(name.ToLower())).Select(x => x.City).ToList();
        }


        public BookPopupViewModel getSingleBook(int id)
        {
            BookPopupViewModel book = _library.Books.Where(x => x.Id == id)
                .Select(x => new BookPopupViewModel
                {
                    BookName = x.Bookname,
                    AuthorName = x.Author,
                    BorrowerName = x.Borrowername,
                    DateOfIssue = x.Dateofissue.Value.Date,
                    Genere = x.Genere,
                    City = x.City,
                }).FirstOrDefault();

            return book;
        }


        public bool CheckExistCity(string city)
        {
            return _library.Borrowers.Any(x => x.City.ToLower().Contains(city.ToLower()));
        }

        public Borrower newBorrower(string city)
        {
            Borrower borrower = new Borrower()
            {
                City = city,
            };
            _library.Borrowers.Add(borrower);
            _library.SaveChanges();
            return borrower;
        }


        public void addBookDetail(BookPopupViewModel book)
        {
            Nullable<int> borowId = null;
            if (CheckExistCity(book.City))
            {
                borowId = _library.Borrowers.FirstOrDefault(x => x.City.ToLower().Contains(book.City.ToLower())).Id;
            }
            else
            {
                Borrower borower = newBorrower(book.City);
                borowId = borower.Id;
            }


            Book bookdetail = new Book()
            {
                Bookname = book.BookName,
                Author = book.AuthorName,
                Borrowerid = borowId,
                Borrowername = book.BorrowerName,
                Dateofissue = book.DateOfIssue,
                Genere = book.Genere,
                City = book.City,
            };
            _library.Books.Add(bookdetail);
            _library.SaveChanges();
        }

        public void updateBookDetail(BookPopupViewModel book)
        {
            Nullable<int> borowId = null;
            if (CheckExistCity(book.City))
            {
                borowId = _library.Borrowers.FirstOrDefault(x => x.City.ToLower().Contains(book.City.ToLower())).Id;
            }
            else
            {
                Borrower borower = newBorrower(book.City);
                borowId = borower.Id;
            }

            Book bookdetail = _library.Books.FirstOrDefault(x => x.Id == book.id);
            bookdetail.Bookname = book.BookName;
            bookdetail.Author = book.AuthorName;
            bookdetail.Borrowerid = borowId;
            bookdetail.Borrowername = book.BorrowerName;
            bookdetail.Dateofissue = book.DateOfIssue.Date;
            bookdetail.Genere = book.Genere;
            bookdetail.City = book.City;
            _library.Books.Update(bookdetail);
            _library.SaveChanges();
        }

        public void removeBookDetail(int id)
        {
            Book bookdetail = _library.Books.FirstOrDefault(x => x.Id == id);
            _library.Books.Remove(bookdetail);
            _library.SaveChanges();
        }
    }
}
