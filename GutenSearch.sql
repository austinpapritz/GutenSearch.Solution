CREATE TABLE `Copies` (
  `CopyId` int PRIMARY KEY,
  `BookId` int
);

CREATE TABLE `Books` (
  `BookId` int PRIMARY KEY,
  `Title` varchar(255)
);

CREATE TABLE `BookAuthors` (
  `BookAuthorId` int PRIMARY KEY,
  `BookId` int,
  `AuthorId` int
);

CREATE TABLE `Authors` (
  `AuthorId` int PRIMARY KEY,
  `Name` varchar(255)
);

CREATE TABLE `Users` (
  `UserId` int PRIMARY KEY,
  `Role` varchar(100)
);

CREATE TABLE `Checkouts` (
  `CheckoutId` int PRIMARY KEY,
  `UserId` int
);

ALTER TABLE `Books` ADD FOREIGN KEY (`BookId`) REFERENCES `Copies` (`BookId`);

ALTER TABLE `Books` ADD FOREIGN KEY (`BookId`) REFERENCES `BookAuthors` (`BookId`);

ALTER TABLE `Authors` ADD FOREIGN KEY (`AuthorId`) REFERENCES `BookAuthors` (`AuthorId`);

ALTER TABLE `Users` ADD FOREIGN KEY (`UserId`) REFERENCES `Checkouts` (`UserId`);
