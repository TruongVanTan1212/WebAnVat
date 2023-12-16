Use Master
GO
    IF exists(Select name From sys.databases Where name='shopanvat' )
    DROP Database shopanvat
GO
    Create Database shopanvat
GO

USE shopanvat;

CREATE TABLE CUAHANG(
	MaCH int primary key identity(1,1),
	Ten nvarchar(100) not null,
	DienThoai varchar(20),
	DiaChi nvarchar(100)
) 
GO

CREATE TABLE DANHMUC(
	MaDM int primary key identity(1,1),
	Ten nvarchar(100) not null
) 
GO

CREATE TABLE MATHANG(
	MaMH int primary key identity(1,1),
	Ten nvarchar(100) not null,
	GiaGoc int default 0,
	GiaBan int default 0,
	SoLuong smallint default 0,
	MoTa nvarchar(1000),
	HinhAnh varchar(255),
	MaDM int not null foreign key(MaDM) references DANHMUC(MaDM),
	LuotXem int default 0,
	LuotMua int default 0
) 
GO

CREATE TABLE CHUCVU(
	MaCV int primary key identity(1,1),
	Ten nvarchar(100) not null,
	HeSo float default 1.0
) 
GO

CREATE TABLE NHANVIEN(
	MaNV int primary key identity(1,1),
	Ten nvarchar(100) not null,
	MaCV int not null foreign key(MaCV) references CHUCVU(MaCV),
	DienThoai varchar(20),
	Email varchar(50),
	MatKhau varchar(50)	
) 
GO

CREATE TABLE KHACHHANG(
	MaKH int primary key identity(1,1),
	Ten nvarchar(100) not null,
	DienThoai varchar(20),
	Email varchar(50),
	MatKhau varchar(255)
) 
GO

CREATE TABLE DIACHI(	
	MaDC int primary key identity(1,1),
	MaKH int not null foreign key(MaKH) references KHACHHANG(MaKH),
	DiaChi nvarchar(100) not null,
	PhuongXa varchar(20) default N'Đông Xuyên',
	QuanHuyen varchar(50) default N'TP. Long Xuyên',
	TinhThanh varchar(50) default N'An Giang',
	MacDinh int default 1	
) 
GO

CREATE TABLE HOADON(
	MaHD int primary key identity(1,1),
	Ngay datetime default getdate(),
	TongTien int default 0,
	MaKH int not null foreign key(MaKH) references KHACHHANG(MaKH),
	TrangThai int default 0
) 
GO

CREATE TABLE CTHOADON(
	MaCTHD int primary key identity(1,1),
	MaHD int not null foreign key(MaHD) references HOADON(MaHD),	
	MaMH int not null foreign key(MaMH) references MATHANG(MaMH),
	DonGia int default 0,
	SoLuong smallint default 1,
	ThanhTien int
) 
GO

-- Dữ liệu bảng CUA_HANG
INSERT INTO CUAHANG(Ten, DienThoai, DiaChi) VALUES(N'Cửa hàng văn phòng phẩm ABC','0296-3841190',N'18 Ung Văn Khiêm, P Đông Xuyên, TP Long Xuyên, An Giang');

-- Dữ liệu bảng LOAI_HANG
INSERT INTO DANHMUC(Ten) VALUES(N'Bánh Tráng');
INSERT INTO DANHMUC(Ten) VALUES(N'Bánh Kẹo NGọt');
INSERT INTO DANHMUC(Ten) VALUES(N'Chân Gà Rút Xương');
INSERT INTO DANHMUC(Ten) VALUES(N'Snack');

