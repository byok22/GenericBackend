# Rutas del proyecto (Resumen)

Fecha de generación: 2025-12-03

Archivo fuente: `src/app/app.routes.ts`

Resumen rápido: ruta, componente cargado (lazy), guard(s) y permiso (si aplica).

---

- **/**: redirect a `presentation/dashboard` (pathMatch: `full`).

- **/presentation**
  - Component: `MasterPageComponent` (cargado con `loadComponent`)
  - Guard: `authGuard` (requiere autenticación)
  - Hijos:
    - **dashboard**
      - Component: `DashboardComponent`
      - Guard: heredado `authGuard` (no `roleGuard` configurado)
      - Permission: (ninguna declarada)

    - **projects**
      - Component: `ProjectsComponent`
      - Guard: `roleGuard`
      - Permission: `Change Project`

    - **editor**
      - Component: `EditorComponent`
      - Guard: (ninguno en `data`) — accesible para usuarios autenticados

    - **create**
      - Component: `CreaterComponent`
      - Guard: (ninguno en `data`)

    - **view**
      - Component: `ViewComponent`
      - Guard: (ninguno en `data`)

    - **tree-menu**
      - Component: `TreeMenuComponent`
      - Guard: (ninguno en `data`)

    - **projects-catalog**
      - Component: `ProjectPageComponent` (en `projects-list/page`)
      - Guard: `roleGuard`
      - Permission: `Projects`

    - **users-catalog**
      - Component: `UsersPageComponent`
      - Guard: `roleGuard`
      - Permission: `Users`

    - **role-catalog**
      - Component: `RolesPageComponent`
      - Guard: `roleGuard`
      - Permission: `Roles`

    - **status-catalog**
      - Component: `StatusPageComponent`
      - Guard: `roleGuard`
      - Permission: `Status`

    - **blockType-catalog**
      - Component: `BlockTypesPageComponent`
      - Guard: `roleGuard`
      - Permission: `BlockTypes`

    - **app-screens**
      - Component: `AppScreensPageComponent` (en `app-screens`)
      - Guard: `roleGuard`
      - Permission: `Screens`

    - **app-permissions**
      - Component: `AdminScreenRoleComponent` (en `admin-screen-role`)
      - Guard: `roleGuard`
      - Permission: `Permissions`

- **/login**
  - Component: `LoginComponent` (en `src/app/shared/pages/login`)
  - Acceso: público (ruta fuera del área `presentation`)

- **/*** (wildcard)
  - Redirect: `login` (cualquier ruta no definida redirige a `/login`)

---

Notas importantes y recomendaciones:

- Las rutas bajo `/presentation` están protegidas por `authGuard` y la mayoría de catálogos/áreas administrativas usan `roleGuard` con `data.permission` para validación en cliente.
- El `roleGuard` actualmente lee permisos desde `localStorage` (`authService.getUserPermissions()`), por lo que:
  - Mantén la validación real de permisos también en el backend (no confiar sólo en el cliente).
  - Si no hay `data.permission` la ruta se permite (comportamiento actual). Considerar invertir la política (denegar por defecto) para mayor seguridad.

- Para editar/añadir rutas: modificar `src/app/app.routes.ts`. Para agregar ítems de menú, revisa el servicio que entrega el menú (`AppScreenService.getSideMenu()`) y/o los assets/configs que consuma.

- Para una vista de permisos más formal, sugiero generar un archivo `PERMISSIONS.md` o un endpoint `/config/permissions` que liste claves de permiso (ej: `Change Project`, `Projects`, `Users`, ...).

---

¿Quieres que genere automáticamente un `PERMISSIONS.md` con las claves que aparecen en las rutas, o que busque y muestre el archivo/endpoint que devuelve el menú/herencia de permisos (`AppScreenService`)?
