import { Link } from "react-router-dom";
import { CircleUserRound } from "lucide-react";
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
} from "../ui/navigation-menu";
import { PATH_PUBLIC } from "../../router/path";
import useAuth from "@/hooks/useAuth";

import logo from "/public/images/87900153.jpg";
const Header = () => {
  const { signOutUser, isAuthenticated } = useAuth();

  const handleClickLogout = () => {
    signOutUser();
  };
  return (
    <header className='py-4 shadow'>
      <div className='container mx-auto px-4 md:px-6 '>
        <nav className='flex items-center justify-between'>
          <div>
            <img src={logo} alt='Logo' className='size-10' />
          </div>
          <div className='space-x-12' style={{ fontSize: "18px" }}>
            <Link
              to='/'
              className='text-zinc-900 dark:text-white dark:hover:text-zinc-400 hover:text-zinc-400  font-medium'>
              Home
            </Link>
            <Link
              to='card'
              className='text-zinc-900 dark:text-white dark:hover:text-zinc-400 hover:text-zinc-400 font-medium'>
              Product
            </Link>
            <Link
              to='#'
              className='text-zinc-900 dark:text-white dark:hover:text-zinc-400 hover:text-zinc-400 font-medium'>
              Service
            </Link>
            <Link
              to='#'
              className='text-zinc-900 dark:text-white dark:hover:text-zinc-400 hover:text-zinc-400 font-medium'>
              Offers
            </Link>
          </div>
          <div className=''>
            <NavigationMenu>
              <NavigationMenuList>
                <NavigationMenuItem>
                  <NavigationMenuTrigger>
                    <CircleUserRound className='text-zinc-900 dark:text-white w-6 h-6 dark:hover:text-zinc-400 hover:text-zinc-500' />
                  </NavigationMenuTrigger>
                  <NavigationMenuContent>
                    <NavigationMenuLink className='block px-4 py-1 text-zinc-900 dark:text-white hover:bg-zinc-200 dark:hover:bg-zinc-700 transition-colors duration-200'>
                      <Link to={PATH_PUBLIC.profileUser}>Profile</Link>
                    </NavigationMenuLink>
                    {!isAuthenticated ? (
                      <>
                        <NavigationMenuLink className='block px-4 py-1 text-zinc-900 dark:text-white hover:bg-zinc-200 dark:hover:bg-zinc-700 transition-colors duration-200'>
                          <Link to={PATH_PUBLIC.signIn}>Login</Link>
                        </NavigationMenuLink>
                        <NavigationMenuLink className='block px-4 py-1 text-zinc-900 dark:text-white hover:bg-zinc-200 dark:hover:bg-zinc-700 transition-colors duration-200'>
                          <Link to={PATH_PUBLIC.signUp}>Register</Link>
                        </NavigationMenuLink>
                      </>
                    ) : (
                      <NavigationMenuLink className='block px-4 py-1 text-zinc-900 dark:text-white hover:bg-zinc-200 dark:hover:bg-zinc-700 transition-colors duration-200'>
                        <Link to={PATH_PUBLIC.home} onClick={handleClickLogout}>
                          Logout
                        </Link>
                      </NavigationMenuLink>
                    )}
                    {/* <ModeToggle /> */}
                  </NavigationMenuContent>
                </NavigationMenuItem>
              </NavigationMenuList>
            </NavigationMenu>
            {/* <Button className='bg-green rounded-3xl text-[16px] flex items-center hover:bg-white'>
              <MdOutlinePhoneInTalk className='mr-4 w-5 h-5' />
              Contact
            </Button> */}
          </div>
        </nav>
      </div>
    </header>
  );
};

export default Header;
