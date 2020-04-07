/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2020/3/12 9:43:03                            */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Access')
            and   type = 'U')
   drop table Base_Access
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_AppSecret')
            and   type = 'U')
   drop table Base_AppSecret
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Button')
            and   type = 'U')
   drop table Base_Button
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.Base_DatabaseLink')
            and   type = 'U')
   drop table dbo.Base_DatabaseLink
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Dict')
            and   type = 'U')
   drop table Base_Dict
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_DictDetail')
            and   type = 'U')
   drop table Base_DictDetail
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_File')
            and   type = 'U')
   drop table Base_File
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_FileMap')
            and   type = 'U')
   drop table Base_FileMap
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Login')
            and   type = 'U')
   drop table Base_Login
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Menu')
            and   type = 'U')
   drop table Base_Menu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_Role')
            and   type = 'U')
   drop table Base_Role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_SysInfo')
            and   type = 'U')
   drop table Base_SysInfo
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_SysLog')
            and   type = 'U')
   drop table Base_SysLog
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_User')
            and   type = 'U')
   drop table Base_User
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Base_UserRoleMap')
            and   type = 'U')
   drop table Base_UserRoleMap
go

drop schema dbo
go

/*==============================================================*/
/* User: dbo                                                    */
/*==============================================================*/
create schema dbo
go

/*==============================================================*/
/* Table: Base_Access                                           */
/*==============================================================*/
create table Base_Access (
   ID                   varchar(20)          not null,
   Master               varchar(10)          null,
   MasterID             varchar(20)          null,
   Access               varchar(10)          null,
   AccessID             varchar(20)          null,
   Operation            varchar(10)          null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   constraint PK_BASE_ACCESS primary key (ID)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Base_Access') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Base_Access' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '权限分配表', 
   'user', @CurrentUser, 'table', 'Base_Access'
go

/*==============================================================*/
/* Table: Base_AppSecret                                        */
/*==============================================================*/
create table Base_AppSecret (
   Id                   varchar(50)          not null,
   AppId                varchar(50)          null,
   AppSecret            varchar(50)          null,
   AppName              varchar(50)          null,
   constraint PK_BASE_APPSECRET primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Base_AppSecret') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Base_AppSecret' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '应用密钥表', 
   'user', @CurrentUser, 'table', 'Base_AppSecret'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_AppSecret')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '自然主键',
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_AppSecret')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AppId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '应用Id',
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_AppSecret')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AppSecret')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppSecret'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '应用密钥',
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppSecret'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_AppSecret')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AppName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '应用名',
   'user', @CurrentUser, 'table', 'Base_AppSecret', 'column', 'AppName'
go

/*==============================================================*/
/* Table: Base_Button                                           */
/*==============================================================*/
create table Base_Button (
   ID                   varchar(20)          not null,
   BtnName              varchar(20)          null,
   BtnCode              varchar(20)          null,
   BtnClass             varchar(20)          null,
   BtnIcon              varchar(20)          null,
   BtnScript            varchar(20)          null,
   MenuCode             varchar(20)          null,
   InitStatus           varchar(20)          null,
   SeqNo                int                  null,
   constraint PK_BASE_BUTTON primary key (ID)
)
go

/*==============================================================*/
/* Table: Base_DatabaseLink                                     */
/*==============================================================*/
create table dbo.Base_DatabaseLink (
   Id                   varchar(50)          not null,
   LinkName             varchar(50)          null,
   ConnectionStr        varchar(50)          null,
   DbType               varchar(50)          null,
   SortNum              varchar(50)          null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   UpdateUser           varchar(20)          null,
   UpdateTime           datetime             null,
   constraint PK_BASE_DATABASELINK primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('dbo.Base_DatabaseLink') and minor_id = 0)
begin 
   execute sp_dropextendedproperty 'MS_Description',  
   'user', 'dbo', 'table', 'Base_DatabaseLink' 
 
end 


execute sp_addextendedproperty 'MS_Description',  
   '数据库连接表', 
   'user', 'dbo', 'table', 'Base_DatabaseLink'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.Base_DatabaseLink')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'Id'

end


execute sp_addextendedproperty 'MS_Description', 
   '自然主键',
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.Base_DatabaseLink')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'LinkName')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'LinkName'

end


execute sp_addextendedproperty 'MS_Description', 
   '连接名',
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'LinkName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.Base_DatabaseLink')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ConnectionStr')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'ConnectionStr'

end


execute sp_addextendedproperty 'MS_Description', 
   '连接字符串',
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'ConnectionStr'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.Base_DatabaseLink')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DbType')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'DbType'

end


execute sp_addextendedproperty 'MS_Description', 
   '数据库类型',
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'DbType'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('dbo.Base_DatabaseLink')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'SortNum')
)
begin
   execute sp_dropextendedproperty 'MS_Description', 
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'SortNum'

end


execute sp_addextendedproperty 'MS_Description', 
   '排序编号',
   'user', 'dbo', 'table', 'Base_DatabaseLink', 'column', 'SortNum'
go

