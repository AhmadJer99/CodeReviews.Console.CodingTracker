# **ConsoleCodingTracker**

Console based CRUD application to track coding sessions. Developed using C#, SQLite3, and Dapper.

# Requirments

✅ When the application starts, it should create a sqlite database, if one isn’t present.

✅ Required to have separate classes in different files .

✅ Specifiy the format I want the date and time to be logged and not allow any other format.

✅ Create a configuration file that you'll contain your database path and connection strings.

✅ Use Dapper ORM for the data access instead of ADO.NET.

✅ Create a "CodingSession" class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration.

✅ The user shouldn't input the duration of the session. It should be calculated based on the Start and End times.

✅ When reading from the database, you can't use an anonymous object, you have to read your table into a List of Coding Sessions.


# Challenges

✅ Added the possibility of tracking the coding time via a stopwatch so the user can track the session as it happens.

✅ Let the users filter their coding records per period (weeks, days, years) and/or order ascending or descending.

✅ Create reports where the users can see their total and average coding session per period.

✅ Create the ability to set coding goals and show how far the users are from reaching their goal, along with how many hours a day they would have to code to reach their goal.

# Features

- Program initlization
    - As the application starts , an SQL connection is established and checks if a table already exists
    - If no database exists, or the table does not exist they will be created on program start and be seed with 100 random rows of data.
- Console UI
    - Used [Spectre console](https://spectreconsole.net/) for more pleasent way to show the menu and the options.
    - User can cycle between menu options with keyboard arrows
    - Enforce user correct date input using Validation Class
    
    ![1](https://github.com/user-attachments/assets/11a98cde-a67c-4ce4-9713-cc3b5f8e1ab0)


    
- CRUD functions queries against the database using Micro-ORM Dapper
    - Users can insert records Session Date is filled based on the day that the user uses the program, Start time of the session and End time in the format of (HH:mm AM/PM) Duration is automatically calculated and user cant insert it or update it.
    - Users can view all the records shown in a table in pleasent way using consoleTableExt
    
![2](https://github.com/user-attachments/assets/9a9bb533-bdad-44e2-9da6-0ac215851efd)


    
    
- Basic report functionality with a table to show each report in a pleasent way using consoleTableExt
- Shows the stats of average daily coding hours and total hours
- If the report was based on past week's report it will show stats alongside if the user reached his weekly goal or not , the weekly goal is set to 26 by default.
    
  ![image](https://github.com/user-attachments/assets/7b71a4e7-48ba-44f9-be2d-b1770e4ab020)

![3 based on pastweek](https://github.com/user-attachments/assets/422b3e65-997f-4357-903d-aeb989bc3d65)

    

# Challenges

- It was my first time using Dapper in C#,I had to learn the basic usage of dapper alongside other ORM to access the data base
- I had to learn how to parse return values of select queries and format it in a good way for the user
- The reports needed some specific SQL queries to make them work using DATE function in SQL , and i had to learn that as well.
- the stopwatch was a bit hard and i got stuck trying to program it, eventually i had to get help from various sources.

# **Areas to Improve**

- I think i might be able to make my code more clean and dynamic by judiciously placing each related class in its related folder
- I need to use OOP features more e.g. inheritance, overloading , abstraction etc..
- i still have some troube doing sql queries , i might need more practice on them.

# Resources Used

- [Learnt how to deal with lists of class objects.](https://www.youtube.com/watch?v=GHzrqt9cdIc)
- [Learnt how to establish a connection with Microsoft.data.sqlite and execute queries using dapper](https://www.youtube.com/watch?v=X0yzOkXSiFU)
- [Learn Dapper usage basics](https://dappertutorial.net/)
- Some stackoverflow to help me fix issues in my code
- [Learnt how to use DATE function in SQL](https://www.w3schools.com/sql/func_mysql_date.asp)
