using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SURYAVANSHI_LIBRARY.Data;
using SURYAVANSHI_LIBRARY.Models;

namespace SURYAVANSHI_LIBRARY.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transaction.Include(t => t.Book).Include(t => t.Customer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Book)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,CustomerId,DateOfIssue,DateOfReturn")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var book = await _context.Book.FindAsync(transaction.BookId);
                if (book.ISBN == transaction.BookId)
                {
                    if (book.IssuedStatus == true)
                    {
                        TempData["success-msg"] = $"Book is Already Issued to Customer.Wating for return";
                        return View();
                    }
                    else
                    {
                        transaction.DateOfIssue = DateTime.Now;
                        _context.Add(transaction);

                        book.IssuedStatus = true;
                        _context.Update(book);

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));

                    }

                }

            }
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "ISBN", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", transaction.CustomerId);
            return View(nameof(Index), transaction);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "ISBN", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", transaction.CustomerId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,CustomerId,DateOfIssue,DateOfReturn")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
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
            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name", transaction.CustomerId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transaction
                .Include(t => t.Book)
                .Include(t => t.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.Id == id);
        }


        // GET: Transactions/ReturnBook
        public IActionResult ReturnBook()
        {
            var transaction = new Transaction();
            transaction.DateOfReturn = DateTime.Now;


            ViewData["BookId"] = new SelectList(_context.Book, "ISBN", "Title");
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Name");
            return View(transaction);
        }

        // POST: Transactions/ReturnBook
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]     
        public async Task<IActionResult> ReturnBook(string bookId, int customerId, DateTime dateOfReturn)
        {
            var transaction = await _context.Transaction
                .Where(t => t.BookId == bookId && t.CustomerId == customerId)
                .FirstOrDefaultAsync();

            transaction.DateOfReturn = dateOfReturn;
            _context.Update(transaction);

            var book = await _context.Book.FindAsync(transaction.BookId);
            book.IssuedStatus = false;
            _context.Update(book);

            await _context.SaveChangesAsync();

            ViewData["BookID"] = new SelectList(_context.Book, "ISBN", "Title", transaction.BookId);
            ViewData["CustomerID"] = new SelectList(_context.Customer, "ID", "Name", transaction.CustomerId);

            var expectedReturndate = transaction.DateOfIssue.AddDays(7);

            if (dateOfReturn > expectedReturndate)
            {
                TempData["late-submission-msg"] = "Late Submission-Currently fine is Rs. 100 per late day.";
                return RedirectToAction(nameof(ReturnBook));
            }
            return RedirectToAction(nameof(Index));

        }

        //Transactions/LateBook
        public async Task<IActionResult> LateBook()
        {
            var transactionQuery =
                from t in _context.Transaction
                join b in _context.Book on t.BookId equals b.ISBN
                join c in _context.Customer on t.CustomerId equals c.Id
                join a in _context.Author on b.AuthorId equals a.Id
                select new LateTransactionInfo(c.Name, a.Name, b.Title, b.ISBN, t.DateOfIssue);

            //select new LateTransactionInfo(c.Name, a.Name, b.Title, b.ISBN,
            // (DateTime.Now - t.DateOfIssue).TotalDays * 100);

            var transactions = await transactionQuery.ToListAsync();
            var transactionsWithFine = transactions.Where(t => (DateTime.Now - t.DateOfIssue).TotalDays > 7);

            return View(transactionsWithFine);
        }


    }
}
