import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  setCurrentPage: (page: number) => void;
}

export const Pagination = ({
  currentPage,
  totalPages,
  setCurrentPage,
}: PaginationProps) => (
  <div className='flex justify-center items-center mt-8 space-x-2'>
    <Button
      variant='outline'
      size='sm'
      onClick={() => setCurrentPage(Math.max(currentPage - 1, 1))}
      disabled={currentPage === 1}>
      <ChevronLeft size={16} className='mr-1' />
      Previous
    </Button>
    <div className='text-sm font-medium'>
      Page {currentPage} of {totalPages}
    </div>
    <Button
      variant='outline'
      size='sm'
      onClick={() => setCurrentPage(Math.min(currentPage + 1, totalPages))}
      disabled={currentPage === totalPages}>
      Next
      <ChevronRight size={16} className='ml-1' />
    </Button>
  </div>
);
