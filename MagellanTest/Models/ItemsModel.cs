using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MagellanTest.Models
{
    public class ItemsModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Item name is required")]
        [RegularExpression(
            @"^[a-zA-Z0-9\s]*$",
            ErrorMessage = "ItemName must only contain letters, numbers, and spaces"
        )]
        [StringLength(
            50,
            ErrorMessage = "The ItemName must be [4-50] characters long.",
            MinimumLength = 4
        )]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        // [Required(ErrorMessage = "Item parent is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Parent item must be a number")]
        [Display(Name = "Parent Item")]
        [DefaultValue(null)]
        public int? ParentItem { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Cost must be a number")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Cost must be between 1 and 2,000,000")]
        [Display(Name = "Item cost")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [Display(Name = "Request Date")]
        [DataType(DataType.Date)]
        public DateTime ReqDate { get; set; }
    }
}
