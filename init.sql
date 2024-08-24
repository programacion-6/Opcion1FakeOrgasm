INSERT INTO public.books (id, title, author, isbn, genre, publicationyear) VALUES
('1b9d6bcd-bbfd-4d2a-8cfd-1d62b3d1c1b1', 'The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 'Fiction', 1925),
('2c60d0b3-2386-4f1e-99e1-4c09cb2d5a08', 'To Kill a Mockingbird', 'Harper Lee', '9780060935467', 'Fiction', 1960),
('3a48d2a7-cd46-4c99-8eb3-8917b5e50d64', '1984', 'George Orwell', '9780451524935', 'Dystopian', 1949),
('4b9e2f62-21b0-4a58-bc0c-58d26204b7d2', 'The Catcher in the Rye', 'J.D. Salinger', '9780316769488', 'Fiction', 1951),
('5c7e0c94-32f8-4d90-9726-009fd7f2e8b4', 'Pride and Prejudice', 'Jane Austen', '9780141439518', 'Romance', 1813),
('6d58b0c5-04a2-4b2a-abe4-1f7fc47fc282', 'The Hobbit', 'J.R.R. Tolkien', '9780261102217', 'Fantasy', 1937),
('7e6f2d8a-cd6c-4a24-b0d4-68961a6f5bb8', 'Moby-Dick', 'Herman Melville', '9781503280786', 'Adventure', 1851),
('8f7d3e9b-2b6f-4828-a596-7d54210c5fbd', 'War and Peace', 'Leo Tolstoy', '9780140447934', 'Historical', 1869),
('9a8e4b2f-f0fc-4b9d-a1bb-5d36a28a15bc', 'Brave New World', 'Aldous Huxley', '9780060850524', 'Dystopian', 1932),
('a9c4e5b1-7d69-4e5f-8bcd-6d6c3c6d81c7', 'The Alchemist', 'Paulo Coelho', '9780061122415', 'Adventure', 1988);


INSERT INTO public.patrons (id, name, membershipnumber, contactdetails) VALUES
('9d6d8e28-f27d-4e02-9a8c-9eae9e7b39bc', 'Alice Smith', 1001, 'alice.smith@example.com'),
('c8e4c6b0-0d6c-4e9f-a41c-c9fbb8c7a1a2', 'Bob Johnson', 1002, 'bob.johnson@example.com'),
('f0c8a4a5-6f6a-4b6f-870b-ef93b1c3f45b', 'Charlie Brown', 1003, 'charlie.brown@example.com'),
('b0c9d58c-7884-4a7c-8d62-1e7e8e32b5a2', 'Diana Prince', 1004, 'diana.prince@example.com'),
('7a5d2d1b-4c29-44c1-8ea4-9a3b2fdfc04a', 'Eve Davis', 1005, 'eve.davis@example.com'),
('d1b54e5b-9b29-44a1-b2a7-09b227c4d97d', 'Frank Miller', 1006, 'frank.miller@example.com'),
('6a2b1b2c-0e6a-4c62-a3b2-6a4b8f7e2c8d', 'Grace Lee', 1007, 'grace.lee@example.com'),
('4b3e6d4c-5c6a-4a8d-bd62-11f1c2d69a93', 'Hank Green', 1008, 'hank.green@example.com'),
('8d0f4b28-3d67-4a82-b4e5-7c2a85b1f2a0', 'Ivy Wilson', 1009, 'ivy.wilson@example.com'),
('e4a6d3c4-2e9b-4a63-8c5d-9b9e1f5c7a6d', 'Jack White', 1010, 'jack.white@example.com');


INSERT INTO public.loans (id, bookid, patronid, loandate, returndate, wasreturn) VALUES
('aab8d7ef-b1b1-4f7b-bb15-9782b8d5f4eb', '1b9d6bcd-bbfd-4d2a-8cfd-1d62b3d1c1b1', '9d6d8e28-f27d-4e02-9a8c-9eae9e7b39bc', '2024-07-01', '2024-07-15', false),
('f4b9e0fc-91c3-4a82-8b1d-b90f1c7c05f3', '2c60d0b3-2386-4f1e-99e1-4c09cb2d5a08', 'c8e4c6b0-0d6c-4e9f-a41c-c9fbb8c7a1a2', '2024-07-03', '2024-07-17', false),
('0dc6bcb8-604f-4f88-a728-8d8e16ecf94e', '3a48d2a7-cd46-4c99-8eb3-8917b5e50d64', 'f0c8a4a5-6f6a-4b6f-870b-ef93b1c3f45b', '2024-07-05', '2024-07-19', false),
('4f12e92f-4537-45e4-a4c2-c7a4e6c72d84', '4b9e2f62-21b0-4a58-bc0c-58d26204b7d2', 'b0c9d58c-7884-4a7c-8d62-1e7e8e32b5a2', '2024-07-07', '2024-07-21', false),
('b5c84312-2a0a-46b5-98bc-f1f8ec73f6d8', '5c7e0c94-32f8-4d90-9726-009fd7f2e8b4', '7a5d2d1b-4c29-44c1-8ea4-9a3b2fdfc04a', '2024-07-09', '2024-07-23', false),
('ebc89720-94ae-4b9a-a248-cf44bcbde2c7', '6d58b0c5-04a2-4b2a-abe4-1f7fc47fc282', 'd1b54e5b-9b29-44a1-b2a7-09b227c4d97d', '2024-07-01', '2024-07-15', true),
('c3f7a6be-6311-48b0-8c97-8e0b4d3f3ae1', '7e6f2d8a-cd6c-4a24-b0d4-68961a6f5bb8', '6a2b1b2c-0e6a-4c62-a3b2-6a4b8f7e2c8d', '2024-07-03', '2024-07-17', true),
('7e5c6d68-6037-4e2c-b8c4-3e6c2e54d3cb', '8f7d3e9b-2b6f-4828-a596-7d54210c5fbd', '4b3e6d4c-5c6a-4a8d-bd62-11f1c2d69a93', '2024-07-05', '2024-07-19', true),
('b7c4d9b1-8d60-4f1d-8d6f-4e74a0a3eaa0', '9a8e4b2f-f0fc-4b9d-a1bb-5d36a28a15bc', '8d0f4b28-3d67-4a82-b4e5-7c2a85b1f2a0', '2024-07-07', '2024-07-21', true),
('d5e0b9a3-d91e-466e-84bc-042b9f9b2a2e', 'a9c4e5b1-7d69-4e5f-8bcd-6d6c3c6d81c7', 'e4a6d3c4-2e9b-4a63-8c5d-9b9e1f5c7a6d', '2024-07-09', '2024-07-23', true);
