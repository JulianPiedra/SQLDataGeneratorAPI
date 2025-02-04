# SqlDataGeneratorAPI Overview

The SQLGeneratorAPI allows users to generate SQL data for SQL databases dynamically based on input parameters. It is designed to output large volumes of data quickly and efficiently. The API is primarily used as an SQL data generator, capable of generating SQL queries for direct insertion into an MSSQL database or exporting raw data in formats such as JSON, XML, CSV, or Excel, based on user preferences. It both generates data (IDs, card numbers, dates, etc..) and also gets data from a MSSQL database, information like (names, country codes, countries).

# Authentication
To authenticate with the API, you must include your API key in the request headers as such.
  X-API-KEY: <API Key>
  
To get the api key head over to the [Live Site](https://julianpiedra.github.io/sqldatagenerator/).

