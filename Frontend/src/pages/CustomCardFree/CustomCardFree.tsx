import React, { useState, useRef, useEffect } from "react";
import { Rnd } from "react-rnd";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Slider } from "@/components/ui/slider";
import html2canvas from "html2canvas";
import jsPDF from "jspdf";
import {
  Bold,
  Italic,
  Underline,
  AlignLeft,
  AlignCenter,
  AlignRight,
  Image as ImageIcon,
  Sticker,
  Type,
  Plus,
  Trash2,
  FileImage,
  RotateCw,
  ChevronLeft,
  ChevronRight,
  Download,
} from "lucide-react";
import { Link, useParams } from "react-router-dom";
import { PATH_USER } from "@/router/path";
import { useGetImageInvitationTemplate } from "@/hooks/useAPI";
import Spinner from "@/components/Spinner";

interface Element {
  id: string;
  type: "text" | "sticker" | "image";
  content: string;
  x: number;
  y: number;
  width: number;
  height: number;
  rotation: number;
  fontSize?: number;
  fontFamily?: string;
  fontStyle?: string;
  fontWeight?: string;
  textDecoration?: string;
  textAlign?: "left" | "center" | "right";
  color?: string;
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
  "Bookman",
  "Comic Sans MS",
  "Trebuchet MS",
  "Arial Black",
  "Impact",
];

const stickers = [
  "❤️",
  "💍",
  "🎉",
  "🥂",
  "🌸",
  "🌺",
  "🌷",
  "🌹",
  "🍾",
  "🕊️",
  "💐",
  "🎊",
  "👰",
  "🤵",
  "💒",
  "🏰",
  "🌈",
  "✨",
  "💫",
  "🎂",
];

