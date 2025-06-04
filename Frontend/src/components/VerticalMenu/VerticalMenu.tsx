import { Mail, Eye, Gift, Users, FileText, Palette } from "lucide-react";
import {
  Tooltip,
  TooltipContent,
  TooltipProvider,
  TooltipTrigger,
} from "../../components/ui/tooltip";

const VerticalMenu = ({
  setActiveComponent,
}: {
  setActiveComponent: (key: string) => void;
}) => {
  const menuItems = [
    { icon: <Mail className='w-8 h-8' />, label: "Card", key: "card" },
    { icon: <Eye className='w-8 h-8' />, label: "Preview", key: "preview" },
    { icon: <Gift className='w-8 h-8' />, label: "Registry", key: "registry" },
    {
      icon: <Users className='w-8 h-8' />,
      label: "Guest Manager",
      key: "guestManager",
    },
    {
      icon: <FileText className='w-8 h-8' />,
      label: "Guest Survey",
      key: "guestSurvey",
    },
    {
      icon: <Palette className='w-8 h-8' />,
      label: "Custom Design",
      key: "customDesign",
    },
  ];

  return (
    <div className='hidden md:flex flex-col items-center space-y-6'>
      <TooltipProvider>
        {menuItems.map(({ icon, label, key }) => (
          <Tooltip key={key}>
            <TooltipTrigger>
              <button
                className='w-12 h-12 bg-gray-100 rounded-full flex items-center justify-center text-gray-600 focus:outline-none'
                onClick={() => setActiveComponent(key)}
                aria-label={label}>
                {icon}
              </button>
            </TooltipTrigger>
            <TooltipContent>
              <p>{label}</p>
            </TooltipContent>
          </Tooltip>
        ))}
      </TooltipProvider>
    </div>
  );
};

export default VerticalMenu;
