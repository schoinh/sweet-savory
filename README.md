# Pierre's Sweet and Savory Treats

#### _An MVC Web application using a local database and user authentication to view and manage bakery treats and flavors - August 16, 2019_

#### _By **Na Hyung Choi**_

## Description

This Web site helps market bakery treats of different flavors. The application enables user registration and authentication with ASP.NET Core Identity.

All users, whether logged in or not, can:
1. See a list of all available treats and their prices, sorted by treat name
2. See a list of all available flavors, sorted by flavor name
3. Click on a specific treat name to view all flavors associated with it
4. Click on a specific flavor to view all treats associated with it

Only a logged-on user can:
1. Create, edit, and delete treats
2. Create, edit, and delete flavors
3. Add and remove flavors associated with a treat.

The database keeps track of which user created a flavor or treat on the Web site. However, any logged-on user can edit or delete any treat or flavor in the databse.

## Setup/Installation Requirements

* This application requires MySQL.

1. Clone this repository:
    ```
    $ git clone https://github.com/schoinh/sweet-savory.git
    ```
2. Open the App Settings file (sweet-savory/SweetSavory/appsettings.json) and ensure that the MySQL username and password match your MySQL credentials.

3. Log onto MySQL:
    ```
    $ mysql -u USERNAME -p PASSWORD
    ```
4. Navigate to the production folder (HairSalon.Solution/HairSalon)
5. Restore dependencies, update your local database, and run the application
    ```
    $ dotnet restore
    $ dotnet ef database update
    $ dotnet run
    ```
7. On a Web browser (Chrome recommended), navigate to http://localhost:5000

## Known Bugs
It is possible to add duplicate flavors to a treat.

## Technologies Used
* C# / .NET Core
* ASP.NET Core MVC
* Entity Framework Core
* LINQ
* MySQL

## Support and contact details

_Please contact Na Hyung with questions and comments._

### License

*GNU GPLv3*

Copyright (c) 2019 **_Na Hyung Choi_**