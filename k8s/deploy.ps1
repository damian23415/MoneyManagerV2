<#
.SYNOPSIS
  Skrypt PowerShell do:
    1) Budowania obrazów Dockerowych
    2) Ładowania ich do klastra kind
    3) Wdrażania RabbitMQ + mikroserwisów w Kubernetes
    4) Restartowania deploymentów, aby pobrały nowe obrazy

.DESCRIPTION
  Zakłada, że:
    - Folder roboczy skryptu to C:\dev\MoneyManagerV2\k8s
    - Usługi są w C:\dev\MoneyManagerV2\services\...
    - Masz zainstalowanego Dockera, kind, kubectl w PATH
    - Claster kind jest już utworzony (polecenie "kind create cluster" było wykonane wcześniej)
    - Pliki manifestów (rabbitmq-deployment.yaml i deployments.yaml) leżą w tym samym folderze co ten skrypt

.NOTES
  Jeśli 'kind' nie jest w PATH w PowerShell, upewnij się, że:
    1) Masz kind zainstalowany przez Chocolatey lub Scoop, albo ręcznie
    2) PATH w ustawieniach systemu Windows zawiera katalog, w którym zainstalowano kind.exe
#>

# Zatrzymaj skrypt, jeśli którykolwiek krok się nie powiedzie
environment: ErrorActionPreference = 'Stop'

Write-Host "=== 1) Budowanie obrazów Dockerowych ===" -ForegroundColor Cyan

# 1.1 Buduj MoneyManager.API
Write-Host "Buduję obraz money-manager-api..."
docker build `
  -t money-manager-api:latest `
  -f ../services/MoneyManager/MoneyManager.API/Dockerfile `
  ../services/MoneyManager

# 1.2 Buduj MoneyManager.EventProcessor
Write-Host "Buduję obraz money-manager-eventprocessor..."
docker build `
  -t money-manager-eventprocessor:latest `
  -f ../services/MoneyManager/MoneyManager.EventProcessor/Dockerfile `
  ../services/MoneyManager

# 1.3 Buduj UserService
Write-Host "Buduję obraz user-service..."
docker build `
  -t user-service:latest `
  -f ../services/UserService/Dockerfile `
  ../services/UserService

Write-Host "`n=== 2) Ładowanie obrazów do klastra kind ===" -ForegroundColor Cyan

# 2. Ładuj obrazy do kind
Write-Host "Ładuję money-manager-api:latest do kind..."
kind load docker-image money-manager-api:latest

Write-Host "Ładuję money-manager-eventprocessor:latest do kind..."
kind load docker-image money-manager-eventprocessor:latest

Write-Host "Ładuję user-service:latest do kind..."
kind load docker-image user-service:latest

Write-Host "`n=== 3) Wdrażanie RabbitMQ i mikroserwisów w Kubernetes ===" -ForegroundColor Cyan

# 3.1 Deploy RabbitMQ (plik rabbitmq-deployment.yaml w tym folderze)
Write-Host "Wdrażam RabbitMQ..."
kubectl apply -f .\rabbitmq-deployment.yaml

# 3.2 Deploy mikroserwisy (plik deployments.yaml)
Write-Host "Wdrażam mikroserwisy..."
kubectl apply -f .\deployments.yaml

Write-Host "`n=== 4) Restartowanie deploymentów ===" -ForegroundColor Cyan

# 4. Restart Deploymentów\Write-Host "Restartuję deployment money-manager-api..."
kubectl rollout restart deployment money-manager-api

Write-Host "Restartuję deployment money-manager-eventprocessor..."
kubectl rollout restart deployment money-manager-eventprocessor

Write-Host "Restartuję deployment user-service..."
kubectl rollout restart deployment user-service

