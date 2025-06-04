import { Bell, Search, User } from "lucide-react";

const HeaderAdmin = () => {
  return (
    <header className='bg-gray-800 text-white p-4 flex justify-between items-center'>
      <div className='flex items-center'>
        <Search className='h-5 w-5 mr-2' />
        <input
          type='text'
          placeholder='Search...'
          className='bg-gray-700 text-white px-3 py-1 rounded'
        />
      </div>
      <div className='flex items-center space-x-4'>
        <button className='p-1 rounded-full hover:bg-gray-700'>
          <Bell className='h-5 w-5' />
        </button>
        <button className='p-1 rounded-full hover:bg-gray-700'>
          <User className='h-5 w-5' />
        </button>
      </div>
    </header>
  );
};

export default HeaderAdmin;
