![logo.png](https://github.com/Alexander-Riabovol/VidifyStream/blob/master/logo.png)

# 🎥 Vidify Stream

![.NET](https://img.shields.io/badge/.NET-7.0-6c3c94) ![Docker](https://img.shields.io/badge/Docker-288ce4)

This is my pet-project. I tried to implement a **video-hosting** solution.

## Installation

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) - to host the application.
- [Postman](https://www.postman.com/downloads/) - to debug the endpoints.
### How to install and run
0. Open your command-line interface (CLI). Choose a *path* where you want to install the project with `cd`
1. Clone the repository by `git clone https://github.com/Alexander-Riabovol/VidifyStream.git`
2. Change your working directory to the one of solution by `cd VidifyStream`. 
3. Make sure that your `Docker` is running. Containerize the application by `docker-compose build`. <small>The initial <b>build</b> will require a certain amount of time</small>.
4. Run the application by `docker-compose up -d`. The application starts in 7-10 seconds at most.

## Usage

Add a `https://api.postman.com/collections/20418337-491d3294-dc54-4d68-9023-a94d06459556?access_key=PMAT-01H4P576GGM9HB8ETY9025TQPT` **Postman** collection that contains requests and a documentation to endpoints via `Import` button in the *left corner* of the screen.

![import.png](https://github.com/Alexander-Riabovol/VidifyStream/blob/master/import.png)

If you want to view the complete documentation, press the following button in the *right corner* of the screen.

![ViewCompleteCollectionDocumentation.png](https://github.com/Alexander-Riabovol/VidifyStream/blob/master/ViewCompleteCollectionDocumentation.png)

That's it. Since you have successfully started the application, you can read the documentation and try out the desired endpoints.

##### Used frameworks:
![SignalR](https://img.shields.io/badge/SignalR-2596be) ![Entity Framework](https://img.shields.io/badge/Entity_Framework-1874a4) ![Mapster](https://img.shields.io/badge/Mapster-ffbc34) ![Fluent Validation](https://img.shields.io/badge/Fluent_Validation-ff0404) 
