## **Description**
This is a product campaign management module that manipulates prices according to demand. System accepts some predefined commands that are defined inside **Available Commands**.

Application has a **web api** entry point and a **console application** entry point. .Net Core and Postgresql are used. Data is stored inside a relational database. Layered structure is used.

#### **Available Commands**
- create_product PRODUCTCODE PRICE STOCK 
  - Creates product in your system with given product information.
- get_product_info PRODUCTCODE 
  - Prints product information for given product code.
- create_order PRODUCTCODE QUANTITY 
  - Creates order in your system with given information.
- create_campaign NAME PRODUCTCODE DURATION PMLIMIT TARGETSALESCOUNT
  - Creates campaign in your system with given information
- get_campaign_info NAME 
  - Prints campaign information for given campaign name
- increase_time HOUR 
  - Increases time in your system.

___

## **Web API**
.NET CORE 5.0 Web Api

##### **DOCKER**

Run the code below in command line to build and start api and db containers.

    docker-compose up

Api will be run `http://localhost:9000` url after starting containers.

It will migrate database automatically.

Sample request to execute a command:

    curl -X POST "http://localhost:9000/Command" -H  "accept: text/plain" -H  "Content-Type: multipart/form-data" -F "command=increase_time 1"
    
Reset database to initial state:

    curl -X POST "https://localhost:9000/Command/reset"
    
##### **LOCAL ENVIRONMENT**

Build and run web api project. 

`https://localhost:44385/swagger/index.html` will be openned.

Requests can be executed from swagger ui.

___

## **Console Application**

It is an alternative to web api to execute commands.

Build and run `ConsoleApp` project.

It will migrate database automatically.

Enter `create_product PHONE 100 20` to execute product creation command and press enter.
    
Reset database to initial state enter `reset_system_data` command.
    
To exit the application enter `exit` or `quit`.

___

## **Unit Tests**

Run tests with command below.

    dotnet test
