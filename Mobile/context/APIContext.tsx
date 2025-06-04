import {
  getImageInvitationTemplate,
  getInvitationTemplates,
} from "@/utils/auth";
import { template } from "@babel/core";
import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import { Alert } from "react-native";

interface InvitationTemplate {
  templateId: string;
  templateName: string;
  description: string;
  backgroundImageUrl: string[];
  textColor: string[];
  textFont: string[];
}

interface InvitationTemplateContextType {
  templates: InvitationTemplate[] | null;
  isLoading: boolean;
  fetchTemplates: () => Promise<void>;
  fetchImageTemplate: (templateId: string) => Promise<void>;
}

const InvitationTemplateContext = createContext<
  InvitationTemplateContextType | undefined
>(undefined);

export const useInvitationTemplate = () => {
  const context = useContext(InvitationTemplateContext);
  if (!context) {
    throw new Error(
      "useInvitationTemplate must be used within an InvitationTemplateProvider"
    );
  }
  return context;
};

export const InvitationTemplateProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [templates, setTemplates] = useState<InvitationTemplate[] | null>(null);
  const [imageTemplates, setImageTemplates] = useState<string[]>();
  const [isLoading, setIsLoading] = useState(true);

  const fetchTemplates = async () => {
    setIsLoading(true);
    try {
      const response = await getInvitationTemplates();
      console.log("Response Fetch Template: ", response);

      if (response.result && Array.isArray(response.result)) {
        const mappedTemplates: InvitationTemplate[] = response.result.map(
          (item: any) => ({
            templateId: item.id || String(item.templateId),
            templateName: item.title || item.templateName,
            description: item.content || item.description || "",
            backgroundImageUrl: item.backgroundImageUrl || [],
            textColor: item.textColor || [],
            textFont: item.textFont || [],
          })
        );
        setTemplates(mappedTemplates);
      } else {
        throw new Error("Invalid response format");
      }
    } catch (error) {
      console.error("Error fetching templates:", error);
      // Alert.alert("Error", "Failed to fetch invitation templates");
    } finally {
      setIsLoading(false);
    }
  };
  const fetchImageTemplate = async (templateId: string) => {
    setIsLoading(true);
    try {
      const response = await getImageInvitationTemplate(templateId);
      console.log("Response fetch image template: ", response.result);
      const imagesTemplateData = response.result;
      setImageTemplates(imagesTemplateData);
    } catch (error) {
      console.log("Error fetching image template: ", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchTemplates();
  }, []);

  return (
    <InvitationTemplateContext.Provider
      value={{
        templates,
        isLoading,
        fetchTemplates,
        fetchImageTemplate,
      }}>
      {children}
    </InvitationTemplateContext.Provider>
  );
};
