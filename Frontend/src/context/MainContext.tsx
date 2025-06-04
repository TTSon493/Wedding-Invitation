import { createContext, ReactNode } from "react";
import { IMainContext } from "@/types/utils.type";
import {
  ICreateInvitationTemplate,
  IInvationTemplatesResponse,
} from "@/types/invitationTemplate.type";
import {
  useCreateInvitationTemplates,
  useInvitationTemplates,
} from "@/hooks/useAPI";
import {
  deleteInvitationTemplate,
  editInvitationTemplate,
  getInvitationTemplateById,
} from "@/apis/auth.services";

export const MainContext = createContext<IMainContext | null>(null);

interface IProps {
  children: ReactNode;
}

const MainContextProvider = ({ children }: IProps) => {
  const { data: invitationTemplates } = useInvitationTemplates();
  const createInvitationTemplateMutation = useCreateInvitationTemplates();

  const valuesObj: IMainContext = {
    getAllInvitationTemplates: (): IInvationTemplatesResponse[] =>
      invitationTemplates?.result || [], // Trả về mảng từ thuộc tính data hoặc mảng rỗng

    createInvitationTemplate: (): // postInvitation: ICreateInvitationTemplate
    IInvationTemplatesResponse | undefined => {
      // Trả về kết quả của việc tạo mẫu thiệp cưới
      return createInvitationTemplateMutation.isSuccess
        ? createInvitationTemplateMutation.data.result
        : undefined;
    },

    getInvitationTemplateId: async (templateId: string) => {
      try {
        const response = await getInvitationTemplateById(templateId); // Gọi API và truyền templateId
        return response.result; // Trả về kết quả từ API
      } catch (error) {
        console.error("Error fetching invitation template by ID:", error);
        return undefined;
      }
    },

    deleteInvitationTemplate: async (templateId: string) => {
      try {
        const response = await deleteInvitationTemplate(templateId);
        return response.result;
      } catch (error) {
        console.error("Error fetching to invitation template by ID:", error);
        return undefined;
      }
    },
    editInvitationTemplate: async (
      templateId: string,
      postInvitation: ICreateInvitationTemplate
    ) => {
      try {
        const response = await editInvitationTemplate(
          templateId,
          postInvitation
        );
        return response.result;
      } catch (error) {
        console.error("Error fetching to invitation template by ID:", error);
        return undefined;
      }
    },
  };

  return (
    <MainContext.Provider value={valuesObj}>{children}</MainContext.Provider>
  );
};

export default MainContextProvider;
