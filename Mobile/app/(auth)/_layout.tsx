import { Redirect, Stack } from "expo-router";
import { StatusBar } from "expo-status-bar";

const AuthLayout = () => {
  //   const { loading, isLogged } = useGlobalContext();

  //   if (!loading && isLogged) return <Redirect href='/home' />;

  return (
    <>
      <Stack>
        <Stack.Screen
          name='signIn'
          options={{
            headerShown: false,
          }}
        />
        <Stack.Screen
          name='signUp'
          options={{
            headerShown: false,
          }}
        />
      </Stack>

      {/* <Loader isLoading={loading} /> */}
      <StatusBar backgroundColor='#161622' style='light' />
    </>
  );
};

export default AuthLayout;
