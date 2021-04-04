# Hahn.ApplicationProcess.Application

<p>This is a demo application for Hahn Softwareentwicklung.</p>

<p>If you want to try de web app, simply follow these steps:<br>
1. Open a command prompt on the Hahn.ApplicationProcess.Application.Web.<br>
2. Run the command `docker-compose up` (You need to have Docker installed on your computer). This command will build the app in Development mode, so Swagger UI will work. If you want to run in production mode, change the argumente ASPNETCORE_ENVIRONMENT in the docker-compose file to 'Production'.<br>
3. Open your browser and go to localhost:8080.<br>
</p>

<p>To debug the project, follow these steps:<br>
1. Open a command prompt on the Hahn.ApplicationProcess.Application.Web/ClientApp<br>
2. The first time you will need to run the command `npm install`. Then run `npm start`. This command will generate the bundles in development mode and will start to watch those files. If you change any file (.ts, .html or .css) and save the changes, webpack will recompile the bundle.<br>
3. Start debuggin the Hahn.ApplicationProcess.Application.Web project. You can use VS Code or Visual Studio. Visual Studio will run the browser automatically. On VS Code you need to run manually and navigate to localhost:5001.<br>
</p>
