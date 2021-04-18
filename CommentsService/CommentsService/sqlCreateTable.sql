DROP TABLE Comments;

CREATE TABLE Comments(
    CommentID uuid not null,
    CommentText varchar(100) not null,
    CommentDate date not null,
    PostID integer not null,
    AccountID integer not null
);

ALTER TABLE Comments ADD CONSTRAINT
PK_Artikl PRIMARY KEY(CommentID);

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

ALTER TABLE Comments
ALTER COLUMN CommentID uuid;


INSERT INTO "Comments"("CommentID", "CommentText", "CommentDate", "PostID", "AccountID")
VALUES ((uuid_generate_v4()), 'Prvi komentar', to_date('11.03.2021.', 'dd.mm.yyyy.'), 1, 1);

DELETE FROM "Comments"
Where "CommentID" = '244a6762-6793-4669-9155-526fcde680b0'