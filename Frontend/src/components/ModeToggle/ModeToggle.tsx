import React, { useState } from 'react';
import { Moon, Sun } from 'lucide-react';

interface ModeToggleProps {
  className?: string;
}

export const ModeToggle: React.FC<ModeToggleProps> = ({ className }) => {
  const [isDark, setIsDark] = useState(false);
  const text = isDark ? 'light' : 'dark';
  const SwitchIcon = isDark ? Sun : Moon;

  const toggleColorMode = () => {
    setIsDark(!isDark);
    document.documentElement.classList.toggle('dark', !isDark); // Toggle dark mode class
  };

  return (
    <button
      className={`p-2 rounded-md bg-transparent hover:bg-gray-200 transition-colors ${className}`}
      aria-label={`Switch to ${text} mode`}
      onClick={toggleColorMode}
    >
      <SwitchIcon />
    </button>
  );
};
export default ModeToggle;