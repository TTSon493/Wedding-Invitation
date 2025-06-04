import { useState} from "react";
import { format, addMonths, subMonths, addYears, subYears } from "date-fns";
import { Popover, PopoverContent, PopoverTrigger } from "../ui/popover";
import { Button } from "../ui/button";
import { CalendarIcon } from "lucide-react";
import { Calendar } from "../ui/calendar";
import clsx from "clsx";

interface DatePickerProps {
  selectedDate?: Date;
  onDateSelect?: (date: Date | null) => void;
  className?: string;
}

function DatePicker({
  selectedDate,
  onDateSelect,
  className,
}: DatePickerProps) {
  const [date, setDate] = useState<Date | undefined>(selectedDate);
  const [currentMonth, setCurrentMonth] = useState<Date>(date || new Date());
  const [isOpen, setIsOpen] = useState(false);

  const handleDateSelect = (selectedDate: Date | undefined) => {
    setDate(selectedDate);
    if (onDateSelect) {
      onDateSelect(selectedDate ?? null);
    }
    setIsOpen(false);
  };

  const handlePrevYear = () => {
    setCurrentMonth((prevMonth) => subYears(prevMonth, 1));
  };

  const handleNextYear = () => {
    setCurrentMonth((prevMonth) => addYears(prevMonth, 1));
  };

  const handlePrevMonth = () => {
    setCurrentMonth((prevMonth) => subMonths(prevMonth, 1));
  };

  const handleNextMonth = () => {
    setCurrentMonth((prevMonth) => addMonths(prevMonth, 1));
  };

  return (
    <Popover open={isOpen} onOpenChange={setIsOpen} >
      <PopoverTrigger asChild>
        <Button
          variant={"outline"}
          className={clsx(
            "w-[240px] justify-start text-left font-normal",
            !date && "text-gray-400",
            className
          )}
          onClick={() => setIsOpen(!isOpen)} 
        >
          <CalendarIcon className='mr-2 h-4 w-4 text-gray-600' />
          {date ? format(date, "dd/MM/yyyy") : <span className="text-gray-500">Pick a date</span>}
        </Button>
      </PopoverTrigger>
      <PopoverContent className='w-350 p-0 relative' align='start'>
        <div className='flex justify-between items-center py-3 mb-2 px-2'>
          <button
            onClick={handlePrevYear}
            className="px-2 text-lg font-bold text-gray-500 hover:text-blue-600 transition duration-200"
          >
            {"<<"}
          </button>
          <button
            onClick={handlePrevMonth}
            className="px-2 text-lg font-bold text-gray-500 hover:text-blue-600 transition duration-200"
          >
            {"<"}
          </button>
          <span className="font-semibold text-lg flex-1 text-center">
            {format(currentMonth, "MMMM yyyy")}
          </span>
          <button
            onClick={handleNextMonth}
            className="px-2 text-lg font-bold text-gray-500 hover:text-blue-600 transition duration-200"
          >
            {">"}
          </button>
          <button
            onClick={handleNextYear}
            className="px-2 text-lg font-bold text-gray-500 hover:text-blue-600 transition duration-200"
          >
            {">>"}
          </button>
        </div>
        <Calendar
          mode='single'
          selected={date}
          onSelect={handleDateSelect}
          initialFocus
          month={currentMonth}
          className="custom-calendar border border-gray-300 rounded-lg shadow-lg w-full"
        />
      </PopoverContent>
    </Popover>
  );
}

export default DatePicker;
