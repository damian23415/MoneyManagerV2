# MoneyManager

## Opis projektu

MoneyManager to mikroserwis do zarządzania budżetem osobistym. (W trakcie realizacji)
Aplikacja oparta jest o .NET 9 i wzorce Clean Architecture.  
Zawiera m.in.:  
- zarządzanie kategoriami wydatków,  
- zarządzanie budżetami,  
- wykorzystanie eventów domenowych,  
- przykładowe testy jednostkowe,  
- dokumentację API dzięki Swagger.

## Technologie

- .NET 9  
- Entity Framework Core  
- ASP.NET Core Minimal APIs  
- Swagger (Swashbuckle)  
- InMemory Database / SQL Server (konfigurowalne)
- RabbitMQ do kolejkowania eventów

## Testy jednostkowe

Testy znajdują się w projekcie `MoneyManager.Tests`.  
Uruchom testy poleceniem:  
```bash
dotnet test
```

## Struktura projektu
- MoneyManager.Domain – encje i interfejsy (logika biznesowa)
- MoneyManager.Application – serwisy aplikacyjne, DTO, logika aplikacyjna
- MoneyManager.Infrastructure – implementacja repozytoriów, EF Core
- MoneyManager.API – warstwa API (kontrolery/endpointy)
- MoneyManager.Tests – testy jednostkowe

## Najbliższe plany
- Dodanie wzorca Outbox z RabbitMq, zapis zdarzenia do tabeli - gwarancja ze event nie przepadnie w razie awarii
- Rozszerzenie walidacji i bezpieczeństwa
- Rozbudowa funkcjonalności zarządzania budżetem
- Dodanie powiadomień mailowych (w celu wykorzystania kolejki rabbit) np. budzet zostaje osiągnięty to wyślij powiadomienie

## Kontakt
Masz pytania lub chcesz współpracować?
Napisz do mnie na [damian.radomski9908@gmail.com] lub stwórz issue w repozytorium.