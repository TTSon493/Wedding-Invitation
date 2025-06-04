import { Button } from "../../components/ui/button";

const RelatedProducts = () => (
  <div className='mt-8'>
    <h3 className='text-2xl font-bold mb-4'>You May Also Like</h3>
    <div className='grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4'>
      {[1, 2, 3].map((_, index) => (
        <div key={index} className='border p-4 rounded-lg'>
          <img
            src='/placeholder.svg?height=300&width=300'
            alt={`Related Product ${index + 1}`}
            className='rounded-lg mb-2'
          />
          <h4 className='text-lg font-semibold'>Related Product {index + 1}</h4>
          <p className='text-gray-600 mb-2'>$1.50/ea</p>
          <Button>Add to Cart</Button>
        </div>
      ))}
    </div>
  </div>
);

export default RelatedProducts;
