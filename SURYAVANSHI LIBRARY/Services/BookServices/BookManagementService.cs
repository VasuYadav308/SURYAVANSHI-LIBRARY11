using SURYAVANSHI_LIBRARY.Models;

namespace SURYAVANSHI_LIBRARY.Services
{
    public class BookManagementService:IBookManagementService
    {

        public void IssuedBook()
        {
            throw new System.NotImplementedException();
        }

        public void IssuedBooks()
        {
            var book = new Book();
            if(book.IssuedStatus==false)
            {
               
            }
            else
            {

            }
        }

    }
}
