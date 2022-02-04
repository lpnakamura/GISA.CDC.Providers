
# GISA.CDC.Providers
Estruturação da tabela de Prestadores de serviço do sistema legado

    create table TB_SOLICITANTES
    (
    	Id varchar(64) not null unique,
    	CidadeId varchar(64) not null,
    	EspecialidadeId varchar(64) not null,
    	Nome varchar(255) not null,
    	EstadoCivil varchar(255) not null CHECK (EstadoCivil in ('Solteiro', 'Casado', 'Separado', 'Divorciado', 'Viúvo')),
    	DataNascimento date not null CHECK (DataNascimento <= NOW()),
    	Nacionalidade varchar(255) not null,
    	Naturalidade varchar(255) not null,
    	RegistroProfissional varchar(100) not null,
    	Endereco varchar(500) not null,
    	Numero varchar(10) null,
    	Complemento varchar(500) null,
    	constraint TB_SOLICITANTES_PK
    		primary key (Id),
    	constraint TB_SOLICITANTES_TB_CIDADES_FK
            foreign key (CidadeId) REFERENCES  TB_CIDADES (Id),
        constraint TB_SOLICITANTES_TB_ESPECIALIDADES_FK
            foreign key (EspecialidadeId) REFERENCES  TB_ESPECIALIDADES (Id)
    );
        
  
  Transformação de dados dos Solicitantes (kSQL)

    CREATE STREAM STM_PRESTADORES_STRUCT
          (before STRUCT<            
                    Id                  VARCHAR,
                    CidadeId            VARCHAR,
                    EspecialidadeId     VARCHAR,
                    Nome                VARCHAR,
                    EstadoCivil         VARCHAR,
                    DataNascimento      DATE,
                    Nacionalidade       VARCHAR,
                    Naturalidade        VARCHAR,
                    RegistroProfissional VARCHAR,
                    Endereco            VARCHAR,
                    Numero              VARCHAR,
                    Complemento         VARCHAR>,
           after STRUCT<            
                    Id                  VARCHAR,
                    CidadeId            VARCHAR,
                    EspecialidadeId     VARCHAR,
                    Nome                VARCHAR,
                    EstadoCivil         VARCHAR,
                    DataNascimento      DATE,
                    Nacionalidade       VARCHAR,
                    Naturalidade        VARCHAR,
                    RegistroProfissional VARCHAR,
                    Endereco            VARCHAR,
                    Numero              VARCHAR,
                    Complemento         VARCHAR>,
                    op VARCHAR)
    WITH (KAFKA_TOPIC='mysql.BOA_SAUDE_PORTAL_DB.TB_SOLICITANTES_AMOSTRAGEM', VALUE_FORMAT='JSON');
    
    CREATE STREAM STM_PRESTADORES AS 
    SELECT before->Id _Id, before->EspecialidadeId _SpecialityId, before->CidadeId _CityId,
    before->Nome _Name, before->EstadoCivil _MaritalStatus,
    before->DataNascimento _BirthDay, before->Nacionalidade _Nationality, before->RegistroProfissional _ProfessionalIdentifier,
    before->Endereco _Address, before->Numero _AddressNumber, before->Complemento _Addition,
    after->Id Id, after->EspecialidadeId SpecialityId, after->CidadeId CityId,
    after->Nome Name, after->EstadoCivil MaritalStatus,
    after->DataNascimento BirthDay, after->Nacionalidade Nationality, after->RegistroProfissional ProfessionalIdentifier,
    after->Endereco Address, after->Numero AddressNumber, after->Complemento Addition, op As Operation
    FROM STM_PRESTADORES_STRUCT EMIT CHANGES;
    
    CREATE STREAM STM_ESPECIALIDADES_STRUCT
          (before STRUCT<
                Id VARCHAR,
                Sigla VARCHAR,
                Titulo VARCHAR>,
           after STRUCT<
                Id VARCHAR,
                Sigla VARCHAR,
                Titulo VARCHAR>,
          op VARCHAR) 
    WITH (KAFKA_TOPIC='mysql.BOA_SAUDE_PORTAL_DB.TB_ESPECIALIDADES', VALUE_FORMAT='JSON');
    
    CREATE STREAM STM_ESPECIALIDADES AS 
    SELECT before->Id _Id, before->Sigla _Acronym, before->Titulo _Title, after->Id Id, after->Sigla Acronym, after->Titulo Title, op As Operation
    FROM STM_ESPECIALIDADES_STRUCT EMIT CHANGES;
    
    CREATE STREAM STM_PRESTADORES_FULL AS
    SELECT COALESCE(PREST.Id, PREST._Id) As Id, ESP.Id, CID.Id,
        STRUCT(Provider := STRUCT(Id := PREST._Id, Name := PREST._Name, 
                    MaritalStatus := PREST._MaritalStatus, BirthDay := FORMAT_DATE(PREST._BirthDay, 'yyyy-MM-dd'), 
                    Nationality := PREST._Nationality, 
                    ProfessionalIdentifier := PREST._ProfessionalIdentifier,
                    Address := PREST._Address, AddressNumber := PREST._AddressNumber, Addition := PREST._Addition),
               Speciality := STRUCT(Id := ESP._Id, Acronym := ESP._Acronym, Title := ESP._Title),
               City := STRUCT(Id := CID._Id, Name := CID._City, State := CID._State, Latitude := CID._Latitude, Longitude := CID._Longitude)) As before, 
        STRUCT(Provider := STRUCT(Id := PREST.Id, Name := PREST.Name, 
                    MaritalStatus := PREST.MaritalStatus, BirthDay := FORMAT_DATE(PREST.BirthDay, 'yyyy-MM-dd'), 
                    Nationality := PREST.Nationality, 
                    ProfessionalIdentifier := PREST.ProfessionalIdentifier,
                    Address := PREST.Address, AddressNumber := PREST.AddressNumber, Addition := PREST.Addition),
               Speciality := STRUCT(Id := ESP.Id, Acronym := ESP.Acronym, Title := ESP.Title),
               City := STRUCT(Id := CID.Id, Name := CID.City, State := CID.State, Latitude := CID.Latitude, Longitude := CID.Longitude)) As after, 
        CASE WHEN PREST.Operation = 'c' THEN 'Insert' 
             WHEN PREST.Operation = 'u' THEN 'Update'
             WHEN PREST.Operation = 'd' THEN 'Delete' 
             ELSE 'Unknown' END Operation
    FROM STM_PRESTADORES PREST
    LEFT JOIN STM_ESPECIALIDADES ESP WITHIN 60 DAYS
    ON COALESCE(PREST.SpecialityId, PREST._SpecialityId) = ESP.Id
    LEFT JOIN STM_CIDADES CID WITHIN 60 DAYS
    ON COALESCE(PREST.CityId, PREST._CityId) = CID.Id EMIT CHANGES;
