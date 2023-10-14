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

Fork **Postman** collections that contain requests and a documentation to endpoints via `Fork` button in the dropdown list or by pressing `Ctrl + Alt + F` from the workspace:

`https://www.postman.com/gold-astronaut-497435/workspace/vidifystream-workspace/collection/28405641-c4d7122b-1b12-41dc-8efe-397785854420?action=share&creator=28405641`  

Make sure to fork both collections from the workspace.

If you want to view the complete documentation, press the following button in the *right corner* of the screen.

![ViewCompleteCollectionDocumentation.png](https://github.com/Alexander-Riabovol/VidifyStream/blob/master/ViewCompleteCollectionDocumentation.png)

That's it. Since you have successfully started the application, you can read the documentation and try out the desired endpoints.

##### Used frameworks:
![SignalR](https://img.shields.io/badge/SignalR-2596be) ![Entity Framework](https://img.shields.io/badge/Entity_Framework-1874a4) ![Mapster](https://img.shields.io/badge/Mapster-ffbc34) ![Fluent Validation](https://img.shields.io/badge/Fluent_Validation-ff0404) 
