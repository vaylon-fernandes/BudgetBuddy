import React, { useContext, useState } from 'react';
import { AppContext } from '../services/AppContext';
import { v4 as uuidv4 } from 'uuid';

const AddExpenseForm = (props) => {
	const { dispatch } = useContext(AppContext);

	const [selectedExpense, setSelectedExpense] = useState('');
	const [cost, setCost] = useState('');

	const expenses = [
		{ id: '1', name: 'Groceries' },
		{ id: '2', name: 'Utilities' },
		{ id: '3', name: 'Rent' },
		// Add more expense options as needed
	];

	const onSubmit = (event) => {
		event.preventDefault();
		const expense = {
			id: uuidv4(),
			name: selectedExpense,
			cost: parseInt(cost),
		};

		dispatch({
			type: 'ADD_EXPENSE',
			payload: expense,
		});

		setSelectedExpense('');
		setCost('');
	};

	return (
		<form onSubmit={onSubmit}>
			<div className='row'>
				<div className='col-sm col-lg-4'>
					<label htmlFor='name'>Name</label>
					<select
						required
						className='form-control'
						id='name'
						value={selectedExpense}
						onChange={(event) => setSelectedExpense(event.target.value)}
					>
						<option value='' disabled>Select an expense</option>
						{expenses.map((expense) => (
							<option key={expense.id} value={expense.name}>
								{expense.name}
							</option>
						))}
					</select>
				</div>
				<div className='col-sm col-lg-4'>
					<label htmlFor='cost'>Cost</label>
					<input
						required
						type='number'
						className='form-control'
						id='cost'
						value={cost}
						onChange={(event) => setCost(event.target.value)}
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
