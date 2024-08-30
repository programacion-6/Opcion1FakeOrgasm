# Library Management System - FakeOrgasm

## Members:

**Original Team**
-   Salet Gutierrez Nava
-   Luiggy Mamani Condori
-   Axel Javier Ayala Siles
-   Gabriel Santiago Concha Saavedra

**New Team**
- Samuel Escalera
- Diego Figueroa
- Bianca Soliz


### ENHANCEMENTS
- Refactoring of the Reporting Module, Enhancement the single responsibility (Samuel Escalera) Status : In progress
---
# Library System Requirements Analysis

An analysis of the requirements for the chosen base Library System was conducted to determine if the system fulfills or covers all the requirements requested by the client.

The analysis was divided into the following requirements:

- [Book Management](https://docs.google.com/document/d/1jIiOO_15NNHu_Kx9AhLvjVFnh5oFXGMEumhVLJcXhVw/edit#heading=h.44sipolj06bj)
- [Borrowing System](https://docs.google.com/document/d/13OtCXY1RtGO67lxv1iOhC15ko5sBuice9oB-5nG9SxY/edit)
- [Patron Management](https://docs.google.com/document/d/1PkQ3zsa2Bttwk1c84Pij4-qZfRlIUAvbsvlY1Qtyets/edit)
- [Search and Reporting](https://docs.google.com/document/d/1HwKl9iTL0CzgY1Dp2AMAwqu1AnxrLIbzg3OFZXfwRRs/edit#heading=h.i4g12c8tsbja)

This analysis was based on the Requirements Diagram shown below:

![Requirements-Diagram](/DiagramRequirements/Requirements.jpeg)

---
## How to use?

Before to start running proyect, you need to have started docker service:
- If you are on Windows, make sure that you have Docker Desktop on execution.
- If you are on Ubuntu, start docker with the following command:
    ```
    sudo systemctl start docker
    ```
**Warning:** `If you have already created the container of the FakeOrgasm project, you must previously delete it, so that when you enter the following command it is recreated and created with data generated in the database.`

`Then enter the command:`
```
docker-compose up
```
If you do not have a container previously continue with these commands:

Then at the root of the proyect you need to execute the following command:
```
docker-compose up -d
```

Then into `/LibrarySystem` you can execute normally the command:
```
dotnet run
```
To start using the system library

The application works with commands, there are commands to change views or handlers and commands for each of them:

### Change view

```
view books
view patrons
view loans
view reports
view fines
```

Commands for each view

### books:

```
new
delete
update
show all
show by genre
find by title
find by author
find by ISBN
```

### patrons:

```
new
delete
update
show all
find by name
find by m-number
```

### loans:

```
lend
return

```

### reports:

```
show overdue books
show borrowed books
show current loans by patron
show loans by patron
show most borrowed books
show most active patrons
show patron debts
```

### fines:

```
pay
make
show all
show actives
```