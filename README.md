**Title: Shopping Basket Application**

**Objective**: Develop a full-stack application to manage a shopping
basket, calculate the total price of grocery

items, apply discounts, and display a detailed receipt.

**Requirements:**

* Docker
* Visual Studio
* Visual Code
* MySQL WorkBench

# Getting Started

## Docker

Installing docker is straightforward, follow this steps.

1. Install docker. We can use this [tutorial](https://dev.azure.com/livetiles-products/Product/_wiki/wikis/IXP%20Developer%20Info/577/running-docker-ce-in-wsl2) to get it installed.
2. When docker is setted up, we can now go to `shopping-basket-application\docker` from the project.
3. On the folder you downloaded the _docker-compose.yml_, run in the command prompt: `docker compose up -d`
4. To view the containers, a simple way of doing it is to add in vscode the [Docker plugg-in](https://docs.docker.com/get-docker/)

## DataBase

1. Using MySQL WorkBenck, access the database _localdevDB_
2. Run script `install_data.sql` at `shopping-basket-application\docker`

#### NOTE
 
 If required create new user to access:

 * Restart docker container and run following commands to get to the bash shell in the mysql container

```bash
    docker ps
    docker exec -it <mysql container name> /bin/bash 
```

* Inside the container, to connect to mysql command line type,
    
```bash
    mysql -u root -p
    create user 'user'@'%' identified by 'pass';
    grant all privileges on *.* to 'user'@'%' with grant option;
    flush privileges;
```


## Client

On ther terminal cd to `shopping-basket-application/client/shopping-basket-site folder` and `npm i` to update packages.

Run the development server:

```bash
npm run dev
# or
yarn dev
# or
pnpm dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

## Server

Just build the project and run.