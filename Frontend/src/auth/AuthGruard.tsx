import useAuth from "@/hooks/useAuth";
import { Navigate, Outlet } from "react-router-dom";
import Spinner from "../components/Spinner/index";
import { PATH_PUBLIC } from "@/router/path";

// Interface for props
interface IProps {
  roles: string[];
}

const AuthGuard = ({ roles }: IProps) => {
  const { isAuthenticated, user, isAuthLoading } = useAuth();

  try {
    // Check if user.roles is an array, then find the role
    const hasAccess =
      isAuthenticated &&
      Array.isArray(user?.roles) &&
      user.roles.some((x) => roles.includes(x));

    if (isAuthLoading) {
      return <Spinner />;
    }

    if (user) {
      return hasAccess ? <Outlet /> : <Navigate to={PATH_PUBLIC.home} />;
    }

    // Optional: Add fallback if `user` is null/undefined
    // return <Navigate to={PATH_PUBLIC.home} />;
  } catch (error) {
    console.log("Error:", error);
    return <Navigate to={PATH_PUBLIC.unauthorized} />;
  }
};

export default AuthGuard;
