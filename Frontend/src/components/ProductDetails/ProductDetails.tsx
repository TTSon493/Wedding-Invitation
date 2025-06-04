import { useState } from "react";
import { Button } from "../../components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "../../components/ui/select";
import { Link } from "react-router-dom";
import { PATH_PUBLIC } from "../../router/path";

const ProductDetails = ({ activeComponent }: { activeComponent: string }) => {
  const [selectedColor, setSelectedColor] = useState<string | null>(null);
  const [eventType, setEventType] = useState<string>("personal");

  const colors = [
    { name: "Green", class: "bg-green-500" },
    { name: "Blue", class: "bg-blue-500" },
    { name: "Purple", class: "bg-purple-500" },
  ];

  const renderComponent = () => {
    switch (activeComponent) {
      case "card":
        return (
          <>
            <h1 className='text-3xl font-bold mb-4'>
              Watercolor Floral Garland - Wedding Invitation
            </h1>
            <p className='text-xl mb-4'>Starting at 2 Colors $0.94/ea</p>

            {/* Color Options */}
            <div className='mb-6'>
              <h2 className='font-semibold mb-2'>Color</h2>
              <div className='flex space-x-2'>
                {colors.map((color) => (
                  <button
                    key={color.name}
                    className={`w-8 h-8 ${
                      color.class
                    } rounded-full focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 ${
                      selectedColor === color.name
                        ? "ring-2 ring-offset-2 ring-blue-500"
                        : ""
                    }`}
                    onClick={() => setSelectedColor(color.name)}
                    aria-label={`Select ${color.name} color`}
                  />
                ))}
              </div>
            </div>

            {/* Event Type Selector */}
            <div className='mb-6'>
              <h2 className='font-semibold mb-2'>
                What kind of event are you having?
              </h2>
              <Select value={eventType} onValueChange={setEventType}>
                <SelectTrigger className='w-full'>
                  <SelectValue placeholder='Select event type' />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value='personal'>Personal</SelectItem>
                  <SelectItem value='wedding'>Wedding</SelectItem>
                  <SelectItem value='birthday'>Birthday</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <Button className='w-full mb-4'>
              <Link to={PATH_PUBLIC.cardDesign}>Customize</Link>
            </Button>

            {/* Product Description */}
            <p className='text-gray-600 mb-6'>
              Get ready to host and toast to the happy couple with this charming
              and sweet wedding invitation. A watercolor garland border gives a
              nod to nature and can be customized in either green or blue. The
              modern text fits colorful florals, simple classic designs, and is
              perfect for any style wedding celebration.
            </p>

            {/* Features */}
            <ul className='space-y-2'>
              {[
                "Customize everything from your Card to the envelope, including paper, colors, monogram, and text boxes.",
                "Add Photos that link to your wedding website and registry, add a schedule of events, and share accommodation details.",
                "Send your save-the-dates, test, or final—and create an easy-to-share Custom URL.",
              ].map((feature, index) => (
                <li key={index} className='flex items-center'>
                  <svg
                    className='w-5 h-5 mr-2 text-green-500'
                    fill='none'
                    stroke='currentColor'
                    viewBox='0 0 24 24'
                    xmlns='http://www.w3.org/2000/svg'>
                    <path
                      strokeLinecap='round'
                      strokeLinejoin='round'
                      strokeWidth='2'
                      d='M5 13l4 4L19 7'
                    />
                  </svg>
                  {feature}
                </li>
              ))}
            </ul>
          </>
        );
      case "preview":
        return (
          <div className='text-center'>
            <h2 className='text-2xl font-bold mb-4'>Preview Your Invitation</h2>
            <img
              src='/placeholder.svg?height=400&width=600'
              alt='Invitation Preview'
              width={600}
              height={400}
              className='rounded-lg mx-auto'
            />
            <p className='mt-4 text-gray-600'>
              This is how your invitation will look. You can make changes in the
              Card section.
            </p>
          </div>
        );
      case "registry":
        return (
          <div>
            <h2 className='text-2xl font-bold mb-4'>Gift Registry</h2>
            <p className='mb-4'>Create and manage your gift registry here.</p>
            <Button>Add Registry Items</Button>
          </div>
        );
      case "guestManager":
        return (
          <div>
            <h2 className='text-2xl font-bold mb-4'>Guest Manager</h2>
            <p className='mb-4'>Manage your guest list and RSVPs here.</p>
            <Button>Add Guests</Button>
          </div>
        );
      case "guestSurvey":
        return (
          <div>
            <h2 className='text-2xl font-bold mb-4'>Guest Survey</h2>
            <p className='mb-4'>
              Create a survey to gather information from your guests.
            </p>
            <Button>Create Survey</Button>
          </div>
        );
      case "customDesign":
        return (
          <div>
            <h2 className='text-2xl font-bold mb-4'>Custom Design</h2>
            <p className='mb-4'>
              Customize your invitation design with our tools.
            </p>
            <Button>Start Designing</Button>
          </div>
        );
      default:
        return null;
    }
  };

  return <div>{renderComponent()}</div>;
};

export default ProductDetails;
