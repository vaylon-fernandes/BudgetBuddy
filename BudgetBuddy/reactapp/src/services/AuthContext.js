import React, { createContext, useContext, useState, useEffect } from "react";
import axios from "axios";


const AuthContext = createContext();

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = async (userData) => {
    try {
      const response = await axios.post(
        "http://localhost:5096/api/User/login",
        userData
      );

      const loginResponse = response.data;
      console.log(loginResponse);
      //console.log(typeof loginResponse)
      //console.log(userData.email)

      if (loginResponse.success===true) {
        setUser(userData.email);
        // console.log("right")
      } else {
        console.log("wrong")
        setUser(null);  
      }
      return loginResponse;
      // console.log(user)
       // This might need to be adjusted based on your needs.
    } catch (error) {
      console.error("Login error:", error);
      setUser(null);
      throw error;
    }
  };

  
  const logout = () => {
    // TODO - logout logic here.
    console.log("logout");
    setUser(null);
  };

  useEffect(() => {
    console.log("User updated:", user);
  }, [user]);

  return (
    <AuthContext.Provider value={{ user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

