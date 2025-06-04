import React from "react";
import { Button } from "@/components/ui/button";

interface ExportButtonsProps {
  onExportPDF: () => void;
  onPushToWeb: () => void;
}

const ExportButtons: React.FC<ExportButtonsProps> = ({ onExportPDF, onPushToWeb }) => {
  return (
    <div className='flex justify-center space-x-4 mt-8'>
      <Button onClick={onExportPDF}>Export as PDF</Button>
      <Button onClick={onPushToWeb}>Publish to Web</Button>
    </div>
  );
};

export default ExportButtons;
