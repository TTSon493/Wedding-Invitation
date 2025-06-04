"use client";

import { useState } from "react";
import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../components/ui/dialog";
import { Label } from "../../components/ui/label";
import { Switch } from "../../components/ui/switch";
import { Edit, Download, Globe } from "lucide-react";
import { toast } from "../../hooks/use-toast";
import { Link } from "react-router-dom";
import { TemplateForm } from "@/components/TemplateFormforAdmin";

export interface Template {
  id: number;
  name: string;
  description: string;
  preview: string;
  content: string;
  isPublished: boolean;
}

const initialTemplates: Template[] = [
  {
    id: 1,
    name: "Elegant Floral",
    description: "A beautiful floral design",
    preview: "/placeholder.svg?height=200&width=300",
    content: "<h1>Elegant Floral Invitation</h1>",
    isPublished: false,
  },
  {
    id: 2,
    name: "Rustic Charm",
    description: "A charming rustic theme",
    preview: "/placeholder.svg?height=200&width=300",
    content: "<h1>Rustic Charm Invitation</h1>",
    isPublished: true,
  },
  {
    id: 3,
    name: "Modern Minimalist",
    description: "A sleek minimalist design",
    preview: "/placeholder.svg?height=200&width=300",
    content: "<h1>Modern Minimalist Invitation</h1>",
    isPublished: false,
  },
  {
    id: 4,
    name: "Vintage Romance",
    description: "A romantic vintage style",
    preview: "/placeholder.svg?height=200&width=300",
    content: "<h1>Vintage Romance Invitation</h1>",
    isPublished: true,
  },
];

export default function InvitationWedding() {
  const [templates, setTemplates] = useState<Template[]>(initialTemplates);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingTemplate, setEditingTemplate] = useState<Template | null>(null);

  const handleCreateTemplate = (newTemplate: Omit<Template, "id">) => {
    const id = Math.max(...templates.map((t) => t.id)) + 1;
    setTemplates([...templates, { ...newTemplate, id }]);
    setIsDialogOpen(false);
    toast({
      title: "Success",
      description: "New template created successfully.",
    });
  };

  const handleEditTemplate = (updatedTemplate: Template) => {
    setTemplates(
      templates.map((t) => (t.id === updatedTemplate.id ? updatedTemplate : t))
    );
    setEditingTemplate(null);
    toast({
      title: "Success",
      description: "Template updated successfully.",
    });
  };

  const handleExportTemplate = (template: Template) => {
    const blob = new Blob([template.content], { type: "text/html" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = `${template.name}.html`;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
    URL.revokeObjectURL(url);
    toast({
      title: "Success",
      description: "Template exported successfully.",
    });
  };

  const handlePublishToggle = (id: number) => {
    setTemplates(
      templates.map((t) =>
        t.id === id ? { ...t, isPublished: !t.isPublished } : t
      )
    );
    toast({
      title: "Success",
      description: `Template ${
        templates.find((t) => t.id === id)?.isPublished
          ? "unpublished"
          : "published"
      } successfully.`,
    });
  };

  return (
    <div className='p-6'>
      <div className='flex justify-between items-center mb-6'>
        <h1 className='text-3xl font-bold'>Invitation Templates</h1>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button>Create New Template</Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Create New Invitation Template</DialogTitle>
              <DialogDescription>
                Design a new invitation template for your wedding.
              </DialogDescription>
            </DialogHeader>
            <TemplateForm onSubmit={handleCreateTemplate} />
          </DialogContent>
        </Dialog>
      </div>
      <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6'>
        {templates.map((template) => (
          <Card key={template.id}>
            <CardHeader>
              <CardTitle>{template.name}</CardTitle>
            </CardHeader>
            <CardContent>
              <img
                src={template.preview}
                alt={`${template.name} preview`}
                className='w-full h-48 object-cover rounded-md mb-4'
              />
              <p className='text-sm text-gray-500 mb-2'>
                {template.description}
              </p>
              <div className='flex items-center space-x-2'>
                <Switch
                  id={`publish-${template.id}`}
                  checked={template.isPublished}
                  onCheckedChange={() => handlePublishToggle(template.id)}
                />
                <Label htmlFor={`publish-${template.id}`}>
                  {template.isPublished ? "Published" : "Unpublished"}
                </Label>
              </div>
            </CardContent>
            <CardFooter className='flex justify-between'>
              <Dialog>
                <DialogTrigger asChild>
                  <Button
                    variant='outline'
                    onClick={() => setEditingTemplate(template)}>
                    <Edit className='w-4 h-4 mr-2' />
                    Edit
                  </Button>
                </DialogTrigger>
                <DialogContent>
                  <DialogHeader>
                    <DialogTitle>Edit Invitation Template</DialogTitle>
                    <DialogDescription>
                      Make changes to your invitation template.
                    </DialogDescription>
                  </DialogHeader>
                  {editingTemplate && (
                    <TemplateForm
                      onSubmit={handleEditTemplate}
                      initialData={editingTemplate}
                    />
                  )}
                </DialogContent>
              </Dialog>
              <Button
                variant='outline'
                onClick={() => handleExportTemplate(template)}>
                <Download className='w-4 h-4 mr-2' />
                Export
              </Button>
              {template.isPublished && (
                <Button variant='outline'>
                  <Link to={`/portfolio/${template.id}`}>
                    <Globe className='w-4 h-4 mr-2' />
                    View
                  </Link>
                </Button>
              )}
            </CardFooter>
          </Card>
        ))}
      </div>
    </div>
  );
}
