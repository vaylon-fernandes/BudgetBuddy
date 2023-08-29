import React, { useContext } from 'react';
import { AppContext } from '../services/AppContext';

const ExpenseTotal = () => {
	const { expenses } = useContext(AppContext);

	const total = expenses.reduce((total, item) => {
		return (total += item.cost);
	}, 0);

	return (
		<div className='alert alert-primary p-4'>
			<span>Spent so far: Rs.{total}</span>
		</div>
	);
};

export default ExpenseTotal;