from idlelib import query

import pyodbc

conn = pyodbc.connect('DRIVER={ODBC Driver 17 for SQL Server};'
                      "SERVER=COMP11A1\\SQLEXPRESS;"
                      "DATABASE=DataBase;"
                      "Trusted_Connection=yes;"
                      "TrustServerCertificate=yes"
                      )

cursor = conn.cursor()

def get_date(select: int):
    query = [
        "create table Requests(id int IDENTITY PRIMARY KEY, quantity int NOT NULL, create_at datetime DEFAULT GetDate() NOT NULL)",
        "INSERT INTO Requests(quantity) VALUES(?)",
        "SELECT TOP 50 id, quantity, created_at FROM Requests",
    ]
    return query[select]

def create_table():
    cursor.execute(get_date(1))
    cursor.commit()

def save_orders():
    cursor.execute(query[1], 3)
    cursor.commit()


def show_collection():
    cursor.execute(get_date(1))
    rows = cursor.fetchall()

    for row in rows:
        print(row)

def main():
    create_table()
    save_orders()
    show_collection()

