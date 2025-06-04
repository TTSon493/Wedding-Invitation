// "use client";

// import React, { useState, useRef, useEffect } from "react";
// import { Button } from "../../components/ui/button";
// import { Input } from "../../components/ui/input";
// import { Slider } from "../../components/ui/slider";
// import {
//   Select,
//   SelectContent,
//   SelectItem,
//   SelectTrigger,
//   SelectValue,
// } from "../../components/ui/select";
// import {
//   Type,
//   Sticker,
//   Image as ImageIcon,
//   Undo2,
//   Redo2,
//   Save,
//   ChevronRight,
//   ZoomIn,
//   ZoomOut,
//   Bold,
//   Italic,
//   AlignLeft,
//   AlignCenter,
//   AlignRight,
//   Minus,
//   Plus,
//   Palette,
//   Copy,
//   Trash,
//   Search,
// } from "lucide-react";

// const fonts = ["Bentham", "Arial", "Times New Roman", "Courier", "Verdana"];

// const stickers = [
//   { id: 1, src: "/placeholder.svg?text=heart", alt: "Heart Sticker" },
//   { id: 2, src: "/placeholder.svg?text=flower", alt: "Flower Sticker" },
//   { id: 3, src: "/placeholder.svg?text=ring", alt: "Ring Sticker" },
//   { id: 4, src: "/placeholder.svg?text=cake", alt: "Cake Sticker" },
// ];

// export default function InvitationDesigner() {
//   const [zoom, setZoom] = useState(100);
//   const [elements, setElements] = useState([]);
//   const [activeElement, setActiveElement] = useState(null);
//   const [activeMode, setActiveMode] = useState(null);
//   const [font, setFont] = useState("Bentham");
//   const [fontSize, setFontSize] = useState(24);
//   const [searchTerm, setSearchTerm] = useState("");
//   const canvasRef = useRef(null);

//   const addElement = (type, content) => {
//     const newElement = {
//       id: Date.now(),
//       type,
//       content,
//       position: { x: 50, y: 50 },
//       size: { width: 100, height: type === "text" ? 50 : 100 },
//       font,
//       fontSize,
//       style: { fontWeight: "normal", fontStyle: "normal", textAlign: "left" },
//       color: "#000000",
//     };
//     setElements([...elements, newElement]);
//     setActiveElement(newElement.id);
//   };

//   const updateElement = (id, updates) => {
//     setElements(
//       elements.map((el) => (el.id === id ? { ...el, ...updates } : el))
//     );
//   };

//   const handleTextChange = (e, id) => {
//     updateElement(id, { content: e.target.value });
//   };

//   const handleStyleChange = (style) => {
//     if (!activeElement) return;
//     updateElement(activeElement, {
//       style: {
//         ...elements.find((el) => el.id === activeElement).style,
//         ...style,
//       },
//     });
//   };

//   const handleFontChange = (newFont) => {
//     setFont(newFont);
//     if (activeElement) {
//       updateElement(activeElement, { font: newFont });
//     }
//   };

//   const handleFontSizeChange = (newSize) => {
//     setFontSize(newSize);
//     if (activeElement) {
//       updateElement(activeElement, { fontSize: newSize });
//     }
//   };

//   const handleColorChange = (e) => {
//     if (activeElement) {
//       updateElement(activeElement, { color: e.target.value });
//     }
//   };

//   const handleUndo = () => {
//     // Implement undo logic
//   };

//   const handleRedo = () => {
//     // Implement redo logic
//   };

//   const handleSave = () => {
//     // Implement save logic
//   };

//   const handleZoom = (newZoom) => {
//     setZoom(newZoom);
//   };

//   const filteredStickers = stickers.filter((sticker) =>
//     sticker.alt.toLowerCase().includes(searchTerm.toLowerCase())
//   );

//   useEffect(() => {
//     const handleClickOutside = (event) => {
//       if (canvasRef.current && !canvasRef.current.contains(event.target)) {
//         setActiveElement(null);
//         setActiveMode(null);
//       }
//     };

//     document.addEventListener("mousedown", handleClickOutside);
//     return () => {
//       document.removeEventListener("mousedown", handleClickOutside);
//     };
//   }, []);

