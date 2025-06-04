export interface IResponse<T> {
    result?: T;
    isSuccess: boolean;
    statusCode: number;
    message: string;
}


export interface ISignIn {
    email: string;
    password: string;
}

export interface ISignInResponse {
    result: {
        accessToken: string;
        refreshToken: string;
    };
    isSuccess: true;
    statusCode: number;
    message: string;
}