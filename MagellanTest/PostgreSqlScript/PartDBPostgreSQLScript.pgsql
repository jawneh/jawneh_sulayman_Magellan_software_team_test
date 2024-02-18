-- Step 1: Check if Database exist and Drop it
-- then Create the Part database.
-- Might be better to run step1 only then after connecting run the rest of the script 

DROP DATABASE IF EXISTS  part;
CREATE DATABASE Part;

-- Connect to the Part database
\c Part;

-- Step 2: Create the 'item' table
CREATE TABLE item (
    id serial PRIMARY KEY,
    item_name VARCHAR(50) NOT NULL,
    parent_item INTEGER REFERENCES item(id),
    cost INTEGER NOT NULL,
    req_date DATE NOT NULL
);

-- Step 3: Insert data into the 'item' table
INSERT INTO item (item_name, parent_item, cost, req_date) VALUES
  ('Item1', NULL, 500, '2024-02-20'),
  ('Sub1', 1, 200, '2024-02-10'),
  ('Sub2', 1, 300, '2024-01-05'),
  ('Sub3', 2, 300, '2024-01-02'),
  ('Sub4', 2, 400, '2024-01-02'),
  ('Item2', NULL, 600, '2024-03-15'),
  ('Sub1', 6, 200, '2024-02-25');

 -- step 4: create Get Total Cost
CREATE OR REPLACE FUNCTION Get_Total_Cost(p_item_name VARCHAR)
RETURNS INTEGER AS $$
DECLARE
    total_cost INTEGER := 0;
BEGIN
    -- Check if the item has a parent_item, if yes, return null
    IF EXISTS (SELECT 1 FROM item WHERE item_name = p_item_name AND parent_item IS NOT NULL) THEN
        RETURN NULL;
    END IF;

    -- Calculate total cost for items without a parent_item
    WITH RECURSIVE item_cte AS (
        SELECT id, cost FROM item WHERE item_name = p_item_name
        UNION ALL
        SELECT i.id, i.cost
        FROM item i
        JOIN item_cte c ON i.parent_item = c.id
    )
    SELECT INTO total_cost SUM(cost) FROM item_cte;

    RETURN total_cost;
END;
$$ LANGUAGE plpgsql;
