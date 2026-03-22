# Enterprise Asset Management API

Esta es una Web API robusta desarrollada con **.NET 10** y **PostgreSQL**, diseñada para gestionar empresas y sus códigos asociados siguiendo los principios de **Arquitectura Limpia (Clean Architecture)** y **Código Limpio (Clean Code)**.

## Requisitos Previos

- [.NET SDK 10.0+](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) ejecutándose localmente.

## Configuración y Ejecución

1.  **Base de Datos**: 
    Asegúrate de ajustar los parámetros de conexión en `appsettings.json` si tu servidor local de PostgreSQL no usa el usuario `postgres/postgres`. La aplicación creará automáticamente la base de datos `EnterpriseDB`.

2.  **Ejecutar Migraciones**:
    Desde la terminal en el directorio raíz, ejecuta el siguiente comando para crear las tablas:
    ```bash
    dotnet ef database update
    ```

3.  **Arrancar la API**:
    Ejecuta el servidor con:
    ```bash
    dotnet run
    ```
    La API estará disponible por defecto en: **http://localhost:5196** (HTTP) y/o **https://localhost:7061** (HTTPS).

## Guía de Endpoints (API Reference)

A continuación se detallan las rutas disponibles para probar la funcionalidad de acuerdo a los requerimientos:

### Entidad: Empresa (Enterprise)
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| **GET** | `/api/enterprises` | **Recuperar todas las empresas**. |
| **GET** | `/api/enterprises/{id}` | Recuperar una empresa por su ID único. |
| **POST** | `/api/enterprises` | Crear una nueva empresa (**JSON Payload**). |
| **PATCH** | `/api/enterprises/{id}` | Modificar una empresa específica por ID. |
| **POST** | `/api/enterprises/{id}` | Modificar una empresa específica (Cumplimiento literal). |
| **GET** | `/api/enterprises/{id}/codes` | **Recuperar todos los códigos asignados a la empresa con ese ID**. |
| **GET** | `/api/enterprises/by-nit/{nit}` | **Empresa con un NIT específico y sus códigos asociados**. |

**Ejemplo JSON (POST):**
```json
{ "name": "BJR Soluciones", "nit": 12345, "gln": 67890 }
```

### Entidad: Código (Code)
| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| **GET** | `/api/codes/{id}` | Recuperar un código específico por su ID. |
| **POST** | `/api/codes` | Crear un nuevo código con relación a una empresa (vía **int ownerId**). |
| **PATCH** | `/api/codes/{id}` | Modificar un código específico por ID. |
| **POST** | `/api/codes/{id}` | Modificar un código específico (Cumplimiento literal). |
| **GET** | `/api/codes/{id}/enterprise` | **Información de la empresa dueña del código, usando el ID del código**. |

**Ejemplo JSON (POST):**
```json
{ "ownerId": 1, "name": "Código Inventario", "description": "Uso interno" }
```

## Pruebas e Interacción
... (Continúa igual)

## Detalles de Arquitectura

- **Separación de Concern**: Las responsabilidades están divididas en carpetas `Entities`, `DTOs`, `Data` (Context) y `Controllers`.
- **Previsión de Ciclos**: La API utiliza DTOs específicos para evitar problemas de recursividad en la serialización JSON de las relaciones uno-a-muchos.

