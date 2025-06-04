import { Input } from "@/components/ui/input";
import { Textarea } from "../ui/textarea";
import { Slider } from "../ui/slider";
import { Switch } from "../ui/switch";
import { Label } from "../ui/label";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Upload, Download, Code, Globe } from "lucide-react";
import { toast } from "@/hooks/use-toast";

interface CustomizationPanelProps {
  invitationData: {
    coupleNames: string;
    date: string;
    time: string;
    venue: string;
    address: string;
    message: string;
    giftLink: string;
    musicLink: string;
  };
  handleInputChange: (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => void;
  design: {
    backgroundIndex: number;
    textColor: string;
    fontSize: number;
    showAnimation: boolean;
  };
  handleDesignChange: (key: string, value: unknown) => void;
  guestView: boolean;
  setGuestView: (checked: boolean) => void;
  handlePush: () => void;
  handleExport: () => void;
  handleExportHTML: () => void;
  publicUrl: string;
}

export function CustomizationPanel({
  invitationData,
  handleInputChange,
  design,
  handleDesignChange,
  guestView,
  setGuestView,
  handlePush,
  handleExport,
  handleExportHTML,
  publicUrl,
}: CustomizationPanelProps) {
  const backgroundImages = [
    "/placeholder.svg?height=800&width=600&text=Floral",
    "/placeholder.svg?height=800&width=600&text=Beach",
    "/placeholder.svg?height=800&width=600&text=Mountain",
    "/placeholder.svg?height=800&width=600&text=City",
    "/images/anhcuoi.jpg", // Your image path
  ];
  return (
    <Card className='flex-[2] shadow-2xl rounded-3xl'>
      <CardContent className='p-8'>
        <div className='space-y-6'>
          <h2 className='text-3xl font-bold  mb-6'>
            Customize Your Invitation
          </h2>
          <div>
            <Label htmlFor='coupleNames' className='text-lg'>
              Couple Names
            </Label>
            <Input
              id='coupleNames'
              name='coupleNames'
              value={invitationData.coupleNames}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='date' className='text-lg'>
              Date
            </Label>
            <Input
              id='date'
              name='date'
              value={invitationData.date}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='time' className='text-lg'>
              Time
            </Label>
            <Input
              id='time'
              name='time'
              value={invitationData.time}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='venue' className='text-lg'>
              Venue
            </Label>
            <Input
              id='venue'
              name='venue'
              value={invitationData.venue}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='address' className='text-lg'>
              Address
            </Label>
            <Input
              id='address'
              name='address'
              value={invitationData.address}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='message' className='text-lg'>
              Message
            </Label>
            <Textarea
              id='message'
              name='message'
              value={invitationData.message}
              onChange={handleInputChange}
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='giftLink' className='text-lg'>
              Gift Registry Link
            </Label>
            <Input
              id='giftLink'
              name='giftLink'
              value={invitationData.giftLink}
              onChange={handleInputChange}
              placeholder='https://example.com/registry'
              className='mt-2'
            />
          </div>
          <div>
            <Label htmlFor='musicLink' className='text-lg'>
              Wedding Playlist Link
            </Label>
            <Input
              id='musicLink'
              name='musicLink'
              value={invitationData.musicLink}
              onChange={handleInputChange}
              placeholder='https://example.com/playlist'
              className='mt-2'
            />
          </div>
          <div>
            <Label className='text-lg'>Background Image</Label>
            <div className='flex space-x-2 mt-2'>
              {backgroundImages.map((_, index) => (
                <Button
                  key={index}
                  variant={
                    design.backgroundIndex === index ? "default" : "outline"
                  }
                  className='w-10 h-10 p-0'
                  onClick={() => handleDesignChange("backgroundIndex", index)}>
                  {index + 1}
                </Button>
              ))}
            </div>
          </div>
          <div>
            <Label htmlFor='textColor' className='text-lg'>
              Text Color
            </Label>
            <Input
              id='textColor'
              type='color'
              value={design.textColor}
              onChange={(e) => handleDesignChange("textColor", e.target.value)}
              className='h-12 p-1 mt-2'
            />
          </div>
          <div>
            <Label htmlFor='fontSize' className='text-lg'>
              Font Size
            </Label>
            <Slider
              id='fontSize'
              min={12}
              max={24}
              step={1}
              value={[design.fontSize]}
              onValueChange={(value) =>
                handleDesignChange("fontSize", value[0])
              }
              className='mt-2'
            />
          </div>
          <div className='flex items-center space-x-2'>
            <Switch
              id='showAnimation'
              checked={design.showAnimation}
              onCheckedChange={(checked) =>
                handleDesignChange("showAnimation", checked)
              }
            />
            <Label htmlFor='showAnimation' className='text-lg'>
              Show Animation
            </Label>
          </div>
          <div className='flex items-center space-x-2'>
            <Switch
              id='guestView'
              checked={guestView}
              onCheckedChange={setGuestView}
            />
            <Label htmlFor='guestView' className='text-lg'>
              Guest View
            </Label>
          </div>
          <div className='flex flex-col space-y-4'>
            <Button
              onClick={handlePush}
              className='w-full bg-green-600 hover:bg-green-700 py-6 text-lg'>
              <Upload className='w-5 h-5 mr-2' /> Push to Website
            </Button>
            {publicUrl && (
              <div className='flex items-center justify-between p-4 bg-gray-100 rounded-lg'>
                <span className='text-sm font-medium text-gray-800 truncate'>
                  {publicUrl}
                </span>
                <Button
                  size='sm'
                  onClick={() => {
                    navigator.clipboard.writeText(publicUrl);
                    toast({
                      title: "URL Copied",
                      description:
                        "The public URL has been copied to your clipboard.",
                    });
                  }}>
                  <Globe className='w-4 h-4 mr-2' /> Copy URL
                </Button>
              </div>
            )}
            <Button
              onClick={handleExport}
              className='w-full bg-blue-600 hover:bg-blue-700 py-6 text-lg'>
              <Download className='w-5 h-5 mr-2' /> Export Card (JSON)
            </Button>
            <Button
              onClick={handleExportHTML}
              className='w-full bg-purple-600 hover:bg-purple-700 py-6 text-lg'>
              <Code className='w-5 h-5 mr-2' /> Export Card (HTML)
            </Button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
