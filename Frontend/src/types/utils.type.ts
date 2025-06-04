import { ICreateInvitationTemplate, IInvationTemplatesResponse } from "./invitationTemplate.type";

export interface ResponseApi<Result> {
    message: string,
    result?: Result,
    isSuccess: boolean,
    statusCode: 400,
}

export interface ErrorResponse<Result> {
    message: string,
    result?: Result,
    isSuccess: boolean,
    statusCode: string,
}

export interface EmailRequest {
    email: string;
}


export interface IMainContext {
    getAllInvitationTemplates: () => IInvationTemplatesResponse[],
    createInvitationTemplate: (postInvitation: ICreateInvitationTemplate) => IInvationTemplatesResponse | undefined,
    getInvitationTemplateId: (templateId: string) => Promise<IInvationTemplatesResponse | undefined>;
    deleteInvitationTemplate: (templateId: string) => Promise<IInvationTemplatesResponse | undefined>
    editInvitationTemplate: (templateId: string, postInvitation: ICreateInvitationTemplate) => Promise<IInvationTemplatesResponse | undefined>
}