//   return (
//     <div className='flex flex-col h-screen'>
//       <header className='flex items-center justify-around p-4 border-b'>
//         <div className='flex space-x-2'>
//           <Button
//             onClick={() => setActiveMode(activeMode === "text" ? null : "text")}
//             variant={activeMode === "text" ? "default" : "outline"}>
//             <Type className='w-4 h-4 mr-2' />
//             Add text
//           </Button>
//           <Button
//             onClick={() =>
//               setActiveMode(activeMode === "sticker" ? null : "sticker")
//             }
//             variant={activeMode === "sticker" ? "default" : "outline"}>
//             <Sticker className='w-4 h-4 mr-2' />
//             Add sticker
//           </Button>
//           <Button
//             onClick={() =>
//               setActiveMode(activeMode === "image" ? null : "image")
//             }
//             variant={activeMode === "image" ? "default" : "outline"}>
//             <ImageIcon className='w-4 h-4 mr-2' />
//             Add image
//           </Button>
//         </div>
//         <div className='flex items-center space-x-2'>
//           <Button onClick={handleUndo} variant='ghost' size='icon'>
//             <Undo2 className='w-4 h-4' />
//           </Button>
//           <Button onClick={handleRedo} variant='ghost' size='icon'>
//             <Redo2 className='w-4 h-4' />
//           </Button>
//           <Button onClick={handleSave} variant='outline'>
//             <Save className='w-4 h-4 mr-2' />
//             Save draft
//           </Button>
//           <Button>
//             Next
//             <ChevronRight className='w-4 h-4 ml-2' />
//           </Button>
//         </div>
//       </header>
//       {activeMode === "text" && (
//         <div className='flex items-center space-x-2 p-2 border-b'>
//           <Select value={font} onValueChange={handleFontChange}>
//             <SelectTrigger className='w-[180px]'>
//               <SelectValue placeholder='Select font' />
//             </SelectTrigger>
//             <SelectContent>
//               {fonts.map((f) => (
//                 <SelectItem key={f} value={f}>
//                   {f}
//                 </SelectItem>
//               ))}
//             </SelectContent>
//           </Select>
//           <div className='flex items-center'>
//             <Button
//               variant='outline'
//               size='icon'
//               onClick={() => handleFontSizeChange(Math.max(8, fontSize - 1))}>
//               <Minus className='h-4 w-4' />
//             </Button>
//             <span className='mx-2'>{fontSize}</span>
//             <Button
//               variant='outline'
//               size='icon'
//               onClick={() => handleFontSizeChange(Math.min(72, fontSize + 1))}>
//               <Plus className='h-4 w-4' />
//             </Button>
//           </div>
//           <Button
//             variant='outline'
//             size='icon'
//             onClick={() => handleStyleChange({ fontWeight: "bold" })}>
//             <Bold className='h-4 w-4' />
//           </Button>
//           <Button
//             variant='outline'
//             size='icon'
//             onClick={() => handleStyleChange({ fontStyle: "italic" })}>
//             <Italic className='h-4 w-4' />
//           </Button>
//           <Button
//             variant='outline'
//             size='icon'
//             onClick={() => handleStyleChange({ textAlign: "left" })}>
//             <AlignLeft className='h-4 w-4' />
//           </Button>
//           <Button
//             variant='outline'
//             size='icon'
//             onClick={() => handleStyleChange({ textAlign: "center" })}>
//             <AlignCenter className='h-4 w-4' />
//           </Button>
//           <Button
//             variant='outline'
//             size='icon'
//             onClick={() => handleStyleChange({ textAlign: "right" })}>
//             <AlignRight className='h-4 w-4' />
//           </Button>
//           <div className='flex items-center'>
//             <Palette className='h-4 w-4 mr-2' />
//             <input type='color' onChange={handleColorChange} />
//           </div>
//           <Button variant='outline' size='icon'>
//             <Copy className='h-4 w-4' />
//           </Button>
//           <Button variant='outline' size='icon'>
//             <Trash className='h-4 w-4' />
//           </Button>
//         </div>
//       )}
//       <main className='flex-1 flex overflow-hidden'>
//         {(activeMode === "sticker" || activeMode === "image") && (
//           <aside className='w-64 border-r p-4 overflow-y-auto'>
//             <div className='mb-4'>
//               <Input
//                 type='text'
//                 placeholder={`Search ${activeMode}s`}
//                 value={searchTerm}
//                 onChange={(e) => setSearchTerm(e.target.value)}
//                 className='w-full'
//                 icon={<Search className='w-4 h-4' />}
//               />
//             </div>
//             {activeMode === "sticker" && (
//               <div className='grid grid-cols-2 gap-2'>
//                 {filteredStickers.map((sticker) => (
//                   <div
//                     key={sticker.id}
//                     className='border rounded p-2 cursor-pointer hover:bg-gray-100'
//                     onClick={() => addElement("sticker", sticker.src)}>
//                     <img
//                       src={sticker.src}
//                       alt={sticker.alt}
//                       width={50}
//                       height={50}
//                     />
//                   </div>
//                 ))}
//               </div>
//             )}
//             {activeMode === "image" && (
//               <div>
//                 <input
//                   type='file'
//                   accept='image/*'
//                   onChange={(e) => {
//                     const file = e.target.files[0];
//                     if (file) {
//                       const reader = new FileReader();
//                       reader.onload = (e) =>
//                         addElement("image", e.target.result);
//                       reader.readAsDataURL(file);
//                     }
//                   }}
//                 />
//               </div>
//             )}
//           </aside>
//         )}
//         <div className='flex-1 relative overflow-auto'>
//           <div
//             className='absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2'
//             style={{ transform: `scale(${zoom / 100})` }}>
//             <div
//               ref={canvasRef}
//               className='w-[600px] h-[800px] bg-white shadow-lg relative'>
//               <img
//                 src='/placeholder.svg?height=800&width=600'
//                 alt='Wedding Invitation Template'
//                 layout='fill'
//                 objectFit='cover'
//               />
//               {elements.map((element) => (
//                 <div
//                   key={element.id}
//                   style={{
//                     position: "absolute",
//                     left: element.position.x,
//                     top: element.position.y,
//                     width: element.size.width,
//                     height: element.size.height,
//                     font: element.font,
//                     fontSize: `${element.fontSize}px`,
//                     ...element.style,
//                     color: element.color,
//                   }}
//                   onClick={() => setActiveElement(element.id)}>
//                   {element.type === "text" && (
//                     <textarea
//                       value={element.content}
//                       onChange={(e) => handleTextChange(e, element.id)}
//                       style={{
//                         background: "transparent",
//                         border:
//                           activeElement === element.id
//                             ? "1px dashed #000"
//                             : "none",
//                         resize: "both",
//                         overflow: "hidden",
//                         width: "100%",
//                         height: "100%",
//                       }}
//                     />
//                   )}
//                   {(element.type === "sticker" || element.type === "image") && (
//                     <img
//                       src={element.content}
//                       alt={
//                         element.type === "sticker" ? "Sticker" : "User Image"
//                       }
//                       layout='fill'
//                       objectFit='contain'
//                     />
//                   )}
//                 </div>
//               ))}
//             </div>
//           </div>
//         </div>
//       </main>
//       <footer className='flex items-center justify-end p-4 border-t'>
//         <div className='flex items-center space-x-2'>
//           <Button
//             onClick={() => handleZoom(Math.max(zoom - 10, 10))}
//             variant='outline'
//             size='icon'>
//             <ZoomOut className='w-4 h-4' />
//           </Button>
//           <Slider
//             value={[zoom]}
//             onValueChange={(value) => handleZoom(value[0])}
//             min={10}
//             max={200}
//             step={1}
//             className='w-32'
//           />
//           <Button
//             onClick={() => handleZoom(Math.min(zoom + 10, 200))}
//             variant='outline'
//             size='icon'>
//             <ZoomIn className='w-4 h-4' />
//           </Button>
//           <span className='text-sm'>{zoom}%</span>
//         </div>
//       </footer>
//     </div>
//   );
// }

const CardDesign = () => {
  return <div>CardDesign</div>;
};

export default CardDesign;
