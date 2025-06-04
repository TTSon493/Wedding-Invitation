import { PlusCircle, Edit, Share } from "lucide-react";

const Features = () => {
  return (
    <section className='max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-20 bg-white'>
      <div className='text-center'>
        <h2 className='text-3xl font-extrabold text-gray-900'>
          Tạo - Chỉnh sửa - Chia sẻ
        </h2>
        <p className='mt-4 text-lg text-gray-500'>
          Mọi thứ bạn cần để có một tấm thiệp hoàn hảo
        </p>
      </div>
      <div className='mt-10'>
        <div className='grid grid-cols-1 gap-10 sm:grid-cols-2 lg:grid-cols-3'>
          <div className='pt-6'>
            <div className='flow-root rounded-lg bg-gray-50 px-6 pb-8'>
              <div className='-mt-6 h-56'>
                <div className='inline-flex items-center justify-center rounded-md bg-blue-500 p-3 shadow-lg'>
                  <PlusCircle
                    className='h-6 w-6 text-white'
                    aria-hidden='true'
                  />
                </div>
                <h3 className='mt-8 text-lg font-medium text-gray-900 tracking-tight'>
                  Choose a card template
                </h3>
                <p className='mt-5 text-base text-gray-500'>
                  Start with one of our professionally designed templates or
                  create your own.
                </p>
              </div>
            </div>
          </div>
          <div className='pt-6'>
            <div className='flow-root rounded-lg bg-gray-50 px-6 pb-8'>
              <div className='-mt-6 h-56'>
                <div className='inline-flex items-center justify-center rounded-md bg-blue-500 p-3 shadow-lg'>
                  <Edit className='h-6 w-6 text-white' aria-hidden='true' />
                </div>
                <h3 className='mt-8 text-lg font-medium text-gray-900 tracking-tight'>
                  Edit card
                </h3>
                <p className='mt-5 text-base text-gray-500'>
                  Add your own text, images and designs to make your card unique
                  and personal.
                </p>
              </div>
            </div>
          </div>
          <div className='pt-6'>
            <div className='flow-root rounded-lg bg-gray-50 px-6 pb-8'>
              <div className='-mt-6 h-56'>
                <div className='inline-flex items-center justify-center rounded-md bg-blue-500 p-3 shadow-lg'>
                  <Share className='h-6 w-6 text-white' aria-hidden='true' />
                </div>
                <h3 className='mt-8 text-lg font-medium text-gray-900 tracking-tight'>
                  Share instantly
                </h3>
                <p className='mt-5 text-base text-gray-500'>
                  Share your card online with a custom link or download it to
                  print.
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default Features;
