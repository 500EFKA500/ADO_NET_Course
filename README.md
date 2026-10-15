# ADO.NET Course

Практический курс ADO.NET, SQL Server, Dapper и ASP.NET Core.

## Подключение к базе

По умолчанию проекты используют локальную базу, показанную в Visual Studio:

```text
Server=(localdb)\\MSSQLLocalDB;Database=model;Trusted_Connection=True;TrustServerCertificate=True
```

Перед запуском выполните [`database/model.sql`](database/model.sql) в окне запроса SQL Server Object Explorer. Скрипт идемпотентен: он создаёт недостающие таблицы и начальные данные, но не удаляет существующие данные.

Можно не хранить строку подключения в коде: задайте переменную среды `COLLEGE_DB_CONNECTION`. Она имеет приоритет над значением по умолчанию.

## Карта уроков

| Урок | Тема | Проект |
| --- | --- | --- |
| 01–02 | Python: mssql / pyodbc | исходные скрипты |
| 03 | `SqlConnection`, `SqlCommand`, `SqlDataReader` | исходный пример |
| 04 | CRUD и параметризованные запросы | исходный пример |
| 05 | База College: чтение студентов | `lesson_05` |
| 06 | Создание групп и студентов | `lesson_06` |
| 07 | Model + Repository | `lesson_07` |
| 08 | Репозиторий групп | `lesson_08` |
| 09 | Dapper: чтение | `lesson_09` |
| 10 | Dapper: создание и возврат `Id` | `lesson_10` |
| 11 | ASP.NET Core Web API | `lesson_11` |
| 12 | Swagger и тестовый endpoint | `lesson_12` |

Откройте `ADO_NET_Course.sln` в Visual Studio. Для пакетов выполните Restore NuGet Packages, затем выберите нужный проект запуска.
