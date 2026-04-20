-- Motor: PostgreSQL 16
-- Dejo este esquema como borrador puesto que creo las tablas y demas con EF


-- =======================================================
-- Schema SQL – Gestión de Cuentas y Transacciones
-- Motor: PostgreSQL 15
-- Dominio: Bancario
-- =======================================================

-- =========================
-- 1. Tipos Enumerados
-- =========================
CREATE TYPE account_type AS ENUM ('AHORROS', 'CORRIENTE');
CREATE TYPE account_status AS ENUM ('ACTIVA', 'BLOQUEADA', 'CERRADA');
CREATE TYPE transaction_type AS ENUM ('DEPOSITO', 'RETIRO', 'TRANSFERENCIA');
CREATE TYPE transaction_status AS ENUM ('COMPLETADA', 'FALLIDA', 'REVERSADA');

-- =========================
-- 2. Tabla: accounts
-- =========================
CREATE TABLE accounts (
    id UUID PRIMARY KEY,
    account_number VARCHAR(20) NOT NULL UNIQUE,
    type account_type NOT NULL,
    currency CHAR(3) NOT NULL DEFAULT 'USD',
    balance NUMERIC(15,2) NOT NULL,
    status account_status NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    CONSTRAINT chk_balance_non_negative CHECK (balance >= 0)
);

CREATE INDEX idx_accounts_account_number ON accounts(account_number);

-- =========================
-- 3. Tabla: transactions
-- =========================
CREATE TABLE transactions (
    id UUID PRIMARY KEY,
    account_id UUID NOT NULL,
    related_account_id UUID,
    amount NUMERIC(15,2) NOT NULL,
    type transaction_type NOT NULL,
    status transaction_status NOT NULL,
    reference VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),

    CONSTRAINT fk_transactions_account
        FOREIGN KEY (account_id)
        REFERENCES accounts(id)
        ON DELETE RESTRICT,

    CONSTRAINT fk_transactions_related_account
        FOREIGN KEY (related_account_id)
        REFERENCES accounts(id)
        ON DELETE RESTRICT,

    CONSTRAINT chk_amount_positive CHECK (amount > 0),
    CONSTRAINT uq_transactions_reference UNIQUE (reference)
);

-- =========================
-- 4. Índices de Rendimiento
-- =========================
CREATE INDEX idx_transactions_account_date
    ON transactions(account_id, created_at DESC);

CREATE INDEX idx_transactions_type
    ON transactions(type);

CREATE INDEX idx_transactions_reference
    ON transactions(reference);

-- ===============================
-- 5. Saldo actual de la cuenta X
-- SELECT balance FROM accounts WHERE id = :accountId;

-- 6. Últimos 20 movimientos de la cuenta X
-- SELECT * FROM transactions
-- WHERE account_id = :accountId
-- ORDER BY created_at DESC
-- LIMIT 20;

-- 7. Transferencias salientes del cliente Y en últimos 30 días
-- SELECT COUNT(*) FROM transactions t
-- JOIN accounts a ON t.account_id = a.id
-- WHERE a.id = :accountId
-- AND t.type = 'TRANSFER'
-- AND t.created_at >= NOW() - INTERVAL '30 days';

-- 8. Existencia de transacción con referencia Z
-- SELECT 1 FROM transactions WHERE reference = :ref LIMIT 1;
-- ==============================
