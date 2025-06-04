import { MainContext } from "@/context/MainContext";
import { useContext } from "react"

const useMainContext = () => {
    const context = useContext(MainContext);
    if (!context) {
        throw new Error('useAuthContext context is not inside of AuthProvider Tag');
    }
    return context;
};

export default useMainContext;