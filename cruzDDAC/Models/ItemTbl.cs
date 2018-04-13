//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cruzDDAC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class ItemTbl
    {
        [Key]
        public int Item_ID { get; set; }
        [Display(Name = "Item Name")]
        [Required]
        public string Item_Name { get; set; }
        [Display(Name = "Item Quantity")]
        [Required]
        public Nullable<int> Item_Quantity { get; set; }
        [Display(Name = "Item Customer")]
        [Required]
        public int Item_Customer { get; set; }
        [Display(Name = "Item Allocated Vessel")]
        public Nullable<int> Item_Vessel { get; set; }

        public virtual CustomerTbl CustomerTbl { get; set; }
        public virtual VesselTbl VesselTbl { get; set; }
    }
}
