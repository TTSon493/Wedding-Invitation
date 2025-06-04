import { useState } from "react";
import { Button } from "@/components/ui/button";
import { ScrollArea } from "@/components/ui/scroll-area";
import { ChevronDown, ChevronUp } from "lucide-react";

interface CategoriesProps {
  categories: string[];
  selectedCategory: string;
  setSelectedCategory: (category: string) => void;
  setCurrentPage: (page: number) => void;
}

export const Categories = ({
  categories,
  selectedCategory,
  setSelectedCategory,
  setCurrentPage,
}: CategoriesProps) => {
  const [isExpanded, setIsExpanded] = useState(false);

  return (
    <div className='w-full md:w-1/4 mb-4 md:mb-0 md:pr-4'>
      <div className='bg-white rounded-lg shadow-md p-4'>
        <div className='flex justify-between items-center mb-2'>
          <h2 className='font-semibold text-lg'>Categories</h2>
          <Button
            variant='ghost'
            size='sm'
            className='md:hidden'
            onClick={() => setIsExpanded(!isExpanded)}>
            {isExpanded ? <ChevronUp size={20} /> : <ChevronDown size={20} />}
          </Button>
        </div>
        <ScrollArea
          className={`h-[300px] md:h-auto ${
            isExpanded ? "block" : "hidden md:block"
          }`}>
          <div className='space-y-1'>
            {categories.map((category, index) => (
              <Button
                key={index}
                variant={selectedCategory === category ? "secondary" : "ghost"}
                className='w-full justify-start text-left'
                onClick={() => {
                  setSelectedCategory(category);
                  setCurrentPage(1);
                  setIsExpanded(false);
                }}>
                {category}
              </Button>
            ))}
          </div>
        </ScrollArea>
      </div>
    </div>
  );
};
