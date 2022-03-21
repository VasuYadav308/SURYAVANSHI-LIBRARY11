using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SURYAVANSHI_LIBRARY.Models
{
    public class LateTransactionInfo
    {
        public LateTransactionInfo(string customerName, string authorName, string bookTitle, string iSBN, DateTime dateOfIssue)
        {
            CustomerName = customerName;
            AuthorName = authorName;
            BookTitle = bookTitle;
            ISBN = iSBN;
            DateOfIssue = dateOfIssue;
        }

        public string CustomerName { get; set; }
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public DateTime DateOfIssue { get; set; }

        public int LateFees => (int)(DateTime.Now - DateOfIssue).TotalDays * 100;

    }
}
