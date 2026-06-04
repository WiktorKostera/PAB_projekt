RunningEventsSystem - system zarzadzania biegami
=================================================

Sklad grupy projektowej
-----------------------

1. Wiktor Kostera
2. Radoslaw Ciemiega

Opis projektu
-------------

Projekt jest aplikacja biznesowa do obslugi wydarzen biegowych. System pozwala
na zarzadzanie biegami, uczestnikami, zapisami oraz wynikami. Czesc dla
administratora jest przygotowana w Blazor Server, a czesc dla uczestnika w
Blazor WebAssembly.

W projekcie zastosowano baze SQLite oraz podzial na warstwy zgodnie z zalozeniami
czystej architektury.

Wymagania do uruchomienia
-------------------------

- .NET 6 SDK
- Visual Studio 2022
- Przy pierwszym uruchomieniu Visual Studio moze pobrac brakujace pakiety NuGet.
  Jezeli pakiety nie odtworza sie automatycznie, nalezy wykonac Restore NuGet
  Packages dla calego rozwiazania.

Projekty startowe
-----------------

W Visual Studio najlepiej ustawic kilka projektow startowych:

1. RunningEventsSystem.WebAPI
2. RunningEventsSystem.BlazorServer
3. RunningEventsSystem.BlazorWasm

WebAPI powinno wystartowac na porcie:

http://localhost:5234

Panel administratora Blazor Server:

http://localhost:55766

Panel uczestnika Blazor WebAssembly:

http://localhost:55803

Adres API jest ustawiony w plikach:

- BlazorServer/appsettings.json
- BlazorWasm/wwwroot/appsettings.json

Oba powinny wskazywac na:

http://localhost:5234/

Baza danych
-----------

System korzysta z bazy SQLite. Plik bazy znajduje sie w projekcie WebAPI:

RunningEventsSystem.WebAPI/runningevents.db

Baza jest tworzona i uzupelniana danymi startowymi przy uruchomieniu WebAPI.
W projekcie uzywane sa m.in. tabele:

- Users
- Events
- Registrations
- Results
- Sponsors
- EventSponsors

Konta i dane zapisane w bazie
-----------------------------

Przykladowe konta uzytkownikow tworzone przez seeder:

- Anna Nowak, anna@test.pl, haslo: test123
- Piotr Wisniewski, piotr@test.pl, haslo: test123
- Katarzyna Wojcik, kasia@test.pl, haslo: test123
- Marek Zielinski, marek@test.pl, haslo: test123

Konto administratora:

- Piotr Wisniewski, piotr@test.pl, haslo: test123

W bazie sa takze przykladowe biegi:

- Cracovia Marathon 2026
- Warsaw Night Run 2026
- Poznan Half Marathon 2024
- Gdansk 10km 2024

Dwa starsze biegi maja przygotowane zapisy uczestnikow oraz wyniki z czasami.
Dwa nowsze biegi sa aktywne i widoczne po stronie uczestnika jako biegi do zapisu.

Architektura projektu
---------------------

Rozwiazanie sklada sie z nastepujacych projektow:

- RunningEventsSystem.Domain - modele domenowe i interfejsy repozytoriow
- RunningEventsSystem.Application - logika aplikacyjna, serwisy, walidatory
- RunningEventsSystem.Infrastructure - Entity Framework Core, SQLite, repozytoria, Unit of Work, seeder
- RunningEventsSystem.WebAPI - kontrolery REST API i Swagger
- RunningEventsSystem.BlazorServer - panel administratora
- RunningEventsSystem.BlazorWasm - panel uczestnika
- RunningEventsSystem.SharedKernel - DTO i elementy wspolne

Logowanie
---------

WebAPI oraz Blazor Server korzystaja z Serilog. Logi sa zapisywane w katalogach
Logs odpowiednich projektow. System tworzy osobne pliki na kazdy dzien oraz
osobne pliki dla bledow:

- webapi-.log
- webapi-errors-.log
- blazorserver-.log
- blazorserver-errors-.log

Najwazniejsze funkcje
---------------------

Blazor WebAssembly:

- rejestracja i logowanie uczestnika
- lista biegow
- szczegoly wybranego biegu
- zapis na bieg po oplaceniu
- podglad wlasnych zapisow
- anulowanie zapisu
- podglad wynikow

Blazor Server:

- pulpit administratora
- lista biegow
- dodawanie, edycja i usuwanie biegow
- lista uczestnikow
- usuwanie uczestnikow
- podglad i dodawanie wynikow

Biblioteki UI
-------------

W projekcie wykorzystano zewnetrzne biblioteki komponentow:

- Radzen.Blazor w panelu Blazor Server
- MudBlazor w panelu Blazor WebAssembly

W interfejsie sa wykorzystywane obrazy z katalogow wwwroot/images.

Uwagi
-----

Projekt jest przygotowany pod .NET 6. W przypadku problemow z portami nalezy
zamknac poprzednie uruchomione procesy WebAPI lub Blazor i uruchomic rozwiazanie
ponownie z Visual Studio.
