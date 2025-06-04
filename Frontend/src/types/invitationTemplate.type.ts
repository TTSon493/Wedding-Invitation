

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


export interface ICreateInvitationTemplate {
    templateId: string;
    templateName: string;
    backgroundImageUrl: string[] | [] | File[];
    textColor: string[];
    textFont: string[];
    description: string;
}

export interface IImageInvitationTemplate {
    backgroundImageUrl: string[];
}

// interface Template {
//   templateId: string;
//   templateName: string;
//   backgroundImageUrl: string[];
//   textColor: string[];
//   textFont: string[];
//   description: string;
//   createdBy: string;
//   invitationId: string | null;
// }