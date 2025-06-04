import React, { useCallback } from "react";
import { SafeAreaView } from "react-native-safe-area-context";
import {
  FlatList,
  RefreshControl,
  Text,
  View,
  TouchableOpacity,
  Image,
  Dimensions,
} from "react-native";
import { useAuth } from "@/context/AuthContext";
import { useInvitationTemplate } from "@/context/APIContext";

const { width } = Dimensions.get("window");
const cardWidth = width * 0.9;

interface Template {
  templateId: string;
  templateName: string;
  description: string;
  backgroundImageUrl: string[];
  textColor: string[];
  textFont: string[];
}

const Home = () => {
  const { isAuthenticated, user } = useAuth();
  const {
    templates: invitationTemplates = [],
    isLoading: isLoadingTemplates,
    fetchTemplates: refetchTemplates,
  } = useInvitationTemplate();

  // Map invitationTemplates to ensure each item has Template properties
  const formattedTemplates: Template[] =
    invitationTemplates?.map((item) => ({
      templateId: item.templateId,
      templateName: item.templateName,
      description: item.description,
      backgroundImageUrl: item.backgroundImageUrl,
      textColor: item.textColor,
      textFont: item.textFont,
    })) || [];

  const renderItem = useCallback(
    ({ item }: { item: Template }) => (
      <TouchableOpacity
        className='bg-white rounded-2xl shadow-lg mb-6 overflow-hidden'
        style={{ width: cardWidth }}>
        <Image
          source={{ uri: item.backgroundImageUrl[0] }}
          style={{
            width: "100%",
            height: undefined,
            aspectRatio: 16 / 9, // You can adjust this depending on the aspect ratio of your images
          }}
          resizeMode='contain' // Ensures the entire image is visible and keeps the aspect ratio intact
        />
        <View className='p-4'>
          <Text
            className='text-xl font-bold mb-2'
            style={{ color: item.textColor[0] }}>
            {item.templateName}
          </Text>
          <Text className='text-gray-600 mb-3' numberOfLines={2}>
            {item.description}
          </Text>
          <View className='flex-row justify-between items-center'>
            <Text className='text-sm text-gray-500'>
              Font: {item.textFont[0]}
            </Text>
            <View
              className='w-6 h-6 rounded-full border border-gray-300'
              style={{ backgroundColor: item.textColor[0] }}
            />
          </View>
        </View>
      </TouchableOpacity>
    ),
    []
  );

  const renderHeader = useCallback(
    () => (
      <View className='mb-6'>
        <Text className='text-3xl font-bold text-center mb-2 text-green-700'>
          Welcome to Invitations
        </Text>
        {isAuthenticated && (
          <Text className='text-green-500 text-center mb-2'>
            Login Successful
          </Text>
        )}
        {user && (
          <Text className='text-gray-600 text-center'>
            Hello, {typeof user === "string" ? user : user.name || user.email}
          </Text>
        )}
      </View>
    ),
    [isAuthenticated, user]
  );

  const onRefresh = useCallback(() => {
    refetchTemplates();
  }, [refetchTemplates]);

  const keyExtractor = useCallback((item: Template) => item.templateId, []);

  return (
    <SafeAreaView className='bg-gray-100 flex-1'>
      <FlatList
        data={formattedTemplates}
        renderItem={renderItem}
        keyExtractor={keyExtractor}
        ListHeaderComponent={renderHeader}
        refreshControl={
          <RefreshControl
            refreshing={isLoadingTemplates}
            onRefresh={onRefresh}
            colors={["#73EC8B"]}
            tintColor='#73EC8B'
          />
        }
        contentContainerStyle={{ paddingHorizontal: 16, paddingVertical: 16 }}
        ListEmptyComponent={
          <Text className='text-gray-600 text-center mt-4'>
            {isLoadingTemplates
              ? "Loading templates..."
              : "No invitation templates available"}
          </Text>
        }
        showsVerticalScrollIndicator={false}
      />
    </SafeAreaView>
  );
};

export default Home;
