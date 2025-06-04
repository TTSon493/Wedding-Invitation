import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { VERIFY_EMAIL_URL } from "../../utils/apiUrl.utils";
import http from "../../utils/http";
import { IResponse } from "../../types/auth.type";
import { Button } from "../../components/ui/button";
import { useMutation } from "@tanstack/react-query";
const VerifyEmailPage = () => {
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(true);
  const navigate = useNavigate();

  // Lấy query parameters từ URL
  const query = new URLSearchParams(window.location.search);
  const userId = query.get("userId");
  const token = query.get("token");
  // const { token, userId } = useParams();
  useEffect(() => {
    verifyEmailMutation.mutate();
  }, [userId, token]);
  async function verifyEmail() {
    // Kiểm tra xem userId và token có hợp lệ không
    if (userId && token) {
      try {
        const response = await http.post<IResponse<string>>(
          `${VERIFY_EMAIL_URL}?userId=${encodeURIComponent(
            userId
          )}&token=${encodeURIComponent(encodeURIComponent(token))}`
        );

        // Nếu thành công, đặt thông báo và điều hướng người dùng về trang chủ
        setMessage(response.data.message);
        navigate("/");
      } catch (error) {
        console.log(
          "URL: ",
          `${VERIFY_EMAIL_URL}?userId=${encodeURIComponent(
            userId
          )}&token=${encodeURIComponent(token)}`
        );
        // Nếu có lỗi, thông báo lỗi xác thực email
        console.log(
          "URL: ",
          `${VERIFY_EMAIL_URL}?userId=${encodeURIComponent(
            userId
          )}&token=${encodeURIComponent(token)}`
        );
        console.error("Error during email verification:", error);
        setMessage("Email verification failed. Please try again.");
      } finally {
        // Tắt trạng thái loading sau khi xử lý xong
        setLoading(false);
      }
    } else {
      console.log("token", token as string);
      setMessage("Invalid verification link.");
      setLoading(false);
    }
  }

  const verifyEmailMutation = useMutation({
    mutationFn: verifyEmail,
    onSuccess: () => {
      console.log("Send Mail thanhf congo");
    },
    onError: () => {
      console.log("Loi me no roi!!!!!! ");
    },
  });

  // const handleSubmit = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
  //   e.preventDefault();
  //   verifyEmailMutation.mutate();
  // };

  return (
    <div className='flex flex-col items-center w-full justify-center'>
      <div className='flex flex-col justify-center m-40 bg-white p-8 rounded-lg shadow-md max-w-md w-full'>
        <h1 className='text-2xl font-bold mb-2 text-green-800'>
          Email Verification
        </h1>
        {loading ? (
          <p className='text-gray-600 mb-10'>Loading...</p>
        ) : (
          <p className='text-gray-600 mb-10'>{message}</p>
        )}
        <Button variant='secondary' type='button'>
          Verify Email
        </Button>
      </div>
    </div>
  );
};

export default VerifyEmailPage;
