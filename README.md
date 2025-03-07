# 🚗 BACHETON

## 📖 Índice

1. [📌 Problemática](#-problemática)
2. [🎯 Objetivo](#-objetivo)
3. [👥 Integrantes](#-integrantes)
4. [🛠 Tecnologías y Librerías](#-tecnologías-y-librerías)
5. [🚀 Cómo Ejecutar el Proyecto](#-cómo-ejecutar-el-proyecto)
6. [🤓 Modelo de Trabajo](#-modelo-de-trabajo)
7. [📢 Contribución](#-contribución)
8. [📧 Contacto](#-contacto)

---

## 📌 Problemática

En muchas ciudades, los baches en las vías representan un problema constante que afecta la seguridad vial y la calidad de vida de los ciudadanos. Los métodos tradicionales de reporte, como llamadas telefónicas o formularios en papel, son lentos y poco accesibles, lo que genera:

- **Aumento de riesgos**: Mayor peligro para conductores y peatones.
- **Deterioro de la infraestructura**: La falta de respuesta oportuna agrava el problema.
- **Asignación ineficiente de recursos**: Dificultad para priorizar reparaciones.
- **Débil comunicación**: Escasa conexión entre la ciudadanía y las autoridades.

## 🎯 Objetivo

Desarrollar una **aplicación web** que permita a los ciudadanos **reportar y dar seguimiento a baches** en su ciudad, facilitando su geolocalización y priorización para que las autoridades puedan planificar reparaciones oportunas.

## 👥 Integrantes

| Nombre | Rol | GitHub                                  | Correo Institucional     |  
|--------|-----|-----------------------------------------|--------------------------|  
| **Rodrigo Bojorquez Chi** | Líder del Proyecto / Desarrollador Fullstack | [rodrigobojorquez](https://github.com/rodrigoBojorquez) | 22393127@utcancun.edu.mx |  
| **Andrés García Leyva** | Desarrollador Fullstack | [GitHub](https://github.com/)           | correo@institucion.edu   |  
| **Dilan Gabriel Och Montero** | Desarrollador Frontend | [GitHub](https://github.com/)           | correo@institucion.edu   |  
| **Juan Carlos López Can** | Desarrollador Frontend | [GitHub](https://github.com/)           | correo@institucion.edu   |  
| **Ricardo Jesús Chi Chan** | Desarrollador Frontend | [RicardojCh10](https://github.com/RicardojCh10)   | 22393166@utcancun.edu.mx   |  

## 🛠 Tecnologías y Librerías

### 📌 Backend
- **ASP.NET Core** - API RESTful
- **Entity Framework Core** - ORM para persistencia de datos
- **JWT (JSON Web Tokens)** - Autenticación y autorización de usuarios
- **PostgreSQL** - Base de datos relacional (Producción)
- **SQLite** - Base de datos embebida (Desarrollo)

### 🎨 Frontend
- **Vue 3** - Framework para el desarrollo de la interfaz de usuario
- **Vue Router** - Manejo de navegación entre vistas
- **Axios** - Comunicación con el backend
- **Prime Vue** - Estilización rápida y responsiva
- **Vee Validate** - Validación de formularios
- **Vue Query** - Manejo de peticiones y caché de datos
- **Vue Toastification** - Notificaciones de alerta

### 🗺️ Integración de Mapas
- **Google Maps API** o **Leaflet** - Visualización y geolocalización de baches

### 📌 Gestión del Código
- **Git con Git Flow** - Control de versiones y manejo de ramas

## 🚀 Cómo Ejecutar el Proyecto

### 🔧 Backend (API con ASP.NET Core)

1. Clonar el repositorio:
   ```sh
   git clone https://github.com/usuario/bacheton.git
   cd bacheton-back
   ```  

2. Aplicar migraciones:
   ```sh
   dotnet ef database update -p Bacheton.Api
   ```  

3. Ejecutar la API:
   ```sh
   dotnet run
   ```  

---

## 🤓 Modelo de Trabajo

Para mantener una estructura organizada, utilizamos **Git Flow** como metodología de trabajo. Esto nos permite manejar mejor el desarrollo y la integración de nuevas funcionalidades.

### 🔹 Ramas principales
- **main**: Contiene el código en producción, estable y listo para su implementación.
- **develop**: Contiene el código en desarrollo, donde se integran nuevas características antes de fusionarlas en `main`.

### 🔹 Flujo de trabajo
1. Cada integrante crea su **rama de trabajo** a partir de `develop`.
2. Una vez terminada la funcionalidad, se realiza un **Pull Request (PR)** hacia `develop`.
3. El PR debe ser **revisado y aprobado** por al menos un integrante antes de fusionarlo.
4. Periódicamente, `develop` se fusiona con `main` para lanzar una nueva versión estable.

📌 **Reglas del equipo:**  
✅ Cada PR debe incluir una descripción clara del cambio realizado.  
✅ No se permite fusionar código directamente en `main` o `develop`.  
✅ Se deben realizar pruebas antes de enviar cambios para revisión.

---

## 📢 Contribución
La colaboración está limitada al equipo de desarrollo, pero si encuentras un error o tienes una idea para mejorar el proyecto, ¡puedes abrir un **issue** en el repositorio!

---

## 📧 Contacto

Si tienes dudas o sugerencias, puedes comunicarte con cualquiera de los integrantes del equipo.

---

© 2024 **BACHETON**. Todos los derechos reservados.  
