import axios, { type AxiosInstance } from "axios";
import HttpStatusCode from "../constants/httpStatusCode.enum";
class Http {
    instance: AxiosInstance

    constructor() {
        this.instance = axios.create({
            baseURL: 'https://weddingservices.azurewebsites.net/api',
            timeout: 10000,
            headers: {
                "Content-Type": "application/json",
                Accept: "application/json",
                "Access-Control-Allow-Origin": "*",
            },
        })

        // Thêm một bộ đón chặn response
        this.instance.interceptors.response.use(function (response) {
            // Bất kì mã trạng thái nào nằm trong tầm 2xx đều khiến hàm này được trigger
            // Làm gì đó với dữ liệu response
            return response;
        }, function (error) {
            // Bất kì mã trạng thái nào lọt ra ngoài tầm 2xx đều khiến hàm này được trigger\
            // Làm gì đó với lỗi response
            // Chỉ toast lỗi không phải 422 và 401
            if (
                ![HttpStatusCode.UnprocessableEntity, HttpStatusCode.Unauthorized].includes(error.response?.status as number)
            ) {
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                const data: any | undefined = error.response?.data
                const message = data?.message || error.message
                console.log("Message error form http: ", message)
                // toast.error(message)
            }
            // Lỗi Unauthorized (401) có rất nhiều trường hợp
            // - Token không đúng
            // - Không truyền token
            // - Token hết hạn*
            return Promise.reject(error);
        });
    }
}

const http = new Http().instance;

export default http;