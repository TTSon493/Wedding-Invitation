export interface ICustomerResponse {
    customerId: string;
    userId: string;
    fullName: string;
    gender: string;
    email: string;
    phoneNumber: string;
    birthDate: string;
    avatarUrl: string;
    country: string;
    address: string;
}

export interface IUpdateCustomer {
    customerId: string;
    email?: string;
    fullName: string;
    birthDate: string;
    country: string;
    address: string;
    gender: string;
}

export interface ICustomerById {
    customerId: string,
    userId: string,
    fullName: string,
    gender: string,
    email: string,
    phoneNumber: string
    birthDate: string
    avatarUrl: string
    country: string
    address: string
}



export interface ICustomerInfo {
    email: string;
    password: string;
    confirmPassword: string;
    address: string;
    fullName: string;
    country: string;
    gender: string;
    birthDate: string;
    phoneNumber: string;
    roles: string[]
}

