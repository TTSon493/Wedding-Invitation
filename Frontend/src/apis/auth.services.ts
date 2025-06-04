import http from '../utils/http';
import { GET_CUSTOMER_INFO_URL, PUT_CUSTOMER, SIGN_IN_CUSTOMER_URL, SIGN_UP_CUSTOMER_URL, GET_CUSTOMER_BY_ID, GET_INVITATION_TEMPLATE, CREATE_INVITATION_TEMPLATE, GET_INVITATION_TEMPLATE_BY_ID, PUT_INVITATION_TEMPLATE, DELETE_INVITATION_TEMPLATE, UPLOAD_IMAGE_INVITATION_TEMPLATE, GET_IMAGE_INVITATION_TEMPLATE } from '../utils/apiUrl.utils';
import { ISignIn, ISignInResponse, ISignUpPost, ISignUpResponse, IResponse } from '../types/auth.type';
import { ICustomerById, ICustomerInfo, IUpdateCustomer } from '@/types/customer.type';
import { ICreateInvitationTemplate, IInvationTemplatesResponse } from '@/types/invitationTemplate.type';

// Hàm đăng nhập
export const signIn = async (signInData: ISignIn): Promise<ISignInResponse> => {
    const { data } = await http.post<ISignInResponse>(SIGN_IN_CUSTOMER_URL, signInData);
    return data;
};

// Hàm đăng ký
export const signUp = async (signUpData: ISignUpPost): Promise<ISignUpResponse> => {
    const { data } = await http.post<ISignUpResponse>(SIGN_UP_CUSTOMER_URL, signUpData);
    return data;
};

// Hàm lấy thông tin người dùng
export const getUserInfo = async (): Promise<ICustomerInfo | undefined> => {
    const { data } = await http.get<IResponse<ICustomerInfo>>(GET_CUSTOMER_INFO_URL);
    // Kiểm tra nếu không có dữ liệu hoặc dữ liệu không đúng kiểu
    if (!data || !data.result || typeof data.result.email !== 'string') {
        throw new Error('Invalid user data');  // Ném lỗi nếu dữ liệu không hợp lệ
    }
    return data.result;
};


// Hàm update customer
export const updateCustomerInfo = async (updateCustomerData: IUpdateCustomer): Promise<IResponse<unknown>> => {
    const response = await http.put<IResponse<null>>(PUT_CUSTOMER, updateCustomerData);
    return response.data;
}


export const getCustomerById = async (userId: string): Promise<IResponse<ICustomerById>> => {
    console.log(" from auth services: ", userId)
    console.log("URL query: ", `${GET_CUSTOMER_BY_ID}/${userId}`)
    const response = await http.get<IResponse<ICustomerById>>(`${GET_CUSTOMER_BY_ID}/${userId}`);
    return response.data;
};


export const getInvitationTemplates = async (): Promise<IResponse<IInvationTemplatesResponse[]>> => {
    const response = await http.get<IResponse<IInvationTemplatesResponse[]>>(GET_INVITATION_TEMPLATE)
    return response.data;
}

export const createInvitaionTemplates = async (postInvitation: ICreateInvitationTemplate): Promise<IResponse<IInvationTemplatesResponse>> => {
    const response = await http.post<IResponse<IInvationTemplatesResponse>>(CREATE_INVITATION_TEMPLATE, postInvitation);
    return response.data;
}

export const getInvitationTemplateById = async (templateId: string): Promise<IResponse<IInvationTemplatesResponse>> => {
    const response = await http.get<IResponse<IInvationTemplatesResponse>>(`${GET_INVITATION_TEMPLATE_BY_ID}/${templateId}`);
    return response.data;
}

export const editInvitationTemplate = async (templateId: string, postInvitation: ICreateInvitationTemplate): Promise<IResponse<IInvationTemplatesResponse>> => {
    const response = await http.put<IResponse<IInvationTemplatesResponse>>(`${PUT_INVITATION_TEMPLATE}/${templateId}`, postInvitation);
    return response.data;
}

export const deleteInvitationTemplate = async (templateId: string): Promise<IResponse<IInvationTemplatesResponse>> => {
    const response = await http.delete<IResponse<IInvationTemplatesResponse>>(`${DELETE_INVITATION_TEMPLATE}/${templateId}`)
    return response.data;
}

export const uploadImageInvitationTemplate = async (templateId: string, formData: FormData): Promise<IResponse<string[]>> => {
    const response = await http.post<IResponse<string[]>>(`${UPLOAD_IMAGE_INVITATION_TEMPLATE}/${templateId}`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data', // Đảm bảo gửi đúng Content-Type
        },
    });
    console.log("FormData Upload from API Services: ", formData);
    console.log("Response File Image Upload from API Services: ", response.data);
    return response.data;
}


export const getImageInvitationTemplate = async (templateId: string): Promise<IResponse<string[]>> => {
    const response = await http.get<IResponse<string[]>>(`${GET_IMAGE_INVITATION_TEMPLATE}/${templateId}`);
    return response.data;
}


// export const updateCustomerInfo = async (data: {
//     customerId: string;
//     fullName: string;
//     gender: string;
//     birthDate: string;
//     country: string;
//     address: string;
// }): Promise<IResponse<null>> => {
//     const response = await http.put<IResponse<null>>(`${ UPDATE_CUSTOMER_INFO } / ${ data.customerId }`, data);
//     return response.data;
// };
