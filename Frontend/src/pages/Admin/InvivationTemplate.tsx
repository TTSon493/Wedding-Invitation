"use client";

import { useState, useEffect, useMemo } from "react";
import { useForm, Controller, SubmitHandler } from "react-hook-form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import {
  Edit,
  Download,
  Trash,
  Plus,
  Loader2,
  Search,
  X,
  ZoomIn,
  ZoomOut,
} from "lucide-react";
import { toast } from "@/hooks/use-toast";
import {
  useInvitationTemplates,
  useCreateInvitationTemplates,
  useDeleteInvitationTemplate,
  useEditInvitationTemplate,
  useUploadInitationTemplate,
  useGetImageInvitationTemplate,
} from "@/hooks/useAPI";
import html2canvas from "html2canvas";
import jsPDF from "jspdf";
import { ICreateInvitationTemplate } from "@/types/invitationTemplate.type";
import { motion, AnimatePresence } from "framer-motion";

interface Template extends ICreateInvitationTemplate {
  templateId: string;
  createdBy?: string;
  invitationId: string | null;
}

const fonts = [
  "Arial",
  "Helvetica",
  "Times New Roman",
  "Courier",
  "Verdana",
  "Georgia",
  "Palatino",
  "Garamond",
];

export default function InvitationTemplate() {
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingTemplate, setEditingTemplate] = useState<Template | null>(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedTemplate, setSelectedTemplate] = useState<Template | null>(
    null
  );
  const [selectedFiles, setSelectedFiles] = useState<FileList | undefined>(
    undefined
  );
  const [previewScale, setPreviewScale] = useState(1);
  const { control, handleSubmit, reset, setValue, watch } =
    useForm<ICreateInvitationTemplate>();

  const {
    data: invitationTemplates,
    refetch: refetchTemplates,
    isLoading: isLoadingTemplates,
    // isError: isErrorTemplates,
    // error: templatesError,
  } = useInvitationTemplates();
  const createTemplateMutation = useCreateInvitationTemplates();
  const deleteTemplateMutation = useDeleteInvitationTemplate();
  const editTemplateMutation = useEditInvitationTemplate();
  const uploadImageMutation = useUploadInitationTemplate();

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

  useEffect(() => {
    if (editingTemplate) {
      setValue("templateName", editingTemplate.templateName);
      setValue("description", editingTemplate.description);
      setValue("backgroundImageUrl", editingTemplate.backgroundImageUrl);
      setValue("textColor", editingTemplate.textColor);
      setValue("textFont", editingTemplate.textFont);
    }
  }, [editingTemplate, setValue]);

  const handleCreateTemplate: SubmitHandler<ICreateInvitationTemplate> = async (
    data
  ) => {
    try {
      const createdTemplate = await createTemplateMutation.mutateAsync(data);
      if (selectedFiles === undefined) return;

      if (selectedFiles.length > 0 && createdTemplate.result?.templateId) {
        await handleImageUpload(createdTemplate.result.templateId);
      }
      setIsDialogOpen(false);
      reset();
      refetchTemplates();
      toast({
        title: "Success",
        description: "New template created successfully.",
      });
    } catch (error) {
      console.error("Error creating template:", error);
      toast({
        title: "Error",
        description: "Failed to create new template.",
        variant: "destructive",
      });
    }
  };

  const handleEditTemplate: SubmitHandler<ICreateInvitationTemplate> = async (
    data
  ) => {
    if (!editingTemplate) return;
    try {
      await editTemplateMutation.mutateAsync([
        editingTemplate.templateId,
        data,
      ]);
      if (selectedFiles === undefined) return;

      if (selectedFiles.length > 0) {
        await handleImageUpload(editingTemplate.templateId);
      }
      setEditingTemplate(null);
      setIsDialogOpen(false);
      reset();
      refetchTemplates();
      toast({
        title: "Success",
        description: "Template updated successfully.",
      });
    } catch (error) {
      console.error("Error updating template:", error);
      toast({
        title: "Error",
        description: "Failed to update template.",
        variant: "destructive",
      });
    }
  };

  const handleDeleteTemplate = async (templateId: string) => {
    try {
      await deleteTemplateMutation.mutateAsync(templateId);
      refetchTemplates();
      toast({
        title: "Success",
        description: "Template deleted successfully.",
      });
    } catch (error) {
      console.error("Error deleting template:", error);
      toast({
        title: "Error",
        description: "Failed to delete template.",
        variant: "destructive",
      });
    }
  };

  const handleImageUpload = async (templateId: string) => {
    if (selectedFiles === undefined) return;

    if (selectedFiles.length === 0) return;

    try {
      const formData = new FormData();
      for (let i = 0; i < selectedFiles.length; i++) {
        formData.append("File", selectedFiles[i]);
      }

      console.log("selectedFiles: ", selectedFiles);
      console.log("FormData: ", formData.values);

      const result = await uploadImageMutation.mutateAsync([
        templateId,
        formData,
      ]);
      if (result.isSuccess && result.result) {
        toast({
          title: "Success",
          description: "Images uploaded successfully.",
        });
        setValue("backgroundImageUrl", result.result);
      } else {
        throw new Error(result.message || "Failed to upload images");
      }
    } catch (error) {
      console.error("Error uploading images:", error);
      toast({
        title: "Error",
        description: "Failed to upload images.",
        variant: "destructive",
      });
    }
  };
  const handleExportTemplate = async (
    template: Template,
    format: "pdf" | "image"
  ) => {
    const element = document.getElementById(`template-${template.templateId}`);
    if (!element) return;

    try {
      const canvas = await html2canvas(element, {
        scale: 2,
        useCORS: true,
        logging: false,
      });

      if (format === "pdf") {
        const pdf = new jsPDF({
          orientation: "portrait",
          unit: "px",
          format: [canvas.width, canvas.height],
        });
        pdf.addImage(
          canvas.toDataURL("image/png"),
          "PNG",
          0,
          0,
          canvas.width,
          canvas.height
        );
        pdf.save(`${template.templateName}.pdf`);
      } else {
        const link = document.createElement("a");
        link.download = `${template.templateName}.png`;
        link.href = canvas.toDataURL("image/png");
        link.click();
      }

      toast({
        title: "Success",
        description: `Template exported as ${format.toUpperCase()} successfully.`,
      });
    } catch (error) {
      console.error(`Error exporting template as ${format}:`, error);
      toast({
        title: "Error",
        description: `Failed to export template as ${format.toUpperCase()}.`,
        variant: "destructive",
      });
    }
  };

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

    // const imageUrl =
    //   imageData.find((url) => url) || "/placeholder.svg?height=300&width=300";

    return (
      <img
        src={imageData.toString()}
        alt='Template preview'
        className='w-full h-full object-cover'
        onError={(e) => {
          e.currentTarget.src = "/placeholder.svg?height=300&width=300";
          e.currentTarget.alt = "Image not found";
        }}
      />
    );
  };

  return (
    <div className='p-6 bg-gradient-to-br from-blue-50 to-purple-50 min-h-screen'>
      <div className='flex justify-between items-center mb-6'>
        <h1 className='text-4xl font-bold text-gray-800'>
          Invitation Templates
        </h1>
        <div className='flex items-center space-x-4'>
          <div className='relative'>
            <Input
              type='text'
              placeholder='Search templates...'
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className='pl-10 pr-4 py-2 rounded-full border-gray-300 focus:border-blue-500 focus:ring focus:ring-blue-200 focus:ring-opacity-50'
            />
            <Search className='absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400' />
          </div>
          <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
            <DialogTrigger asChild>
              <Button className='bg-blue-600 hover:bg-blue-700 text-white rounded-full'>
                <Plus className='w-5 h-5 mr-2' />
                Create New Template
              </Button>
            </DialogTrigger>
            <DialogContent className='max-w-6xl'>
              <DialogHeader>
                <DialogTitle className='text-2xl font-bold text-gray-800'>
                  {editingTemplate
                    ? "Edit Invitation Template"
                    : "Create New Invitation Template"}
                </DialogTitle>
              </DialogHeader>
              <div className='grid grid-cols-2 gap-6'>
                <form
                  onSubmit={handleSubmit(
                    editingTemplate ? handleEditTemplate : handleCreateTemplate
                  )}
                  className='space-y-4'>
                  <Controller
                    name='templateName'
                    control={control}
                    defaultValue=''
                    rules={{ required: "Template name is required" }}
                    render={({ field, fieldState: { error } }) => (
                      <div>
                        <label
                          htmlFor='templateName'
                          className='block text-sm font-medium text-gray-700'>
                          Template Name
                        </label>
                        <Input
                          {...field}
                          id='templateName'
                          placeholder='Enter template name'
                          className='mt-1'
                        />
                        {error && (
                          <p className='mt-1 text-sm text-red-600'>
                            {error.message}
                          </p>
                        )}
                      </div>
                    )}
                  />
                  <Controller
                    name='description'
                    control={control}
                    defaultValue=''
                    render={({ field }) => (
                      <div>
                        <label
                          htmlFor='description'
                          className='block text-sm font-medium text-gray-700'>
                          Description
                        </label>
                        <Textarea
                          {...field}
                          id='description'
                          placeholder='Enter template description'
                          className='mt-1'
                        />
                      </div>
                    )}
                  />
                  <Controller
                    name='backgroundImageUrl'
                    control={control}
                    defaultValue={[]}
                    render={({ field }) => (
                      <div>
                        <label
                          htmlFor='backgroundImageUrl'
                          className='block text-sm font-medium text-gray-700'>
                          Background Images
                        </label>
                        <Input
                          type='file' // Input kiểu file để người dùng chọn tệp
                          id='backgroundImageUrl'
                          accept='image/*' // Chỉ chấp nhận các tệp hình ảnh
                          multiple // Cho phép người dùng chọn nhiều tệp cùng lúc
                          // onChange={(e) => {
                          //   // Lấy danh sách các tệp được chọn hoặc rỗng nếu không có tệp nào
                          //   const files = e.target.files
                          //     ? Array.from(e.target.files)
                          //     : [];

                          //   // Nếu có template đang chỉnh sửa và có tệp được chọn
                          //   if (editingTemplate && files.length > 0) {
                          //     // Gọi hàm handleImageUpload để upload các tệp đã chọn
                          //     handleImageUpload(
                          //       editingTemplate.templateId,
                          //       files
                          //     );
                          //     console.log("File Upload in Input: ", files);
                          //   }

                          //   // Cập nhật giá trị của field với tất cả các tệp đã chọn
                          //   field.onChange([
                          //     ...field.value,
                          //     ...files.map((file) => URL.createObjectURL(file)),
                          //   ]); // Cập nhật URL tạm thời cho ảnh đã chọn
                          // }}
                          onChange={(e) => {
                            if (e.target.files && e.target.files.length > 0) {
                              setSelectedFiles(e.target.files);
                            }
                          }}
                          className='mt-1'
                        />
                        <div className='mt-2 flex flex-wrap gap-2'>
                          {field.value.map((url, index) => (
                            <div key={index} className='relative'>
                              <img
                                src={url.toString()}
                                alt={`Background ${index + 1}`}
                                className='w-20 h-20 object-cover rounded'
                              />
                              <button
                                type='button'
                                onClick={() => {
                                  const newUrls = [...field.value];
                                  newUrls.splice(index, 1);
                                  field.onChange(newUrls);
                                }}
                                className='absolute -top-2 -right-2 bg-red-500 text-white rounded-full p-1'>
                                <X className='w-4 h-4' />
                              </button>
                            </div>
                          ))}
                        </div>
                      </div>
                    )}
                  />

                  <Controller
                    name='textColor'
                    control={control}
                    defaultValue={[]}
                    render={({ field }) => (
                      <div>
                        <label
                          htmlFor='textColor'
                          className='block text-sm font-medium  text-gray-700'>
                          Text Colors
                        </label>
                        <div className='flex flex-wrap gap-2 mt-1'>
                          {field.value.map((color, index) => (
                            <Input
                              key={index}
                              type='color'
                              value={color}
                              onChange={(e) => {
                                const newColors = [...field.value];
                                newColors[index] = e.target.value;
                                field.onChange(newColors);
                              }}
                              className='w-10 h-10'
                            />
                          ))}
                          <Button
                            type='button'
                            onClick={() =>
                              field.onChange([...field.value, "#000000"])
                            }
                            className='bg-gray-200 hover:bg-gray-300 text-gray-800'>
                            Add Color
                          </Button>
                        </div>
                      </div>
                    )}
                  />
                  <Controller
                    name='textFont'
                    control={control}
                    defaultValue={[]}
                    render={({ field }) => (
                      <div>
                        <label
                          htmlFor='textFont'
                          className='block text-sm font-medium text-gray-700'>
                          Text Fonts
                        </label>
                        <div className='flex flex-wrap gap-2 mt-1'>
                          {field.value.map((font, index) => (
                            <select
                              key={index}
                              value={font}
                              onChange={(e) => {
                                const newFonts = [...field.value];
                                newFonts[index] = e.target.value;
                                field.onChange(newFonts);
                              }}
                              className='mt-1 block w-40 pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm rounded-md'>
                              {fonts.map((f) => (
                                <option key={f} value={f}>
                                  {f}
                                </option>
                              ))}
                            </select>
                          ))}
                          <Button
                            type='button'
                            onClick={() =>
                              field.onChange([...field.value, fonts[0]])
                            }
                            className='bg-gray-200 hover:bg-gray-300 text-gray-800'>
                            Add Font
                          </Button>
                        </div>
                      </div>
                    )}
                  />
                  <Button
                    type='submit'
                    className='w-full bg-blue-600 hover:bg-blue-700 text-white'>
                    {editingTemplate ? "Update Template" : "Create Template"}
                  </Button>
                </form>
                <div className='space-y-4'>
                  <h3 className='text-lg font-semibold'>Preview</h3>
                  <div
                    className='relative border rounded-lg overflow-hidden'
                    style={{ height: "500px" }}>
                    <div
                      style={{
                        transform: `scale(${previewScale})`,
                        transformOrigin: "top left",
                      }}>
                      {watch("backgroundImageUrl") &&
                        watch("backgroundImageUrl").length > 0 && (
                          <img
                            src={watch("backgroundImageUrl")[0].toString()}
                            alt='Template preview'
                            className='w-full h-full object-cover absolute top-0 left-0'
                          />
                        )}
                      <div className='relative z-10 p-4'>
                        <h2
                          style={{
                            color: watch("textColor")?.[0] || "#000000",
                            fontFamily: watch("textFont")?.[0] || "Arial",
                          }}
                          className='text-3xl font-bold mb-2'>
                          {watch("templateName")}
                        </h2>
                        <p
                          style={{
                            color: watch("textColor")?.[0] || "#000000",
                            fontFamily: watch("textFont")?.[0] || "Arial",
                          }}
                          className='text-lg'>
                          {watch("description")}
                        </p>
                      </div>
                    </div>
                    <div className='absolute top-2 right-2 flex space-x-2'>
                      <Button
                        onClick={() =>
                          setPreviewScale((prev) => Math.min(prev + 0.1, 2))
                        }
                        size='sm'>
                        <ZoomIn className='w-4 h-4' />
                      </Button>
                      <Button
                        onClick={() =>
                          setPreviewScale((prev) => Math.max(prev - 0.1, 0.5))
                        }
                        size='sm'>
                        <ZoomOut className='w-4 h-4' />
                      </Button>
                    </div>
                  </div>
                </div>
              </div>
            </DialogContent>
          </Dialog>
        </div>
      </div>
      {isLoadingTemplates ? (
        <div className='flex justify-center items-center h-64'>
          <Loader2 className='w-12 h-12 animate-spin text-blue-500' />
        </div>
      ) : (
        <AnimatePresence>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            exit={{ opacity: 0 }}
            className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6'>
            {filteredTemplates.map((template) => (
              <motion.div
                key={template.templateId}
                layout
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                exit={{ opacity: 0, y: -20 }}
                transition={{ duration: 0.3 }}>
                <Card className='bg-white shadow-lg rounded-lg overflow-hidden hover:shadow-xl transition-shadow duration-300'>
                  <CardHeader className='bg-gradient-to-r from-blue-500 to-purple-500 text-white'>
                    <CardTitle className='text-xl font-semibold'>
                      {template.templateName}
                    </CardTitle>
                  </CardHeader>
                  <CardContent className='p-4'>
                    <div
                      id={`template-${template.templateId}`}
                      className='relative w-full h-64 rounded-md overflow-hidden cursor-pointer'
                      onClick={() => setSelectedTemplate(template)}>
                      <TemplateImage templateId={template.templateId} />
                      <div className='absolute inset-0 flex items-center justify-center bg-black bg-opacity-40 opacity-0 hover:opacity-100 transition-opacity duration-300'>
                        <p className='text-white text-lg font-semibold'>
                          Click to view full size
                        </p>
                      </div>
                    </div>
                    <p className='text-sm text-gray-600 mt-4'>
                      {template.description}
                    </p>
                  </CardContent>
                  <CardFooter className='bg-gray-50 border-t p-4 flex justify-between'>
                    <Button
                      variant='outline'
                      onClick={() => {
                        setEditingTemplate(template);
                        setIsDialogOpen(true);
                      }}
                      className='text-blue-600 hover:bg-blue-50'>
                      <Edit className='w-4 h-4 mr-2' />
                      Edit
                    </Button>
                    <div className='flex space-x-2'>
                      <Button
                        onClick={() => handleExportTemplate(template, "pdf")}
                        className='bg-green-500 text-white hover:bg-green-600'>
                        <Download className='w-4 h-4 mr-2' />
                        PDF
                      </Button>
                      <Button
                        onClick={() => handleExportTemplate(template, "image")}
                        className='bg-purple-500 text-white hover:bg-purple-600'>
                        <Download className='w-4 h-4 mr-2' />
                        Image
                      </Button>
                    </div>
                    <Button
                      onClick={() => handleDeleteTemplate(template.templateId)}
                      className='bg-red-500 text-white hover:bg-red-600'>
                      <Trash className='w-4 h-4 mr-2' />
                      Delete
                    </Button>
                  </CardFooter>
                </Card>
              </motion.div>
            ))}
          </motion.div>
        </AnimatePresence>
      )}
      {selectedTemplate && (
        <Dialog
          open={!!selectedTemplate}
          onOpenChange={() => setSelectedTemplate(null)}>
          <DialogContent className='max-w-4xl'>
            <DialogHeader>
              <DialogTitle>{selectedTemplate.templateName}</DialogTitle>
            </DialogHeader>
            <div className='relative w-full h-[calc(100vh-200px)]'>
              <TemplateImage templateId={selectedTemplate.templateId} />
            </div>
          </DialogContent>
        </Dialog>
      )}
    </div>
  );
}
