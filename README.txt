===========================================================
  RunningEventsSystem – System zarządzania biegami
===========================================================

SKŁAD GRUPY PROJEKTOWEJ:
  [Uzupełnij nazwiska]

===========================================================
ARCHITEKTURA SYSTEMU
===========================================================

Rozwiązanie składa się z 7 projektów .NET 6:

  RunningEventsSystem.SharedKernel    – DTO, elementy wspólne
  RunningEventsSystem.Domain          – Modele, interfejsy repozytoriów
  RunningEventsSystem.Application     – Serwisy aplikacyjne (logika biznesowa)
  RunningEventsSystem.Infrastructure  – EF Core + SQLite, repozytoria, UoW
  RunningEventsSystem.WebAPI          – ASP.NET Core WebAPI (REST)
  RunningEventsSystem.BlazorWasm      – Blazor WebAssembly (klient / uczestnik)
  RunningEventsSystem.BlazorServer    – Blazor Server (panel administratora)

===========================================================
WYMAGANIA WSTĘPNE
===========================================================

  - .NET 6 SDK (https://dotnet.microsoft.com/download/dotnet/6.0)
  - Visual Studio 2022 lub VS Code z rozszerzeniem C#

===========================================================
URUCHAMIANIE
===========================================================

1. Uruchom WebAPI (musi być pierwsze):
   cd RunningEventsSystem.WebAPI
   dotnet run
   → Domyślnie: https://localhost:7001  (lub http://localhost:5001)

2. Uruchom Blazor Server (panel admina):
   cd RunningEventsSystem.BlazorServer
   dotnet run
   → Domyślnie: https://localhost:7002

3. Uruchom Blazor WebAssembly (klient):
   cd RunningEventsSystem.BlazorWasm
   dotnet run
   → Domyślnie: https://localhost:7003

Uwaga: Port WebAPI można zmienić w:
  - BlazorServer/appsettings.json  → "ApiBaseUrl"
  - BlazorWasm/Program.cs          → BaseAddress w HttpClient

===========================================================
BAZA DANYCH (SQLite)
===========================================================

Baza tworzy się automatycznie przy pierwszym uruchomieniu WebAPI.
Plik: RunningEventsSystem.WebAPI/running_events.db

Tabele (≥ 5):
  Users           – Użytkownicy (uczestnicy + administratorzy)
  Events          – Biegi / eventy
  Registrations   – Zapisy uczestników na biegi
  Results         – Wyniki biegów
  Sponsors        – Sponsorzy
  EventSponsors   – Powiązanie sponsor–event (tabela pośrednia)

===========================================================
KONTA DOMYŚLNE (seed)
===========================================================

  Admin:
    e-mail:  admin@running.pl
    hasło:   Admin123

  Uczestnik testowy:
    e-mail:  jan.kowalski@example.com
    hasło:   Test123

===========================================================
LOGOWANIE
===========================================================

WebAPI i BlazorServer zapisują logi do katalogu Logs/:
  - Logs/webapi-YYYYMMDD.log          – informacje ogólne
  - Logs/webapi-errors-YYYYMMDD.log   – tylko błędy
  - Logs/blazorserver-YYYYMMDD.log
  - Logs/blazorserver-errors-YYYYMMDD.log

Nowy plik tworzony każdego dnia (RollingInterval.Day).

===========================================================
FUNKCJONALNOŚCI
===========================================================

Blazor WebAssembly (klient / uczestnik):
  ✓ Rejestracja nowego konta
  ✓ Logowanie / wylogowanie
  ✓ Przeglądanie listy biegów (filtry: miasto, nazwa, aktywność)
  ✓ Szczegóły biegu (opis, data, opłata, wyniki)
  ✓ Zapis na bieg
  ✓ Wypisanie się z biegu
  ✓ Symulacja płatności
  ✓ Lista własnych zapisów (tabela z sortowaniem i paginacją)
  ✓ Wyniki biegów (podgląd)

Blazor Server (administrator):
  ✓ Pulpit z podsumowaniem statystyk
  ✓ Zarządzanie biegami: CRUD + filtry + sortowanie
  ✓ Zarządzanie uczestnikami: lista, filtrowanie, sortowanie, usuwanie
  ✓ Podgląd szczegółów uczestnika (z listą jego zapisów)
  ✓ Zarządzanie wynikami: dodawanie wyników dla biegu

===========================================================
BIBLIOTEKI UI
===========================================================

  BlazorWasm:   MudBlazor 6.x  (karty, tabele, dialogi, formularze)
  BlazorServer: Radzen.Blazor 4.x (DataGrid, Dialog, Notification, Menu)

===========================================================
