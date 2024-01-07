# SafeSpace.Backend - Local Development Setup

This README outlines the steps required for setting up the local development environment for the SafeSpace application using .NET 8 and Visual Studio 2022 Community Edition.

## Prerequisites

Before you begin, ensure you have the following installed:
- .NET 8 SDK
- Visual Studio 2022 Community Edition
- MySQL Server
- MySQL Workbench

## Steps for Setup

### 1. Fork and Clone the Repository
- First, fork the SafeSpace repository to your own GitHub account. Then, clone your fork to your local machine.
- Add the original SafeSpace repository as an upstream remote to your local repository.
  `git remote add upstream https://github.com/SafeSpace-Solutions/SafeSpace.Backend.git`

### 2. Run the LocalDBInitializer Tool in Visual Studio
- Open the SafeSpace solution in Visual Studio 2022.
- Find the LocalDBInitializer project in the Tools folder.
- Right-click on the project and select 'Set as StartUp Project'.
- Press F5 to build and run the tool.
- When prompted, enter your MySQL root password.

The tool will create the safespace user and safespace_local_db database.

### 3. Set Up a New Connection in MySQL Workbench
To manage the database using MySQL Workbench:

- Open MySQL Workbench.
- Click on the "+" icon next to "MySQL Connections" to create a new connection.
- Use the following details to set up the connection:
        - Connection Name: SafeSpace Local
        - Hostname: localhost
        - Port: 3306
        - Username: safespace
        - Password: local-db-password (click on "Store in Vault..." and enter this password)

Click "Test Connection" to ensure it works, then save the connection.

### 4. Set SafeSpace.Web as startup project
Right-click on the SafeSpace.Web project and select 'Set as StartUp Project