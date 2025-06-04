import React, { useState } from "react";

const stickers = Array.from({ length: 10 }, (_, index) => ({
  id: index + 1,
  src: `/images/sticker${index + 1}.png`,
}));

interface StickersTabProps {
  onStickerSelect: (stickerId: number) => void;
}

const StickersTab: React.FC<StickersTabProps> = ({ onStickerSelect }) => {
  const [selectedSticker, setSelectedSticker] = useState<number | null>(null);

  const handleStickerSelect = (stickerId: number) => {
    setSelectedSticker(stickerId);
    onStickerSelect(stickerId); 
  };

  return (
    <div className='space-y-4'>
      <h3 className='text-lg font-semibold'>Select a Sticker</h3>
      <div className='grid grid-cols-3 gap-4'>
        {stickers.map((sticker) => (
          <div
            key={sticker.id}
            className={`cursor-pointer p-2 border rounded-lg transition ${selectedSticker === sticker.id ? "border-blue-500" : "border-gray-300"}`}
            onClick={() => handleStickerSelect(sticker.id)}
          >
            <img
              src={sticker.src}
              alt={`Sticker ${sticker.id}`}
              className='w-full h-auto'
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default StickersTab;
