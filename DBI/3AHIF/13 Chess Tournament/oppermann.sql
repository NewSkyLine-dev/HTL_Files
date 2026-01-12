create database chess;
use chess;

drop table if exists match_player;
drop table if exists match;
drop table if exists player;
drop table if exists organizes;
drop table if exists tournament;
drop table if exists clubmembers;
drop table if exists member;
drop table if exists club;

create table club(
    id int primary key,
    [name] varchar(100),
    year_of_foundation int,
    [address] varchar(100),
);

create table member(
    id int primary key,
    firstname varchar(100),
    lastname varchar(100),
    [address] varchar(100),
    phone_number int,
    email varchar(100),
);

create table clubmembers(
    joined_date date,
    member_id int,
    club_id int,
    primary key (member_id, club_id),
    foreign key (member_id) references member(id),
    foreign key (club_id) references club(id),
);

create table tournament(
    code int primary key,
    [name] varchar(100),
    [location] varchar(100),
    starting_date date,
    ending_date date,
    club_id int references club(id),
);

create table player(
    id int primary key,
    member_id int,
    ranking varchar(4),
    nationality varchar(100),
    ranking_date date,
    check (ranking = 'CL' 
        or ranking = 'IM' 
        or ranking = 'GM' 
        or (ranking is null and ranking_date is null) 
        or (ranking_date is not null and ranking is not null) ),
    foreign key (member_id) references member(id),
);

create table match(
    id int primary key,
    end_point date,
    starting_point date,
    result varchar(5),
    number_of_moves int,
    black_player int references player(id),
    white_player int references player(id),
    tournament_code int references tournament(code),
    check (result IN ('W', 'B', 'D'))
);

create table organizes(
    tournament_id int references tournament(code),
    member_id int references member(id),
    primary key (tournament_id, member_id),
);

insert into club values
    (1, 'Chess Club', 1990, '123 Main St'),
    (2, 'Chess Club 2', 1995, '456 Main St');

insert into member values
    (1, 'John', 'Doe', '123 Main St', 1234567890, 'john@doe.at'),
    (2, 'Jane', 'Doe', '456 Main St', 1234567890, 'jane@doe.at');

insert into clubmembers values
    ('2020-01-01', 1, 1),
    ('2022-01-01', 2, 2);

insert into tournament values
    (1, 'Chess Tournament', '123 Main St', '2020-01-01', '2020-01-02', 1),
    (2, 'Chess Tournament 2', '456 Main St', '2020-01-01', '2020-01-02', 2);

insert into player values
    (1, 1, 'CL', 'Austria', '2020-01-01'),
    (2, 2, 'IM', 'Austria', '2020-01-01');

insert into match values
    (1, '2020-01-01', '2020-01-02', 'W', 50, 1, 2, 1),
    (2, '2020-01-01', '2020-01-02', 'B', 50, 2, 1, 2);

insert into match values
    (3, '2020-01-01', '2020-01-02', 'W', 50, 1, 2, 2),
    (4, '2020-01-01', '2020-01-02', 'B', 50, 2, 1, 1);

insert into organizes values
    (1, 1),
    (2, 2);

-- 5.1
select *
from club
where id not in (
    select club_id 
    from tournament
);

-- 5.2
select c.name
from club c
    join clubmembers cm on c.id = cm.club_id
    join member m on cm.member_id = m.id
    join player p on p.member_id = m.id
where p.nationality = 'Austria'
group by c.name;

-- 5.3 
select t.code, count(p.id) as [number of players]
from tournament t
    left join match m on t.code = m.tournament_code
    left join player p on m.white_player = p.id or m.black_player = p.id
group by t.code;

-- 5.4
select p.id, count(m.result) as [number of wins]
from player p
    left join match m on (p.id = m.white_player and m.result = 'W')
                    or (p.id = m.black_player and m.result = 'B')
group by p.id;