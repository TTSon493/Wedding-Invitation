import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import { Button } from "../ui/button";

const Pricing = () => {
  const plans = [
    {
      name: "Basic",
      price: "Free",
      features: [
        "5 card designs per month",
        "Basic templates",
        "Standard support",
      ],
      buttonText: "Get Started",
    },
    {
      name: "Pro",
      price: "$9.99/month",
      features: [
        "Unlimited card designs",
        "Premium templates",
        "Priority support",
        "Custom stickers",
      ],
      buttonText: "Upgrade to Pro",
    },
    {
      name: "Enterprise",
      price: "Custom",
      features: [
        "All Pro features",
        "Custom templates",
        "Dedicated support",
        "API access",
      ],
      buttonText: "Contact Sales",
    },
  ];
  return (
    <div className='container mx-auto px-4 py-8'>
      <h1 className='text-3xl font-bold mb-8 text-center'>Choose Your Plan</h1>
      <div className='grid grid-cols-1 md:grid-cols-3 gap-8'>
        {plans.map((plan) => (
          <Card key={plan.name} className='flex flex-col'>
            <CardHeader>
              <CardTitle className='text-2xl font-bold'>{plan.name}</CardTitle>
            </CardHeader>
            <CardContent className='flex-grow'>
              <p className='text-3xl font-bold mb-4'>{plan.price}</p>
              <ul className='space-y-2 mb-6'>
                {plan.features.map((feature, index) => (
                  <li key={index} className='flex items-center'>
                    <svg
                      className='w-4 h-4 mr-2 text-green-500'
                      fill='none'
                      strokeLinecap='round'
                      strokeLinejoin='round'
                      strokeWidth='2'
                      viewBox='0 0 24 24'
                      stroke='currentColor'>
                      <path d='M5 13l4 4L19 7'></path>
                    </svg>
                    {feature}
                  </li>
                ))}
              </ul>
            </CardContent>
            <div className='p-6 mt-auto'>
              <Button className='w-full'>{plan.buttonText}</Button>
            </div>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default Pricing;
