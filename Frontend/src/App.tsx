import { ThemeProvider } from "./context/ThemeContext.tsx";
// import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import GlobalRouter from "./router/index.tsx";

// const queryClient = new QueryClient({
//   defaultOptions: {
//     queries: {
//       refetchOnWindowFocus: false,
//     },
//   },
// });
function App() {
  return (
    <>
      {/* <QueryClientProvider client={queryClient}> */}
      <ThemeProvider defaultTheme='light' storageKey='vite-ui-theme'>
        <ReactQueryDevtools initialIsOpen={false} />
        <GlobalRouter />
        {/* <RouterProvider router={router} /> */}
      </ThemeProvider>
      {/* </QueryClientProvider> */}
    </>
  );
}

export default App;
