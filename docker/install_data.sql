CREATE TABLE Customers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    last_login DATETIME
);

CREATE TABLE Products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    tax DECIMAL(5,2) NOT NULL DEFAULT 0.00
);

CREATE TABLE Transactions (
    id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    transaction_date DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES Customers(id) ON DELETE CASCADE
);

CREATE TABLE Transaction_Items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    transaction_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL CHECK (quantity > 0),
    price DECIMAL(10,2) NOT NULL,
    total_price DECIMAL(10,2) GENERATED ALWAYS AS (quantity * price) STORED,
    FOREIGN KEY (transaction_id) REFERENCES Transactions(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE
);

CREATE TABLE Discounts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    discount_name VARCHAR(255) NOT NULL,
    product_id INT NULL, -- If NULL, it's a global discount
    discount_type ENUM('PERCENTAGE', 'MULTI_BUY') NOT NULL,
    discount_value DECIMAL(10,2) NOT NULL, -- Percentage discount or price reduction
    required_product_id INT NULL, -- For multi-buy, the required product
    required_quantity INT NULL, -- The quantity needed to trigger the discount
	start_date DATETIME NOT NULL, -- When the discount starts
    end_date DATETIME NOT NULL, -- When the discount ends
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE SET NULL,
    FOREIGN KEY (required_product_id) REFERENCES Products(id) ON DELETE SET NULL
);

CREATE TABLE Transaction_Discounts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    transaction_id INT NOT NULL,
    discount_id INT NOT NULL,
    FOREIGN KEY (transaction_id) REFERENCES Transactions(id) ON DELETE CASCADE,
    FOREIGN KEY (discount_id) REFERENCES Discounts(id) ON DELETE CASCADE
);

INSERT INTO Products (name, price, tax) 
VALUES 
    ('Soup Tin', 0.65, 5.00),
    ('Bread', 0.80, 5.00),
    ('Milk', 1.30, 5.00),
    ('Apples (per bag)', 1.00, 5.00);
    
    -- 10% off Apples (valid for 1 week)
INSERT INTO Discounts (discount_name, product_id, discount_type, discount_value, start_date, end_date) 
VALUES ('Apple Discount', (SELECT id FROM Products WHERE name='Apples (per bag)'), 'PERCENTAGE', 10, 
        '2025-03-10 00:00:00', '2025-03-17 23:59:59');

	-- Buy 2 Soup Tins, get Bread for half price (valid for 2 weeks)
INSERT INTO Discounts (discount_name, product_id, discount_type, discount_value, required_product_id, required_quantity, start_date, end_date) 
VALUES ('Soup & Bread Deal', (SELECT id FROM Products WHERE name='Bread'), 'MULTI_BUY', 00, 
        (SELECT id FROM Products WHERE name='Soup Tin'), 2, 
        '2025-03-10 00:00:00', '2025-03-24 23:59:59');
		
		
CREATE PROCEDURE get_discounted_transactions (IN transactionId INT)
BEGIN
    SELECT 
        t.id AS transaction_id, 
        t.transaction_date,
        c.name AS customer, 
        p.name AS product, 
        ti.quantity, 
        ti.price, 
        (ti.price * ti.quantity) AS original_price,
        COALESCE(td.discount_applied, 0) AS discount_applied, 
        ((ti.price * ti.quantity) - COALESCE(td.discount_applied, 0)) AS final_price
    FROM Transactions t
    JOIN Customers c ON t.customer_id = c.id
    JOIN Transaction_Items ti ON t.id = ti.transaction_id
    JOIN Products p ON ti.product_id = p.id
    LEFT JOIN Transaction_Discounts td ON t.id = td.transaction_id
    WHERE t.id = transactionId;
END