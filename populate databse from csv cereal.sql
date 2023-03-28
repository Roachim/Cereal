-- truncate the table first
TRUNCATE TABLE dbo.CerealConversion;
TRUNCATE TABLE dbo.Cereal;
GO
 
-- import the file
BULK INSERT dbo.Cereal
FROM 'C:\Users\KOM\Desktop\Opgaver\Cereal Opgave\cereal_perfect.csv'
WITH
(
        /**/FORMAT='CSV',
		/**/DATAFILETYPE = 'char',
		/*CODEPAGE = 'code_page',*/
		FIRSTROW=2,
		MAXERRORS = 5,
		/**/FIELDTERMINATOR = ';'
		/*ROWTERMINATOR = '\n'*/ /*- CRLF - \r\n  - 0x0a -0x0d0a -*/
		 /*TABLOCK*/ 
)
GO

--Transfer as conert files from convert table to actual table of cereal
Insert Into dbo.Cereal (Name, Mfr, Type, Calories, Protein, Fat, Sodium, Fiber, Carbo, Sugars, Potass, Vitamins, Shelf, Weight, Cups, Rating)
Select TRY_CAST(Name AS varchar(50)), TRY_CAST(Mfr AS varchar(5)), TRY_CAST(Type AS varchar(5)), TRY_CAST(Calories as int), TRY_CAST(Protein AS int),
TRY_CAST(Fat AS int), TRY_CAST(Sodium AS int), TRY_CAST(Fiber AS float), TRY_CAST(Carbo AS float), TRY_CAST(Sugars AS int), TRY_CAST(Potass AS int),
TRY_CAST(Vitamins AS int), TRY_CAST(Shelf AS int), TRY_CAST(Weight AS float), TRY_CAST(Cups AS float), TRY_CAST(Rating AS float)
From dbo.CerealConversion

GO