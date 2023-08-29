import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useAuth } from "../services/AuthContext";
import './ShowAllExpenses.css'; 

const ShowAllExpenses = () => {
    const [expenses, setExpenses] = useState([]);
    const { userId, getToken } = useAuth(); // Assuming getToken returns the authentication token

    useEffect(() => {
        // Fetch user's expenses
        const fetchExpenses = async () => {

            await axios.get(`http://localhost:5096/api/User/${userId}/GetExpenses`, {
                headers: {
                    Authorization: `Bearer ${getToken()}` // Include the token in the headers
                }
            })
                .then(response => {
                    console.table(response.data)
                    setExpenses(response.data);
                })
                .catch(error => {
                    console.error('Error fetching expenses:', error);
                });
        }
        fetchExpenses();

    }, [userId, getToken]);

    const handleDeleteExpense = (expenseId) => {
        // Delete expense by ID
        axios.delete(`http://localhost:5096/api/Expenses/${expenseId}`)
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
                {expenses.map((user) => (
                    user.expenses.map((expense) => (
                        <li key={expense.expenseId}>
                            {expense.expenseCategory} - {expense.amount}
                            <button onClick={() => handleDeleteExpense(expense.expenseId)}>Delete</button>
                        </li>
                    ))
                ))}
            </ul>
        </div>
    );
};

export default ShowAllExpenses;
