import { useState } from "react";
import { Href, Link, router } from "expo-router";
import { SafeAreaView } from "react-native-safe-area-context";
import {
  View,
  Text,
  Pressable,
  ScrollView,
  Dimensions,
  Alert,
  Image,
  Button,
} from "react-native";
import CustomButton from "@/components/CustomButton";
import { GoogleSignin } from "@react-native-google-signin/google-signin";
import FormField from "@/components/FormField";

import * as WebBrowser from "expo-web-browser";
import * as Google from "expo-auth-session/providers/google";
import { images } from "../../constants";
// import { CustomButton, FormField } from "../../components";
// import { getCurrentUser, signIn } from "../../lib/appwrite";
// import { useGlobalContext } from "../../context/GlobalProvider";
import { useAuth } from "@/context/AuthContext";
import AsyncStorage from "@react-native-async-storage/async-storage";
// import { GoogleSignin } from "@react-native-google-signin/google-signin";
// GoogleSignin.configure({
//   webClientId:
//     "1026307385705-9bjnte55me922s1okqr7okojtmdgdfhn.apps.googleusercontent.com",
//   scopes: ["profile", "email"],
// });

// android 1026307385705-o21rabvbguor7q4oq3uu24q9sodbmv8u.apps.googleusercontent.com
// web 1026307385705-9bjnte55me922s1okqr7okojtmdgdfhn.apps.googleusercontent.com

WebBrowser.maybeCompleteAuthSession();

const SignIn = () => {
  //   const { setUser, setIsLogged } = useGlobalContext();
  const { signIn, user, signInWithGoogleAuth } = useAuth();
  const [isSubmitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const [request, response, prompAsync] = Google.useAuthRequest({
    androidClientId:
      "1026307385705-o21rabvbguor7q4oq3uu24q9sodbmv8u.apps.googleusercontent.com",
    webClientId:
      "1026307385705-9bjnte55me922s1okqr7okojtmdgdfhn.apps.googleusercontent.com",
    iosClientId:
      "1026307385705-jbicvp9lkh0e7e2oaua7gpbr5oa4tdo3.apps.googleusercontent.com",
  });

  console.log("Response: ", response);
  console.log("Response: ", prompAsync);
  console.log("Response: ", request?.clientSecret);

  async function handleSignInWithGoogle() {
    if (!user) {
      if (response?.type === "success") {
        console.log(
          "response.authentication?.accessToken: ",
          response.authentication?.accessToken
        );
        if (response.authentication?.accessToken) {
          signInWithGoogleAuth(response.authentication?.accessToken);
        }
      }
    }
  }

  //   const GoogleLogin = async () => {
  //     await GoogleSignin.hasPlayServices();
  //     const userInfo = await GoogleSignin.getTokens();
  //     return userInfo;
  //   };

  //   const handleGoogleLogin = async () => {
  //     setLoading(true);
  //     try {
  //       const response = await GoogleLogin();
  //       const { idToken } = response;

  //       if (idToken) {
  //         const resp = await signInWithGoogle(idToken);
  //       }
  //     } catch (apiError) {
  //       setError("Something went wrong");
  //     } finally {
  //       setLoading(false);
  //     }
  //   };

  // GoogleSignin.configure({
  //   webClientId:
  //     "1026307385705-9bjnte55me922s1okqr7okojtmdgdfhn.apps.googleusercontent.com",
  //   iosClientId:
  //     "1026307385705-9l6id4ig1k6j8lu2l5262dc10qei54i8.apps.googleusercontent.com",
  // });

  // async function onGoogleButtonPress() {
  //   // Check if your device supports Google Play
  //   await GoogleSignin.hasPlayServices({ showPlayServicesUpdateDialog: true });
  //   // Get the users ID token
  //   // const { idToken, } = await GoogleSignin.signIn();
  //   const sInfo = await GoogleSignin.signIn();

  //   console.log("====================================");
  //   console.log("sInfo ", sInfo.data?.idToken);
  //   console.log("====================================");
  // }

  const submit = async () => {
    if (form.email === "" || form.password === "") {
      Alert.alert("Error", "Please fill in all fields");
    }

    if (user) {
      router.navigate("/home");
    }
    setSubmitting(true);

    // router.replace("/home");

    try {
      await signIn(form.email, form.password);
      //   const result = await getCurrentUser();
      //   setUser(result);
      //   setIsLogged(true);

      Alert.alert("Success", "User signed in successfully");
      router.replace("/home");
    } catch (error) {
      Alert.alert("Error", error);
    } finally {
      setSubmitting(false);
    }
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
            title='Sign In'
            handlePress={submit}
            containerStyles='mt-7'
            isLoading={isSubmitting}
            textStyles={""}
          />
          <Button
            title='Google Sign-In'
            onPress={() => handleSignInWithGoogle()}
          />
          {/* 
          <Pressable onPress={handleGoogleLogin}>
            <Text>Continue with Google</Text>
          </Pressable> */}

          <View className='flex justify-center pt-5 flex-row gap-2'>
            <Text className='text-lg text-gray-100 font-pregular'>
              Don't have an account?
            </Text>
            <Link href={"/signUp" as unknown as Href<string | object>}>
              <Text className='text-lg font-psemibold text-secondary'>
                Signup
              </Text>
            </Link>
          </View>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

export default SignIn;
