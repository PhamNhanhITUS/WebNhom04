//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace shopthethao.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonDatHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonDatHang()
        {
            this.ChiTietDonDatHangs = new HashSet<ChiTietDonDatHang>();
        }
    
        public string MaDonDatHang { get; set; }
        public Nullable<System.DateTime> NgayLap { get; set; }
        public Nullable<int> TongThanhTien { get; set; }
        public Nullable<int> MaTaiKhoan { get; set; }
        public Nullable<int> MaTinhTrang { get; set; }
        public string DiaChiNhanHang { get; set; }
        public string HoTenNhanHang { get; set; }
        public string DienThoaiNhanHang { get; set; }
        public string EmailNhanhang { get; set; }
        public Nullable<bool> BiXoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonDatHang> ChiTietDonDatHangs { get; set; }
        public virtual TaiKhoan TaiKhoan { get; set; }
        public virtual TinhTrang TinhTrang { get; set; }
    }
}
