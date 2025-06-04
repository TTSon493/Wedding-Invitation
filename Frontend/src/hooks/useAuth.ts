import { useContext } from "react"
import { AuthContext } from "../context/AuthContext";

const useAuth = () => {
    const context = useContext(AuthContext);
    if (!context) {
        throw new Error('useAuthContext context is not inside of AuthProvider Tag');
    }
    return context;
};

export default useAuth;