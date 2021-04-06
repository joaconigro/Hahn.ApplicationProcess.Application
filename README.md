# Hahn.ApplicationProcess.Application

This is a demo application for Hahn Softwareentwicklung.

## To run the web app:
#### 1. Open a command prompt on the Hahn.ApplicationProcess.Application.Web. directory.
#### 2. Run the command `docker-compose up` (You need to have Docker installed on your computer). This command will build the app in Development mode, so Swagger UI will work. If you want to run in production mode, change the argument ASPNETCORE_ENVIRONMENT in the docker-compose file to 'Production'.
#### 3. Open your browser and go to localhost:8080.


## To debug the project:
#### 1. Open a command prompt on the Hahn.ApplicationProcess.Application.Web/ClientApp directory.
#### 2. The first time you will need to run the command `npm install`. Then run `npm start`. This command will generate the bundles in development mode and will start to watch those files. If you change any file (.ts, .html or .css) and save the changes, webpack will recompile the bundle.
#### 3. Start debugging the Hahn.ApplicationProcess.Application.Web project. You can use VS Code or Visual Studio. Visual Studio will run the browser automatically. On VS Code you need to run manually and navigate to localhost:5001.

## To try the Xamarin.Forms app:
#### 1. Run the web app using `docker-compose up`, like indicates the first instructions.
#### 2. Start Visual Studio and choose Hahn.Mobile.Android to start debuging. If you don't have an emulator, Visual Studio will offer to create a new one. If you want to debug Hahn.Mobile.iOS, you will need a Mac and use Visual Studio for Mac (also XCode is needed). Also you connect to your Mac using a remote session and debug your iOS app from Windows.
