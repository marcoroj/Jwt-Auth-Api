# JWT Authentication API - ASP.NET Core

API REST desarrollada con **ASP.NET Core** que implementa autenticación segura utilizando **JWT (JSON Web Tokens)** junto con **Refresh Tokens con rotación y revocación**.
El proyecto utiliza **ASP.NET Core Identity** para la gestión de usuarios y control de acceso mediante **claims y políticas de autorización**.

---

# Tecnologías utilizadas

* ASP.NET Core Web API
* ASP.NET Core Identity
* Entity Framework Core
* SQL Server
* JWT Authentication
* Refresh Tokens
* SHA256 Hashing
* Authorization Policies

---

# Características

* Registro de usuarios
* Inicio de sesión con generación de **Access Token**
* Generación y almacenamiento de **Refresh Tokens**
* **Hashing de Refresh Tokens** usando SHA256
* **Rotación de Refresh Tokens**
* **Revocación de tokens usados**
* Protección de endpoints mediante **JWT**
* Control de acceso con **Claims y Policies**
* Endpoint para asignar o remover permisos de administrador

---

# Endpoints principales

### Registro de usuario

POST

```
/api/usuarios/registro
```

Body ejemplo:

```
{
  "email": "usuario@email.com",
  "password": "Password123!"
}
```

---

### Login

POST

```
/api/usuarios/login
```

Devuelve:

* Access Token
* Refresh Token

---

### Refresh Token

POST

```
/api/usuarios/refresh-token
```

Permite generar un nuevo **Access Token** utilizando un **Refresh Token válido**.

---

### Listar usuarios (solo admin)

GET

```
/api/usuarios
```

Requiere política de autorización **admin**.

---

### Hacer administrador

POST

```
/api/usuarios/hacer-admin
```

---

### Remover administrador

POST

```
/api/usuarios/remover-admin
```



# Estructura del proyecto

```
Controllers
Servicios
Entities
Models (DTOs)
Data (DbContext)
```

El proyecto sigue una separación básica de responsabilidades entre **controladores, servicios y modelos de datos**.


