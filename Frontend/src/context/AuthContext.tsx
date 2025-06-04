import React, {
  createContext,
  ReactNode,
  useCallback,
  useEffect,
  useReducer,
} from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { PATH_PUBLIC } from "../router/path";
import {
  REFRESH_URL,
  SEND_VERIFY_EMAIL_URL,
  SIGN_IN_CUSTOMER_URL,
  GET_ALL_CUSTOMER_URL,
  SIGN_IN_GOOGLE_URL,
} from "../utils/apiUrl.utils";
import {
  IAuthContext,
  IAuthContextAction,
  IAuthContextActionTypes,
  IAuthContextState,
  IJwtToken,
  IResponse,
  ISignIn,
  ISignInResponse,
  ISignUpResponse,
  ISignUpPost,
  ISignInByGoogleDTO,
} from "../types/auth.type";
import { ICustomerInfo, ICustomerResponse } from "@/types/customer.type";
import http from "../utils/http";
import { getJwtTokenSession, setJwtTokenSession } from "./../utils/auth.utils";
import { getUserInfo, signUp } from "@/apis/auth.services";
import Spinner from "@/components/Spinner";

const authReducer = (
  state: IAuthContextState,
  action: IAuthContextAction
): IAuthContextState => {
  switch (action.type) {
    case IAuthContextActionTypes.SIGNIN:
      return {
        ...state,
        isAuthenticated: true,
        isFullInfo: true,
        isAuthLoading: false,
        user: action.payload,
      };
    case IAuthContextActionTypes.SIGNINBYGOOGLE:
      return {
        ...state,
        isAuthenticated: true,
        isFullInfo: false,
        isAuthLoading: false,
        user: action.payload,
      };
    case IAuthContextActionTypes.SIGNOUT:
      return {
        ...state,
        isAuthenticated: false,
        isFullInfo: false,
        isAuthLoading: false,
        user: undefined,
      };
    default:
      return state;
  }
};

export const AuthContext = createContext<IAuthContext | null>(null);

const initialAuthState: IAuthContextState = {
  isAuthenticated: false,
  isFullInfo: false,
  isAuthLoading: true,
  user: undefined,
};

interface IProps {
  children: ReactNode;
}

