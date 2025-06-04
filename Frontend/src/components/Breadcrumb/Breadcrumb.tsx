import { ChevronRight } from "lucide-react";

const Breadcrumb = () => (
  <nav className='flex mb-4 text-xl' aria-label='Breadcrumb'>
    <ol className='inline-flex items-center space-x-1 md:space-x-3'>
      <li className='inline-flex items-center'>
        <a href='#' className='text-gray-500 hover:text-gray-700'>
          Home
        </a>
      </li>
      <li>
        <div className='flex items-center'>
          <ChevronRight className='w-4 h-4 text-gray-400' />
          <a href='#' className='ml-1 text-gray-500 hover:text-gray-700 md:ml-2'>
            Wedding
          </a>
        </div>
      </li>
      <li>
        <div className='flex items-center'>
          <ChevronRight className='w-4 h-4 text-gray-400' />
          <a href='#' className='ml-1 text-gray-500 hover:text-gray-700 md:ml-2'>
            Wedding Invitations
          </a>
        </div>
      </li>
      <li aria-current='page'>
        <div className='flex items-center'>
          <ChevronRight className='w-4 h-4 text-gray-400' />
          <span className='ml-1 text-gray-900 md:ml-2'>
            Watercolor Floral Garland
          </span>
        </div>
      </li>
    </ol>
  </nav>
);

export default Breadcrumb;
