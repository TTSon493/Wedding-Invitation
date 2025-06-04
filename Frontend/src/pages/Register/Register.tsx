import { useEffect, useState } from "react";
import { yupResolver } from "@hookform/resolvers/yup";
import { Button } from "../../components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "../../components/ui/card";
import { Input } from "../../components/ui/input";
import { Label } from "../../components/ui/label";
import { useForm, Controller } from "react-hook-form";
import { schema, Schema } from "../../utils/rules";
import { useNavigate } from "react-router-dom";
import { RadioGroupForm } from "../../components/RadioGroup/RadioGroupForm";
import CountrySelector from "../../components/CountrySelector/CountrySelector";
import { COUNTRIES } from "../../lib/countries";
import { Country, SelectMenuOption } from "../../types/selectMenuType";
import useAuth from "../../hooks/useAuth";
import { toast } from "react-toastify";
import { PATH_PUBLIC } from "../../router/path";

type FormData = Schema;

const Register = () => {
  const [isOpen, setIsOpen] = useState(false);
  // const [setLoading] = useState<boolean>(false);
  // Default this to a country's code to preselect it
  const [country, setCountry] = useState<SelectMenuOption["title"]>("VN");
  const { signUpUser, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    if (isAuthenticated) {
      navigate(PATH_PUBLIC.home);
    }
  });

  const {
    control,
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: yupResolver(schema),
    defaultValues: {
      birthDate: new Date().toISOString().split("T")[0],
    },
  });

  const onSubmit = async (data: FormData) => {
    try {
      // setLoading(true);
      await signUpUser(data);
      console.log(data);
      // setLoading(false);
    } catch (error) {
      // setLoading(false);
      const err = error as { data: string; status: number };
      const { status } = err;
      if (status === 401) {
        toast.error("Invalid username or password");
      } else {
        toast.error("An error occurred. Please contact admins");
      }
    }
  };

  // Find the selected country or default to Vietnam
  const selectedCountry = COUNTRIES.find(
    (option: Country) => option.value === country
  ) || { value: "VN", title: "Vietnam" }; // Ensure a default value

  return (
    <div className='flex flex-col items-center justify-center min-h-screen bg-blue-50'>
      <main className='flex flex-col items-center justify-center flex-1 w-full px-4 py-8 space-y-8 md:flex-row md:space-y-0 md:space-x-8'>
        <div className='text-center md:text-left'>
          <h2 className='text-4xl font-bold text-yellow-600'>Welcome to SWD</h2>
          <p className='mt-2 text-gray-600'>
            Fight with all your might against capitalism
          </p>
        </div>
        <form className='w-full max-w-md' onSubmit={handleSubmit(onSubmit)}>
          <Card className='p-6 bg-white shadow-md'>
            <CardHeader>
              <CardTitle className='text-xl font-bold text-yellow-600'>
                Register
              </CardTitle>
              <CardDescription>Access your SWD account</CardDescription>
            </CardHeader>
            <CardContent className='space-y-4'>
              {/* Full Name Input */}
              <div className='space-y-1'>
                <Label htmlFor='name'>Full Name</Label>
                <Input
                  id='name'
                  placeholder='Enter your full name'
                  {...register("fullName")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.fullName?.message}
                </div>
              </div>

              {/* Email Input */}
              <div className='space-y-1'>
                <Label htmlFor='email'>Email</Label>
                <Input
                  id='email'
                  placeholder='Enter your email'
                  type='email'
                  {...register("email")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.email?.message}
                </div>
              </div>

              {/* Password Input */}
              <div className='space-y-1'>
                <Label htmlFor='password'>Password</Label>
                <Input
                  id='password'
                  autoComplete='on'
                  placeholder='Enter your password'
                  type='password'
                  {...register("password")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.password?.message}
                </div>
              </div>

              {/* Repeat Password Input */}
              <div className='space-y-1'>
                <Label htmlFor='repeat-password'>Repeat Password</Label>
                <Input
                  autoComplete='on'
                  id='repeat-password'
                  placeholder='Repeat your password'
                  type='password'
                  {...register("confirmPassword")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.confirmPassword?.message}
                </div>
              </div>

              {/* <div className='flex justify-center items-center'> */}
              {/* Birthdate Picker */}
              {/* <div className='space-y-1'>
                  <Label htmlFor='birth-date'>Birthdate</Label>
                  <div className='flex items-center'>
                    <Controller
                      name='birthDate'
                      control={control}
                      render={({ field: { onChange, value } }) => (
                        <input
                          type='date'
                          className='h-16 pr-5'
                          value={
                            value
                              ? new Date(value).toISOString().split("T")[0]
                              : ""
                          }
                          onChange={(e) => {
                            const dateValue = e.target.value;
                            onChange(
                              dateValue
                                ? new Date(dateValue).toISOString()
                                : null
                            );
                          }}
                        />
                      )}
                    />
                  </div>
                  <div className='text-red-500 text-sm'>
                    {errors.birthDate?.message}
                  </div>
                </div> */}

              {/* Gender Radio Group */}

              {/* </div> */}
              <div className='space-y-1'>
                <Label htmlFor='gender'>Gender</Label>
                <Controller
                  name='gender'
                  control={control}
                  render={({ field: { onChange, value } }) => (
                    <RadioGroupForm value={value} onChange={onChange} />
                  )}
                />
                <div className='text-red-500 text-sm'>
                  {errors.gender?.message}
                </div>
              </div>

              {/* Phone Number Input */}
              <div className='space-y-1'>
                <Label htmlFor='phone-number'>Phone Number</Label>
                <Input
                  id='phone-number'
                  placeholder='Enter your phone number'
                  {...register("phoneNumber")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.phoneNumber?.message}
                </div>
              </div>

              {/* Address Input */}
              <div className='space-y-1'>
                <Label htmlFor='address'>Address</Label>
                <Input
                  id='address'
                  placeholder='Enter your address'
                  {...register("address")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.address?.message}
                </div>
              </div>

              {/* Country Input */}
              <div className='space-y-1'>
                <Label htmlFor='country'>Country</Label>
                <Controller
                  name='country'
                  control={control}
                  render={({ field: { onChange, value } }) => (
                    <CountrySelector
                      id='country-selector'
                      open={isOpen}
                      value={
                        COUNTRIES.find(
                          (option: Country) => option.value === value
                        ) || selectedCountry // Ensure `selectedCountry` is valid
                      }
                      onChange={(
                        selectedCountry: SelectMenuOption["value"]
                      ) => {
                        onChange(selectedCountry); // Update form state
                        setCountry(selectedCountry); // Update local state
                      }}
                      onToggle={() => setIsOpen(!isOpen)}
                      selectedValue={selectedCountry} // Always provide a valid Country
                    />
                  )}
                />
                <div className='text-red-500 text-sm'>
                  {errors.country?.message}
                </div>
              </div>
            </CardContent>
            <CardFooter>
              <Button className='w-full' type='submit'>
                Register
              </Button>
            </CardFooter>
          </Card>
        </form>
      </main>
    </div>
  );
};

export default Register;