-- Dữ liệu bảng MAT_HANG
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Bánh Sữa Chua 5 Vị Đài Loan',N'Bánh sữa chua Horsh của Đài Loan là món bánh “hot” nhất hiện nay sở hữu vỏ bánh mềm mềm từ bột mì, nhân bánh thơm ngon, vừa đủ ngọt, mang vị hơi chua đã khiến tan chảy trái tim người thưởng thức. Bánh sữa chua Horsh là loại bánh có kích thước nhỏ bằng 2 ngón tay màu trắng sữa, bẻ đôi sẽ thấy nhân đặc sánh chảy ra, nếm với vị béo ngậy kết hợp có vị ngọt và chua nhẹ… khiến cho rộng rãi chị em đang ăn kiêng giảm béo cũng khó cầm lòng. Bánh Sữa Chua 5 Vị Đài Loan',200000,180000,10,'Banh1.jpg',2,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Chân gà rút xương vị tê cay',N'Chân gà (97%), muối ăn, đường kính, xì dầu, hoa hồi, quế, thảo quả, trần bì, đương quy, ớt, tinh dầu tỏi. Chân gà rút xương vị tê cay Thành phần dinh dưỡng: Project: NRV%, Energy 6%, Protein 27%, Lipit 11%, Natri 53% Chân gà rút xương vị tê cay',757000,681000,20,'ChanGa1.jpg',3,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Gói 150 gram Chân Gà CHEF BIGGY Vị cay/ Mật Ong',N'Đáp ứng đầy đủ và toàn bộ yêu cầu VSATTP. Lưu ý : Đối với những trường hợp khi nhận hàng bị bung hút chân không, hay châm kim bao bì hư hỏng, vui lòng giữ nguyên, chụp hình gửi cho shop để được hỗ trợ đổi sản phẩm mới. TUYỆT ĐỐI KHÔNG CẮT BAO BÌ SẢN PHẨM. Toàn bộ nguyên liệu đưa vào sản xuất có nguồn gốc xuất xứ rõ ràng và được kiểm soát chất lượng chặt chẽ từ đầu vào. Gói 150 gram Chân Gà Sử dụng nhiều nguyên liệu có nguồn gốc tự nhiên như : Mật ong, cam thảo, quế, hồi, thảo quả, ớt… Tạo nên hương vị cay tê tê và mật ong thơm tự nhiên .',820000,738000,20,'ChanGa2.jpg',3,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Combo 10 gói/30 gói Snack Hương Bò món ăn tuổi thơ trường học gói 30g',N'Hương liệu thực phẩm: màu thực phẩm, ớt ,hồi, quế – Bảo quan nơi nhiệt độ thấp tránh ánh nắng mặt trời Ngày sản xuất: in trên bao bì – Hạn sử dụng : 6 tháng kể từ ngày sản xuất Xin chào, Đầu tiên, xin cảm ơn bạn khách thân yêu đã tin tưởng lựa chọn và ủng hộ shop bọn mình trong muôn vàn lựa chọn.  luôn cam kết mang đến những trải nghiệm tuyệt vời nhất cho khách hàng, nếu có điều gì chưa hài lòng ở bọn mình, xin đừng vội đánh giá 1 sao, hãy nhắn tin cho chúng mình, chúng mình sẽ giải quyết ngay!',635000,571000,30,'Snack1.jpg',4,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Que Cay Mix 4 Vị',N'Bao bì đóng hộp chắc chắn, đầy đủ ngày sản xuất và hạn sử dụng. Hàng được Ship đi bằng hộp Carton cứng 3 lớp nên bạn yên tâm đồ ăn chắc chắn sẽ luôn còn mới và ngon lành cho đến khi bạn nhận được. Thông tin QUE CAY, TĂM CAY Bạn có thể mua riêng hoặc mix 4 loại nếu thích Que Cay Mix 4 Vị',178000,1000,25,'Snack2.jpg',4,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'SNACK ĐÙI GÀ NHỎ',N'Đóng kín – sạch sẽ – sang trọng Số lượng càng tăng – giá càng giảm. Uy tín – Chất lượng Có giấy phép kinh doanh, giấy ATVSTP đầy đủ Miễn phí tem có thông tin đầy đủ của BB Foods gồm SĐT, Đ/c, Giấy Phép…  Miễn phí tem chỉ có thông tin sản phẩm , mã vạch nhưng không có thông tin nhà sản xuất Hơn 500 món đủ loại quý khách tha hồ lựa chọn để kinh doanh',52000,46800,50,'Snack3.jpg',4,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Tóp Mỡ Rim Cay',N'Tóp mỡ rim cay không chỉ là một bữa ăn, mà là một trải nghiệm hấp dẫn, kết hợp sự giòn ngon của tóp mỡ và hương vị độc đáo của gia vị rim cay. Mỗi miếng tóp mỡ là một chuyến phiêu lưu đầy kịch tính trong thế giới ẩm thực. Không chỉ ngon miệng, tóp mỡ rim cay còn mang lại nhiều lợi ích sức khỏe. Cay nồng từ gia vị rim cay có thể kích thích sự tiêu hóa',60000,54000,30,'TopMo.jpg',4,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Top 3 Combo bánh tráng chấm sốt',N'Bao gồm: 1 hủ sốt chấm nửa kí + 1 sấp bánh phơi sương nửa kí + Sốt chấm dùng chấm hải sản tôm mực, cua ghẹ,…Chấm bánh tráng phơi sương, bánh tráng trắng,… Hạn sử dụng: 3 tháng Cách bảo quản: để nơi thoáng mát hoặc trong tủ lạnh, đậy dùng muỗng sạch để lấy sốt, đậy nắp kín sau mỗi lần sử dụng Khôi lượng: 500g ( hủ ngang mí viền, không đầy hủ)',60000,54000,30,'BanhTrang1.jpg',1,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'Xúc Xích Bò BBQ Cay Nhẹ',N'-Xúc xích Bò BBQ ăn liền thơm ngon và ăn bao phê luôn, Chỉ cần xé vỏ cái là chén, Cảm nhận ngay được mùi thơm TỪ KHI XÉ VỎ. Khi cắn sẽ cảm nhận được độ giòn của vỏ xúc xích.– Xúc xích Thơm Cay nhìn thôi Cũng thấy hấp dẫn rồi, ăn có một chút xíu cay và ngon thơm khó diễn tả..',85000,765000,10,'XucXich1.jpg',3,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'BÁNH MOCHI ĐÀI LOAN',N'Thưởng thức hương vị ẩm thực thơm ngon, hấp dẫn từ chiếc bánh Mochi Taiwan với vỏ bánh dẻo ngon, nhân ngọt vừa, hoàn toàn không sử dụng chất bảo quản. Có 7 vị cho bạn lựa chọn : – Milk mochi : mochi sữa – Matcha mochi : mochi trà xanh nhân đậu đỏ – Strawberry mochi : mochi dâu tây',83000,74700,20,'BanhMoChi1.jpg',1,0,0);
INSERT INTO MATHANG(Ten,MoTa,GiaGoc,GiaBan,SoLuong,HinhAnh,MaDM,LuotXem,LuotMua) VALUES(N'top 3 bánh tráng phơi sương lẻ nhiều gạo loại dẻo ngon mỏng',N'Vì bánh luôn được làm mới liên tục nên tính từ thời điểm mua mang về thì bánh tráng phơi sương sẽ để được từ 5 – 7 ngày ở nơi khô ráo, thoáng mát bảo quản Bánh Tráng Phơi Sương Vì là bánh phơi sương nên bánh sẽ không để được lâu, thường là khoảng 2 tuần tùy theo tình hình thời tiết, nếu độ ẩm không khí cao bánh sẽ dễ bị mốc.',205000,184500,20,'BanhTrang2.jpg',1,0,0);
-- Dữ liệu bảng CHUC_VU
INSERT INTO CHUCVU(Ten) VALUES(N'Quản lý');
INSERT INTO CHUCVU(Ten) VALUES(N'Nhân viên thu ngân');
INSERT INTO CHUCVU(Ten) VALUES(N'Nhân viên kho');

