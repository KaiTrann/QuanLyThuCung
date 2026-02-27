/* Converted from MySQL to SQL Server T-SQL
   Keep same tables/columns/constraints/data.
*/

SET NOCOUNT ON;

-- Drop views first (because they depend on tables)
IF OBJECT_ID(N'dbo.vw_vaccinations', N'V') IS NOT NULL DROP VIEW dbo.vw_vaccinations;
IF OBJECT_ID(N'dbo.vw_appointments', N'V') IS NOT NULL DROP VIEW dbo.vw_appointments;

-- Drop tables in FK-safe order
IF OBJECT_ID(N'dbo.vaccinations', N'U') IS NOT NULL DROP TABLE dbo.vaccinations;
IF OBJECT_ID(N'dbo.appointments', N'U') IS NOT NULL DROP TABLE dbo.appointments;
IF OBJECT_ID(N'dbo.vaccines', N'U') IS NOT NULL DROP TABLE dbo.vaccines;
IF OBJECT_ID(N'dbo.pets', N'U') IS NOT NULL DROP TABLE dbo.pets;
IF OBJECT_ID(N'dbo.owners', N'U') IS NOT NULL DROP TABLE dbo.owners;
IF OBJECT_ID(N'dbo.users', N'U') IS NOT NULL DROP TABLE dbo.[users];

--------------------------------------------------------------------------------
-- Table: owners
--------------------------------------------------------------------------------
CREATE TABLE dbo.owners (
    owner_id   INT IDENTITY(1,1) NOT NULL,
    full_name  VARCHAR(120) NOT NULL,
    phone      VARCHAR(20)  NOT NULL,
    address    VARCHAR(255) NULL,
    created_at DATETIME2(0) NOT NULL CONSTRAINT DF_owners_created_at DEFAULT (SYSUTCDATETIME()),
    updated_at DATETIME2(0) NULL,
    CONSTRAINT PK_owners PRIMARY KEY (owner_id),
    CONSTRAINT UQ_owners_phone UNIQUE (phone)
);

CREATE INDEX idx_owner_name ON dbo.owners(full_name);

-- Auto-update updated_at on UPDATE (equivalent of MySQL ON UPDATE CURRENT_TIMESTAMP)
GO
CREATE OR ALTER TRIGGER dbo.trg_owners_set_updated_at
ON dbo.owners
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE o
    SET updated_at = SYSUTCDATETIME()
    FROM dbo.owners o
    INNER JOIN inserted i ON o.owner_id = i.owner_id;
END
GO

