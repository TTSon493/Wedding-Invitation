import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.tsx";
import AuthContextProvider from "./context/AuthContext";
import { ToastContainer } from "react-toastify";
import { GoogleOAuthProvider } from "@react-oauth/google";
import "react-toastify/dist/ReactToastify.css";
import { BrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import MainContextProvider from "./context/MainContext.tsx";
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
    },
  },
});
createRoot(document.getElementById("root")!).render(
  // <StrictMode>
  <BrowserRouter>
    <QueryClientProvider client={queryClient}>
      <AuthContextProvider>
        <MainContextProvider>
          <GoogleOAuthProvider clientId='1026307385705-9bjnte55me922s1okqr7okojtmdgdfhn.apps.googleusercontent.com'>
            <ToastContainer />
            <App />
          </GoogleOAuthProvider>
        </MainContextProvider>
      </AuthContextProvider>
    </QueryClientProvider>
  </BrowserRouter>
  // </StrictMode>
);
