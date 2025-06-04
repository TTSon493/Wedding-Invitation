import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';

const BASE_URL = 'https://weddingservices.azurewebsites.net/api'; // Replace with your actual API base URL

const http = axios.create({
    baseURL: 'https://weddingservices.azurewebsites.net/api',
    timeout: 10000,
    headers: {
        "Content-Type": "application/json",
        Accept: "application/json",
        "Access-Control-Allow-Origin": "*",
    },
});

export const setJwtTokenSession = async (accessToken: string | null, refreshToken: string | null) => {
    if (accessToken && refreshToken) {
        await AsyncStorage.setItem('accessToken', accessToken);
        await AsyncStorage.setItem('refreshToken', refreshToken);
        http.defaults.headers.common['Authorization'] = `Bearer ${accessToken}`;
    } else {
        await AsyncStorage.removeItem('accessToken');
        await AsyncStorage.removeItem('refreshToken');
        delete http.defaults.headers.common['Authorization'];
    }
};

export const getJwtTokenSession = async () => {
    const accessToken = await AsyncStorage.getItem('accessToken');
    const refreshToken = await AsyncStorage.getItem('refreshToken');
    return { accessToken, refreshToken };
};

// Interceptor to handle token refresh
http.interceptors.response.use(
    (response: any) => response,
    async (error: { config: any; response: { status: number; }; }) => {
        const originalRequest = error.config;
        if (error.response.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            const { refreshToken } = await getJwtTokenSession();
            try {
                const response = await axios.post(`${BASE_URL}/auth/refresh-token`, { refreshToken });
                const { accessToken, refreshToken: newRefreshToken } = response.data;
                await setJwtTokenSession(accessToken, newRefreshToken);
                originalRequest.headers['Authorization'] = `Bearer ${accessToken}`;
                return http(originalRequest);
            } catch (refreshError) {
                // Handle refresh token failure (e.g., logout user)
                await setJwtTokenSession(null, null);
                // You might want to navigate to the login screen here
                return Promise.reject(refreshError);
            }
        }
        return Promise.reject(error);
    }
);

export default http;