--------------------------------------------------------------------------------
-- Table: pets
--------------------------------------------------------------------------------
CREATE TABLE dbo.pets (
    pet_id     INT IDENTITY(1,1) NOT NULL,
    owner_id   INT NOT NULL,
    pet_name   VARCHAR(100) NOT NULL,
    species    VARCHAR(50)  NULL,
    breed      VARCHAR(50)  NULL,
    sex        VARCHAR(10)  NULL,
    created_at DATETIME2(0) NOT NULL CONSTRAINT DF_pets_created_at DEFAULT (SYSUTCDATETIME()),
    updated_at DATETIME2(0) NULL,
    CONSTRAINT PK_pets PRIMARY KEY (pet_id),
    CONSTRAINT FK_pets_owner FOREIGN KEY (owner_id)
        REFERENCES dbo.owners(owner_id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
);

CREATE INDEX idx_pet_name ON dbo.pets(pet_name);
CREATE INDEX idx_pet_owner ON dbo.pets(owner_id);

GO
CREATE OR ALTER TRIGGER dbo.trg_pets_set_updated_at
ON dbo.pets
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET updated_at = SYSUTCDATETIME()
    FROM dbo.pets p
    INNER JOIN inserted i ON p.pet_id = i.pet_id;
END
GO

--------------------------------------------------------------------------------
-- Table: appointments
--------------------------------------------------------------------------------
CREATE TABLE dbo.appointments (
    appointment_id   INT IDENTITY(1,1) NOT NULL,
    pet_id           INT NOT NULL,
    appointment_date DATE NOT NULL,
    reason           VARCHAR(200) NULL,
    status           VARCHAR(50)  NULL,
    created_at       DATETIME2(0) NOT NULL CONSTRAINT DF_appointments_created_at DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_appointments PRIMARY KEY (appointment_id),
    CONSTRAINT FK_appt_pet FOREIGN KEY (pet_id)
        REFERENCES dbo.pets(pet_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

CREATE INDEX idx_appt_date ON dbo.appointments(appointment_date);
CREATE INDEX idx_appt_pet  ON dbo.appointments(pet_id);

--------------------------------------------------------------------------------
-- Table: users
--------------------------------------------------------------------------------
CREATE TABLE dbo.[users] (
    user_id        INT IDENTITY(1,1) NOT NULL,
    username       VARCHAR(50)  NOT NULL,
    password_hash  VARCHAR(255) NOT NULL,
    full_name      VARCHAR(100) NULL,
    role           VARCHAR(10)  NOT NULL CONSTRAINT DF_users_role DEFAULT ('staff'),
    is_active      BIT          NOT NULL CONSTRAINT DF_users_is_active DEFAULT (1),
    created_at     DATETIME2(0) NOT NULL CONSTRAINT DF_users_created_at DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_users PRIMARY KEY (user_id),
    CONSTRAINT UQ_users_username UNIQUE (username),
    CONSTRAINT CK_users_role CHECK (role IN ('admin','staff'))
);

--------------------------------------------------------------------------------
-- Table: vaccines
--------------------------------------------------------------------------------
CREATE TABLE dbo.vaccines (
    vaccine_id   INT IDENTITY(1,1) NOT NULL,
    vaccine_name VARCHAR(100) NOT NULL,
    description  VARCHAR(200) NULL,
    CONSTRAINT PK_vaccines PRIMARY KEY (vaccine_id),
    CONSTRAINT UQ_vaccines_vaccine_name UNIQUE (vaccine_name)
);

--------------------------------------------------------------------------------
-- Table: vaccinations
--------------------------------------------------------------------------------
CREATE TABLE dbo.vaccinations (
    vaccination_id        INT IDENTITY(1,1) NOT NULL,
    pet_id                INT NOT NULL,
    vaccine_id            INT NOT NULL,
    vaccination_date      DATE NOT NULL,
    next_vaccination_date DATE NULL,
    created_at            DATETIME2(0) NOT NULL CONSTRAINT DF_vaccinations_created_at DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_vaccinations PRIMARY KEY (vaccination_id),
    CONSTRAINT FK_vax_pet FOREIGN KEY (pet_id)
        REFERENCES dbo.pets(pet_id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT FK_vax_vaccine FOREIGN KEY (vaccine_id)
        REFERENCES dbo.vaccines(vaccine_id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE
);

CREATE INDEX idx_vax_next ON dbo.vaccinations(next_vaccination_date);
CREATE INDEX idx_vax_pet  ON dbo.vaccinations(pet_id);
CREATE INDEX fk_vax_vaccine ON dbo.vaccinations(vaccine_id);

--------------------------------------------------------------------------------
-- Insert data (keep same values)
--------------------------------------------------------------------------------

-- owners
SET IDENTITY_INSERT dbo.owners ON;
INSERT INTO dbo.owners (owner_id, full_name, phone, address, created_at, updated_at) VALUES
(3,  N'k',     '123',         '123',         '2026-02-27T17:02:57', '2026-02-27T17:02:57'),
(7,  N'Khánh', '1234',        '2132165',     '2026-02-27T17:15:10', '2026-02-27T17:15:10'),
(8,  N'Khánhh','12345',       '213216555',   '2026-02-27T17:17:30', '2026-02-27T17:17:30'),
(10, N'lợi',   '12345672222', '213216521',   '2026-02-27T17:17:46', '2026-02-27T18:02:15'),
(11, N'hướng', '1234567',     '21321652112', '2026-02-27T17:17:54', '2026-02-27T17:17:54'),
(12, N'ánh',   '123123123',   '123123123123','2026-02-27T17:18:03', '2026-02-27T17:18:03'),
(13, N'huy',   '1345682',     N'đà nẵng',     '2026-02-27T18:00:39', '2026-02-27T18:00:39');
SET IDENTITY_INSERT dbo.owners OFF;

-- pets
SET IDENTITY_INSERT dbo.pets ON;
INSERT INTO dbo.pets (pet_id, owner_id, pet_name, species, breed, sex, created_at, updated_at) VALUES
(3, 10, N'Liz', N'Mèo', N'Chân Lùn', N'Cái', '2026-02-27T17:18:41', '2026-02-27T18:53:28'),
(4, 13, N'Kaz', N'Chó', N'Mặt Xệ',  N'Cái', '2026-02-27T18:01:11', '2026-02-27T18:53:49');
SET IDENTITY_INSERT dbo.pets OFF;

-- appointments
SET IDENTITY_INSERT dbo.appointments ON;
INSERT INTO dbo.appointments (appointment_id, pet_id, appointment_date, reason, status, created_at) VALUES
(3, 3, '2026-02-27', N'Khám định kỳ', N'Đã xác nhận', '2026-02-27T17:19:10'),
(4, 4, '2026-02-27', N'Buồn Ngủuuu',  N'Chờ xác nhận','2026-02-27T18:02:59');
SET IDENTITY_INSERT dbo.appointments OFF;

-- users
SET IDENTITY_INSERT dbo.[users] ON;
INSERT INTO dbo.[users] (user_id, username, password_hash, full_name, role, is_active, created_at) VALUES
(1, 'admin', '123456', N'Administrator', 'admin', 1, '2026-02-24T16:21:56');
SET IDENTITY_INSERT dbo.[users] OFF;

-- vaccines
SET IDENTITY_INSERT dbo.vaccines ON;
INSERT INTO dbo.vaccines (vaccine_id, vaccine_name, description) VALUES
(1, N'Vắc-xin 5 bệnh cho chó (DHPPI)', N'Phòng Care, Viêm gan truyền nhiễm, Parvo, Parainfluenza (và có thể kèm Lepto tùy loại).'),
(2, N'Vắc-xin Dại', N'Phòng bệnh Dại cho chó/mèo. Tiêm nhắc định kỳ theo khuyến cáo thú y.'),
(3, N'Vắc-xin Parvo', N'Phòng bệnh Parvo ở chó (đặc biệt nguy hiểm với chó con).'),
(4, N'Vắc-xin Giảm bạch cầu ở mèo (FPV)', N'Phòng bệnh giảm bạch cầu/Parvo mèo (Feline Panleukopenia).'),
(5, N'Vắc-xin 3 bệnh cho mèo (FVRCP)', N'Phòng Viêm mũi khí quản (Herpes), Calici, Giảm bạch cầu (Panleukopenia).');
SET IDENTITY_INSERT dbo.vaccines OFF;

-- vaccinations
SET IDENTITY_INSERT dbo.vaccinations ON;
INSERT INTO dbo.vaccinations (vaccination_id, pet_id, vaccine_id, vaccination_date, next_vaccination_date, created_at) VALUES
(2, 3, 1, '2026-02-27', '2027-02-27', '2026-02-27T17:45:10'),
(3, 4, 4, '2026-02-27', '2027-02-27', '2026-02-27T18:03:29');
SET IDENTITY_INSERT dbo.vaccinations OFF;

--------------------------------------------------------------------------------
-- Views (SQL Server)
--------------------------------------------------------------------------------
GO
CREATE VIEW dbo.vw_appointments
AS
SELECT
    a.appointment_id,
    p.pet_name,
    a.appointment_date,
    a.reason,
    a.status
FROM dbo.appointments a
JOIN dbo.pets p ON p.pet_id = a.pet_id;
GO

CREATE VIEW dbo.vw_vaccinations
AS
SELECT
    v.vaccination_id,
    p.pet_name,
    vc.vaccine_name,
    v.vaccination_date,
    v.next_vaccination_date
FROM dbo.vaccinations v
JOIN dbo.pets p     ON p.pet_id = v.pet_id
JOIN dbo.vaccines vc ON vc.vaccine_id = v.vaccine_id;
GO