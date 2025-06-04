// import * as signalR from "@microsoft/signalr"; // Import tất cả các tính năng của SignalR
// import { getJwtTokenSession } from "./auth.utils"; // Import hàm để lấy JWT từ session cho xác thực


// class SignalRService {
//     private connection: signalR.HubConnection; // Đối tượng HubConnection để quản lý kết nối SignalR

//     constructor() {
//         // Khởi tạo kết nối SignalR với Hub
//         this.connection = new signalR.HubConnectionBuilder()
//             .withUrl(`https://weddingservice.azurewebsites.net/hubs/notification`, // URL của SignalR Hub
//                 {
//                     accessTokenFactory(): string | Promise<string> {
//                         // Trả về JWT để xác thực cho kết nối
//                         return getJwtTokenSession().accessToken || "";
//                     }
//                 })
//             .withAutomaticReconnect()
//             .configureLogging(signalR.LogLevel.Information)
//             // Ưu tiên WebSocket trước, rồi đến LongPolling nếu WebSocket không khả dụng
//             .withHubProtocol(new signalR.JsonHubProtocol())
//             .build();

//         // Xử lý sự kiện khi kết nối được khôi phục sau khi gián đoạn
//         this.connection.onreconnected((connectionId) => {
//             console.log(`Reconnected: ${connectionId}`); // In ra connectionId khi kết nối lại thành công
//         });

//         // Xử lý sự kiện khi kết nối bị đóng
//         this.connection.onclose((error) => {
//             console.log(`Connection closed due to error: ${error}`); // In ra thông báo lỗi khi kết nối đóng
//         });
//     }

//     // Phương thức khởi động kết nối với SignalR Hub
//     public startConnection = async () => {
//         try {
//             await this.connection.start(); // Bắt đầu kết nối với SignalR Hub
//             console.log("Connected to SignalR Hub"); // In ra thông báo khi kết nối thành công
//         } catch (error) {
//             // Bắt lỗi nếu kết nối thất bại
//             console.log(`Error while establishing connection: ${error}`);
//             // Tự động thử kết nối lại sau 5 giây
//             setTimeout(this.startConnection, 5000);
//         }
//     };

//     // Phương thức dừng kết nối với SignalR Hub
//     public stopConnection = async () => {
//         try {
//             await this.connection.stop(); // Dừng kết nối
//             console.log("Disconnected from SignalR Hub"); // In ra thông báo khi ngắt kết nối thành công
//         } catch (error) {
//             // Bắt lỗi nếu việc ngắt kết nối thất bại
//             console.log(`Error while stopping connection: ${error}`);
//         }
//     };

//     // Đăng ký sự kiện (listener) cho các phương thức từ SignalR Hub
//     public on = (methodName: string, newMethod: (...args: any[]) => void) => {
//         this.connection.on(methodName, newMethod); // Khi server gọi methodName, newMethod sẽ được gọi
//     };

//     // Hủy đăng ký sự kiện (listener) cho các phương thức từ SignalR Hub
//     public off = (methodName: string, method: (...args: any[]) => void) => {
//         this.connection.off(methodName, method); // Hủy listener của methodName
//     };

//     // Gọi phương thức từ server (invoke method trên SignalR Hub)
//     public invoke = async (methodName: string, ...args: any[]) => {
//         try {
//             // Gửi yêu cầu đến server để gọi phương thức với tên methodName và các tham số args
//             await this.connection.invoke(methodName, ...args);
//         } catch (error) {
//             // Bắt lỗi nếu việc gọi phương thức thất bại
//             console.error(`Error while invoking ${methodName}: ${error}`);
//         }
//     };
// }

// export default new SignalRService(); // Tạo một instance của SignalRService và xuất nó để sử dụng trong ứng dụng
