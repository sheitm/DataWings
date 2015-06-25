# DataWings
*Data driven tests made simple!*

Project Description
-------------------
Data driven integration testing made easy! Tool to be used when authoring integration tests for code that touches a database. Set up the database so that your tests are accessing known data. Assert that the data in the database is in the expected state after the test has executed

Design paradigms
---------------
No dependencies (except the .NET base library, of course).
Simple and flexible configuration
Fluent interface

For a more detailed overview [read this](http://sheitm.blogspot.no/2009/05/yes-im-now-officially-open-source.html). 

What does it do?
---------------
DataWings addresses three functional areas within the broader area of supporting integration testing of code that accesses and modifies data stored in a relational database:

**Simple and flexible configuration**

Provides an attribute based configuration model, supporting connection string set up directly in code and in config file simultaneously. You can provide the string directly in code for quick and easy access to the database while developing the test, and then later (while hooking up the test in your system for continuous integration, for instance) specify the string in the configuration file without altering the tes code at all.

**DbAssert**

Functionality similar to the familiar Assert class of unit test frameworks (such as NUnit ). Instead of comparing two values, a given value is compared against a named column for a given row in a database table.

**DataBoy**

A tool for maintaining data in your database directly in your testing code.

**DataAdversary**

Provides functionality useful for testing concurrency issues. Lets you similate situations where the data has been changed by another party (the adversary).

Supported database engines
-------------------------
DataWings supports *Sql Server* and *Oracle* directly by making use of the functionality in the System.Data.SqlClient and System.Data.OracleClient namespaces. In addition, a separate assembly DataWings.SQLite.dll is provided, giving support to the SQLite database engine. A provisioning model is included, making it fairly easy to implement pluggable support for additional database engines
