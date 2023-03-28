CREATE TABLE Cereal (
    Id int IDENTITY(1,1) PRIMARY KEY,
    Name varchar(50) NOT NULL,
    Mfr varchar(5) NULL,
    Type varchar(5) NULL,
    Calories int,
    Protein int NULL,
    Fat int NULL,
    Sodium int NULL,
    Fiber float(50),
    Carbo float(50) NULL,
    Sugars int NULL,
    Potass int NULL,
    Vitamins int NULL,
    Shelf int NULL,
    Weight float NULL,
	Cups float NULL,
	Rating float
);