import { IResponse, ISignIn, ISignInResponse } from "@/types/auth";
import http from "./http";
import { IInvationTemplatesResponse } from "@/types/invitation";
import { ICustomerInfo } from "./customer";

export const API_ENDPOINTS = {
    SIGN_UP_CUSTOMER: '/auth/customer/sign-up',
    SEND_VERIFY_EMAIL: '/auth/send-verify-email',
    GET_CUSTOMER_INFO: '/auth/user/info',
    SIGN_IN_CUSTOMER: '/auth/sign-in',
    SIGN_IN_GOOGLE: '/auth/google/sign-in',
    GET_INVITATION_TEMPLATE: '/invitation-template',
    GET_IMAGE_INVITATION_TEMPLATE: '/image-template',
};

export const signUp = async (userData: any) => {
    const response = await http.post(API_ENDPOINTS.SIGN_UP_CUSTOMER, userData);
    return response.data;
};

export const sendVerifyEmail = async (email: string) => {
    const response = await http.post(API_ENDPOINTS.SEND_VERIFY_EMAIL, { email });
    return response.data;
};

export const getCustomerInfo = async (): Promise<ICustomerInfo | undefined> => {
    const { data } = await http.get<IResponse<ICustomerInfo>>(API_ENDPOINTS.GET_CUSTOMER_INFO);
    if (!data || !data.result || typeof data.result.email !== 'string') {
        throw new Error('Invalid user data');  // Ném lỗi nếu dữ liệu không hợp lệ
    }
    return data.result;
};

export const signIn = async (signInData: ISignIn): Promise<ISignInResponse> => {
    const response = await http.post(API_ENDPOINTS.SIGN_IN_CUSTOMER, signInData);
    return response.data;
};

export const signInWithGoogle = async (token: string): Promise<ISignInResponse> => {
    const response = await http.post<Promise<ISignInResponse>>(API_ENDPOINTS.SIGN_IN_GOOGLE, { token });
    return response.data;
};

export const getInvitationTemplates = async (): Promise<IResponse<IInvationTemplatesResponse[]>> => {
    const response = await http.get<IResponse<IInvationTemplatesResponse[]>>(API_ENDPOINTS.GET_INVITATION_TEMPLATE)
    console.log("Respone from getInvitationTemplates: ", response.data.result);
    return response.data;
}

export const getImageInvitationTemplate = async (templateId: string): Promise<IResponse<string[]>> => {
    const response = await http.get<IResponse<string[]>>(`${API_ENDPOINTS.GET_IMAGE_INVITATION_TEMPLATE}/${templateId}`);
    console.log("Response get url image invitation: ", response.data)
    return response.data;
}
