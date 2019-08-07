I would store the API connections in a SQL table.

For seeing currency rates from multiple providers I would bring back all the results from the APIs and send them to the view in a list and use a script to filter the list depending on the API dropdownlist. This would mean less calls to the server.

As for the system being able to easily extend the providers used it would be a case of adding the new connection in the SQL table.
