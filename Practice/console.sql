/*
create table account
(
    account_id         uuid            not null
        constraint account_pk
            primary key,
    currency_name     text            not null);

alter table account
    owner to postgres;

create unique index account_account_id_uindex
    on account (account_id);
*/

/*
create table client
(
    client_id         uuid            not null
        constraint client_pk
            primary key,
    name              text            not null,
    surname           text            not null,
    number_passport   text            not null,
    phone             text            not null,
    "account_number " text,
    "balance "        money default 0 not null,
    date_birthday     date            not null
);

alter table client
    owner to postgres;

create unique index client_client_id_uindex
    on client (client_id);
*/

/*
create table employee
(
    employee_id     uuid              not null
        constraint employee_pk
            primary key,
    name            text              not null,
    surname         text              not null,
    number_passport text              not null,
    phone           text              not null,
    position        text              not null,
    salary          integer default 0 not null,
    date_start      date              not null,
    date_birthday   date              not null
);

alter table employee
    owner to postgres;

create unique index employee_employee_id_uindex
    on employee (employee_id);
*/

/*
INSERT INTO client (client_id, name, surname, number_passport, phone, "account_number ", "balance ", date_birthday )
VALUES
(uuid_generate_v4(), 'John', 'Doe', 'AB1234567', '+1234567890', 'ACC1234567', 1000, '1990-01-01'),
(uuid_generate_v4(), 'Jane', 'Smith', 'CD2345678', '+2345678901', 'ACC2345678', 2500, '1985-05-15'),
(uuid_generate_v4(), 'Alice', 'Johnson', 'EF3456789', '+3456789012', 'ACC3456789', 3500, '1992-08-21'),
(uuid_generate_v4(), 'Bob', 'Williams', 'GH4567890', '+4567890123', 'ACC4567890', 500, '1978-11-10'),
(uuid_generate_v4(), 'Charlie', 'Brown', 'IJ5678901', '+5678901234', 'ACC5678901', 1500, '1983-03-05'),
(uuid_generate_v4(), 'David', 'Miller', 'KL6789012', '+6789012345', 'ACC6789012', 4500, '1995-09-30'),
(uuid_generate_v4(), 'Emily', 'Davis', 'MN7890123', '+7890123456', 'ACC7890123', 2000, '1990-07-18'),
(uuid_generate_v4(), 'Frank', 'Garcia', 'OP8901234', '+8901234567', 'ACC8901234', 3000, '1988-04-23'),
(uuid_generate_v4(), 'Grace', 'Martinez', 'QR9012345', '+9012345678', 'ACC9012345', 1200, '1982-12-25'),
(uuid_generate_v4(), 'Helen', 'Rodriguez', 'ST0123456', '+0123456789', 'ACC0123456', 600, '1975-06-12');
 */

/*
insert into employee (employee_id, name, surname, number_passport, phone, date_start, date_birthday, position, salary)
VALUES
(uuid_generate_v4(), 'John', 'Doe', 'AB1234567', '+1234567890', '2020-01-01', '1990-01-01', 'Cashier', 3200),
(uuid_generate_v4(), 'Jane', 'Smith', 'CD2345678', '+2345678901', '2018-03-15', '1985-05-15', 'Service Specialist', 3700),
(uuid_generate_v4(), 'Alice', 'Johnson', 'EF3456789', '+3456789012', '2019-08-21', '1992-08-21', 'Counselor', 4100),
(uuid_generate_v4(), 'Bob', 'Williams', 'GH4567890', '+4567890123', '2017-07-10', '1978-11-10', 'Manager', 5000),
(uuid_generate_v4(), 'Charlie', 'Brown', 'IJ5678901', '+5678901234', '2021-05-05', '1983-03-05', 'Bank Accountant', 4500),
(uuid_generate_v4(), 'David', 'Miller', 'KL6789012', '+6789012345', '2022-09-30', '1995-09-30', 'Financial Analyst', 5300),
(uuid_generate_v4(), 'Emily', 'Davis', 'MN7890123', '+7890123456', '2020-07-18', '1990-07-18', 'Auditor', 4700),
(uuid_generate_v4(), 'Frank', 'Garcia', 'OP8901234', '+8901234567', '2016-04-23', '1988-04-23', 'IT Specialist', 6000),
(uuid_generate_v4(), 'Grace', 'Martinez', 'QR9012345', '+9012345678', '2021-12-25', '1982-12-25', 'Manager', 5400),
(uuid_generate_v4(), 'Helen', 'Rodriguez', 'ST0123456', '+0123456789', '2015-06-12', '1975-06-12', 'Counselor', 4200);
*/