/*==============================================================*/
/* Table: Base_Dict                                             */
/*==============================================================*/
create table Base_Dict (
   ID                   varchar(20           not null,
   TypeCode             varchar(10)          null,
   TypeName             varchar(200)         null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   UpdateUser           varchar(20)          null,
   UpdateTime           datetime             null,
   constraint PK_BASE_DICT primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_DictDetail                                       */
/*==============================================================*/
create table Base_DictDetail (
   ID                   varchar(20           not null,
   ItemCode             varchar(10)          null,
   ItemName             varchar(200)         null,
   TypeCode             varchar(10)          null,
   OrderNo              int                  null,
   Remark               varchar(200)         null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   UpdateUser           varchar(20)          null,
   UpdateTime           datetime             null,
   constraint PK_BASE_DICTDETAIL primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_File                                             */
/*==============================================================*/
create table Base_File (
   ID                   varchar(20)          not null,
   FileName             varchar(200)         null,
   FilePath             varchar(500)         null,
   FileExt              varchar(20)          null,
   FileSize             bigint               null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   constraint PK_BASE_FILE primary key (ID)
)
go

/*==============================================================*/
/* Table: Base_FileMap                                          */
/*==============================================================*/
create table Base_FileMap (
   ID                   varchar(20)          not null,
   FileID               varchar(20)          null,
   RelateObj            varchar(10)          null,
   RelateObjID          varchar(20)          null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   constraint PK_BASE_FILEMAP primary key (ID)
)
go

/*==============================================================*/
/* Table: Base_Login                                            */
/*==============================================================*/
create table Base_Login (
   ID                   varchar(20)          not null,
   UserCode             varchar(20)          null,
   UserToken            varchar(600)         null,
   CreateTime           datetime             null,
   UpdateTime           datetime             null,
   constraint PK_BASE_LOGIN primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_Menu                                             */
/*==============================================================*/
create table Base_Menu (
   ID                   int                  identity,
   MenuCode             varchar(20)          null,
   MenuName             varchar(20)          null,
   MenuParentCode       varchar(20)          null,
   MenuType             int                  null,
   OrderNo              int                  null,
   PageUrl              varchar(200)         null,
   PageUrlOriginal      varchar(200)         null,
   IsAccessControl      bit                  null,
   IsValid              bit                  null,
   IsLeaf               bit                  null,
   SysCode              varchar(20)          null,
   constraint PK_BASE_MENU primary key nonclustered (ID)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Base_Menu') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Base_Menu' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '菜单表', 
   'user', @CurrentUser, 'table', 'Base_Menu'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'MenuType')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_Menu', 'column', 'MenuType'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1.文件夹类型
   2.页面
   3.内容页面',
   'user', @CurrentUser, 'table', 'Base_Menu', 'column', 'MenuType'
go

/*==============================================================*/
/* Table: Base_Role                                             */
/*==============================================================*/
create table Base_Role (
   ID                   varchar(20)          not null,
   RoleName             varchar(60)          null,
   RoleDes              varchar(200)         null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   UpdateUser           varchar(20)          null,
   UpdateTime           datetime             null,
   constraint PK_BASE_ROLE primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_SysInfo                                          */
/*==============================================================*/
create table Base_SysInfo (
   ID                   varchar(20)          not null,
   SysCode              varchar(20)          null,
   SysName              varchar(20)          null,
   OrderNo              int                  null,
   IconUrl              varchar(200)         null,
   IsValid              bit                  null,
   IsAccessControl      bit                  null,
   constraint PK_BASE_SYSINFO primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_SysLog                                           */
/*==============================================================*/
create table Base_SysLog (
   ID                   varchar(20)          not null,
   LogType              varchar(60)          null,
   LogLevel             varchar(20)          null,
   LogContent           varchar(max)         null,
   Data                 text                 null,
   OpTime               datetime             null,
   OpLoginName          varchar(20)          null,
   OpUserName           varchar(200)         null,
   constraint PK_BASE_SYSLOG primary key nonclustered (ID)
)
go

/*==============================================================*/
/* Table: Base_User                                             */
/*==============================================================*/
create table Base_User (
   ID                   varchar(20)          not null,
   LoginName            varchar(20)          null,
   Password             varchar(64)          null,
   UserName             varchar(60)          null,
   Phone                varchar(30)          null,
   IdentityCard         varchar(30)          null,
   Email                varchar(100)         null,
   IsDelete             bit                  null,
   IsValid              bit                  null default 1,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   UpdateUser           varchar(20)          null,
   UpdateTime           datetime             null,
   constraint PK_BASE_USER primary key nonclustered (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Base_User')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IsValid')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Base_User', 'column', 'IsValid'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1 启用
   0 未启用',
   'user', @CurrentUser, 'table', 'Base_User', 'column', 'IsValid'
go

/*==============================================================*/
/* Table: Base_UserRoleMap                                      */
/*==============================================================*/
create table Base_UserRoleMap (
   ID                   varchar(20)          not null,
   UserID               varchar(20)          null,
   RoleID               varchar(20)          null,
   CreateUser           varchar(20)          null,
   CreateTime           datetime             null,
   constraint PK_BASE_USERROLEMAP primary key nonclustered (ID)
)
go

