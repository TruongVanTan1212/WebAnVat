﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DoAnVat.Models
{
    [Table("CHUCVU")]
    public partial class Chucvu
    {
        public Chucvu()
        {
            Nhanvien = new HashSet<Nhanvien>();
        }

        [Key]
        [Column("MaCV")]

        public int MaCv { get; set; }
        [Required(ErrorMessage = "vui lòng nhập tên chức vụ")]
        [Display(Name = "Tên Chức vụ")]
        [StringLength(100)]
        public string Ten { get; set; }
        public double? HeSo { get; set; }

        [InverseProperty("MaCvNavigation")]
        public virtual ICollection<Nhanvien> Nhanvien { get; set; }
    }
}
