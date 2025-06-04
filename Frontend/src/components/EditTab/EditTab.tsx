import React from "react";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";

interface EditTabProps {
  invitationData: Record<string, string>;
  onInputChange: (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void;
}

const EditTab: React.FC<EditTabProps> = ({ invitationData, onInputChange }) => {
  return (
    <div className='space-y-4'>
      {Object.keys(invitationData).map((key) => (
        <div key={key}>
          <Label htmlFor={key}>{key.replace(/([A-Z])/g, " $1")}</Label>
          {key === "message" ? (
            <Textarea
              id={key}
              name={key}
              value={invitationData[key]}
              onChange={onInputChange}
              className='mt-1'
              rows={3}
            />
          ) : (
            <Input
              id={key}
              name={key}
              value={invitationData[key]}
              onChange={onInputChange}
              className='mt-1'
            />
          )}
        </div>
      ))}
    </div>
  );
};

export default EditTab;
