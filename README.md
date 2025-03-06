# **Order Management Microservice**
Этот микросервис предназначен для управления заказами и продуктами, используя **Clean Architecture**, **CQRS**, **Entity Framework Core** и **PostgreSQL** в качестве базы данных.

## **Технологии**
- **.NET 8**  
- **ASP.NET Core**  
- **Entity Framework Core** (PostgreSQL)  
- **MediatR** (CQRS)  
- **FluentValidation**  
- **XUnit + Moq** (для тестирования)  

## **Установка и запуск**
### **1. Клонирование репозитория**
```sh
git clone https://github.com/MagzhanJSoft/orderingProducts.git
cd orderingProducts

##Создание базы данных
CREATE DATABASE order_db;
##Настройка переменных окружения
Перед запуском укажите строку подключения в appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=order_db;Username=postgres;Password=yourpassword"
}
