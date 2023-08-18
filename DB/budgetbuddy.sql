create database budget_buddy;

use budget_buddy; 

CREATE TABLE users (
    user_id INT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    email VARCHAR(100) NOT NULL,
    password VARCHAR(100) NOT NULL, 
    role VARCHAR(10) NOT NULL
);

CREATE TABLE expenses (
    expense_id INT PRIMARY KEY,
    user_id INT,
    category VARCHAR(50) NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    date DATE NOT NULL,
    FOREIGN KEY uid (user_id) REFERENCES users(user_id)
);

CREATE TABLE budget (
    budget_id INT PRIMARY KEY,
    user_id INT,
    category VARCHAR(50) NOT NULL,
    limit_amount DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY uid (user_id) REFERENCES users(user_id)
);

CREATE TABLE financial_goal (
    goal_id INT PRIMARY KEY,
    user_id INT,
    description VARCHAR(100) NOT NULL,
    target_amount DECIMAL(10, 2) NOT NULL,
    target_date DATE NOT NULL,
    current_progress DECIMAL(10, 2) DEFAULT 0,
    FOREIGN KEY uid (user_id) REFERENCES users(user_id)
);

CREATE TABLE income (
    income_id INT PRIMARY KEY,
    user_id INT,
    source VARCHAR(100) NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    frequency VARCHAR(20) NOT NULL,
    FOREIGN KEY uid (user_id) references users(user_id)
);

