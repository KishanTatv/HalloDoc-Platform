using AssignmentTask.Entity.Models;
using AssignmentTask.Entity.ViewModels;
using AssignmentTask.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentTask.Controllers
{
    public class LibraryController : Controller
    {

        private readonly ILibrary _library;

        public LibraryController(ILibrary library)
        {
            _library = library;
        }

        #region Main Page
        public IActionResult LibraryHome()
        {
            return View();
        }

        public IActionResult LibraryData(string searchName, int page)
        {
            int pageSize = 5;
            HomeDataTableModel booksDetail = _library.getBooks(searchName, page, pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TPage = Math.Ceiling(booksDetail.totalRecord.Value / 5.0);
            ViewBag.TotalRecord = booksDetail.totalRecord.Value;
            return PartialView("_libTable", booksDetail);
        }
        #endregion


        #region Popup
        public IActionResult AddBook(int id)
        {
            if (id != 0)
            {
                BookPopupViewModel book = _library.getSingleBook(id);
                return PartialView("_PopupAddBook", book);
            }
            return PartialView("_PopupAddBook");
        }
        #endregion


        #region Add
        public IActionResult BookDetailSave(BookPopupViewModel modelData)
        {
            if (ModelState.IsValid)
            {
                _library.addBookDetail(modelData);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion


        #region Update
        public IActionResult UpdateBookDetail(BookPopupViewModel modelData)
        {
            if (ModelState.IsValid)
            {
                _library.updateBookDetail(modelData);
                return Ok();
            }
            return BadRequest();
        }
        #endregion


        #region Delete
        public IActionResult BookDetailDelete(int id)
        {
            if (id != 0)
            {
                _library.removeBookDetail(id);
            }
            return Ok();
        }
        #endregion



        public List<string> getSuggestCity(string name)
        {
            List<string> cityList = _library.getCityFromBorrow(name);
            return cityList;
        }
    }
}
