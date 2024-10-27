/*
Script de implantação para D:\DEV\FI.WEBATIVIDADEENTREVISTA\FI.WEBATIVIDADEENTREVISTA\APP_DATA\BANCODEDADOS.MDF

Este código foi gerado por uma ferramenta.
As alterações feitas nesse arquivo poderão causar comportamento incorreto e serão perdidas se
o código for gerado novamente.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "D:\DEV\FI.WEBATIVIDADEENTREVISTA\FI.WEBATIVIDADEENTREVISTA\APP_DATA\BANCODEDADOS.MDF"
:setvar DefaultFilePrefix "D_\DEV\FI.WEBATIVIDADEENTREVISTA\FI.WEBATIVIDADEENTREVISTA\APP_DATA\BANCODEDADOS.MDF_"
:setvar DefaultDataPath "C:\Users\Rennan\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Rennan\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
Detecta o modo SQLCMD e desabilita a execução do script se o modo SQLCMD não tiver suporte.
Para reabilitar o script após habilitar o modo SQLCMD, execute o comando a seguir:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'O modo SQLCMD deve ser habilitado para executar esse script com êxito.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO

IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Alterando Tabela [dbo].[CLIENTES]...';


GO
ALTER TABLE [dbo].[CLIENTES]
    ADD [CPF] VARCHAR (14) NOT NULL;


GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END

IF OBJECT_ID(N'tempdb..#tmpErrors') IS NULL
    CREATE TABLE [#tmpErrors] (
        Error INT
    );

IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES                 (1);
        BEGIN TRANSACTION;
    END


GO

IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'A parte transacionada da atualização do banco de dados obteve êxito.'
COMMIT TRANSACTION
END
ELSE PRINT N'A parte transacionada da atualização do banco de dados falhou.'
GO
IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
GO
PRINT N'Atualização concluída.';


GO

