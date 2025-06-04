import { useState } from "react";
import { Href, Link, router } from "expo-router";
import { SafeAreaView } from "react-native-safe-area-context";
import { View, Text, ScrollView, Dimensions, Alert, Image } from "react-native";
import FormField from "@/components/FormField";
import CustomButton from "@/components/CustomButton";

import { images } from "../../constants";
// import { createUser } from "../../lib/appwrite";
// import { CustomButton, FormField } from "../../components";
// import { useGlobalContext } from "../../context/GlobalProvider";

const SignUp = () => {
  const [isSubmitting, setSubmitting] = useState(false);
  const [form, setForm] = useState({
    username: "",
    email: "",
    password: "",
  });

  const submit = async () => {
    if (form.username === "" || form.email === "" || form.password === "") {
      Alert.alert("Error", "Please fill in all fields");
    }

    setSubmitting(true);
    // try {
    //   const result = await createUser(form.email, form.password, form.username);
    //   setUser(result);
    //   setIsLogged(true);

    //   router.replace("/home");
    // } catch (error) {
    //   Alert.alert("Error", error.message);
    // } finally {
    //   setSubmitting(false);
    // }
  };

  return (
    <SafeAreaView className='bg-primary h-full'>
      <ScrollView>
        <View
          className='w-full flex justify-center h-full px-4 my-6'
          style={{
            minHeight: Dimensions.get("window").height - 100,
          }}>
          <View className='flex items-center'>
            <Image
              source={images.logo}
              resizeMode='contain'
              className='w-[120px] h-[50px]'
            />
            <Text className='flex text-2xl text-center font-semibold text-white mt-5 font-psemibold'>
              Log in to SWD
            </Text>
          </View>

          <FormField
            title='Username'
            value={form.username}
            handleChangeText={(e) => setForm({ ...form, username: e })}
            otherStyles='mt-10'
          />

          <FormField
            title='Email'
            value={form.email}
            handleChangeText={(e) => setForm({ ...form, email: e })}
            otherStyles='mt-7'
            keyboardType='email-address'
          />

          <FormField
            title='Password'
            value={form.password}
            handleChangeText={(e) => setForm({ ...form, password: e })}
            otherStyles='mt-7'
          />

          <CustomButton
            title='Sign Up'
            handlePress={submit}
            containerStyles='mt-7'
            isLoading={isSubmitting}
            textStyles={""}
          />

          <View className='flex justify-center pt-5 flex-row gap-2'>
            <Text className='text-lg text-gray-100 font-pregular'>
              Have an account already?
            </Text>
            <Link href={"/signIn" as unknown as Href<string | object>}>
              <Text className='text-lg font-psemibold text-secondary'>
                Sign In
              </Text>
            </Link>
          </View>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

export default SignUp;
