import { Label } from "@radix-ui/react-label";
import { RadioGroup, RadioGroupItem } from "../ui/radio-group";

interface RadioGroupProps {
  value: string;
  onChange: (value: string) => void;
}

export function RadioGroupForm({ value, onChange }: RadioGroupProps) {
  return (
    <RadioGroup value={value} onValueChange={onChange} className='flex'>
      <div className='flex items-center space-x-2'>
        <RadioGroupItem value='male' id='r1' />
        <Label htmlFor='r1'>Male</Label>
      </div>
      <div className='flex items-center space-x-2'>
        <RadioGroupItem value='female' id='r2' />
        <Label htmlFor='r2'>Female</Label>
      </div>
      <div className='flex items-center space-x-2'>
        <RadioGroupItem value='other' id='r3' />
        <Label htmlFor='r3'>Other</Label>
      </div>
    </RadioGroup>
  );
}
