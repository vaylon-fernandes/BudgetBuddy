import React, { createContext, useContext, useState, useEffect } from "react";
import axios from "axios";


const AuthContext = createContext();

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [userEmail, setUserEmail] = useState(null);
  const [userId, setUserId] = useState('');
  const [authToken, setAuthToken] = useState('');

  const login = async (userData) => {
    try {
      const response = await axios.post(
        "http://localhost:5096/api/Auth/login",
        userData
      );

      const loginResponse = response.data;
      console.log("resp:  "+loginResponse.userId);
      //console.log(typeof loginResponse)
      //console.log(userData.email)

      if (loginResponse.success===true) {
        setUserEmail(loginResponse.email);
        setUserId(loginResponse.userId);
        setAuthToken(loginResponse.token)
        // console.log("right")
      } else {
        console.log("wrong")
        setUserEmail(null);  
      }
      return loginResponse;
      // console.log(user)
       // This might need to be adjusted based on your needs.
    } catch (error) {
      console.error("Login error:", error);
      setUserEmail(null);
      throw error;
    }
  };

  
  const logout = () => {
    // TODO - logout logic here.
    console.log("logout");
    setUserEmail(null);
  };

  const getToken = () => {
    return authToken; // Return the stored token
  };

  useEffect(() => {
    console.log("User updated:", setUserEmail);
  }, [setUserEmail]);

  return (
    <AuthContext.Provider value={{ userEmail, userId, login, logout, getToken }}>
      {children}
    </AuthContext.Provider>
  );
};

