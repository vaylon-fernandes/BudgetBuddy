import React, { createContext, useContext, useState, useEffect } from "react";

const AuthContext = createContext();

export const useAuth = () => {
  return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);

  const login = (userData) => {
    // TODO - Here you would perform the actual login logic.
    const validUsers = [
      { email: "vaibhav@budgetbuddy.com", password: "Jogad" },
      { email: "vaylon@budgetbuddy.com", password: "test" },
    ];

    const validUser = validUsers.find(
      (user) =>
        user.email === userData.email && user.password === userData.password
    );

    if (validUser) {
      setUser(validUser.email);
    } else {
      setUser(null);
    }
    return user;
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
