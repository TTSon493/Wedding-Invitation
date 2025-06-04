import { Facebook, Instagram, Twitter } from "lucide-react";
import { Link } from "react-router-dom";

const Footer = () => {
  return (
    <footer className='bg-muted py-8 md:py-12'>
      <div className='container mx-auto flex flex-col items-center justify-between gap-8 md:flex-row'>
        <div className='flex items-center gap-4'>
          <span className='text-xl font-bold'>SWD</span>
        </div>
        <nav className='flex flex-col items-center gap-4 md:flex-row md:gap-8'>
          <Link to='#' className='text-muted-foreground hover:text-primary'>
            Inicio
          </Link>
          <Link to='#' className='text-muted-foreground hover:text-primary'>
            Vuelos
          </Link>
          <Link to='#' className='text-muted-foreground hover:text-primary'>
            Sobre Nosotros
          </Link>
          <Link to='#' className='text-muted-foreground hover:text-primary'>
            Contacto
          </Link>
        </nav>
        <div className='flex flex-col items-center gap-2 text-muted-foreground'>
          <p>SWD FPT SAY Hi</p>
          <p>Calle Falsa 123, Ciudad</p>
          <p>Tel: 555-1234</p>
          <p>info@aerotravel.com</p>
          <div className='flex gap-4'>
            <Link to='#' className='text-muted-foreground hover:text-primary'>
              <Facebook className='h-6 w-6' />
            </Link>
            <Link to='#' className='text-muted-foreground hover:text-primary'>
              <Twitter className='h-6 w-6' />
            </Link>
            <Link to='#' className='text-muted-foreground hover:text-primary'>
              <Instagram className='h-6 w-6' />
            </Link>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
