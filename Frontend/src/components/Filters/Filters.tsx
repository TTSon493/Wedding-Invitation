import { Button } from "@/components/ui/button";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Badge } from "@/components/ui/badge";
import { X, Filter } from "lucide-react";

interface FiltersProps {
  filters: { name: string; options: string[] }[];
  selectedFilters: { color: string; designer: string; price: string };
  handleFilterChange: (filterName: string, option: string) => void;
  handleClearFilter: (filterName: string) => void;
  selectedCategory: string;
  setSelectedCategory: (category: string) => void;
  hasActiveFilters: boolean;
}

export const Filters = ({
  filters,
  selectedFilters,
  handleFilterChange,
  handleClearFilter,
  selectedCategory,
  setSelectedCategory,
  hasActiveFilters,
}: FiltersProps) => (
  <div className='mb-4 bg-white rounded-lg shadow-md p-4'>
    <div className='flex items-center mb-2'>
      <Filter size={20} className='mr-2' />
      <h2 className='font-semibold text-lg'>Filters</h2>
    </div>
    <div className='flex flex-wrap items-center gap-2 mb-4'>
      {filters.map((filter, index) => (
        <DropdownMenu key={index}>
          <DropdownMenuTrigger asChild>
            <Button variant='outline' size='sm'>
              {filter.name}
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent>
            {filter.options.map((option, idx) => (
              <DropdownMenuItem
                key={idx}
                onClick={() =>
                  handleFilterChange(filter.name.toLowerCase(), option)
                }>
                {option}
              </DropdownMenuItem>
            ))}
          </DropdownMenuContent>
        </DropdownMenu>
      ))}
    </div>

    {hasActiveFilters && (
      <div className='text-gray-700'>
        <h3 className='text-sm font-semibold mb-2'>Active Filters:</h3>
        <div className='flex flex-wrap gap-2'>
          {selectedCategory !== "All" && (
            <Badge variant='secondary' className='px-2 py-1'>
              Category: {selectedCategory}
              <Button
                variant='ghost'
                size='sm'
                className='ml-1 p-0 h-auto'
                onClick={() => setSelectedCategory("All")}>
                <X size={14} />
                <span className='sr-only'>Remove category filter</span>
              </Button>
            </Badge>
          )}
          {Object.entries(selectedFilters).map(
            ([key, value]) =>
              value !== "All" &&
              value !== "All Designers" && (
                <Badge key={key} variant='secondary' className='px-2 py-1'>
                  {key.charAt(0).toUpperCase() + key.slice(1)}: {value}
                  <Button
                    variant='ghost'
                    size='sm'
                    className='ml-1 p-0 h-auto'
                    onClick={() => handleClearFilter(key)}>
                    <X size={14} />
                    <span className='sr-only'>Remove {key} filter</span>
                  </Button>
                </Badge>
              )
          )}
        </div>
      </div>
    )}
  </div>
);
