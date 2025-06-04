import { Link } from "react-router-dom";
import {
  Home,
  Users,
  Mail,
  Calendar,
  Settings,
  CheckSquare,
} from "lucide-react";
import { PATH_ADMIN } from "@/router/path";

const Sidebar = () => {
  return (
    <aside className='bg-gray-900 text-white w-64 min-h-screen p-4'>
      <nav>
        <ul className='space-y-2'>
          <li>
            <Link
              to='/admin/dashboards'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Home className='h-5 w-5' />
              <span>Dashboard</span>
            </Link>
          </li>
          <li>
            <Link
              to='/admin/guests'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Users className='h-5 w-5' />
              <span>Guests</span>
            </Link>
          </li>
          <li>
            <Link
              to='/admin/users'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Users className='h-5 w-5' />
              <span>User</span>
            </Link>
          </li>
          <li>
            <Link
              to={PATH_ADMIN.templateInvitation}
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Mail className='h-5 w-5' />
              <span>Invitation Template</span>
            </Link>
          </li>
          <li>
            <Link
              to={PATH_ADMIN.templatesWedding}
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Mail className='h-5 w-5' />
              <span>Invitation Wedding Template</span>
            </Link>
          </li>

          <li>
            <Link
              to='admin/rsvp'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <CheckSquare className='h-5 w-5' />
              <span>RSVP Management</span>
            </Link>
          </li>
          <li>
            <Link
              to='admin/template-single'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Mail className='h-5 w-5' />
              <span>Invitation Single</span>
            </Link>
          </li>
          <li>
            <Link
              to='/admin/events'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Calendar className='h-5 w-5' />
              <span>Events</span>
            </Link>
          </li>
          <li>
            <Link
              to='/admin/settings'
              className='flex items-center space-x-2 p-2 hover:bg-gray-800 rounded'>
              <Settings className='h-5 w-5' />
              <span>Settings</span>
            </Link>
          </li>
        </ul>
      </nav>
    </aside>
  );
};

export default Sidebar;
