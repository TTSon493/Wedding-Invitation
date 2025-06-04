import { Link, useNavigate } from "react-router-dom";
import { Button } from "../../components/ui/button";
import { LoginSchema } from "../../utils/rules";
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
import { useForm } from "react-hook-form";
import { useEffect } from "react";
import useAuth from "../../hooks/useAuth";
import { ISignIn, ISignInByGoogleDTO } from "../../types/auth.type";
import { PATH_PUBLIC } from "../../router/path";
import { toast } from "react-toastify";
import { useGoogleLogin } from "@react-oauth/google";

type FormData = LoginSchema;

const Login = () => {
  // const [setLoading] = useState<boolean>(false);
  const { signInByEmailPassword, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({});

  // React.useEffect(() => {
  //   if (isSubmitSuccessful) {
  //     reset({
  //       email: "",
  //       password: "",
  //     });
  //   }
  // }, [isSubmitSuccessful, reset]);

  useEffect(() => {
    if (isAuthenticated) {
      navigate(PATH_PUBLIC.home);
    }
  });

  const onSubmit = async (data: ISignIn) => {
    try {
      // setLoading(true);
      await signInByEmailPassword(data);

      // setLoading(false);
      navigate(PATH_PUBLIC.home);
    } catch (error) {
      // setLoading(false);
      console.log(error);
      const err = error as { data: string; status: number };
      const { status } = err;
      if (status === 401) {
        toast.error("Invalid username or password");
      } else {
        toast.error("An error occurred. Please contact admins");
      }
    }
  };

  const { signInByGoogle } = useAuth();

  // const handleGoogle = async () => {
  //   try {
  //     const provider = await new GoogleAuthProvider();
  //     const result = await signInWithPopup(auth, provider);
  //     const idTokenResult = await result.user?.getIdTokenResult();
  //     const signInByGoogleDto: ISignInByGoogleDTO = {
  //       Token: idTokenResult.token,
  //     };

  //     console.log("idTokenResult.token: ", idTokenResult.token);
  //     await signInByGoogle(signInByGoogleDto);
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };

  // const handleGoogle = async () => {
  //   try {
  //     const provider = new GoogleAuthProvider();
  //     const result = await signInWithPopup(auth, provider);
  //     const idTokenResult = await result.user?.getIdToken();

  //     if (idTokenResult) {
  //       // Encode the token to be URL-safe
  //       console.log("idTokenResult: ", encodeURI(idTokenResult));

  //       // Use the encoded token in the DTO
  //       const signInByGoogleDto: ISignInByGoogleDTO = {
  //         Token: idTokenResult,
  //       };

  //       await signInByGoogle(signInByGoogleDto);
  //     }
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };

  const handleGoogle = useGoogleLogin({
    onSuccess: (tokenResponse) => {
      console.log("tokenResponse: ", tokenResponse.access_token);
      if (tokenResponse.access_token) {
        const signInByGoogleDto: ISignInByGoogleDTO = {
          Token: tokenResponse.access_token,
        };
        signInByGoogle(signInByGoogleDto);
      }
    },
  });

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
                Login
              </CardTitle>
              <CardDescription>Access your SWD account</CardDescription>
            </CardHeader>
            <CardContent className='space-y-4'>
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
              <div className='space-y-1'>
                <Label htmlFor='password'>Password</Label>
                <Input
                  autoComplete='on'
                  id='password'
                  placeholder='Enter your password'
                  type='password'
                  {...register("password")}
                />
                <div className='text-red-500 text-sm'>
                  {errors.password?.message}
                </div>
              </div>
            </CardContent>
            <CardFooter className='flex flex-col gap-4 mt-4'>
              {/* Login Button */}
              <Button
                type='submit'
                className='w-full py-2 text-white font-semibold rounded-lg shadow-md ocus:outline-none focus:ring-4 focus:ring-blue-300'>
                Login
              </Button>

              {/* Google Sign-in Button */}
              <button
                onClick={() => handleGoogle()}
                type='button'
                className='flex items-center justify-center w-full px-4 py-2 bg-[#1a1d22] text-white font-medium rounded-lg shadow-md transition-transform duration-300 ease-in-out hover:bg-[#2f333a] hover:scale-60 focus:outline-none focus:ring-4 focus:ring-[#4285F4]/50'>
                <svg
                  className='w-5 h-5 mr-2'
                  aria-hidden='true'
                  focusable='false'
                  data-prefix='fab'
                  data-icon='google'
                  role='img'
                  xmlns='http://www.w3.org/2000/svg'
                  viewBox='0 0 488 512'>
                  <path
                    fill='currentColor'
                    d='M488 261.8C488 403.3 391.1 504 248 504 110.8 504 0 393.2 0 256S110.8 8 248 8c66.8 0 123 24.5 166.3 64.9l-67.5 64.9C258.5 52.6 94.3 116.6 94.3 256c0 86.5 69.1 156.6 153.7 156.6 98.2 0 135-70.4 140.8-106.9H248v-85.3h236.1c2.3 12.7 3.9 24.9 3.9 41.4z'></path>
                </svg>
                Sign in with Google
              </button>
            </CardFooter>

            <div className='flex justify-center'>
              <p className='pr-2'>Don't have account?</p>
              <span className='text-yellow-600'>
                <Link to={PATH_PUBLIC.signUp}>Register</Link>
              </span>
            </div>
          </Card>
        </form>
      </main>
    </div>
  );
};

export default Login;
