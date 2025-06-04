export interface IInvationTemplatesResponse {
    createdAt: string | number | Date;
    templateId: string;
    templateName: string;
    backgroundImageUrl: string[];
    textColor: string[];
    textFont: string[];
    description: string;
    createdBy: string;
    createAt: string;
    invitationId: string | null;
}