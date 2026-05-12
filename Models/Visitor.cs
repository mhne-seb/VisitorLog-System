using System.ComponentModel.DataAnnotations;

namespace VisitorLog.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100, ErrorMessage = "Max 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose of Visit is required")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        [Display(Name = "Purpose of Visit")]
        public string PurposeOfVisit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Person to Visit is required")]
        [StringLength(100, ErrorMessage = "Max 100 characters")]
        [Display(Name = "Person to Visit")]
        public string PersonToVisit { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Number is required")]
        [StringLength(20, ErrorMessage = "Max 20 characters")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Display(Name = "Date & Time Visited")]
        public DateTime DateTimeVisited { get; set; } = DateTime.Now;
    }
}
