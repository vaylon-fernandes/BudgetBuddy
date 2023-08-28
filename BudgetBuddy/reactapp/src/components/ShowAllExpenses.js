import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useAuth } from "../services/AuthContext";

const ShowAllExpenses = () => {
  const [expenses, setExpenses] = useState([]);
  const { userId, getToken } = useAuth(); // Assuming getToken returns the authentication token

  useEffect(() => {
    // Fetch user's expenses
    axios.get(`http://localhost:5096/api/User/${userId}/GetExpenses`, {
      headers: {
        Authorization: `Bearer ${getToken()}` // Include the token in the headers
      }
    })
    .then(response => {
      setExpenses(response.data.expenses);
    })
    .catch(error => {
      console.error('Error fetching expenses:', error);
    });
  }, [userId, getToken]);

  const handleDeleteExpense = (expenseId) => {
    // Delete expense by ID
    axios.delete(`/api/Expenses/${expenseId}`)
      .then(response => {
        // Remove the deleted expense from the state
        setExpenses(prevExpenses => prevExpenses.filter(expense => expense.expenseId !== expenseId));
      })
      .catch(error => {
        console.error('Error deleting expense:', error);
      });
  };

  return (
    <div>
      <h1>My Expenses</h1>
      <ul>
        {expenses.map((expense) => (
           console.log( expense)
        //   <li key={expense.expenseId}>
        //     {expense.expenseCategory} - {expense.amount}
        //     <button onClick={() => handleDeleteExpense(expense.expenseId)}>Delete</button>
        //   </li>
        ))}
      </ul>
    </div>
  );
};

export default ShowAllExpenses;
