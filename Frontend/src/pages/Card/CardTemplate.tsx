import { useState, useEffect, useMemo } from "react";
import { Input } from "@/components/ui/input";
import { Card, CardContent } from "@/components/ui/card";
import { Loader2, Search } from "lucide-react";
import {
  useInvitationTemplates,
  useGetImageInvitationTemplate,
} from "@/hooks/useAPI";
import { motion, AnimatePresence } from "framer-motion";
import { Link } from "react-router-dom";
import { PATH_USER } from "@/router/path";

export default function CardTemplate() {
  const [searchTerm, setSearchTerm] = useState("");

  const {
    data: invitationTemplates,
    refetch: refetchTemplates,
    isLoading: isLoadingTemplates,
  } = useInvitationTemplates();

  const filteredTemplates = useMemo(() => {
    return (
      invitationTemplates?.result?.filter(
        (template) =>
          template.templateName
            .toLowerCase()
            .includes(searchTerm.toLowerCase()) ||
          template.description.toLowerCase().includes(searchTerm.toLowerCase())
      ) || []
    );
  }, [invitationTemplates, searchTerm]);

  useEffect(() => {
    refetchTemplates();
  }, []);

  const TemplateImage = ({ templateId }: { templateId: string }) => {
    const {
      data: imageData,
      isLoading,
      isError,
    } = useGetImageInvitationTemplate(templateId);

    if (isLoading) {
      return (
        <div className='flex items-center justify-center w-full h-full bg-gray-100'>
          <Loader2 className='w-8 h-8 animate-spin text-gray-400' />
        </div>
      );
    }

    if (isError) {
      return (
        <div className='flex items-center justify-center w-full h-full bg-gray-100 text-red-500'>
          Error loading image
        </div>
      );
    }

    if (!imageData || imageData.length === 0) {
      return (
        <div className='flex items-center justify-center w-full h-full bg-gray-100 text-gray-500'>
          No image available
        </div>
      );
    }

    return (
      <img
        src={imageData.toString()}
        alt='Template preview'
        className='w-full h-full object-contain'
        onError={(e) => {
          e.currentTarget.src = "/placeholder.svg?height=300&width=300";
          e.currentTarget.alt = "Image not found";
        }}
      />
    );
  };

  return (
    <div className='m-[50px] min-h-screen px-4 py-8 bg-gradient-to-br'>
      <h1 className='text-4xl font-bold mb-4 md:mb-0 text-center'>
        Invitation Templates
      </h1>
      <div className='flex flex-col md:flex-row mb-10'>
        <div className='relative'>
          <Input
            type='text'
            placeholder='Search templates...'
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className='pl-10 pr-4 py-2 w-full md:w-64 rounded-full border-2 border-indigo-300 focus:border-indigo-500 focus:ring focus:ring-indigo-200 focus:ring-opacity-50'
          />
          <Search className='absolute left-3 top-1/2 transform -translate-y-1/2 text-indigo-400' />
        </div>
      </div>
      {isLoadingTemplates ? (
        <div className='flex justify-center items-center h-64'>
          <Loader2 className='w-12 h-12 animate-spin text-indigo-500' />
        </div>
      ) : (
        <AnimatePresence>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className='grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-8'>
            {filteredTemplates.map((template) => (
              <motion.div
                key={template.templateId}
                layout
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -20 }}
                transition={{ duration: 0.3 }}>
                <Card className='bg-white shadow-lg rounded-lg overflow-hidden hover:shadow-2xl transition-shadow duration-300 transform hover:-translate-y-1'>
                  <Link
                    to={`${PATH_USER.customCardFree.replace(
                      ":templateId",
                      template.templateId
                    )}`}>
                    <CardContent className='p-4'>
                      <div
                        id={`template-${template.templateId}`}
                        className='relative w-full h-48 rounded-md overflow-hidden cursor-pointer'>
                        <TemplateImage templateId={template.templateId} />
                      </div>
                      <h3 className='text-lg font-semibold text-indigo-700 mt-4 truncate'>
                        {template.templateName}
                      </h3>
                      <p className='text-sm text-gray-600 mt-2 line-clamp-2'>
                        {template.description}
                      </p>
                    </CardContent>
                  </Link>
                </Card>
              </motion.div>
            ))}
          </motion.div>
        </AnimatePresence>
      )}
    </div>
  );
}
