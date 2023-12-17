using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DoAnVat.Models
{
    [Table("NHANVIEN")]
    public partial class Nhanvien
    {
        [Key]
        [Column("MaNV")]
        public int MaNv { get; set; }
        [Required(ErrorMessage = "vui lòng nhập nhân viên")]
        [StringLength(100)]
        [Display(Name = "Tên nhân viên")]
        public string Ten { get; set; }
        [Column("MaCV")]
        [Display(Name = "Tên chức vụ")]
        public int MaCv { get; set; }
        [StringLength(20)]
        [Display(Name = "Số điện thoại")]
        public string DienThoai { get; set; }
        [StringLength(50)]
        [Display(Name = "gmail")]
        public string Email { get; set; }
        [StringLength(50)]
        [Display(Name = "Mật Khẩu")]
        public string MatKhau { get; set; }

        [ForeignKey(nameof(MaCv))]
        [InverseProperty(nameof(Chucvu.Nhanvien))]
        public virtual Chucvu MaCvNavigation { get; set; }
    }
}
