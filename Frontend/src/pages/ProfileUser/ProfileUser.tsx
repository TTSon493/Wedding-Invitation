
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useMutation, useQuery } from "@tanstack/react-query";
import { Controller, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import useAuth from "@/hooks/useAuth";
import { PATH_PUBLIC } from "@/router/path";
import { getUserInfo, updateCustomerInfo } from "@/apis/auth.services";
import { COUNTRIES } from "@/lib/countries";
import { schema, Schema } from "../../utils/rules";
import { Country, SelectMenuOption } from "@/types/selectMenuType";
import CountrySelector from "@/components/CountrySelector/CountrySelector";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { IUpdateCustomer } from "@/types/customer.type";
import { toast } from "react-toastify";

type FormData = Schema;

type PasswordFormData = {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
};

export default function Component() {
  const { isAuthenticated, user } = useAuth();
  const [isOpen, setIsOpen] = useState(false);
  const [country, setCountry] = useState<SelectMenuOption["title"]>(
    user?.country || ""
  );
  const [isEditing, setIsEditing] = useState(false);

  const navigate = useNavigate();
  // const queryClient = useQueryClient();

  const {
    control,
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<FormData>({
    resolver: yupResolver(schema),
    defaultValues: {
      email: user?.email || "",
      fullName: user?.fullName || "",
      country: user?.country || "",
      address: user?.address || "",
    },
  });

  const {
    register: registerPassword,
    handleSubmit: handleSubmitPassword,
    formState: { errors: passwordErrors },
    reset: resetPassword,
  } = useForm<PasswordFormData>();

  const { data: customerInfo, isSuccess } = useQuery({
    queryKey: ["customerInfo"],
    queryFn: getUserInfo,
    enabled: isAuthenticated,
  });

  const updateCustomerMutation = useMutation({
    mutationFn: updateCustomerInfo,
    onSuccess: () => {
      // queryClient.invalidateQueries(["customerInfo"]);
      toast.success("Profile updated successfully");
      setIsEditing(false);
    },
    onError: (error) => {
      console.error("Update failed:", error);
      toast.error("Failed to update profile. Please try again.");
    },
  });

  useEffect(() => {
    if (!isAuthenticated) {
      navigate(PATH_PUBLIC.signIn);
    }
  }, [isAuthenticated, navigate]);

  useEffect(() => {
    if (isSuccess && customerInfo) {
      setCountry(customerInfo.country);
      reset(customerInfo);
    }
  }, [isSuccess, customerInfo, reset]);

  const selectedCountry = COUNTRIES.find(
    (option: Country) => option.value === country
  ) || { value: "VN", title: "Vietnam" };

  const onSubmit = (data: FormData) => {
    const updateData: IUpdateCustomer = {
      fullName: data.fullName,
      email: data.email,
      country: data.country,
      address: data.address,
      customerId: "",
      birthDate: "",
      gender: "",
    };
    updateCustomerMutation.mutate(updateData);
  };

  const onPasswordSubmit = (data: PasswordFormData) => {
    console.log("Password change data:", data);
    // Implement password change logic here
    toast.success("Password changed successfully");
    resetPassword();
  };

  return (
    <div className='flex flex-col w-full min-h-screen bg-muted/40'>
      <main className='flex flex-1 flex-col gap-4 p-4 md:gap-8 md:p-10'>
        <div className='grid md:grid-cols-[250px_1fr] lg:grid-cols-[300px_1fr] items-start gap-6 max-w-6xl w-full mx-auto'>
          <div className='grid gap-6'>
            <Card>
              <CardContent className='flex flex-col items-center gap-4 p-6'>
                <Avatar className='h-24 w-24'>
                  <AvatarImage src='/placeholder-user.jpg' alt='@shadcn' />
                  <AvatarFallback>Cikenote</AvatarFallback>
                </Avatar>
                <div className='grid gap-1 text-center'>
                  <div className='text-xl font-semibold'>{user?.fullName}</div>
                  <div className='text-sm text-muted-foreground'>
                    {user?.email}
                  </div>
                </div>
              </CardContent>
            </Card>
            <Card>
              <CardContent className='grid gap-4'>
                <div className='grid gap-1 mt-5'>
                  <div className='text-sm font-medium'>Gender</div>
                  <div className='text-sm text-muted-foreground'>
                    {user?.gender}
                  </div>
                </div>
                <div className='grid gap-1'>
                  <div className='text-sm font-medium'>Country</div>
                  <div className='text-sm text-muted-foreground'>
                    {user?.country}
                  </div>
                </div>
                <div className='grid gap-1'>
                  <div className='text-sm font-medium'>Address</div>
                  <div className='text-sm text-muted-foreground'>
                    {user?.address}
                  </div>
                </div>
                <div className='grid gap-1'>
                  <div className='text-sm font-medium'>Bio</div>
                  <div className='text-sm text-muted-foreground'>
                    I'm a software engineer and I love to code!
                  </div>
                </div>
              </CardContent>
              <CardFooter className='border-t p-4'>
                <Button
                  variant='outline'
                  className='w-full'
                  onClick={() => setIsEditing(true)}>
                  Edit
                </Button>
              </CardFooter>
            </Card>
            <Card>
              <CardContent className='grid gap-4'>
                <AlertDialog>
                  <AlertDialogTrigger asChild>
                    <Button variant='destructive' className='w-full'>
                      Delete Account
                    </Button>
                  </AlertDialogTrigger>
                  <AlertDialogContent>
                    <AlertDialogHeader>
                      <AlertDialogTitle>
                        Are you sure you want to delete your account?
                      </AlertDialogTitle>
                      <AlertDialogDescription>
                        This action cannot be undone. This will permanently
                        delete your account and remove your data from our
                        servers.
                      </AlertDialogDescription>
                    </AlertDialogHeader>
                    <AlertDialogFooter>
                      <AlertDialogCancel>Cancel</AlertDialogCancel>
                      <AlertDialogAction>Continue</AlertDialogAction>
                    </AlertDialogFooter>
                  </AlertDialogContent>
                </AlertDialog>
              </CardContent>
            </Card>
          </div>
          <div className='grid gap-6'>
            <Tabs defaultValue='profile' className='w-full'>
              <TabsList className='border-b'>
                <TabsTrigger value='profile'>Profile</TabsTrigger>
                <TabsTrigger value='settings'>Settings</TabsTrigger>
              </TabsList>
              <TabsContent value='profile' className='pt-6'>
                <Card>
                  <CardHeader>
                    <CardTitle>Personal Information</CardTitle>
                    <CardDescription>
                      Update your personal information.
                    </CardDescription>
                  </CardHeader>
                  <form onSubmit={handleSubmit(onSubmit)}>
                    <CardContent className='grid gap-4'>
                      <div className='grid gap-2'>
                        <Label htmlFor='fullName'>Name</Label>
                        <Input
                          id='fullName'
                          {...register("fullName")}
                          disabled={!isEditing}
                        />
                        {errors.fullName && (
                          <p className='text-red-500 text-sm'>
                            {errors.fullName.message}
                          </p>
                        )}
                      </div>
                      <div className='grid gap-2'>
                        <Label htmlFor='email'>Email</Label>
                        <Input
                          id='email'
                          type='email'
                          {...register("email")}
                          disabled={!isEditing}
                        />
                        {errors.email && (
                          <p className='text-red-500 text-sm'>
                            {errors.email.message}
                          </p>
                        )}
                      </div>
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
                                ) || selectedCountry
                              }
                              onChange={(
                                selectedCountry: SelectMenuOption["value"]
                              ) => {
                                onChange(selectedCountry);
                                setCountry(selectedCountry);
                              }}
                              onToggle={() => setIsOpen(!isOpen)}
                              selectedValue={selectedCountry}
                              disabled={!isEditing}
                            />
                          )}
                        />
                        {errors.country && (
                          <p className='text-red-500 text-sm'>
                            {errors.country.message}
                          </p>
                        )}
                      </div>
                      <div className='grid gap-2'>
                        <Label htmlFor='address'>Address</Label>
                        <Input
                          id='address'
                          {...register("address")}
                          disabled={!isEditing}
                        />
                        {errors.address && (
                          <p className='text-red-500 text-sm'>
                            {errors.address.message}
                          </p>
                        )}
                      </div>
                    </CardContent>
                    <CardFooter className='border-t p-4'>
                      {isEditing ? (
                        <>
                          <Button type='submit' className='ml-auto'>
                            Save
                          </Button>
                          <Button
                            variant='outline'
                            onClick={() => {
                              setIsEditing(false);
                              reset(customerInfo);
                            }}
                            className='ml-2'>
                            Cancel
                          </Button>
                        </>
                      ) : (
                        <Button
                          onClick={() => setIsEditing(true)}
                          className='ml-auto'>
                          Edit
                        </Button>
                      )}
                    </CardFooter>
                  </form>
                </Card>
              </TabsContent>
              <TabsContent value='settings' className='pt-6'>
                <Card>
                  <CardHeader>
                    <CardTitle>Change Password</CardTitle>
                    <CardDescription>
                      Update your account password here.
                    </CardDescription>
                  </CardHeader>
                  <form onSubmit={handleSubmitPassword(onPasswordSubmit)}>
                    <CardContent className='grid gap-4'>
                      <div className='grid gap-2'>
                        <Label htmlFor='current-password'>
                          Current Password
                        </Label>
                        <Input
                          id='current-password'
                          type='password'
                          {...registerPassword("currentPassword", {
                            required: "Current password is required",
                          })}
                        />
                        {passwordErrors.currentPassword && (
                          <p className='text-red-500 text-sm'>
                            {passwordErrors.currentPassword.message}
                          </p>
                        )}
                      </div>
                      <div className='grid gap-2'>
                        <Label htmlFor='new-password'>New Password</Label>
                        <Input
                          id='new-password'
                          type='password'
                          {...registerPassword("newPassword", {
                            required: "New password is required",
                            minLength: {
                              value: 8,
                              message: "Password must be at least 8 characters",
                            },
                          })}
                        />
                        {passwordErrors.newPassword && (
                          <p className='text-red-500 text-sm'>
                            {passwordErrors.newPassword.message}
                          </p>
                        )}
                      </div>
                      <div className='grid gap-2'>
                        <Label htmlFor='confirm-password'>
                          Confirm Password
                        </Label>
                        <Input
                          id='confirm-password'
                          type='password'
                          {...registerPassword("confirmPassword", {
                            required: "Please confirm your password",
                            validate: (value, formValues) =>
                              value === formValues.newPassword ||
                              "Passwords do not match",
                          })}
                        />
                        {passwordErrors.confirmPassword && (
                          <p className='text-red-500 text-sm'>
                            {passwordErrors.confirmPassword.message}
                          </p>
                        )}
                      </div>
                    </CardContent>
                    <CardFooter className='border-t p-4'>
                      <Button type='submit' className='ml-auto'>
                        Change Password
                      </Button>
                    </CardFooter>
                  </form>
                </Card>
              </TabsContent>
            </Tabs>
          </div>
        </div>
      </main>
    </div>
  );
}
