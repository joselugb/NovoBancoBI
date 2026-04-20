# Microservicio de Gestión de Cuentas y Transacciones – NovoBancoBI

## 1. Contexto
Este proyecto implementa un **microservicio backend bancario** para la gestión de cuentas y transacciones, destinado a ser consumido por aplicaciones móviles y web.

El dominio bancario requiere **consistencia, atomicidad, trazabilidad y correctitud**, ya que errores lógicos pueden generar pérdidas financieras o implicaciones regulatorias.

---

## 2. Arquitectura

Se aplica una arquitectura en capas inspirada en **Clean Architecture / Hexagonal**, favoreciendo la separación de responsabilidades y la mantenibilidad.
src/
├── Api              # Controllers, middleware y contratos HTTP
├── Application      # Casos de uso, servicios y DTOs
├── Domain           # Entidades, reglas de negocio y excepciones
└── Infrastructure   # EF Core, PostgreSQL y persistencia
tests/

### Responsabilidades
- **Domain**: reglas bancarias puras (saldo, estado de cuenta, validaciones).
- **Application**: orquestación de casos de uso (depósitos, retiros, transferencias).
- **Infrastructure**: acceso a datos (EF Core, PostgreSQL).
- **Api**: endpoints REST, manejo de errores HTTP.

---

## 3. Stack Tecnológico

- **.NET 8 / C#**
- **ASP.NET Core Web API**
- **PostgreSQL 15**
- **Entity Framework Core**
- **Docker / Docker Compose**
- **xUnit** (pruebas)

---

## 4. Reglas de Negocio Bancarias

### Saldo negativo
- Validado en dominio (`Debito`)
- Refuerzo a nivel de base de datos (`CHECK balance >= 0`)

### Cuenta inactiva
- Cuentas `BLOQUEADA` o `CERRADA` no pueden operar
- Se devuelven errores específicos

### Transferencias
- Operación **atómica**
- Si falla el crédito, el débito se revierte

### Concurrencia
- Uso de transacciones de base de datos
- Bloqueo de filas (PostgreSQL)

### Idempotencia
- Cada transacción tiene una referencia única
- Índice `UNIQUE` para evitar duplicados

---

## 5. Manejo de Errores

Se implementa un **middleware global de errores** que centraliza el manejo de excepciones.

### Formato estándar
json
{
  "error": "CuentaInactivaExcepcion",
  "message": "Cuenta no esta activa",
  "traceId": "00-acde1234"
}

## 6. Historial de Transacciones

- Endpoint paginado
- Ordenado por fecha descendente
- Optimizado mediante índices de base de datos

GET /api/cuentas/{idCuenta}/transacciones?pagina=1&cantidadPorPagina=20

## 7. Base de datos

El esquema se documenta en schema.sql, incluyendo:

- Claves primarias y foráneas
- Restricciones de integridad
- Índices explícitos
- La selección del motor, el diseño del esquema y las decisiones de modelado forman parte del entregable principal de la prueba técnica.
- El endpoint de historial está optimizado mediante un índice compuesto, permitiendo paginación eficiente incluso con grandes volúmenes de datos.

## 8. Pruebas
Se incluyen pruebas unitarias enfocadas en reglas bancarias críticas:

- Saldo insuficiente
- Cuenta bloqueada
- Transferencia atómica
- Idempotencia

Ejecución:
> dotnet test

## 9. Ejecucion local

Docker:
> docker compose up -d
> docker ps

Migraciones:
> dotnet tool run dotnet-ef migrations add InitialPostgres --project src/Infrastructure --startup-project src/Api
> dotnet tool run dotnet-ef database update --project src/Infrastructure --startup-project src/Api

API:
> dotnet run --project src/Api

Swagger:
http://localhost:5xxx/swagger

## 10. Supuestos y Limitaciones

- Todas las cuentas operan en USD
- No se implementa autenticación (fuera de alcance)
- No se incluyen comisiones, pero el diseño lo permite

## 11. Architecture Decision Records (ADR)
ADR‑001: Elección de PostgreSQL
- Contexto
-- Se requiere consistencia y transacciones ACID.
- Opciones
-- PostgreSQL, MySQL, MongoDB
- Decisión
-- PostgreSQL version 15
- Justificación técnica
-- Consistencia y transacciones ACID avanzadas: PostgreSQL ofrece transacciones ACID robustas, aislamiento configurable y MVCC, fundamentales para operaciones financieras (depósitos, retiros y transferencias) donde la integridad es crítica.
-- Soporte nativo de tipos avanzados y restricciones: Permite ENUMs, UUID, CHECK constraints e índices compuestos, facilitando un modelado expresivo y seguro del dominio.
-- Rendimiento y escalabilidad en consultas analíticas y operativas: Índices B‑Tree optimizados, índices parciales y planificación avanzada soportan eficientemente historiales paginados y búsquedas por referencia.
- Normalización vs. Desnormalización(ver schema.sql)
-- Se opta por normalización (3FN): accounts y transactions están separadas para evitar duplicación.
-- El saldo se mantiene como campo persistente en accounts por eficiencia de lectura.
- Índices
-- idx_transactions_account_date permite obtener los últimos movimientos de una cuenta de forma eficiente (ORDER BY + LIMIT).
-- Índice único por reference garantiza idempotencia y detección de duplicados.
-- Índices simples mantienen bajo costo de escritura.
- Historial paginado
-- El historial de transacciones se consulta por (account_id, created_at DESC) con LIMIT/OFFSET o keyset pagination (created_at < last_seen).

ADR‑002: Saldo como campo persistente
- Contexto
-- El historial puede crecer a millones de registros.
- Opciones
-- Calcular saldo desde transacciones
-- Guardar saldo como campo
- Decisión
-- Guardar saldo
- Consecuencias
-- Mayor rendimiento
-- Menos carga de cómputo
-- Reglas protegidas por constraints

ADR‑003: Lógica de negocio fuera de la BD
- Contexto
-- Evitar acoplamiento y facilitar pruebas.
- Opciones
-- Procedimientos almacenados
-- Lógica en dominio + transacciones
- Decisión
-- Lógica en dominio
- Consecuencias
-- Código testeable
-- Menor acoplamiento
-- Base de datos como guardián de integridad