import * as yup from "yup"

export const schema = yup.object({
    email: yup.string().required("Email is required").email("Email không hợp lệ").min(5, "Độ dài từ 5 - 160 ký tự").max(160, "Độ dài từ 5 - 160 ký tự"),

    password: yup.string().required("Password is required")
        .min(8, "Password must be at least 8 characters")
        .matches(/[a-z]/, "Password must contain at least one lowercase letter")
        .matches(/[A-Z]/, "Password must contain at least one uppercase letter")
        .matches(/[0-9]/, "Password must contain at least one number")
        .matches(/[!@#$%^&*(),.?":{}|<>]/, "Password must contain at least one special character"),

    confirmPassword: yup.string().required("Confirm Password is required")
        .oneOf([yup.ref('password')], 'Confirm Password must match'),

    address: yup.string().required("Address is required").min(5, "Độ dài từ 5 - 160 ký tự").max(160, "Độ dài từ 5 - 160 ký tự"),

    fullName: yup.string().required("Full Name is required").min(3, "Độ dài từ 3 - 160 ký tự").max(160, "Độ dài từ 3 - 160 ký tự"),

    birthDate: yup.string().required("BirthDate is required"),

    gender: yup.string().required("Gender is required"),

    phoneNumber: yup.string().required("Phone Number is required").min(10, "Số điện thoại không hợp lệ").max(11, "Số điện thoại không hợp lệ"),

    country: yup.string().required("Country is required")
})

export type Schema = yup.InferType<typeof schema>

// export const loginSchema = schema.omit(['repeat_password', 'phone', 'address', 'full_name'])

export type LoginSchema = yup.InferType<typeof schema>

