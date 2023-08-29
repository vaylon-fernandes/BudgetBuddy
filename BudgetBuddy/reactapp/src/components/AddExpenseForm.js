import React, { useContext, useState } from 'react';
import { useNavigate } from "react-router-dom";
import { AppContext } from '../services/AppContext';
import { useAuth } from "../services/AuthContext";
import { v4 as uuidv4 } from 'uuid';
import axios from 'axios';

const AddExpenseForm = (props) => {
	const { dispatch } = useContext(AppContext);
	const { userId } = useAuth();
		console.log(userId)
	const [selectedExpense, setSelectedExpense] = useState('');
	const [amount, setAmount] = useState('');
	const [expenseDescription, setExpenseDescription] = useState(''); // New state for expenseDescription
	const navigate = useNavigate();
	const expenses = [
		{ id: '1', expenseCategory: 'HOUSING' },
		{ id: '2', expenseCategory: 'FOOD' },
		{ id: '3', expenseCategory: 'TRANSPORTATION' },
		{ id: '4', expenseCategory: 'HEALTH' },
		{ id: '5', expenseCategory: 'ENTERTAINMENT' },
		{ id: '6', expenseCategory: 'DEBT' },
		{ id: '7', expenseCategory: 'SAVING' },
		{ id: '8', expenseCategory: 'EDUCATION' },
		{ id: '9', expenseCategory: 'CLOTHING' },
		{ id: '10', expenseCategory: 'PERSONAL_CARE' },
		{ id: '11', expenseCategory: 'DONATION' },
		{ id: '12', expenseCategory: 'TRAVEL' },
		{ id: '13', expenseCategory: 'UTILITIES' },
		{ id: '14', expenseCategory: 'GIFTS' },
		{ id: '15', expenseCategory: 'PETS' },
		{ id: '16', expenseCategory: 'MISCELLLANEOUS' }
		// Add more expense options as needed
	];

	const onSubmit = async (event) => {
		event.preventDefault();
		const currentDateTime = new Date(); // Get the current date and time
  		const formattedDateTime = currentDateTime.toISOString(); // Convert to ISO string format

		const expenseData = {
			description: expenseDescription, // Add the description to the expense data
			expenseCategory: selectedExpense,
			amount: parseInt(amount),
			enteredDate: formattedDateTime, // Include the formatted date and time
			userId: userId
		};
		console.log(expenseData)
		try {
			const response = await axios.post('http://localhost:5096/api/Expenses', expenseData);
			console.log('Expense saved successfully:', response.data);

			// const expenseResponse = response.data;
			// if(expenseResponse.success===true){
			// 	// Clear form fields after successful submission
			// setSelectedExpense('');
			// setAmount('');
			// setExpenseDescription('');
			navigate("/ManageExpenses");
			// }
			
		} catch (error) {
			console.error('Error saving expense:', error);
		}
	};

	return (
		<form onSubmit={onSubmit}>
			<div className='row'>
				<div className='col-sm col-lg-4'>
					<label htmlFor='expenseCategory'>expenseCategory</label>
					<select
						required
						className='form-control'
						id='expenseCategory'
						value={selectedExpense}
						onChange={(event) => setSelectedExpense(event.target.value)}
					>
						<option value='' disabled>Select an expense</option>
						{expenses.map((expense) => (
							<option key={expense.id} value={expense.expenseCategory}>
								{expense.expenseCategory}
							</option>
						))}
					</select>
				</div>
				<div className='col-sm col-lg-4'>
					<label htmlFor='amount'>amount</label>
					<input
						required
						type='number'
						className='form-control'
						id='amount'
						value={amount}
						onChange={(event) => setAmount(event.target.value)}
					/>
				</div>
				<div className='col-sm col-lg-4'>
					<label htmlFor='description'>Description</label>
					<textarea
						className='form-control'
						id='description'
						value={expenseDescription}
						onChange={(event) => setExpenseDescription(event.target.value)}
					/>
				</div>
			</div>
			<div className='row mt-3'>
				<div className='col-sm'>
					<button type='submit' className='btn btn-primary'>
						Save
					</button>
				</div>
			</div>
		</form>
	);
};

export default AddExpenseForm;
