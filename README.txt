RunningEventsSystem - system zarządzania biegami
=================================================

Skład grupy projektowej
-----------------------

1. Wiktor Kostera
2. Radosław Ciemięga

Opis projektu
-------------

Projekt jest aplikacją biznesową do obsługi wydarzeń biegowych. System pozwala
na zarządzanie biegami, uczestnikami, zapisami oraz wynikami. Część dla
administratora jest przygotowana w Blazor Server, a część dla uczestnika w
Blazor WebAssembly.

W projekcie zastosowano bazę SQLite oraz podział na warstwy zgodnie z założeniami
czystej architektury.

Wymagania do uruchomienia
-------------------------

- .NET 6 SDK
- Visual Studio 2022
- Przy pierwszym uruchomieniu Visual Studio może pobrać brakujące pakiety NuGet.
  Jeżeli pakiety nie odtworzą się automatycznie, należy wykonać Restore NuGet
  Packages dla całego rozwiązania.

Projekty startowe
-----------------

W Visual Studio najlepiej ustawić kilka projektów startowych:

1. RunningEventsSystem.WebAPI
2. RunningEventsSystem.BlazorServer
3. RunningEventsSystem.BlazorWasm

WebAPI powinno wystartować na porcie:

http://localhost:5234

Panel administratora Blazor Server:

http://localhost:55766

Panel uczestnika Blazor WebAssembly:

http://localhost:55803

Adres API jest ustawiony w plikach:

- BlazorServer/appsettings.json
- BlazorWasm/wwwroot/appsettings.json

Oba powinny wskazywać na:

http://localhost:5234/

Baza danych
-----------

System korzysta z bazy SQLite. Plik bazy znajduje się w projekcie WebAPI:

RunningEventsSystem.WebAPI/runningevents.db

Baza jest tworzona i uzupełniana danymi startowymi przy uruchomieniu WebAPI.
W projekcie używane są m.in. tabele:

- Users
- Events
- Registrations
- Results
- Sponsors
- EventSponsors

Konta i dane zapisane w bazie
-----------------------------

Przykładowe konta użytkowników tworzone przez seeder:

- Anna Nowak, anna@test.pl, hasło: test123
- Piotr Wiśniewski, piotr@test.pl, hasło: test123
- Katarzyna Wójcik, kasia@test.pl, hasło: test123
- Marek Zieliński, marek@test.pl, hasło: test123

W bazie są także przykładowe biegi:

- Cracovia Marathon 2026
- Warsaw Night Run 2026
- Poznań Half Marathon 2024
- Gdańsk 10km 2024

Dwa starsze biegi mają przygotowane zapisy uczestników oraz wyniki z czasami.
Dwa nowsze biegi są aktywne i widoczne po stronie uczestnika jako biegi do zapisu.

Architektura projektu
---------------------

Rozwiązanie składa się z następujących projektów:

- RunningEventsSystem.Domain - modele domenowe i interfejsy repozytoriów
- RunningEventsSystem.Application - logika aplikacyjna, serwisy, walidatory
- RunningEventsSystem.Infrastructure - Entity Framework Core, SQLite, repozytoria, Unit of Work, seeder
- RunningEventsSystem.WebAPI - kontrolery REST API i Swagger
- RunningEventsSystem.BlazorServer - panel administratora
- RunningEventsSystem.BlazorWasm - panel uczestnika
- RunningEventsSystem.SharedKernel - DTO i elementy wspólne

Logowanie
---------

WebAPI oraz Blazor Server korzystają z Serilog. Logi są zapisywane w katalogach
Logs odpowiednich projektów. System tworzy osobne pliki na każdy dzień oraz
osobne pliki dla błędów:

- webapi-.log
- webapi-errors-.log
- blazorserver-.log
- blazorserver-errors-.log

Najważniejsze funkcje
---------------------

Blazor WebAssembly:

- rejestracja i logowanie uczestnika
- lista biegów
- szczegóły wybranego biegu
- zapis na bieg po opłaceniu
- podgląd własnych zapisów
- anulowanie zapisu.

Blazor Server:

- pulpit administratora
- lista biegów
- dodawanie, edycja i usuwanie biegów
- lista uczestników
- usuwanie uczestników
- podgląd wyników.

Biblioteki UI
-------------

W projekcie wykorzystano zewnętrzne biblioteki komponentów:

- Radzen.Blazor w panelu Blazor Server
- MudBlazor w panelu Blazor WebAssembly

W interfejsie są wykorzystywane obrazy z katalogów wwwroot/images.