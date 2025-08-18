import apiClient from "./apiClient";

interface GoogleTokenExchangeRequest {
    authorizationCode: string;
    codeVerifier: string;
  }

  interface GoogleRefreshTokenRequest {
    refreshToken: string;
  }

  export const exchangeToken = async (request: GoogleTokenExchangeRequest) => {
    const response = await apiClient.post('/GoogleOAuth/login', request);
    return response.data;
  };

  export const refreshToken = async (request: GoogleRefreshTokenRequest) => {
    const response = await apiClient.post('/GoogleOAuth/refresh-token', request);
    return response.data;
  };