-- Dữ liệu bảng NHANVIEN
--INSERT INTO NHANVIEN(Ten,MaCV,DienThoai,Email,MatKhau) VALUES(N'Nguyễn Phước Tân',1,'0909456789','nptan@abc.com','202cb962ac59075b964b07152d234b70');
--INSERT INTO NHANVIEN(Ten,MaCV,DienThoai,Email,MatKhau) VALUES(N'Dương Thị Mỹ Thuận',2,'0988778899','dtmthuan@abc.com','202cb962ac59075b964b07152d234b70');
--INSERT INTO NHANVIEN(Ten,MaCV,DienThoai,Email,MatKhau) VALUES(N'Trần Huỳnh Sơn',3,'0903123123','thson@abc.com','202cb962ac59075b964b07152d234b70');
--INSERT INTO NHANVIEN(Ten,MaCV,DienThoai,Email,MatKhau) VALUES(N'Lê Ngọc Thanh',2,'0913454544','lnthanh@abc.com','202cb962ac59075b964b07152d234b70');

-- Dữ liệu bảng KHACHHANG
--INSERT INTO KHACHHANG(Ten,DienThoai,Email,MatKhau) VALUES(N'','','','');

-- Dữ liệu bảng DIACHI
--INSERT INTO DIACHI(MaKH,DiaChi,PhuongXa,QuanHuyen,TinhThanh,MacDinh) VALUES(1,N'',N'',N'',N'',1);

-- Dữ liệu bảng HOADON
--INSERT INTO HOADON(TongTien,MaKH,TrangThai) VALUES(70000,1,0);


-- Dữ liệu bảng CTHOA_DON
--INSERT INTO CTHOADON(MaHD,MaMH,DonGia,SoLuong,ThanhTien) VALUES(1,2,23000,1,23000);

GO

SELECT * FROM DANHMUC;
SELECT * FROM MATHANG;
SELECT * FROM KHACHHANG;

