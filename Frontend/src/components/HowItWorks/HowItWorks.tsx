import { Link } from "react-router-dom";
import { Button } from "../ui/button";

const HowItWorks = () => {
  return (
    <section id='how-it-works' className='py-20'>
      <div className='max-w-7xl mx-auto px-4 sm:px-6 lg:px-8'>
        <div className='text-center'>
          <h2 className='text-3xl font-extrabold text-gray-900 sm:text-4xl'>
            How It Works
          </h2>
          <p className='mt-4 max-w-2xl mx-auto text-xl text-gray-500'>
            Create your perfect card in just a few simple steps.
          </p>
        </div>

        <div className='mt-20'>
          <div className='relative'>
            <div
              className='absolute inset-0 flex items-center'
              aria-hidden='true'>
              <div className='w-full border-t border-gray-300' />
            </div>
            <div className='relative flex justify-between'>
              {[
                { step: 1, title: "Choose a Template" },
                { step: 2, title: "Customize Your Design" },
                { step: 3, title: "Share or Print" },
              ].map((item) => (
                <div key={item.step} className='bg-white px-4'>
                  <span className='flex items-center justify-center w-12 h-12 rounded-full bg-blue-600 text-white text-xl font-bold'>
                    {item.step}
                  </span>
                  <p className='mt-2 text-center font-medium text-gray-900'>
                    {item.title}
                  </p>
                </div>
              ))}
            </div>
          </div>
          <div className='mt-10 text-center'>
            <Button size='lg' asChild>
              <Link to='/home'>Start Creating Now</Link>
            </Button>
          </div>
        </div>
      </div>
    </section>
  );
};

export default HowItWorks;
