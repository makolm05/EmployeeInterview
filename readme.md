### Methods Overview



* **GetEmployeeById**

&#x20; Retrieves the full hierarchy of employees starting from the specified root manager.

&#x20; This implementation is less performant as it builds the tree in memory, but includes protection against self-referencing loops (e.g., when a manager references themselves).



* **GetEmployeeCTEById**

&#x20; Retrieves the full hierarchy of employees starting from the specified root manager using a SQL recursive CTE.

&#x20; This approach is significantly more performant since the hierarchy is built at the database level.

&#x20; However, it does not include protection against self-referencing loops.



* **EnableEmployee**

&#x20; Updates the `Enable` flag for a specific employee.



