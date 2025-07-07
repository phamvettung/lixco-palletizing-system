# PHẦN MỀM IN NHÃN CHO HỆ BỐC XẾP PALLET LIXCO - PALLETIZING SYSTEM
![Palletizing System](/assets/palletizing-system.jpg)
### NGUYÊN LÝ HOẠT ĐỘNG
Pallet sau quá trình bốc xếp được hệ thống điều khiển đến vị trí máy in nhãn. Phần mềm tích hợp với Hệ thống của Nhà máy để lấy thông tin sản phẩm, lưu thông tin pallet đã tính sản lượng (đã in lên pallet) và thực hiện in nhãn lên trên pallet. 
### CÁC CHỨC NĂNG
- Tích hợp với Hệ thống của Nhà máy thông qua RESTful API, lấy thông tin sản phẩm cần in tem lên Pallet,...
- Hiển thị thông tin sản phẩm, tổ sản xuất, ca, sản lượng và lưu lịch sử Pallet đã tính sản lượng.
- Các chế độ in nhãn tự động, in nhãn thủ công.
- Thêm, sửa, xóa Model.

### SƠ ĐỒ KẾT NỐI
![Palletizing System](/assets/Lixco_diagram.PNG)
- Kết nối với Bộ điều khiển PLC Siemens, đọc dữ liệu và gửi tín hiệu in xuống băng chuyền.
- Kết nối với máy in TSC sử dụng SDK do hãng phát triển.
- 
### CÁC CÔNG NGHỆ SỬ DỤNG
- WPF, C# Entity Framework
- MS SQL Server
### DEMO
Tem sau khi in lên pallet</br>
![Palletizing System](/assets/lixco.jpg)

