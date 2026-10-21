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

## Веб-приложение «Колледж»

Проект `src/WebApplication1` содержит адаптированный код из [DozFire-College/ADO.NET, Only-Code-Files](https://github.com/DozFire-College/ADO.NET/tree/Only-Code-Files) (коммит `3aaddb085244fde566c25befa0fb6c316e693006`).

В Visual Studio 2022 откройте `ADO_NET_Course.sln`, назначьте `WebApplication1` запускаемым проектом, выберите профиль **http** и нажмите F5. Страница доступна по адресу http://localhost:5058/.

Или из корня репозитория:

```powershell
dotnet run --project src/WebApplication1 --launch-profile http
```

Нужны .NET 8 и SQL Server Express LocalDB. Приложение использует отдельную базу `WebApplication1_College_DB` и автоматически создаёт её, таблицы и три начальные группы при первом запуске. Строка подключения задаётся в `src/WebApplication1/appsettings.json` (`ConnectionStrings:CollegeDB`) или переменной среды `ConnectionStrings__CollegeDB`.

Работают списки студентов и групп, добавление студента с проверкой полей и выбором группы. Исправлены SQL-запросы, получение ID, считывание формы и обработка ошибок. Приложение использует Dapper и Microsoft.Data.SqlClient.
