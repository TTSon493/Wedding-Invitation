import React, { useState } from "react";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import EditTab from "../EditTab";
import DesignTab from "../DesignTab";
import StickersTab from "../StickersTab";

interface TabsComponentProps {
  invitationData: Record<string, string>;
  onInputChange: (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => void;
  design: Design;
  onDesignChange: (key: string, value: string | number) => void;
  onStickerSelect: (stickerId: number) => void; // New prop for sticker selection
}

interface Design {
  font: string;
  fontSize: number;
  textColor: string;
  backgroundColor: string;
  backgroundImage: string;
  textAlign: TextAlign;
}

type TextAlign = "left" | "center" | "right";

const TabsComponent: React.FC<TabsComponentProps> = ({
  invitationData,
  onInputChange,
  design,
  onDesignChange,
  onStickerSelect,
}) => {
  const [activeTab, setActiveTab] = useState<string>("edit");

  return (
    <Tabs value={activeTab} onValueChange={setActiveTab}>
      <TabsList className='grid w-full grid-cols-3 mb-6'>
        <TabsTrigger value='edit'>Edit</TabsTrigger>
        <TabsTrigger value='design'>Design</TabsTrigger>
        <TabsTrigger value='stickers'>Stickers</TabsTrigger>
      </TabsList>
      <TabsContent value='edit'>
        <EditTab
          invitationData={invitationData}
          onInputChange={onInputChange}
        />
      </TabsContent>
      <TabsContent value='design'>
        <DesignTab design={design} onDesignChange={onDesignChange} />
      </TabsContent>
      <TabsContent value='stickers'>
        <StickersTab onStickerSelect={onStickerSelect} />
      </TabsContent>
    </Tabs>
  );
};

export default TabsComponent;
