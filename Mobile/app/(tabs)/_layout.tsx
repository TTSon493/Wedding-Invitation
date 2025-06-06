import { StatusBar } from "expo-status-bar";
import { Redirect, Tabs } from "expo-router";
import { Image, Text, View } from "react-native";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import { icons } from "../../constants";
const queryClient = new QueryClient();

interface PropsTabIcon {
  icon: any;
  color: string;
  name: string;
  focused: boolean;
}
const TabIcon = ({ icon, color, name, focused }: PropsTabIcon) => {
  return (
    <View className='flex items-center justify-center gap-2'>
      <Image
        source={icon}
        resizeMode='contain'
        tintColor={color}
        className='w-6 h-6'
      />
      <Text
        className={`${focused ? "font-psemibold" : "font-pregular"} text-xs`}
        style={{ color: color }}>
        {name}
      </Text>
    </View>
  );
};

const TabLayout = () => {
  //   const { loading, isLogged } = useGlobalContext();

  //   if (!loading && !isLogged) return <Redirect href='/sign-in' />;

  return (
    <>
      <QueryClientProvider client={queryClient}>
        <Tabs
          screenOptions={{
            tabBarActiveTintColor: "#73EC8B",
            tabBarInactiveTintColor: "#CDCDE0",
            tabBarShowLabel: false,
            tabBarStyle: {
              backgroundColor: "#1A1A19",
              borderTopWidth: 1,
              borderTopColor: "#1A1A19",
              height: 84,
            },
          }}>
          <Tabs.Screen
            name='home'
            options={{
              title: "Home",
              headerShown: false,
              tabBarIcon: ({ color, focused }) => (
                <TabIcon
                  icon={icons.home}
                  color={color}
                  name='Home'
                  focused={focused}
                />
              ),
            }}
          />
          <Tabs.Screen
            name='bookmark'
            options={{
              title: "Bookmark",
              headerShown: false,
              tabBarIcon: ({ color, focused }) => (
                <TabIcon
                  icon={icons.bookmark}
                  color={color}
                  name='Bookmark'
                  focused={focused}
                />
              ),
            }}
          />

          <Tabs.Screen
            name='create'
            options={{
              title: "Create",
              headerShown: false,
              tabBarIcon: ({ color, focused }) => (
                <TabIcon
                  icon={icons.plus}
                  color={color}
                  name='Create'
                  focused={focused}
                />
              ),
            }}
          />
          <Tabs.Screen
            name='profile'
            options={{
              title: "Profile",
              headerShown: false,
              tabBarIcon: ({ color, focused }) => (
                <TabIcon
                  icon={icons.profile}
                  color={color}
                  name='Profile'
                  focused={focused}
                />
              ),
            }}
          />
        </Tabs>

        {/* <Loader isLoading={loading} /> */}
        <StatusBar backgroundColor='#161622' style='light' />
      </QueryClientProvider>
    </>
  );
};

export default TabLayout;
