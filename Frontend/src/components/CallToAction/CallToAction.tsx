import { Button } from "../ui/button";
import { Link } from "react-router-dom";

const CallToAction = () => {
  return (
    <div>
      <section className='bg-blue-600 py-20'>
        <div className='max-w-7xl mx-auto px-4 sm:px-6 lg:px-8'>
          <div className='text-center'>
            <h2 className='text-3xl font-extrabold text-white sm:text-4xl'>
              Ready to Create Your Perfect Card?
            </h2>
            <p className='mt-4 max-w-2xl mx-auto text-xl text-blue-100'>
              Join thousands of satisfied customers and start creating beautiful
              cards today.
            </p>
            <div className='mt-8'>
              <Button size='lg' variant='secondary' asChild>
                <Link to='/home'>Get Started for Free</Link>
              </Button>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default CallToAction;
