using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.ViewModels.ExpenseViewModels
{
    public class ExpenseCreateEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Description")]
        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
