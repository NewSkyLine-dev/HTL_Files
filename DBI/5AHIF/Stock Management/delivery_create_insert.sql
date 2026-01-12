

drop table delivery;
drop table product;
drop table stock;
/


create table product(
   prodnr int not null primary key,
   prod_description varchar2(30) not null
);


create table stock(
   stocknr int not null primary key,
   location varchar2(30),
   capacity int
);


create table delivery(
   stocknr int not null references stock,
   serial_nr int,
   prodnr int not null references product,
   delivery_date date,
   amount  int not null,
   primary key (stocknr,serial_nr)
);

insert into product values(1,'product1');
insert into product values(2,'product2');
insert into product values(3,'product3');


insert into stock values(1,'Halle A',400);
insert into stock values(2,'Halle B',200);
insert into stock values(3,'Halle C',300);
insert into stock values(4,'Halle D',100);


insert into delivery values(1,1,1,to_date('2020.10.1','YYYY.MM.DD'),20);
insert into delivery values(1,2,1,to_date('2020.10.2','YYYY.MM.DD'),30);
insert into delivery values(1,3,2,to_date('2020.10.5','YYYY.MM.DD'),40);
insert into delivery values(2,1,1,to_date('2020.10.11','YYYY.MM.DD'),20);
insert into delivery values(2,2,1,to_date('2020.10.12','YYYY.MM.DD'),10);
insert into delivery values(1,4,1,to_date('2020.10.8','YYYY.MM.DD'),20);
insert into delivery values(1,5,1,to_date('2020.10.10','YYYY.MM.DD'),30);
insert into delivery values(1,6,2,to_date('2020.11.13','YYYY.MM.DD'),40);
insert into delivery values(2,3,1,to_date('2020.11.1','YYYY.MM.DD'),20);
insert into delivery values(2,4,3,to_date('2020.11.4','YYYY.MM.DD'),10);
insert into delivery values(1,7,1,to_date('2020.11.15','YYYY.MM.DD'),20);
insert into delivery values(1,8,1,to_date('2020.11.16','YYYY.MM.DD'),30);
insert into delivery values(1,9,2,to_date('2020.11.21','YYYY.MM.DD'),40);
insert into delivery values(2,5,1,to_date('2020.11.21','YYYY.MM.DD'),20);
insert into delivery values(2,6,3,to_date('2020.11.22','YYYY.MM.DD'),10);
/

select * from delivery;
/

