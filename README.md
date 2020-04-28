# FinalProject
This is Version 1.0 of our Advanced C# Final Project. This is Quizlet solver meant to solve the standard SQL quizzes that we had to tediously complete in our Database &amp; SQL class. This application allows users to enter an SQL-related question and our algorithm searches Quizlet to see what it can find.

A few things:
1) Currently this project uses a local database to store user info. Thus, in order for the login feature to work, the database will have to be reinitialized through the PM manager:
  >> Add-Migration "New Schema"
  >> Update-Database
2) There were some errors installing the proper packages for EF Core. Thus, the above commands may have to be modified to accomodate to the specific EF Core package. This error will occur when you are trying to run the above commands, the console will tell you what you should run instead.
3) If you can't get the application to run, simply start a new EF Core project (that's just a basic ASP.NET Core C# Web Application--make sure you check 'Authentication'), and then just copy the code that I already have (including code in the wwwroot directory; I have additional CSS styling code in there).
4) Currently the algorithm is a bit slow, but we can improve this later.
