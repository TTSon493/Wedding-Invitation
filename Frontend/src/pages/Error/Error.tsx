import { Link } from "react-router-dom";

const Error = () => {
  return (
    <div className='flex min-h-[100dvh] flex-col items-center justify-center bg-background px-4 py-12 sm:px-6 lg:px-8'>
      <div className='mx-auto max-w-md text-center'>
        <div className='mx-auto h-12 w-12 text-primary' />
        <h1 className='mt-4 text-3xl font-bold tracking-tight text-foreground sm:text-4xl'>
          Oops, something went wrong!
        </h1>
        <p className='mt-4 text-muted-foreground'>
          We're sorry, but an unexpected error has occurred. Please try again
          later or contact support if the issue persists.
        </p>
        <div className='mt-6 inline-flex items-center bg-neutral-950 px-4 py-2 text-sm font-medium text-white shadow-md transition-colors hover:bg-white hover:text-neutral-950 focus:outline-none focus:ring-2 focus:ring-primary focus:ring-offset-2'>
          <Link to={"/home"}>Go to Homepage</Link>
        </div>
      </div>
    </div>
  );
};

export default Error;
