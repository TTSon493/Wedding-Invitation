import { useState } from "react";
import { Input } from "../ui/input";
import { Label } from "../ui/label";
import { Template } from "../../pages/Admin/InvitationWedding";
import { Textarea } from "../ui/textarea";
import { DialogFooter } from "../ui/dialog";
import { Button } from "../ui/button";

interface TemplateFormProps {
  onSubmit: (template: Template) => void;
  initialData?: Template;
}
function InvitationTemplateForm({ onSubmit, initialData }: TemplateFormProps) {
  const [name, setName] = useState(initialData?.name || "");
  const [description, setDescription] = useState(
    initialData?.description || ""
  );
  const [content] = useState(initialData?.content || "");

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      id: 1, // Này để vậy thôi
      name,
      description,
      content,
      preview: initialData?.preview || "/placeholder.svg?height=200&width=300",
      isPublished: initialData?.isPublished || false,
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className='grid gap-4 py-4'>
        <div className='grid grid-cols-4 items-center gap-4'>
          <Label htmlFor='name' className='text-right'>
            Name
          </Label>
          <Input
            id='name'
            value={name}
            onChange={(e) => setName(e.target.value)}
            className='col-span-3'
          />
        </div>
        <div className='grid grid-cols-4 items-center gap-4'>
          <Label htmlFor='description' className='text-right'>
            Text Color
          </Label>
          <Input
            id='text color'
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className='col-span-3'
          />
        </div>
        <div className='grid grid-cols-4 items-center gap-4'>
          <Label htmlFor='description' className='text-right'>
            Text Font
          </Label>
          <Input
            id='text font'
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className='col-span-3'
          />
        </div>
        <div className='grid grid-cols-4 items-center gap-4'>
          <Label htmlFor='description' className='text-right'>
            Description
          </Label>
          <Textarea
            id='description'
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            className='col-span-3'
          />
        </div>
        <div className='grid grid-cols-4 items-center gap-4'>
          <Label htmlFor='picture' className='text-right w-full'>
            Image
          </Label>
          {/* <Textarea
            id='content'
            value={content}
            onChange={(e) => setContent(e.target.value)}
            className='col-span-3'
            rows={10}
          /> */}
          <Input id='picture' type='file' />
        </div>
      </div>
      <DialogFooter>
        <Button type='submit'>Save Template</Button>
      </DialogFooter>
    </form>
  );
}

export default InvitationTemplateForm;