const AuthContextProvider: React.FC<IProps> = ({ children }) => {
  const [state, dispatch] = useReducer(authReducer, initialAuthState);
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const isTokenValid = useCallback((token: string | null): boolean => {
    if (!token) return false;
    try {
      const decodedToken = jwtDecode<{ exp: number }>(token);
      return decodedToken.exp > Date.now() / 1000;
    } catch {
      return false;
    }
  }, []);

  const refreshTokenMutation = useMutation({
    mutationFn: async (token: {
      accessToken: string;
      refreshToken: string;
    }) => {
      const response = await http.post<IJwtToken>(REFRESH_URL, token);
      return response.data;
    },
    onSuccess: (data) => {
      if (!data.isSuccess) {
        throw new Error(data.message);
      }
      const { accessToken, refreshToken } = data.result;
      setJwtTokenSession(accessToken, refreshToken);
      http.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
      queryClient.invalidateQueries({ queryKey: ["userInfo"] });
    },
    onError: () => {
      dispatch({ type: IAuthContextActionTypes.SIGNOUT });
      navigate(PATH_PUBLIC.signIn);
    },
  });

  const {
    data: userInfo,
    isLoading: isUserInfoLoading,
    isError: isUserInfoError,
    error: userInfoError,
  } = useQuery<ICustomerInfo>({
    queryKey: ["userInfo"],
    queryFn: async () => {
      const result = await getUserInfo();
      if (result === undefined) {
        throw new Error("User info not found");
      }
      return result;
    },
    enabled: state.isAuthenticated && !state.user,
    retry: false,
  });

  useEffect(() => {
    if (userInfo) {
      dispatch({
        type: IAuthContextActionTypes.SIGNIN,
        payload: userInfo,
      });
    }
  }, [userInfo]);

  useEffect(() => {
    if (isUserInfoError) {
      console.error("Error fetching user info:", userInfoError);
      dispatch({ type: IAuthContextActionTypes.SIGNOUT });
      navigate(PATH_PUBLIC.signIn);
    }
  }, [isUserInfoError, userInfoError, navigate]);

  const initializeAuthContext = useCallback(async () => {
    const { refreshToken, accessToken } = getJwtTokenSession();
    if (refreshToken && accessToken) {
      if (!isTokenValid(accessToken)) {
        await refreshTokenMutation.mutateAsync({ accessToken, refreshToken });
      } else {
        http.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
        queryClient.invalidateQueries({ queryKey: ["userInfo"] });
      }
      dispatch({ type: IAuthContextActionTypes.SIGNIN, payload: undefined });
    } else {
      dispatch({ type: IAuthContextActionTypes.SIGNOUT });
    }
  }, [refreshTokenMutation, queryClient, isTokenValid]);

  useEffect(() => {
    initializeAuthContext();
  }, []);

  const signInByGoogle = useMutation<
    ISignInResponse,
    Error,
    ISignInByGoogleDTO
  >({
    mutationFn: async (signInByGoogleDTO) => {
      const response = await http.post<ISignInResponse>(
        SIGN_IN_GOOGLE_URL,
        signInByGoogleDTO
      );
      return response.data;
    },
    onSuccess: (data) => {
      if (data.isSuccess) {
        const { accessToken, refreshToken } = data.result;
        http.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
        setJwtTokenSession(accessToken, refreshToken);
        queryClient.invalidateQueries({ queryKey: ["userInfo"] });
        toast.success("Sign in was successful");
      } else {
        toast.error(data.message);
      }
      navigate(PATH_PUBLIC.home);
    },
    onError: () => {
      toast.error("Sign in failed with Google");
    },
  });

  const signUpMutation = useMutation<ISignUpResponse, Error, ISignUpPost>({
    mutationFn: signUp,
    onSuccess: async (data, variables) => {
      if (data.isSuccess) {
        toast.success(data.message);
        const emailToSend = { email: variables.email };
        const response = await http.post<IResponse<string>>(
          SEND_VERIFY_EMAIL_URL,
          emailToSend
        );
        if (response.data.isSuccess) {
          toast.success(response.data.message);
        }
        navigate(PATH_PUBLIC.signIn);
      } else {
        toast.error(data.message);
      }
    },
    onError: () => {
      toast.error("Something went wrong during sign up");
    },
  });

  const signInMutation = useMutation<ISignInResponse, Error, ISignIn>({
    mutationFn: async (signIn: ISignIn) => {
      const response = await http.post<ISignInResponse>(
        SIGN_IN_CUSTOMER_URL,
        signIn
      );
      return response.data;
    },
    onSuccess: (data) => {
      if (data.isSuccess) {
        const { accessToken, refreshToken } = data.result;
        http.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
        setJwtTokenSession(accessToken, refreshToken);
        queryClient.invalidateQueries({ queryKey: ["userInfo"] });
        toast.success("Sign in was successful");
      } else {
        toast.error(data.message);
      }
    },
    onError: () => {
      toast.error("Sign in failed");
    },
  });

  const signOutUser = useCallback(() => {
    setJwtTokenSession(null, null);
    toast.success("Logout successful!");
    dispatch({ type: IAuthContextActionTypes.SIGNOUT });
    queryClient.clear();
    navigate(PATH_PUBLIC.signIn);
  }, [navigate, queryClient]);

  const { data: allCustomers, isLoading: isAllCustomersLoading } = useQuery({
    queryKey: ["allCustomers"],
    queryFn: async () => {
      const response = await http.get<IResponse<ICustomerResponse[] | null>>(
        GET_ALL_CUSTOMER_URL
      );
      return response.data.result;
    },
    enabled: state.isAuthenticated,
  });

  const contextValue: IAuthContext = {
    isAuthenticated: state.isAuthenticated,
    isAuthLoading:
      state.isAuthLoading ||
      isUserInfoLoading ||
      signInMutation.isPending ||
      signUpMutation.isPending ||
      isAllCustomersLoading,
    isFullInfo: state.isFullInfo,
    user: state.user,
    signUpUser: signUpMutation.mutate,
    signInByGoogle: signInByGoogle.mutate,
    signOutUser,
    getCustomerAll: (): ICustomerResponse[] | null | undefined => allCustomers,
    signInByEmailPassword: signInMutation.mutate,
  };

  if (contextValue.isAuthLoading) {
    return <Spinner />;
  }

  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export default AuthContextProvider;
