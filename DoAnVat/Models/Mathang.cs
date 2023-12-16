using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DoAnVat.Models
{
    [Table("MATHANG")]
    public partial class Mathang
    {
        public Mathang()
        {
            Cthoadon = new HashSet<Cthoadon>();
        }

        [Key]
        [Column("MaMH")]
        public int MaMh { get; set; }
        [Required(ErrorMessage = "vui lòng nhập tên sản phẩm")]
        [StringLength(100)]
        [Display(Name = "Tên Sản Phẩm")]
        public string Ten { get; set; }
        [Display(Name = "Giá Gốc")]
        public int GiaGoc { get; set; }
        [Display(Name = "Giá Bán")]
        public int GiaBan { get; set; }
        [Display(Name = "Số Lượng")]
        public short? SoLuong { get; set; }
        [StringLength(1000)]
        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }
        [StringLength(255)]
        [Display(Name = "Hình Ảnh")]
        public string HinhAnh { get; set; }
        [Column("MaDM")]
        [Display(Name = "Danh Mục")]
        public int MaDm { get; set; }
        public int? LuotXem { get; set; }
        public int? LuotMua { get; set; }

        [ForeignKey(nameof(MaDm))]
        [InverseProperty(nameof(Danhmuc.Mathang))]
        public virtual Danhmuc MaDmNavigation { get; set; }
        [InverseProperty("MaMhNavigation")]
        public virtual ICollection<Cthoadon> Cthoadon { get; set; }
    }
}
