using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SURYAVANSHI_LIBRARY.Data;
using SURYAVANSHI_LIBRARY.Models;
using SURYAVANSHI_LIBRARY.Services;

namespace SURYAVANSHI_LIBRARY.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        //private IBookManagementService _bookManagementService;
        //public BooksController(IBookManagementService bookManagementService)
        //{
        //    _bookManagementService = bookManagementService;
        //}

        // GET: Books
        public IActionResult Index(string SearchBy, string search)
        {
            if (SearchBy == "ISBN")
                return View(_context.Book.Where(result => result.ISBN == search || search == null).ToList());
            //else if (SearchBy == "AuthorId")
            //    return View(_context.Book.Where(result => result.AuthorId == search || search == null).ToList());
            //else if (SearchBy == "PublisherId")
            //    return View(_context.Book.Where(result => result.PublisherId == search || search == null).ToList());
            else
                return View(_context.Book.Where(result => result.Title.StartsWith(search) || search == null).ToList());

        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ISBN,Title,PublisherId,AuthorId,IssuedStatus,IsDeleted")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Id", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "Id", "Id", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ISBN,Title,PublisherId,AuthorId,IssuedStatus,IsDeleted")] Book book)
        {
            if (id != book.ISBN)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ISBN))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Author, "Id", "Name", book.AuthorId);
            ViewData["PublisherId"] = new SelectList(_context.Publisher, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(m => m.ISBN == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(string id)
        {
            return _context.Book.Any(e => e.ISBN == id);
        }

        //public IActionResult Search(string SearchBy, string search)
        //{
        //    if (SearchBy == "ISBN")
        //        return View(_context.Book.Where(result => result.ISBN == search || search == null).ToList());
        //    //else if (SearchBy == "AuthorId")
        //    //    return View(_context.Book.Where(result => result.AuthorId == search || search == null).ToList());
        //    //else if (SearchBy == "PublisherId")
        //    //    return View(_context.Book.Where(result => result.PublisherId == search || search == null).ToList());
        //    else
        //        return View(_context.Book.Where(result => result.Title.StartsWith(search) || search == null).ToList());
        //}
        public async Task<IActionResult> AvailBooks()
        {
            //var applicationDbContext = _context.Book.Include(b => b.Author).Include(b => b.Publisher);
            //return View(await applicationDbContext.ToListAsync());
            return View(await _context.Book.ToListAsync());
        }
        public async Task<IActionResult> IssuedBooks()
        {
            return View(await _context.Book.ToListAsync());
        }

    }
}
