import { IResponse } from "@/types/auth";
import {
  getCustomerInfo,
  signIn,
  signInWithGoogle,
  signUp,
} from "@/utils/auth";
import http, { getJwtTokenSession, setJwtTokenSession } from "@/utils/http";
import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  ReactNode,
} from "react";
import { Alert } from "react-native";

interface AuthContextType {
  isAuthenticated: boolean;
  isLoading: boolean;
  user: any | null;
  signIn: (email: string, password: string) => Promise<void>;
  signUp: (userData: any) => Promise<void>;
  signInWithGoogleAuth: (token: string) => Promise<void>;
  signOut: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export const AuthProvider: React.FC<{ children: ReactNode }> = ({
  children,
}) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [user, setUser] = useState<any | null>(null);

  useEffect(() => {
    checkAuthStatus();
  }, []);

  const checkAuthStatus = async () => {
    try {
      const { accessToken } = await getJwtTokenSession();
      if (accessToken) {
        const userInfo = await getCustomerInfo();
        console.log("User info: " + userInfo);
        setUser(userInfo);
        setIsAuthenticated(true);
      }
    } catch (error) {
      console.error("Error checking auth status:", error);
    } finally {
      setIsLoading(false);
    }
  };

  const signInUser = async (email: string, password: string) => {
    setIsLoading(true);
    try {
      const response = await signIn({ email, password });
      if (response.isSuccess) {
        const { accessToken, refreshToken } = response.result;
        await setJwtTokenSession(accessToken, refreshToken);
      }
      setIsAuthenticated(true);
    } catch (error) {
      Alert.alert("Error", "Invalid credentials");
    } finally {
      setIsLoading(false);
    }
  };

  const signInWithGoogleAuth = async (token: string) => {
    setIsLoading(true);
    try {
      const response = await signInWithGoogle(token);
      if (response.isSuccess === true) {
        const { accessToken, refreshToken } = response.result;
        await setJwtTokenSession(accessToken, refreshToken);
      }
      setIsAuthenticated(true);
    } catch (error) {
      Alert.alert("Error", "Invalid credentials");
    } finally {
      setIsLoading(false);
    }
  };

  const signUpUser = async (userData: any) => {
    setIsLoading(true);
    try {
      await signUp(userData);
      Alert.alert(
        "Success",
        "Account created successfully. Please check your email for verification."
      );
    } catch (error) {
      Alert.alert("Error", "Failed to create account");
    } finally {
      setIsLoading(false);
    }
  };

  // const signInWithGoogleUser = async (token: string) => {
  //   setIsLoading(true);
  //   try {
  //     const response = await signInWithGoogle(token);
  //     await setJwtTokenSession(response.accessToken, response.refreshToken);
  //     setUser(response.user);
  //     setIsAuthenticated(true);
  //   } catch (error) {
  //     Alert.alert("Error", "Failed to sign in with Google");
  //   } finally {
  //     setIsLoading(false);
  //   }
  // };

  const signOutUser = async () => {
    await setJwtTokenSession(null, null);
    setUser(null);
    setIsAuthenticated(false);
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        isLoading,
        user,
        signIn: signInUser,
        signUp: signUpUser,
        signInWithGoogleAuth: signInWithGoogleAuth,
        signOut: signOutUser,
      }}>
      {children}
    </AuthContext.Provider>
  );
};
