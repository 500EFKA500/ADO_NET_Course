import mssql_python

connection_string = ("SERVER=COMP11A1\\SQLEXPRESS;"
                     "DATABASE=DataBase;"
                     "Trusted_Connection=yes;"
                     "Encrypt=yes;"
                     "TrustServerCertificate=yes"
                     )

connection = mssql_python.connect(connection_string)

cursor = connection.cursor()

cursor.execute("""
    SELECT TOP 10
        ProductName,
        ListPrice
    FROM dbo.Products
""")
rows = cursor.fetchall()

for row in rows:
    print(row)

# Close the connection
connection.close()

if __name__ == '__main__':
    print('hello')