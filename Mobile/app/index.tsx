import { Image, ScrollView, Text, View } from "react-native";
import "../global.css";
import { SafeAreaView } from "react-native-safe-area-context";
import Loader from "@/components/Loader";
import CustomButton from "@/components/CustomButton";
import { router } from "expo-router";
import { images } from "../constants";
export default function Index() {
  return (
    <SafeAreaView className='bg-green-950 h-full'>
      {/* <Loader isLoading /> */}
      <ScrollView
        contentContainerStyle={{
          height: "100%",
        }}>
        <View className='w-full flex justify-center items-center h-full px-4'>
          <Image
            source={images.logo}
            className='w-[130px] h-[84px]'
            resizeMode='contain'
          />
          <View className='relative mt-5'>
            <Text className='text-3xl text-white font-bold text-center'>
              Passed SWD with
              <Text className='text-secondary-200'> 💕Cikenote💕</Text>
            </Text>
          </View>
          <Text className='text-sm font-pregular text-white mt-7 text-center'>
            Where Creativity Meets Innovation: Embark on a Journey of Unlimited
            Discovery with Cikenote
          </Text>
          <CustomButton
            title='Continue with Email'
            handlePress={() => router.push("/signIn")}
            containerStyles='w-full mt-7'
            textStyles={""}
            isLoading={false}
          />
        </View>
      </ScrollView>
    </SafeAreaView>
  );
}
