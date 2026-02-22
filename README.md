# 🏢 Smart Inventory Management Dashboard
A complete Full-Stack web application built to manage inventory, track sales, and analyze data in real-time. This project demonstrates a solid architecture, connecting a responsive front-end interface directly to a relational database through a robust back-end API.

## 🚀 Features

- **Live Search:** Instant, client-side filtering of products without server round-trips.
- **Smart Sorting:** Sort inventory items by quantity (Max/Min) with a single click.
- **Full CRUD Operations:** Seamlessly Add, Edit, Delete, and Sell items.
- **Real-Time Analytics:** Dynamic visual chart representation of current stock levels.
- **Interactive UI:** Responsive design with dynamic icons and modern CSS styling.

## 💻 Tech Stack

- **Front-End:** HTML5, CSS3, Vanilla JavaScript (DOM manipulation, Fetch API)
- **Back-End:** C# (.NET Minimal API)
- **Database:** MS SQL Server (ADO.NET)

## 📸 Screenshots
<img width="1903" height="943" alt="Screenshot 2026-02-22 145151" src="https://github.com/user-attachments/assets/087f128e-7c32-4c3a-84ac-b770c7261611" />
<img width="1904" height="933" alt="Screenshot 2026-02-22 145158" src="https://github.com/user-attachments/assets/0ceb60f4-dc98-4cd2-9de9-8da0acba8a1e" />
<img width="1903" height="369" alt="Screenshot 2026-02-22 145204" src="https://github.com/user-attachments/assets/41549585-fd00-44b7-b301-3a8dcf0a6b1b" />

## 💡 Architecture & Workflow

This project highlights the complete data flow:
1. **Database:** SQL Server stores the inventory data securely.
2. **API:** A C# Minimal API handles requests, executes SQL commands using ADO.NET, and returns JSON data.
3. **Client:** JavaScript fetches the API, dynamically renders HTML components using Template Literals, and handles live DOM updates for a seamless user experience.
