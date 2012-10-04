
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6648C4C3934EA45B]') AND parent_object_id = OBJECT_ID('Contacts'))
alter table Contacts  drop constraint FK6648C4C3934EA45B


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK6648C4C31B39533F]') AND parent_object_id = OBJECT_ID('Contacts'))
alter table Contacts  drop constraint FK6648C4C31B39533F


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK97114EFA82A147D7]') AND parent_object_id = OBJECT_ID('Families'))
alter table Families  drop constraint FK97114EFA82A147D7


    if exists (select * from dbo.sysobjects where id = object_id(N'Addresses') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Addresses

    if exists (select * from dbo.sysobjects where id = object_id(N'Contacts') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Contacts

    if exists (select * from dbo.sysobjects where id = object_id(N'Families') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Families

    if exists (select * from dbo.sysobjects where id = object_id(N'hibernate_unique_key') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table hibernate_unique_key

    create table Addresses (
        Id INT not null,
       AddressLine1 NVARCHAR(255) null,
       AddressLine2 NVARCHAR(255) null,
       AddressLine3 NVARCHAR(255) null,
       PostCode NVARCHAR(255) null,
       City NVARCHAR(255) null,
       Country NVARCHAR(255) null,
       Description NVARCHAR(255) null,
       Lat NVARCHAR(255) null,
       Long NVARCHAR(255) null,
       primary key (Id)
    )

    create table Contacts (
        Id INT not null,
       FamilyId INT null,
       Gender NVARCHAR(255) null,
       FirstName NVARCHAR(255) null,
       Mobile NVARCHAR(255) null,
       MobileVisibility INT null,
       Email NVARCHAR(255) null,
       EmailVisibility INT null,
       FacebookId NVARCHAR(255) null,
       MobileInDirectory BIT null,
       Txt BIT null,
       MailingList BIT null,
       FamilyFk INT null,
       primary key (Id)
    )

    create table Families (
        Id INT not null,
       FamilyName NVARCHAR(255) null,
       Children NVARCHAR(255) null,
       AddressId INT null,
       Phone NVARCHAR(255) null,
       ShowInDirectory BIT null,
       primary key (Id)
    )

    alter table Contacts 
        add constraint FK6648C4C3934EA45B 
        foreign key (FamilyId) 
        references Families

    alter table Contacts 
        add constraint FK6648C4C31B39533F 
        foreign key (FamilyFk) 
        references Families

    alter table Families 
        add constraint FK97114EFA82A147D7 
        foreign key (AddressId) 
        references Addresses

    create table hibernate_unique_key (
         next_hi INT 
    )

    insert into hibernate_unique_key values ( 1 )
