# saga-pattern-demo
Demonstrates micro service orchestration using saga pattern via MassTransit. Leverages Kafka as message broker. Participating services uses cap library(https://cap.dotnetcore.xyz/) to connect with Kafka and orchestration service uses MassTransit Kafka Rider(https://masstransit.io/documentation/transports/kafka).

## Overview
There are 4 services participating in this solution.

#### 1) Order Service
Initiate the order and triggers the flow by sending CreateOrder message.

#### 2) Stock Service
Handles the stock management activities.

#### 3) Payment Service
Handles the payment related activities.

#### 4) Orchestration Service
Hosts an orchestration service for Create Order workflow by leveraging MassTransit Saga State Machine(https://masstransit.io/documentation/patterns/saga/state-machine).

## Happy Path Flow
![Happy Path](images/Design-Happy_Path.jpg)

## Stock Reservation Failed Flow
![Stock Reservation Failed](images/Design-StockReservationFailed_Flow.jpg)

## Payment Failed Flow
![Payment Failed](images/Design-PaymentFailed_Flow.jpg)