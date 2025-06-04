# MoneyManagerV2

## Opis projektus
MoneyManager to zestaw mikroserwisów do zarządzania budżetem osobistym. (W trakcie realizacji)
Aplikacja oparta jest o .NET 9 i wzorzec cleanArchitecture.
Projekt ma na celu umożliwienie użytkownikom śledzenia swoich wydatków, zarządzania budżetami oraz kategoryzowania transakcji, ale też dla mnie rozwój z zakresu .NET, wzorców projektowych oraz mikroserwisów.
Zawiera m.in.:  
- zarządzanie kategoriami wydatków,  
- zarządzanie budżetami,  
- wykorzystanie eventów domenowych,  
- dokumentację API dzięki Swagger.
- komunikację grpc

## Technologie
- **.NET 9**  
- **Dapper**  
- **ASP.NET Core Minimal APIs**  
- **Swagger** (Swashbuckle)  
- **RabbitMQ** do kolejkowania eventów
- **gRPC** – komunikacja między mikroserwisami

## Struktura projektu
- `MoneyManager` - serwis odpowiedzialny za komunikację klienta z serwerem - API
- `UserService` - serwis odpowiedzialny za zarządzanie użytkownikami'

## Najbliższe plany
- Dodanie wzorca Outbox z RabbitMq, zapis zdarzenia do tabeli - gwarancja ze event nie przepadnie w razie awarii
- Rozszerzenie walidacji i bezpieczeństwa
- Rozbudowa funkcjonalności zarządzania budżetem
- Dodanie powiadomień mailowych (w celu wykorzystania kolejki rabbit) np. budzet zostaje osiągnięty to wyślij powiadomienie
- Rozszerzenie funkcjonalności UserService o możliwość rejestracji użytkownika, logowania, resetowania hasła
- Dodanie testów jednostkowych i integracyjnych

## Uruchamianie i wdrożenie

Projekt docelowo będzie uruchamiany w środowisku Kubernetes (K8s) z wykorzystaniem kontenerów Docker. Pozwoli to na łatwe skalowanie, zarządzanie i wdrażanie mikroserwisów.

**Plany na przyszłość:**
- Przygotowanie plików `Dockerfile` dla każdego serwisu
- Konfiguracja manifestów Kubernetes (`deployment.yaml`, `service.yaml`)
- Integracja z CI/CD (np. GitHub Actions)
- Monitoring i logowanie (np. Prometheus, Grafana, ELK)

W przyszłości pojawi się szczegółowa instrukcja uruchamiania projektu w klastrze Kubernetes.

## Kontakt
Masz pytania lub chcesz współpracować?
Napisz do mnie na [damian.radomski9908@gmail.com] lub stwórz issue w repozytorium.