const CustomCardFree: React.FC = () => {
  const [elements, setElements] = useState<Element[]>([]);
  const [selectedElement, setSelectedElement] = useState<string | null>(null);
  const [isTabsVisible, setIsTabsVisible] = useState(true);
  const [backgroundImage, setBackgroundImage] = useState<string | null>(null);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const cardRef = useRef<HTMLDivElement>(null);

  const { templateId } = useParams();

  const {
    data: imageData,
    isLoading,
    isError,
  } = useGetImageInvitationTemplate(templateId || "");

  useEffect(() => {
    if (imageData && imageData.length > 0) {
      setBackgroundImage(imageData[0]);
    }
  }, [imageData]);

  if (isLoading) {
    return <Spinner />;
  }

  if (isError) {
    return <div className='text-red-500'>Error loading template images</div>;
  }

  const addElement = (type: "text" | "sticker" | "image", content: string) => {
    const newElement: Element = {
      id: `${type}-${Date.now()}`,
      type,
      content,
      x: 0,
      y: 0,
      width: type === "text" ? 200 : 100,
      height: type === "text" ? 50 : 100,
      rotation: 0,
      ...(type === "text" && {
        fontSize: 16,
        fontFamily: "Arial",
        fontStyle: "normal",
        fontWeight: "normal",
        textDecoration: "none",
        textAlign: "left",
        color: "#000000",
      }),
    };
    setElements([...elements, newElement]);
    setSelectedElement(newElement.id);
  };

  const updateElement = (id: string, updates: Partial<Element>) => {
    setElements(
      elements.map((el) => (el.id === id ? { ...el, ...updates } : el))
    );
  };

  const deleteElement = (id: string) => {
    setElements(elements.filter((el) => el.id !== id));
    setSelectedElement(null);
  };

  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (event) => {
        if (event.target) {
          addElement("image", event.target.result as string);
        }
      };
      reader.readAsDataURL(e.target.files[0]);
    }
  };

  const exportAsImage = async () => {
    if (cardRef.current) {
      const canvas = await html2canvas(cardRef.current, {
        scale: 2,
        useCORS: true,
        allowTaint: true,
      });
      const dataUrl = canvas.toDataURL("image/png");
      const link = document.createElement("a");
      link.download = "custom-invitation-card.png";
      link.href = dataUrl;
      link.click();
    }
  };

  const exportAsPDF = async () => {
    if (cardRef.current) {
      const canvas = await html2canvas(cardRef.current, {
        scale: 2,
        useCORS: true,
        allowTaint: true,
      });
      const imgData = canvas.toDataURL("image/jpeg", 1.0);
      const pdf = new jsPDF({
        orientation: "portrait",
        unit: "px",
        format: [canvas.width, canvas.height],
      });
      pdf.addImage(imgData, "JPEG", 0, 0, canvas.width, canvas.height);
      pdf.save("custom-invitation-card.pdf");
    }
  };

  const selectedElementData = selectedElement
    ? elements.find((el) => el.id === selectedElement)
    : null;

  const changeBackgroundImage = (direction: "next" | "prev") => {
    if (imageData) {
      const newIndex =
        direction === "next"
          ? (currentImageIndex + 1) % imageData.length
          : (currentImageIndex - 1 + imageData.length) % imageData.length;
      setCurrentImageIndex(newIndex);
      setBackgroundImage(imageData[newIndex]);
    }
  };

  return (
    <div className='flex h-screen bg-gradient-to-r from-purple-100 to-pink-100'>
      <div
        className={`w-1/4 bg-white shadow-lg transition-all duration-300 ease-in-out ${
          isTabsVisible ? "translate-x-0" : "-translate-x-full"
        }`}>
        {isTabsVisible && (
          <Tabs defaultValue='text' className='w-full p-4'>
            <TabsList className='grid w-full grid-cols-4 mb-4'>
              <TabsTrigger value='text'>
                <Type className='w-4 h-4 mr-2' /> Text
              </TabsTrigger>
              <TabsTrigger value='stickers'>
                <Sticker className='w-4 h-4 mr-2' /> Stickers
              </TabsTrigger>
              <TabsTrigger value='images'>
                <ImageIcon className='w-4 h-4 mr-2' /> Images
              </TabsTrigger>
              <TabsTrigger value='background'>
                <ImageIcon className='w-4 h-4 mr-2' /> Background
              </TabsTrigger>
            </TabsList>
            <TabsContent value='text' className='space-y-4'>
              <Button
                onClick={() => addElement("text", "New Text")}
                className='w-full'>
                <Plus className='w-4 h-4 mr-2' /> Add Text
              </Button>
              {selectedElementData && selectedElementData.type === "text" && (
                <>
                  <div>
                    <Label htmlFor='text-content'>Text Content</Label>
                    <Input
                      id='text-content'
                      value={selectedElementData.content}
                      onChange={(e) =>
                        updateElement(selectedElementData.id, {
                          content: e.target.value,
                        })
                      }
                    />
                  </div>
                  <div>
                    <Label htmlFor='font-family'>Font Family</Label>
                    <select
                      id='font-family'
                      value={selectedElementData.fontFamily}
                      onChange={(e) =>
                        updateElement(selectedElementData.id, {
                          fontFamily: e.target.value,
                        })
                      }
                      className='w-full p-2 border rounded'>
                      {fonts.map((font) => (
                        <option key={font} value={font}>
                          {font}
                        </option>
                      ))}
                    </select>
                  </div>
                  <div>
                    <Label>Font Size</Label>
                    <Slider
                      min={8}
                      max={72}
                      step={1}
                      value={[selectedElementData.fontSize || 16]}
                      onValueChange={(value) =>
                        updateElement(selectedElementData.id, {
                          fontSize: value[0],
                        })
                      }
                    />
                  </div>
                  <div>
                    <Label htmlFor='text-color'>Text Color</Label>
                    <Input
                      id='text-color'
                      type='color'
                      value={selectedElementData.color || "#000000"}
                      onChange={(e) =>
                        updateElement(selectedElementData.id, {
                          color: e.target.value,
                        })
                      }
                    />
                  </div>
                  <div className='flex space-x-2'>
                    <Button
                      variant={
                        selectedElementData.fontStyle === "italic"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          fontStyle:
                            selectedElementData.fontStyle === "italic"
                              ? "normal"
                              : "italic",
                        })
                      }>
                      <Italic className='h-4 w-4' />
                    </Button>
                    <Button
                      variant={
                        selectedElementData.fontWeight === "bold"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          fontWeight:
                            selectedElementData.fontWeight === "bold"
                              ? "normal"
                              : "bold",
                        })
                      }>
                      <Bold className='h-4 w-4' />
                    </Button>
                    <Button
                      variant={
                        selectedElementData.textDecoration === "underline"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          textDecoration:
                            selectedElementData.textDecoration === "underline"
                              ? "none"
                              : "underline",
                        })
                      }>
                      <Underline className='h-4 w-4' />
                    </Button>
                    <Button
                      variant={
                        selectedElementData.textAlign === "left"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          textAlign: "left",
                        })
                      }>
                      <AlignLeft className='h-4 w-4' />
                    </Button>
                    <Button
                      variant={
                        selectedElementData.textAlign === "center"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          textAlign: "center",
                        })
                      }>
                      <AlignCenter className='h-4 w-4' />
                    </Button>
                    <Button
                      variant={
                        selectedElementData.textAlign === "right"
                          ? "default"
                          : "outline"
                      }
                      size='icon'
                      onClick={() =>
                        updateElement(selectedElementData.id, {
                          textAlign: "right",
                        })
                      }>
                      <AlignRight className='h-4 w-4' />
                    </Button>
                  </div>
                </>
              )}
            </TabsContent>
            <TabsContent value='stickers'>
              <div className='grid grid-cols-5 gap-2'>
                {stickers.map((sticker, index) => (
                  <Button
                    key={index}
                    variant='outline'
                    onClick={() => addElement("sticker", sticker)}
                    className='text-2xl'>
                    {sticker}
                  </Button>
                ))}
              </div>
            </TabsContent>
            <TabsContent value='images'>
              <Input
                type='file'
                accept='image/*'
                onChange={handleImageUpload}
                className='mb-4'
              />
            </TabsContent>
            <TabsContent value='background'>
              <div className='flex justify-between items-center mb-4'>
                <Button onClick={() => changeBackgroundImage("prev")}>
                  <ChevronLeft className='w-4 h-4' />
                </Button>
                <span>
                  Background {currentImageIndex + 1} of {imageData?.length}
                </span>
                <Button onClick={() => changeBackgroundImage("next")}>
                  <ChevronRight className='w-4 h-4' />
                </Button>
              </div>
            </TabsContent>
          </Tabs>
        )}
      </div>
      <div
        className={`flex-1 p-4 overflow-auto transition-all duration-300 ${
          isTabsVisible ? "ml-1/4" : ""
        }`}>
        <div className='flex justify-between items-center mb-4'>
          <Button
            onClick={() => setIsTabsVisible(!isTabsVisible)}
            className='transition-all duration-300 ease-in-out transform hover:scale-105'>
            {isTabsVisible ? "Hide Options" : "Show Options"}
          </Button>
          <div className='flex space-x-2'>
            <Button onClick={exportAsImage} variant='outline'>
              <Download className='w-4 h-4 mr-2' /> Export as Image
            </Button>
            <Button onClick={exportAsPDF} variant='outline'>
              <FileImage className='w-4 h-4 mr-2' /> Export as PDF
            </Button>
            <Link to={PATH_USER.customCardPre}>
              <Button variant='default'>Next To Push Web</Button>
            </Link>
          </div>
        </div>
        <div
          ref={cardRef}
          className='relative mx-auto'
          style={{
            width: "100%",
            height: "80vh",
            maxWidth: "600px",
            aspectRatio: "3 / 4",
          }}>
          <img
            src={backgroundImage || "/placeholder.svg"}
            alt='Card Background'
            className='w-full h-full object-cover rounded-lg shadow-lg'
          />
          <div className='absolute inset-0'>
            {elements.map((element) => (
              <Rnd
                key={element.id}
                size={{ width: element.width, height: element.height }}
                position={{ x: element.x, y: element.y }}
                onDragStop={(_e, d) => {
                  updateElement(element.id, { x: d.x, y: d.y });
                }}
                onResizeStop={(_e, _direction, ref, _delta, position) => {
                  updateElement(element.id, {
                    width: parseInt(ref.style.width),
                    height: parseInt(ref.style.height),
                    ...position,
                  });
                }}
                bounds='parent'
                enableResizing={selectedElement === element.id}
                disableDragging={selectedElement !== element.id}
                style={{
                  transform: `rotate(${element.rotation}deg)`,
                }}>
                <div
                  className={`w-full h-full flex items-center justify-center cursor-move ${
                    selectedElement === element.id ? "ring-2 ring-blue-500" : ""
                  }`}
                  onClick={() => setSelectedElement(element.id)}
                  style={{
                    ...(element.type === "text" && {
                      fontSize: `${element.fontSize}px`,
                      fontFamily: element.fontFamily,
                      fontStyle: element.fontStyle,
                      fontWeight: element.fontWeight,
                      textDecoration: element.textDecoration,
                      textAlign: element.textAlign,
                      color: element.color,
                    }),
                  }}>
                  {element.type === "text" && element.content}
                  {element.type === "sticker" && (
                    <span className='text-4xl'>{element.content}</span>
                  )}
                  {element.type === "image" && (
                    <img
                      src={element.content}
                      alt='Uploaded'
                      className='w-full h-full object-contain'
                    />
                  )}
                  {selectedElement === element.id && (
                    <div
                      className='absolute top-0 right-0 w-6 h-6 bg-blue-500 rounded-full cursor-pointer'
                      onMouseDown={(e) => {
                        e.stopPropagation();
                        const startAngle = element.rotation;
                        const startMouseX = e.clientX;
                        const startMouseY = e.clientY;
                        const onMouseMove = (moveEvent: MouseEvent) => {
                          const deltaX = moveEvent.clientX - startMouseX;
                          const deltaY = moveEvent.clientY - startMouseY;
                          const angle =
                            Math.atan2(deltaY, deltaX) * (180 / Math.PI);
                          updateElement(element.id, {
                            rotation: startAngle + angle,
                          });
                        };
                        const onMouseUp = () => {
                          document.removeEventListener(
                            "mousemove",
                            onMouseMove
                          );
                          document.removeEventListener("mouseup", onMouseUp);
                        };
                        document.addEventListener("mousemove", onMouseMove);
                        document.addEventListener("mouseup", onMouseUp);
                      }}>
                      <RotateCw className='w-4 h-4 text-white m-1' />
                    </div>
                  )}
                </div>
              </Rnd>
            ))}
          </div>
        </div>
      </div>
      {selectedElement && (
        <div className='absolute bottom-4 right-4'>
          <Button
            variant='destructive'
            onClick={() => deleteElement(selectedElement)}>
            <Trash2 className='w-4 h-4 mr-2' /> Delete Element
          </Button>
        </div>
      )}
    </div>
  );
};

export default CustomCardFree;
