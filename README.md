# saga-pattern-demo
Demonstrates orchestration with saga pattern using MassTransit. Leverages Kafka as message broker. Participating services uses cap library(https://cap.dotnetcore.xyz/) to connect with Kafka and orchestration service uses MassTransit Kafka Rider(https://masstransit.io/documentation/transports/kafka).

## Overview
There are 4 services paricipating in this solution.

1) Order Service
Initiate the order and triggers the flow by sending CreateOrder message.

2) Stock Service
Handles the stock management activities.

3) Payment Service
Handles the payment related activities.

4) Orchestration Service
Hosts an orchestration service for Create Order workflow by leveraging MassTransit Saga State Machine(https://masstransit.io/documentation/patterns/saga/state-machine).

## Happy Path Flow

## Stock Reservation Failed Flow

## Payment Failed Flow