/*
alter table account
add constraint account_client_id
foreign key (client_id) references client(client_id) on delete cascade;
 */

/*
insert into account (account_id, currency_name, amount, client_id)
VALUES
  (uuid_generate_v4(), 'Euro', 874.30, 'e28e6012-23c7-433c-8d9d-2f71e8ff3a5e'),
  (uuid_generate_v4(), 'Russian rouble', 45900.50, 'd01cd5b7-1639-4596-b022-f32ffa968413'),
  (uuid_generate_v4(), 'Dollar USA', 3295.00, 'c3b3217a-b9ae-4d3a-a289-1da2fc3bf187'),
  (uuid_generate_v4(), 'Euro', 1230.45, '99e6c025-b687-42af-9bf2-6404e4aea142'),
  (uuid_generate_v4(), 'Russian rouble', 9800.20, '7ab60e72-73e5-461f-a721-283c33cc9a36'),
  (uuid_generate_v4(), 'Dollar USA', 2300.10, '372463d9-398c-4e39-98c1-016f68e96e51'),
  (uuid_generate_v4(), 'Euro', 670.55, '285c4a17-1ac3-4274-a262-e48123a89a21'),
  (uuid_generate_v4(), 'Russian rouble', 14450.75, '27cdd9c8-ca1d-4a99-8cea-857b23dc5093'),
  (uuid_generate_v4(), 'Dollar USA', 520.25, '0825dd59-da4b-4f99-96db-47c2b8a44ff7'),
  (uuid_generate_v4(), 'Dollar USA', 6450.75, 'fdae2adf-6cec-478d-9c23-b067cb9811ca'),
  (uuid_generate_v4(), 'Euro', 1455.60, 'e28e6012-23c7-433c-8d9d-2f71e8ff3a5e'),
  (uuid_generate_v4(), 'Russian rouble', 34500.80, 'd01cd5b7-1639-4596-b022-f32ffa968413'),
  (uuid_generate_v4(), 'Dollar USA', 8925.10, 'c3b3217a-b9ae-4d3a-a289-1da2fc3bf187'),
  (uuid_generate_v4(), 'Euro', 3150.00, '99e6c025-b687-42af-9bf2-6404e4aea142'),
  (uuid_generate_v4(), 'Russian rouble', 22500.35, '99e6c025-b687-42af-9bf2-6404e4aea142'),
  (uuid_generate_v4(), 'Dollar USA', 760.95, '372463d9-398c-4e39-98c1-016f68e96e51'),
  (uuid_generate_v4(), 'Euro', 2730.25, '372463d9-398c-4e39-98c1-016f68e96e51'),
  (uuid_generate_v4(), 'Russian rouble', 15800.90, '27cdd9c8-ca1d-4a99-8cea-857b23dc5093'),
  (uuid_generate_v4(), 'Dollar USA', 4250.50, '99e6c025-b687-42af-9bf2-6404e4aea142');
 */

/*
 -- ошибка, нет клиента с таким id
insert into account (account_id, currency_name, amount, client_id)
VALUES
  (uuid_generate_v4(), 'Euro', 874.30, 'e28e6012-23c7-433c-8d9d-2f71e8ff3a5d');
 */

--select * from client where "balance " < (money(1500)) order by "balance ";

--select * from client where "balance "=(select min("balance ") from client);

--select sum("balance ") from client;

--select client.name, client.surname, account.account_id from client join account on client.client_id = account.client_id order by name, surname;

--select * from client order by "age" desc;
--select * from client order by date_birthday;

--select sum(count_equal_age) from (select extract(year from age(date_birthday)), count(*) as count_equal_age from client group by extract(year from age(date_birthday)) having count(*)>=2);

--select extract(year from age(date_birthday)), COUNT(*) as count_clients from client group by extract(year from age(date_birthday)) order by extract(year from age(date_birthday));

--select * from client limit 3;