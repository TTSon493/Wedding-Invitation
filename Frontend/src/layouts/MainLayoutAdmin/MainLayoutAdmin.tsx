import HeaderAdmin from "../../components/HeaderAdmin";
import SideBar from "../../components/SlideBar";
import { Outlet } from "react-router-dom";
const MainLayoutAdmin = () => {
  return (
    <div className='flex h-screen'>
      <SideBar />
      <div className='flex-1 flex flex-col overflow-hidden'>
        <HeaderAdmin />
        <main className='flex-1 overflow-x-hidden overflow-y-auto'>
          <Outlet />
        </main>
      </div>
    </div>
  );
};

export default MainLayoutAdmin;
