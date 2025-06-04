import { useMutation, useQuery } from "@tanstack/react-query";
import { createInvitaionTemplates, deleteInvitationTemplate, editInvitationTemplate, getImageInvitationTemplate, getInvitationTemplateById, getInvitationTemplates, uploadImageInvitationTemplate } from "@/apis/auth.services";
import { ICreateInvitationTemplate, IInvationTemplatesResponse } from "@/types/invitationTemplate.type";
import { IResponse } from "@/types/auth.type";
import { toast } from "react-toastify";

export const useInvitationTemplates = () => {
    return useQuery<IResponse<IInvationTemplatesResponse[]>>({
        queryKey: ["allInvitationsTemplates"],
        queryFn: getInvitationTemplates,
        select: (data) => data || { data: [], message: '', status: 200 }, // Xử lý dữ liệu mặc định
    });
};

export const useCreateInvitationTemplates = () => {
    return useMutation<IResponse<IInvationTemplatesResponse>, Error, ICreateInvitationTemplate>({
        mutationFn: async (postInvitation: ICreateInvitationTemplate) => {
            const response = await createInvitaionTemplates(postInvitation);
            return response; // Trả về phản hồi từ hàm createInvitaionTemplates
        },
        onSuccess(data) {
            toast.success("Create Initation Template Successfully");
            console.log("Data Upload Image: ", data)
        },
        onError(data) {
            toast.success("Failed to create Invitation Template");
            console.log("Data Upload Image: ", data)
        }
    });
};

export const useGetInvitationTemplate = (templateId: string) => {
    return useQuery<IResponse<IInvationTemplatesResponse>>({
        queryKey: ['invitationTemplateId', templateId],
        queryFn: async ({ queryKey }) => {
            const [, invitationTemplateId] = queryKey as [string, string]; // Ép kiểu queryKey thành chuỗi
            const response = await getInvitationTemplateById(invitationTemplateId); // Truyền trực tiếp chuỗi invitationTemplateId
            return response;
        }
    });
};


export const useDeleteInvitationTemplate = () => {
    return useMutation<IResponse<IInvationTemplatesResponse>, Error, string>({
        mutationFn: async (templateId: string) => {
            const response = await deleteInvitationTemplate(templateId);
            return response;
        },
    });
};

export const useEditInvitationTemplate = () => {
    return useMutation<IResponse<IInvationTemplatesResponse>, Error, [string, ICreateInvitationTemplate]>({
        mutationFn: async ([templateId, postInvitation]) => {
            const response = await editInvitationTemplate(templateId, postInvitation);
            return response;
        }
    });
}


export const useUploadInitationTemplate = () => {
    return useMutation<IResponse<string[]>, Error, [string, FormData]>({
        mutationFn: async ([templateId, formData]) => {
            // Kiểm tra nếu formData chứa file trước khi upload
            console.log("FormData nè: ", formData)
            // if (!formData.has("files[]")) {
            //     throw new Error("The File field is required");
            // }

            // Log từng phần tử trong FormData để kiểm tra
            formData.forEach((value, key) => {
                console.log("Log ra nè: ", key, value);
            });

            // Thực hiện upload với formData đã chứa file
            const response = await uploadImageInvitationTemplate(templateId, formData);
            console.log("FormData Upload In useMutation: ", formData);
            return response;
        },
        onSuccess(data) {
            toast.success("Upload Image Successfully");
            console.log("Data Upload Image: ", data);
        },
        onError(error, data) {
            toast.error("Failed to upload image");
            console.log("Error Upload Image: ", error);
            console.log("Data Error Upload Image: ", data);
        }
    });
};




export const useGetImageInvitationTemplate = (templateId: string) => {
    return useQuery({
        queryKey: ['imageInvitationTemplate', templateId],
        queryFn: async ({ queryKey }) => {
            const [, invitationTemplateId] = queryKey as [string, string];
            const response = await getImageInvitationTemplate(invitationTemplateId);
            console.log("Respone get Image Invitation Template: ", response);
            return response.result;
        }
    })
}
