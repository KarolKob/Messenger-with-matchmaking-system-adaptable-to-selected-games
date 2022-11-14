# Messenger with matchmaking system adaptable to selected games

## Overview
The point of this project is to create a system that would integrate basic social media features along with the possibility of adding a customizable matchmaking system to games that don't have those solutions implemented. The system is made out of the following parts:
* Window client app
* Communicator server and database 
* Matchmaking configuration app and database
* WebAPI handling configured matchmaking and user game stats

![image](https://user-images.githubusercontent.com/42039707/201717961-86680cd9-6e6b-4cd0-b02a-893234ea5b56.png)

## Technologies used in the project
C#, .NET Framework, Windows Forms .NET, ADO.NET Entity Framework, NAudio, SQLite, ASP.NET Core

## Client application
The client app allows the user to:
* create an account and log in,
* search for users and add them to the contacts list,
* block other users,
* create messaging groups or private chats,
* have voice calls with one other user,
* launch games with opponents of similar skill level to the user.

Main app window:
![image](https://user-images.githubusercontent.com/42039707/201741591-e448cb63-3e45-4214-8b78-5639ea6bcf93.png)


## Communicator server
This server is responsible for all communicator functions like forwarding messages if a user isn't blocked or creating chat rooms. It also has main user database management.

## Matchmaking configuration application


## WebAPI
