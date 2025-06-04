import { UseMutateFunction } from "@tanstack/react-query";
import { ICustomerInfo, ICustomerResponse } from '@/types/customer.type';

export interface ISignUpResponse {
    result: ISignUpPost;
    isSuccess: boolean;
    statusCode: number;
    message: string;
}

export interface ISignUpPost {
    email: string;
    password: string;
    confirmPassword: string;
    address: string;
    fullName: string;
    country: string;
    gender: string;
    birthDate: string;
    phoneNumber: string;
}

// Auth Context Interfaces
export interface IAuthContextState {
    isAuthenticated: boolean;
    isFullInfo: boolean;
    isAuthLoading: boolean;
    user?: ICustomerInfo;
}

export enum IAuthContextActionTypes {
    INITIAL = 'INITIAL',
    SIGNIN = 'SIGNIN',
    SIGNINBYGOOGLE = 'SIGNINBYGOOGLE',
    SIGNOUT = 'SIGNOUT',
    COMPLETE_PROFILE = 'COMPLETE_PROFILE'
}

export interface IAuthContextAction {
    type: IAuthContextActionTypes;
    payload?: ICustomerInfo;
}

export interface IAuthContext {
    isAuthenticated: boolean;
    isFullInfo: boolean;
    isAuthLoading: boolean;
    user?: ICustomerInfo;

    signUpUser: UseMutateFunction<ISignUpResponse, Error, ISignUpPost, unknown>
    signInByEmailPassword: UseMutateFunction<ISignInResponse, Error, ISignIn, unknown>;
    signOutUser: () => void;
    getCustomerAll: () => ICustomerResponse[] | null | undefined
    signInByGoogle: UseMutateFunction<ISignInResponse,
        Error,
        ISignInByGoogleDTO, unknown>
    // getCustomerProfile: UseQueryResult<IUserInfo, Error>
    // customerInfo: IUserInfo | undefined
}

// Represent to SignResponseDTO of back-end
export interface ISignInResponse {
    result: {
        accessToken: string;
        refreshToken: string;
    };
    isSuccess: true;
    statusCode: number;
    message: string;
}

export interface ISignIn {
    email: string;
    password: string;
}

export interface IResponse<T> {
    result?: T;
    isSuccess: boolean;
    statusCode: number;
    message: string;
}

export enum RolesEnum {
    USER = 'USER',
    ADMIN = 'ADMIN',
    CUSTOMER = "Customer"
}

export interface IJwtToken {
    result: {
        accessToken: string;
        refreshToken: string;
    };
    isSuccess: boolean;
    statusCode: number;
    message: string;
}


export interface ISignInByGoogleDTO {
    Token: string;
}