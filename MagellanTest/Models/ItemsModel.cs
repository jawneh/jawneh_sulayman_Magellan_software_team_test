using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MagellanTest.Models
{
    public class ItemsModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [StringLength(
            50,
            ErrorMessage = "The {0} must be at least {2} characters long.",
            MinimumLength = 4
        )]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        // [Required(ErrorMessage = "Item parent is required")]
        [DefaultValue(null)]
        [Display(Name = "Parent Item")]
        public int? ParentItem { get; set; }

        [RegularExpression(@"\d+", ErrorMessage = "Cost must be a number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Cost must be between 1 and 2,000,000")]
        [Display(Name = "Item cost")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Request Date")]
        [DataType(DataType.Date)]
        public DateTime ReqDate { get; set; }
    }
}
