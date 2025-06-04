import React from "react";
import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from "@/components/ui/select";
import { Slider } from "@/components/ui/slider";
import { Button } from "@/components/ui/button";
import { Label } from "../ui/label";
import { Input } from "../ui/input";
import { AlignLeft, AlignCenter, AlignRight } from "lucide-react";

type TextAlign = "left" | "center" | "right";

interface Design {
  font: string;
  fontSize: number;
  textColor: string;
  backgroundColor: string;
  backgroundImage: string;
  textAlign: TextAlign;
}

interface DesignTabProps {
  design: Design; 
  onDesignChange: (key: keyof Design, value: string | number) => void;
}

const images = [
  "/images/anhcuoi.jpg",
  "/images/background2.jpg",
  "/images/background3.jpg",
  "/images/background4.jpg",
  "/images/background5.jpg",
  "/images/background6.jpg",
];

const DesignTab: React.FC<DesignTabProps> = ({ design, onDesignChange }) => {
  return (
    <div className='space-y-4'>
      <div>
        <Label htmlFor='font'>Font</Label>
        <Select value={design.font} onValueChange={(value) => onDesignChange("font", value)}>
          <SelectTrigger className='w-full'>
            <SelectValue placeholder='Select a font' />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value='Bentham'>Bentham</SelectItem>
            <SelectItem value='Arial'>Arial</SelectItem>
            <SelectItem value='Times New Roman'>Times New Roman</SelectItem>
            {/* Add more font options here */}
          </SelectContent>
        </Select>
      </div>
      <div>
        <Label htmlFor='fontSize'>Font Size</Label>
        <Slider
          id='fontSize'
          min={12}
          max={72}
          step={1}
          value={[design.fontSize]} 
          onValueChange={(value) => onDesignChange("fontSize", value[0])}
          className='mt-2'
        />
      </div>
      <div>
        <Label htmlFor='textColor'>Text Color</Label>
        <Input
          id='textColor'
          type='color'
          value={design.textColor}
          onChange={(e) => onDesignChange("textColor", e.target.value)}
          className='h-10 p-1 mt-1'
        />
      </div>
      <div>
        <Label htmlFor='backgroundColor'>Background Color</Label>
        <Input
          id='backgroundColor'
          type='color'
          value={design.backgroundColor}
          onChange={(e) => onDesignChange("backgroundColor", e.target.value)}
          className='h-10 p-1 mt-1'
        />
      </div>
      <div>
        <Label>Background Images</Label>
        <div className='grid grid-cols-3 gap-2 mt-2'>
          {images.map((image, index) => (
            <div key={index} className='cursor-pointer'>
              <img
                src={image}
                alt={`Background ${index + 1}`}
                className={`w-full h-20 object-cover rounded-lg border ${design.backgroundImage === image ? "border-blue-500" : "border-transparent"}`}
                onClick={() => onDesignChange("backgroundImage", image)}
              />
            </div>
          ))}
        </div>
      </div>
      <div>
        <Label>Text Alignment</Label>
        <div className='flex space-x-2 mt-2'>
          <Button
            variant={design.textAlign === "left" ? "default" : "outline"}
            onClick={() => onDesignChange("textAlign", "left")}
          >
            <AlignLeft />
          </Button>
          <Button
            variant={design.textAlign === "center" ? "default" : "outline"}
            onClick={() => onDesignChange("textAlign", "center")}
          >
            <AlignCenter />
          </Button>
          <Button
            variant={design.textAlign === "right" ? "default" : "outline"}
            onClick={() => onDesignChange("textAlign", "right")}
          >
            <AlignRight />
          </Button>
        </div>
      </div>
    </div>
  );
};

export default DesignTab;
