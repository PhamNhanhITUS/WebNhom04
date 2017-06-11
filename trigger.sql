CREATE TRIGGER capNhatSoLuongTon
ON DonDatHang
for update as
if update(MaTinhTrang)
begin
	declare @maDonDatHang int
	declare @maTinhTrang int
	select @maDonDatHang=MaDonDatHang, @maTinhTrang=MaTinhTrang from inserted
	if @maTinhTrang=3
	begin
		declare cur cursor for select MaSanPham, SoLuong from ChiTietDonDatHang where MaDonDatHang = @maDonDatHang
		open cur
		declare @MaSanPham int
		declare @SoLuong int
		fetch next from cur into @MaSanPham, @SoLuong
		while @@FETCH_STATUS =0
		begin
			update SanPham
			set SoLuongBan=SoLuongBan+@SoLuong, SoLuongTon=SoLuongTon-@SoLuong
			where MaSanPham=@MaSanPham
			fetch next from cur into @MaSanPham, @SoLuong
		end
			close cur
		deallocate cur
	end
end